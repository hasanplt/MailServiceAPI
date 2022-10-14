# Mail Servis API

Öncelikle IdentityService'mize kayıt olmalıyız:

      https://localhost:7202/api/Auth/Register 
      adresinin body'sine
      `{
          "FirstName": "ISIM",
          "LastName": "SOYISIM",
          "City": "SEHIR",
          "AccountPassword": "HESAPSIFRE",
          "MailAddress": "MAILGONDERECEKEMAİLADRESI",
          "MailPassword": "MAILGONDERECEKMAILADRESININSIFRESİ"
      }`
      json nesnesini göndermeliyiz

Kayıt olduktan sonra giriş yaparak JWT'ımızı almalıyız.

      https://localhost:7202/api/Auth
      adresinin body'sine
      `{
          "mail": "hasanpolat60official@gmail.com",
          "password": "12345"
      }`
      json nesnesini göndermeliyiz
      ! password kısmı AccountPassword'e yazdığınız şifre gelmeli
      
      bize cevap olarak : 
      `{
          "value": "TOKEN BURADA",
          "id": "00000000-0000-0000-0000-000000000000",
          "message": null,
          "isSuccess": true
      }`
      buradaki value değeri bizim tokenımız.
      
Tokenımızı aldığımıza göre artık mail gönderme yapabiliriz.
      https://localhost:7210/api/Mail/send
      adresinin body'sine
      {
        "receiver": "GÖNDERECEĞİNİZ MAİL ADRESİ",
        "content": "İÇERİK",
        "header": "BAŞLIK"
      }
      ancak bu isteğe Authorization kısmına aldığınız tokeni eklemek zorundasınız.

      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
