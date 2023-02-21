using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity
{
    public class AddExternalLoginRequest
    {
        [Required]
        [Display(Name = "Token de acceso externo")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordRequestDTO
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "La direccion de correo excedio los 256 caractéres.")]
        public string Email { get; set; }
    }

    public class RegisterUserRequestDTO
    {
        [Required(ErrorMessage = "El nombre de usario es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede ser mayor a 100 caractéres.")]
        [RegularExpression("^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ ]*$", ErrorMessage = "El nombre solo puede contener letras.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El primer apellido es requerido.")]
        [StringLength(75, ErrorMessage = "El apellido no puede ser mayor a 75 caractéres.")]
        [RegularExpression("^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]*$", ErrorMessage = "El apellido paterno solo puede contener letras.")]
        public string FirstSurname { get; set; }

        [Required(ErrorMessage = "El segundo apellido es requerido.")]
        [StringLength(75, ErrorMessage = "El apellido no puede ser mayor a 75 caractéres.")]
        [RegularExpression("^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]*$", ErrorMessage = "El apellido materno solo puede contener letras.")]
        public string SecondSurname { get; set; }

        [Required(ErrorMessage = "El Email es requerido.")]
        [StringLength(256, ErrorMessage = "La direccion de correo excedio los 256 caractéres.")]
        public string Email { get; set; }

        [StringLength(5)]
        public string CodeProvider { get; set; }
    }

    public class RegisterExternalRequest
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class RemoveLoginRequest
    {
        [Required]
        [Display(Name = "Proveedor de inicio de sesión")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Clave de proveedor")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
