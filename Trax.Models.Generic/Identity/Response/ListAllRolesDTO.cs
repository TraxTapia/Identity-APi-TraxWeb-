using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Response
{
    public class ListAllRolesDTO
    {
        public ListAllRolesDTO()
        {
            this.Result = new OperationResult.OperationResult();
        }

        public List<ApplicationResponseDTO> Applications { get; set; }
        public OperationResult.OperationResult Result { get; set; }
    }
}
