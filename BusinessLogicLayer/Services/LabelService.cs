using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;

namespace BusinessLogicLayer.Services
{

    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepository;

        public LabelService(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public Label CreateLabel(int userId, string labelName)
        {
            return _labelRepository.CreateLabel(userId, labelName);
        }

       
        public List<Label> GetMyLabels(int userId)
        {
            return _labelRepository.GetLabelsByUser(userId);
        }

        public void AssignLabelToNote(int noteId, int labelId)
        {
            _labelRepository.AssignLabelToNote(noteId, labelId);
        }
    }
}
