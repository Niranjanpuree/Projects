using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
   public class QuestionaireManagerTypeService : IQuestionaireManagerTypeService
    {
        private readonly IQuestionaireManagerTypeRepository _questionaireManagerTypeRepository;
        public QuestionaireManagerTypeService(IQuestionaireManagerTypeRepository questionaireManagerType)
        {
            _questionaireManagerTypeRepository = questionaireManagerType;
        }

        public Guid GetGuidByNormailzedName(string managerTypeNormalize)
        {
            return _questionaireManagerTypeRepository.GetGuidByNormailzedName(managerTypeNormalize);
        }

        public string GetNameByGuid(Guid id)
        {
            return _questionaireManagerTypeRepository.GetNameByGuid(id);
        }
    }
}
