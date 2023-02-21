using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Trax.Models.Generic.Identity.Request
{
    public class UpdateUserRequestDTO
    {
        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression("^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ ]*$", ErrorMessage = "El nombre solo puede contener letras.")]
        public string Name { get; set; }

        [Required]
        [StringLength(75)]
        [RegularExpression("^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]*$", ErrorMessage = "El apellido paterno solo puede contener letras.")]
        public string FirstSurname { get; set; }

        [Required]
        [StringLength(75)]
        [RegularExpression("^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]*$", ErrorMessage = "El apellido materno solo puede contener letras.")]
        public string SecondSurname { get; set; }

        [StringLength(10)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El código de Proveedor solo puede contener números.")]
        public string CodeProvider { get; set; }

        public bool Enable { get; set; }
    }
}
