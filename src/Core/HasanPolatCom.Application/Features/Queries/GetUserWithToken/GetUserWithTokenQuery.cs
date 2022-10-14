using AutoMapper;
using HasanPolatCom.Application.DTO;
using HasanPolatCom.Application.Interfaces.Repository;
using HasanPolatCom.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Application.Features.Queries.GetUserWithToken
{
    public class GetUserWithTokenQuery: IRequest<ServiceResponse<UserViewDTO>>
    {

        public class GetUserWithTokenQueryHandler : IRequestHandler<GetUserWithTokenQuery, ServiceResponse<UserViewDTO>>
        {
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;
            private readonly HttpContext httpContext;

            public GetUserWithTokenQueryHandler(IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                this.userRepository = userRepository;
                this.mapper = mapper;
                httpContext = httpContextAccessor.HttpContext;
            }


            public async Task<ServiceResponse<UserViewDTO>> Handle(GetUserWithTokenQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<Claim> data = httpContext.User.Claims;

                Claim claim = data.Where(x => x.Type == "UserId").FirstOrDefault();

                if (claim == null) 
                {
                    var response = new ServiceResponse<UserViewDTO>(null);

                    response.IsSuccess = false;
                    response.Message = "Token girdiğinizden emin olunuz.";

                    return response;
                }

                string userId = claim.Value;
                Guid userGuid = Guid.Parse(userId);

                var user = await userRepository.GetByIdAsync(userGuid);

                if(user == null)
                {
                    var response = new ServiceResponse<UserViewDTO>(null);

                    response.IsSuccess = false;
                    response.Message = "Tokena göre kullanıcı bulunamadı!";

                    return response;
                }

                return new ServiceResponse<UserViewDTO>(mapper.Map<UserViewDTO>(user));
            }
        }

    }
}
