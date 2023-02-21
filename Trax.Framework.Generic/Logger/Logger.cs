using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trax.Framework.Generic.Logger
{
    public class Logger : ILogger
    {
        private string _FilePathName = "";
        public bool DebugMode { get; set; }
        public Logger(string FilePathName)
        {
            _FilePathName = FilePathName;
        }

        public void LogError(Exception ex)
        {
            LogTexto(Error.Error.InnerExceptionMessage(ex) + "\r\n" + ex.StackTrace);
        }
        public void LogDebug(string Texto)
        {
            if (DebugMode)
                LogTexto(Texto);
        }
        public void LogTexto(string Texto)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(@_FilePathName, true);
            try
            {
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.ms tt") + " | " + Texto);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }
        public void LogText(string Texto)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(@_FilePathName, true);
            try
            {
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.ms tt") + " | " + Texto);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }
        public void LogText(string Texto, bool TrackDateTime)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(@_FilePathName, true);
            try
            {
                if (TrackDateTime)
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.ms tt") + " | " + Texto);
                else
                    sw.WriteLine(Texto);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sw != null) sw.Close();
            }
        }
        public void LogText(string Texto, string FileName)
        {
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(_FilePathName) + FileName)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(_FilePathName) + FileName));

            string _NewPath = System.IO.Path.GetDirectoryName(_FilePathName) + FileName;

            System.IO.StreamWriter sw = new System.IO.StreamWriter(_NewPath, true);
            try
            {
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.ms tt") + " | " + Texto);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sw != null) sw.Close();
            }
        }
        public void LogText(string Texto, string FileName, bool TrackDateTime)
        {
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(_FilePathName) + FileName)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(_FilePathName) + FileName));

            string _NewPath = System.IO.Path.GetDirectoryName(_FilePathName) + FileName;

            System.IO.StreamWriter sw = new System.IO.StreamWriter(_NewPath, true);
            try
            {
                if (TrackDateTime)
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.ms tt") + " | " + Texto);
                else
                    sw.WriteLine(Texto);
            }
            catch (Exception) { }
            finally
            {
                if (sw != null) sw.Close();
            }
        }
    }
}
