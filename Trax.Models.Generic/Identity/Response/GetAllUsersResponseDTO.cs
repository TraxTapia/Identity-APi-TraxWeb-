using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Response
{
    public class GetAllUsersResponseDTO
    {
        public GetAllUsersResponseDTO()
        {
            this.Result = new OperationResult.OperationResult();
        }

        public List<UserDTO> Users { get; set; }
        public OperationResult.OperationResult Result { get; set; }
    }
}
