using AutoMapper;
using HasanPolatCom.Application.Helpers.JWT;
using HasanPolatCom.Application.Interfaces.Repository;
using HasanPolatCom.Application.Wrappers;
using HasanPolatCom.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HasanPolatCom.Application.Features.Queries.ValidUserAndCreateToken
{
    public class ValidUserAndCreateTokenQuery : IRequest<ServiceResponse<string>>
    {
        public string mail { get; set; }
        public string password { get; set; }

        public class ValidUserAndCreateTokenQueryHandler : IRequestHandler<ValidUserAndCreateTokenQuery, ServiceResponse<string>>
        {
            private readonly IMapper mapper;
            private readonly IUserRepository userRepository;
            private readonly IConfiguration configuration;

            public ValidUserAndCreateTokenQueryHandler(IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
            {
                this.mapper = mapper;
                this.userRepository = userRepository;
                this.configuration = configuration;
            }

            public async Task<ServiceResponse<string>> Handle(ValidUserAndCreateTokenQuery request, CancellationToken cancellationToken)
            {

                string? responseText, message;
                bool isSuccess;
                User searchUser = await userRepository.GetByAuth(request.mail, request.password);
                if (searchUser == null)
                {
                    responseText = null;
                    isSuccess = false;
                    message = "Email or Password wrong!";
                }
                else
                {
                    var privateKey = configuration["PrivateTokenKey"];
                    var token = new JwtTokenBuilder()
                                        .AddSubject("Hasan Polat")
                                        .AddClaim("MemberShipId", "ill")
                                        .AddClaim("UserId", searchUser.Id.ToString())
                                        .AddIssuer("Hasan.Security.Bearer")
                                        .AddAudience("Hasan.Security.Bearer")
                                        .AddSecurityKey(JwtSecurityKey.Create(privateKey))
                                        .AddExprity(5)
                                        .Build();
                    
                    responseText = token.Value;
                    isSuccess = true;
                    message = null;
                }

                ServiceResponse<string> response = new ServiceResponse<string>(responseText, isSuccess, message);

                return response;

            }
        }
    }
}
