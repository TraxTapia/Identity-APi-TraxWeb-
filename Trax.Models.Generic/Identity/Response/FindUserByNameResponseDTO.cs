using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Response
{
    public class FindUserByNameResponseDTO
    {
        public FindUserByNameResponseDTO()
        {
            this.Result = new OperationResult.OperationResult();
        }

        public UserDTO User { get; set; }
        public OperationResult.OperationResult Result { get; set; }
    }
}
