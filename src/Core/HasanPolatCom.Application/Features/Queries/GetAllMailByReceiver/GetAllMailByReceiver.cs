using AutoMapper;
using HasanPolatCom.Application.DTO;
using HasanPolatCom.Application.Interfaces.Repository;
using HasanPolatCom.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Application.Features.Queries.GetAllMailByReceiver
{
    public class GetAllMailByReceiver : IRequest<ServiceResponse<List<MailViewDTO>>>
    {
        public string mailAddress { get; set; }
        public GetAllMailByReceiver(string mailaddress)
        {
            mailAddress = mailaddress;
        }

        public class GetAllMailByReceiverHandler : IRequestHandler<GetAllMailByReceiver, ServiceResponse<List<MailViewDTO>>>
        {
            private readonly IMailRepository mailRepository;
            private readonly IMapper mapper;
            private readonly HttpContext httpContext;

            public GetAllMailByReceiverHandler(IMailRepository mailRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                this.mailRepository = mailRepository;
                this.mapper = mapper;
                httpContext = httpContextAccessor.HttpContext;
            }

            public async Task<ServiceResponse<List<MailViewDTO>>> Handle(GetAllMailByReceiver request, CancellationToken cancellationToken)
            {
                string token = await httpContext.GetTokenAsync("access_token");

                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:7202");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var result = httpClient.GetAsync("api/Auth").Result;
                var jsonString = result.Content.ReadAsStringAsync().Result;
                jsonString.Replace("/", "");

                var UserResponse = JsonConvert.DeserializeObject<ServiceResponse<UserViewDTO>>(jsonString);

                if(!UserResponse.IsSuccess || string.IsNullOrEmpty(UserResponse.Value.MailAddress))
                {
                    var badResponse = new ServiceResponse<List<MailViewDTO>>(null);
                    
                    badResponse.IsSuccess = false;
                    badResponse.Message = "Token ya da identity service ile ilgili bir sorun oluştu";

                    return badResponse;
                }

                var mails = await mailRepository.GetAllByReceiverAsync(request.mailAddress, UserResponse.Value.MailAddress);

                var dtomapper = mapper.Map<List<MailViewDTO>>(mails);

                return new ServiceResponse<List<MailViewDTO>>(dtomapper);
            }
        }
    }
}
