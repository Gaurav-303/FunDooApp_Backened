using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class Label
    {
        public int LabelId { get; set; }
        public string LabelName { get; set; }

        public int UserId { get; set; }
        

        public ICollection<NoteLabel> NoteLabels { get; set; }
    }
}
