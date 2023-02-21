using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Framework.Generic.Error
{
    public class Error
    {
        public static Exception InnerException(Exception ex)
        {
            if (ex.InnerException != null)
                return InnerException(ex.InnerException);
            else
                return ex;
        }
        public static string InnerExceptionMessage(Exception ex)
        {
            if (ex.InnerException != null)
                return InnerExceptionMessage(ex.InnerException);
            else
                return ex.Message;
        }
        public static string InnerExceptionCode(Exception ex)
        {
            if (ex.InnerException != null)
                return InnerExceptionCode(ex.InnerException);
            else
                return ex.HResult.ToString();
        }
        public static string InnerExceptionStackTrace(Exception ex)
        {
            if (ex.InnerException != null)
                return InnerExceptionStackTrace(ex.InnerException);
            else
                return ex.StackTrace;
        }
    }
}
