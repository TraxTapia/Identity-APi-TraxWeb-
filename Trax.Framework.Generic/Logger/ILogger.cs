using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Framework.Generic.Logger
{
    public interface ILogger
    {
        void LogError(Exception ex);
        void LogText(string Texto);
        void LogText(string Texto, bool TrackDateTime);
        void LogText(string Texto, string FileName);
        void LogText(string Texto, string FileName, bool TrackDateTime);
    }
}
