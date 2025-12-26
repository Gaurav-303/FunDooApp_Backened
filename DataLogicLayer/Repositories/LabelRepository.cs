using DataLogicLayer.Context;
using DataLogicLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogicLayer.Repositories
{
    public class LabelRepository : ILabelRepository
    {
        private readonly FundooContext _context;

        public LabelRepository(FundooContext context)
        {
            _context = context;
        }

        public Label CreateLabel(int userId, string labelName)
        {
            var label = new Label
            {
                LabelName = labelName,
                UserId = userId
            };

            _context.Labels.Add(label);
            _context.SaveChanges();
            return label;
        }

        public void AssignLabelToNote(int noteId, int labelId)
        {
            var noteLabel = new NoteLabel
            {
                NoteId = noteId,
                LabelId = labelId
            };

            _context.NoteLabels.Add(noteLabel);
            _context.SaveChanges();
        }

        public List<Label> GetLabelsByUser(int userId)
        {
            return _context.Labels
                .Where(l => l.UserId == userId)
                .ToList();
        }
    }
}

