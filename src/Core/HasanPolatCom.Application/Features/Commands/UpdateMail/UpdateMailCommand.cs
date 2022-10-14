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
using HasanPolatCom.Application.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace HasanPolatCom.Application.Features.Commands.UpdateMail
{
    public class UpdateMailCommand : IRequest<ServiceResponse<Guid>>
    {
        public Guid Id { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
        public string Header { get; set; }
        public bool IsSend { get; set; } = false;
        public string SenderMail { get; set; }
        public string SenderPassword { get; set; }


        public class UpdateMailCommandHandler : IRequestHandler<UpdateMailCommand, ServiceResponse<Guid>>
        {
            private readonly IMailRepository mailRepository;
            private readonly IMapper mapper;
            private readonly HttpContext httpContext;

            public UpdateMailCommandHandler(IMailRepository mailRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                this.mailRepository = mailRepository;
                this.mapper = mapper;
                httpContext = httpContextAccessor.HttpContext;
            }

            public async Task<ServiceResponse<Guid>> Handle(UpdateMailCommand request, CancellationToken cancellationToken)
            {
                string token = await httpContext.GetTokenAsync("access_token");

                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:7202");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var result = httpClient.GetAsync("api/Auth").Result;
                var jsonString = result.Content.ReadAsStringAsync().Result;
                jsonString.Replace("/", "");

                var UserResponse = JsonConvert.DeserializeObject<ServiceResponse<UserViewDTO>>(jsonString);

                Mail mail = await mailRepository.UpdateAsync(mapper.Map<Mail>(request), UserResponse.Value.MailAddress);

                return new ServiceResponse<Guid>(mail.Id);
            }
        }
    }
}
