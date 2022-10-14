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

namespace HasanPolatCom.Application.Features.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<ServiceResponse<Guid>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string AccountPassword { get; set; }
        public string MailAddress { get; set; }
        public string MailPassword { get; set; }


        public bool IsNullOrEmpty()
        {
            if(string.IsNullOrEmpty(this.FirstName))
                return true;

            if (string.IsNullOrEmpty(this.LastName))
                return true;

            if (string.IsNullOrEmpty(this.City))
                return true;

            if (string.IsNullOrEmpty(this.AccountPassword))
                return true;

            if (string.IsNullOrEmpty(this.MailAddress))
                return true;

            if (string.IsNullOrEmpty(this.MailPassword))
                return true;

            return false;
        }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ServiceResponse<Guid>>
        {
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;

            public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
            {
                this.userRepository = userRepository;
                this.mapper = mapper;
            }

            public async Task<ServiceResponse<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {

                if (request.IsNullOrEmpty())
                {
                    var response = new ServiceResponse<Guid>(Guid.Empty);
                    response.IsSuccess = false;
                    response.Message = "Boş ya da eksik alan var! API Dokümantasyonuna göz atınız.";

                    return response;
                }

                User user = await userRepository.AddAsync(mapper.Map<User>(request));
                
                return new ServiceResponse<Guid>(user.Id);
            }
        }
    }
}
