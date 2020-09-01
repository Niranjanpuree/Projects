using System;
using System.Collections.Generic;
using AutoCare.Product.Application.Infrastructure.Bus.Command;
using AutoCare.Product.Application.Models.Commands;
using AutoCare.Product.Application.Models.DomainModel;
using AutoCare.Product.Application.RepositoryServices;
using AutoMapper;

namespace AutoCare.Product.Application.Models.CommandHandlers
{
    public class MakeCommandHandler :
        ICommandHandler<AddMakeCommand>,
        ICommandHandler<UpdateMakeCommand>,
        ICommandHandler<DeleteMakeCommand>
    {
        private readonly IMakeRepositoryService _makeRepositoryService;

        public MakeCommandHandler(IMakeRepositoryService makeRepositoryService)
        {
            _makeRepositoryService = makeRepositoryService;
        }

        public void Handle(DeleteMakeCommand message)
        {
            throw new NotImplementedException();
        }

        public void Handle(UpdateMakeCommand message)
        {
            throw new NotImplementedException();
        }

        public void Handle(AddMakeCommand message)
        {
            var make = Make.AddNewMake(message.MakeName);
            _makeRepositoryService.AddAsync(Mapper.Map<EntityModels.Make>(make));
        }
    }
}
