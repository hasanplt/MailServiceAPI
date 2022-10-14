using AutoMapper;
using HasanPolatCom.Application.Interfaces.Repository;
using HasanPolatCom.Application.Wrappers;
using HasanPolatCom.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;
using HasanPolatCom.Application.MessageQueues;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using HasanPolatCom.Application.DTO;
using System.Text.Json.Serialization;

namespace HasanPolatCom.Application.Features.Commands.CreateMail
{
    public class CreateMailCommand : IRequest<ServiceResponse<Guid>>
    {
        public string Receiver { get; set; }
        public string Content { get; set; }
        public string Header { get; set; }


        public bool IsNullOrEmpty()
        {
            if (string.IsNullOrEmpty(this.Receiver))
                return true;

            if (string.IsNullOrEmpty(this.Content))
                return true;

            if (string.IsNullOrEmpty(this.Header))
                return true;

            return false;
        }

        public class CreateMailCommandHandler : IRequestHandler<CreateMailCommand, ServiceResponse<Guid>>
        {
            private readonly IMailRepository mailRepository;
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;
            private readonly HttpContext httpContext;
            private readonly RabbitMQClient rabbitMQClient;

            public CreateMailCommandHandler(IMailRepository mailRepository, IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                this.mailRepository = mailRepository;
                this.userRepository = userRepository;
                this.mapper = mapper;
                httpContext = httpContextAccessor.HttpContext;
                rabbitMQClient = new RabbitMQClient();
            }

            public HttpContent HttpContent { get; }

            public async Task<ServiceResponse<Guid>> Handle(CreateMailCommand request, CancellationToken cancellationToken)
            {
                if (request.IsNullOrEmpty())
                {
                    var badResponse = new ServiceResponse<Guid>(Guid.Empty);
                    badResponse.IsSuccess = false;
                    badResponse.Message = "Eksik ya da boş alan bulunmakta! API Dokümantasyonunu inceleyebilirsiniz";

                    return badResponse;
                }

                Mail mail = mapper.Map<Mail>(request);

                string token = await httpContext.GetTokenAsync("access_token");

                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:7202");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var result = httpClient.GetAsync("api/Auth").Result;

                var jsonString = result.Content.ReadAsStringAsync().Result;

                jsonString.Replace("/", "");

                var response = JsonConvert.DeserializeObject<ServiceResponse<UserViewDTO>>(jsonString);

                if(response.Value == null || string.IsNullOrEmpty(response.Value.MailPassword) || string.IsNullOrEmpty(response.Value.MailPassword) || !response.IsSuccess)
                {
                    var badResponse = new ServiceResponse<Guid>(Guid.Empty);
                    badResponse.IsSuccess = false;
                    badResponse.Message = "Token ya da identityservice ile ilgili bir sorun oluştu!";

                    return badResponse;
                }

                mail.SenderMail = response.Value.MailAddress;
                mail.SenderPassword = response.Value.MailPassword;

                mail = await mailRepository.AddAsync(mail);

                MailRabbitMQDTO mailRabbitMQDTO = mapper.Map<MailRabbitMQDTO>(mail);
                mailRabbitMQDTO.Token = token;
                
                rabbitMQClient.Add("MailServiceQueue", mailRabbitMQDTO);

                return new ServiceResponse<Guid>(mail.Id);
            }
        }
    }
}
