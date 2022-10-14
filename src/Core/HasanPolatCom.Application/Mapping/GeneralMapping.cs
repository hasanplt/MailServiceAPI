using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Application.Mapping
{
    public class GeneralMapping: Profile
    {

        public GeneralMapping()
        {

            // Mail Mappings
            CreateMap<Features.Commands.CreateMail.CreateMailCommand, Domain.Entities.Mail>()
                .ReverseMap();

            CreateMap<Features.Commands.UpdateMail.UpdateMailCommand, Domain.Entities.Mail>()
                .ReverseMap();

            CreateMap<Domain.Entities.Mail, Features.Queries.GetAllMailByReceiver.GetAllMailByReceiver>()
                .ReverseMap();

            CreateMap<DTO.MailViewDTO, Domain.Entities.Mail>()
                .ReverseMap();

            CreateMap<Domain.Entities.Mail, Features.Commands.CreateMail.MailRabbitMQDTO>()
                .ReverseMap();
            
            // User Mappings
            CreateMap<Domain.Entities.User, Features.Commands.CreateUser.CreateUserCommand>()
                .ReverseMap();

            CreateMap<Domain.Entities.User, DTO.UserViewDTO>()
                .ReverseMap();
        }

    }
}
