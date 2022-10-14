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

namespace HasanPolatCom.Application.Features.Queries.GetAllMails
{
    public class GetAllMailsQuery : IRequest<ServiceResponse<List<MailViewDTO>>>
    {

        public class GetAllMailsQueryHandler : IRequestHandler<GetAllMailsQuery, ServiceResponse<List<MailViewDTO>>>
        {
            private readonly IMailRepository mailRepository;
            private readonly IMapper mapper;
            private readonly HttpContext httpContext;

            public GetAllMailsQueryHandler(IMailRepository mailRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                this.mailRepository = mailRepository;
                this.mapper = mapper;
                this.httpContext = httpContextAccessor.HttpContext;
            }
            public async Task<ServiceResponse<List<MailViewDTO>>> Handle(GetAllMailsQuery request, CancellationToken cancellationToken)
            {
                string token = await httpContext.GetTokenAsync("access_token");

                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:7202");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var result = httpClient.GetAsync("api/Auth").Result;
                var jsonString = result.Content.ReadAsStringAsync().Result;
                jsonString.Replace("/", "");

                var response = JsonConvert.DeserializeObject<ServiceResponse<UserViewDTO>>(jsonString); // null geliyor mail gelmiyorr

                var allDatas = await mailRepository.GetAllBySenderAsync(response.Value.MailAddress);

                return new ServiceResponse<List<MailViewDTO>>(mapper.Map<List<MailViewDTO>>(allDatas));
            }
        }

    }
}
