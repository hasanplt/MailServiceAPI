using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mail;
using System.Net;
using System.Text;
using HasanPolatCom.Domain.Entities;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using HasanPolatCom.Application.Features.Commands.CreateMail;

try
{
    var factory = new ConnectionFactory { HostName = "localhost" };

    var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();

    channel.QueueDeclare(queue: "MailServiceQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, eventArgs) =>
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body); // mesaj burada geldi

        MailRabbitMQDTO mail = JsonConvert.DeserializeObject<MailRabbitMQDTO>(message);

        Console.WriteLine(message);

        if (mail != null)
        {

            SendMail(mail);

        }


    };

    channel.BasicConsume(queue: "MailServiceQueue", autoAck: true, consumer: consumer);

}
catch (Exception)
{
    Console.WriteLine("rabbitmq bağlantı sorunu");
    throw;
}

Console.ReadKey();


void SendMail(MailRabbitMQDTO mail)
{
    MailMessage msg = new MailMessage();
    msg.Subject = mail.Header;
    msg.From = new MailAddress(mail.SenderMail);
    msg.To.Add(new MailAddress(mail.Receiver));
    msg.Body = mail.Content;
    msg.IsBodyHtml = true;
    msg.Priority = MailPriority.High;
    // Host ve Port Gereklidir!
    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
    // Güvenli bağlantı gerektiğinden kullanıcı adı ve şifrenizi giriniz.
    NetworkCredential AccountInfo = new NetworkCredential(mail.SenderMail, mail.SenderPassword);
    smtp.UseDefaultCredentials = false;
    smtp.Credentials = AccountInfo;
    smtp.EnableSsl = true;
    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

    try
    {
        smtp.Send(msg);

        Console.WriteLine("mail gönderildi...");

        mail.IsSend = true;

        using (HttpClient httpclient = new HttpClient())
        {
            httpclient.BaseAddress = new Uri("https://localhost:7210");

            // burası lazım
            // rabbitmq'ya sıra eklerken tokenı'da göndermen lazım
            httpclient.DefaultRequestHeaders.Add("Authorization", "Bearer " + mail.Token);

            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(mail), Encoding.UTF8, "application/json");
            var result = httpclient.PostAsync("api/Mail/Update", httpContent).Result;

            var jsonString = result.Content.ReadAsStringAsync().Result;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Mail Gönderilemedi");
        Console.WriteLine(ex.Message);
    }

}