using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Api.Response
{
    public class ClientesListResponseDTO
    {
        public ClientesListResponseDTO()
        {
            this.Result = new OperationResult.OperationResult();
        }
        public List<ClientesDTO> List { get; set; }
        public OperationResult.OperationResult Result { get; set; }
    }
}
