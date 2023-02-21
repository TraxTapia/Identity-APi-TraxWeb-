using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Framework.identity
{
    public class Validator
    {
        public static bool IsEmail(string Email)
        {
            try
            {
                System.Net.Mail.MailAddress _MailAddress = new System.Net.Mail.MailAddress(Email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
