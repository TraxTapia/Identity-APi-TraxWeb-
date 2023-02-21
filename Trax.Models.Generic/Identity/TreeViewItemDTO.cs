using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity
{
    public class TreeViewItemDTO
    {
        public TreeViewItemDTO()
        {
            this.Collapsed = true;
            this.Disabled = false;
            this.Children = null;
        }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Disabled { get; set; }
        public bool Checked { get; set; }
        public bool Collapsed { get; set; }
        public List<TreeViewItemDTO> Children { get; set; }
    }
}
