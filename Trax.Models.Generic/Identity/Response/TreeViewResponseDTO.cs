using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Response
{
    public class TreeViewResponseDTO
    {
        public TreeViewResponseDTO()
        {
            this.Result = new OperationResult.OperationResult();
            this.Items = new List<TreeViewItemDTO>();
        }
        public List<TreeViewItemDTO> Items { get; set; }
        public OperationResult.OperationResult Result { get; set; }
    }
}
