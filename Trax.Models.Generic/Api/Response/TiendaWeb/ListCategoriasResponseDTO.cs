using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Api.Response.TiendaWeb
{
    public class ListCategoriasResponseDTO
    {
        public ListCategoriasResponseDTO() {
        
            this.Result =  new OperationResult.OperationResult();
            this.List = new List<CategoriaDTO>();
        }
        public OperationResult.OperationResult Result { get; set; }
        public List<CategoriaDTO> List { get; set; }
    }
}
