using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity
{
    public class ContentAuthorizeDTO
    {
        [Required]
        [StringLength(50)]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "El nombre de una Acción solo puede contener letras.")]
        public string Action { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "El nombre de un Modulo solo puede contener letras.")]
        public string Module { get; set; }
    }
}
