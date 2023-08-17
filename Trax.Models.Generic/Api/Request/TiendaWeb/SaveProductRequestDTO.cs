using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Api.Request.TiendaWeb
{
    public class SaveProductRequestDTO
    {
        [Required]
        [StringLength(100,ErrorMessage ="No se pueden ingresar mas de 100 caracteres")]
        public string Codigo { get; set; }
        [Required]
        [StringLength(300, ErrorMessage = "No se pueden ingresar mas de 300 caracteres")]
        public string Nombre { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "No se pueden ingresar mas de 500 caracteres")]
        public string Descripcion { get; set; }
        [Required]
        [Range(0,int.MaxValue,ErrorMessage ="Se encontraron valores negativos en el precio")]
        public decimal Precio { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Se encontraron valores negativos en el IdCategoria")]

        public int IdCategoria { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Se encontraron valores negativos en el Stock")]

        public int Stock { get; set; }
    }
}
