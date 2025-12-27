using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogicLayer.Interfaces
{
    public interface ICollaboratorRepository
    {
        Collaborator Add(int noteId, int userId);
        List<Collaborator> GetByNoteId(int noteId);
        bool RemoveById(int collaboratorId);
        bool RemoveByEmail(int noteId, string email);
        List<Notes> GetSharedNotes(int userId);
    }
}
