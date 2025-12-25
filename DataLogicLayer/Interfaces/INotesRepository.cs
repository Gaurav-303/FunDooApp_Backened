using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;


namespace DataLogicLayer.Interfaces
{
    public interface INotesRepository
    {
        Notes GetNoteById(int noteId);
        List<Notes> GetNotesByUserId(int userId);
        void AddNote(int userId, Notes note);
        void UpdateNote(Notes note);
        void DeleteNote(int noteId);
    }
}
