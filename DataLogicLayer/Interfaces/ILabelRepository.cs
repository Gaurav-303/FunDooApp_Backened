using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;


namespace DataLogicLayer.Interfaces
{
    public interface ILabelRepository
    {
        void AssignLabelToNote(int noteId, int labelId);
        Label CreateLabel(int userId, string labelName);
        List<Label> GetLabelsByUser(int userId);
    }
}
