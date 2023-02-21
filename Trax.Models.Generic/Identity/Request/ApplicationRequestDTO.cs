using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity.Request
{
    public class ApplicationRequestDTO
    {
        [Required]
        [StringLength(50)]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "El nombre de Aplicación solo puede contener letras.")]
        public string AppName { get; set; }

        [StringLength(50)]
        [RegularExpression("^[A-Za-z_-]*$", ErrorMessage = "El nombre del Role solo puede contener letras.")]
        public string RoleName { get; set; }

        public List<ContentAuthorizeDTO> ContentAuthorize { get; set; }
    }
}
