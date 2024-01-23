using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Api
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Categoria { get; set; }
        public bool Activo { get; set; }
    }
}
