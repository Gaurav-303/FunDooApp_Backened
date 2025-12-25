using DataLogicLayer.Context;
using DataLogicLayer.Interfaces;
using Microsoft.Extensions.Logging;
using ModelLayer.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DataLogicLayer.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly FundooContext _context;
        private readonly ILogger<NotesRepository> _logger;

        public NotesRepository(
            FundooContext context,
            ILogger<NotesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Notes GetNoteById(int noteId)
        {
            return _context.Notes.FirstOrDefault(n => n.NoteId == noteId);
        }

        public List<Notes> GetNotesByUserId(int userId)
        {
            return _context.Notes
                           .Where(n => n.UserId == userId)
                           .ToList();
        }

        public void AddNote(int userId, Notes note)
        {
            note.UserId = userId;
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public void UpdateNote(Notes note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
        }

        public void DeleteNote(int noteId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.NoteId == noteId);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
            }
        }
    }
}
