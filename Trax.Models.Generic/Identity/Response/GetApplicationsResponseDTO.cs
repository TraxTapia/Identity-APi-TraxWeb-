using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Response
{
    public class GetApplicationsResponseDTO
    {
        public GetApplicationsResponseDTO()
        {
            this.Applications = new List<ApplicationResponseDTO>();
            this.Result = new OperationResult.OperationResult();
        }

        public List<ApplicationResponseDTO> Applications { get; set; }
        public OperationResult.OperationResult Result { get; set; }
    }
}
