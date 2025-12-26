using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class NoteLabel
    {
        public int NoteId { get; set; }
        public Notes Note { get; set; }

        public int LabelId { get; set; }
        public Label Label { get; set; }
    }
}
