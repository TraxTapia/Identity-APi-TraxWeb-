using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Api.Response.TiendaWeb
{
    public class ListProductosResponseDTO
    {
        public ListProductosResponseDTO()
        {

            this.Result = new OperationResult.OperationResult();
            this.List = new List<ProductoDTO>();
        }
        public OperationResult.OperationResult Result { get; set; }
        public List<ProductoDTO> List { get; set; }
    }
}
