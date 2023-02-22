using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Api
{
   
    public partial class ClientesDTO
    {
        public int id_cliente { get; set; }
     
        public string nombre_cliente { get; set; }
 
        public string email_cliente { get; set; }
    
        public string telefono_cliente { get; set; }
  
        public DateTime fecha_registro { get; set; }

       
    }
}
