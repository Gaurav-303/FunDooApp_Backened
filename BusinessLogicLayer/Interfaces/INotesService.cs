using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface INotesService
    {
        void AddNote(int userId, Notes note);
        List<Notes> GetMyNotes(int userId);
        Notes GetNoteById(int noteId);
        bool UpdateNote(int userId, int noteId, Notes updatedNote);
        bool DeleteNote(int userId, int noteId);


    }
}
