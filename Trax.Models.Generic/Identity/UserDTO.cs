using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Models.Generic.Identity
{
    public class UserDTO
    {
        public string Name { get; set; }

        public string FirstSurname { get; set; }

        public string SecondSurname { get; set; }

        public string Email { get; set; }

        public string CodeProvider { get; set; }

        public bool Enable { get; set; }
    }
}
