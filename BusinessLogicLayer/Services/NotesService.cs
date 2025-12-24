using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Context;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class NotesService : INotesService
    {
        private readonly FundooContext _context;
      

        public NotesService(FundooContext context)
        {
            _context = context;
        }

        public void AddNote(int userId, Notes note)
        {
            note.UserId = userId;
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public List<Notes> GetMyNotes(int userId)
        {
            return _context.Notes
                .Where(n => n.UserId == userId)
                .ToList();
        }
        public Notes GetNoteById(int noteId)
        {
            return _context.Notes.FirstOrDefault(n => n.NoteId == noteId);
        }
    }
}
