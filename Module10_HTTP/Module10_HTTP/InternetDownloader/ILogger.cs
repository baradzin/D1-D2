using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetDownloaderLib
{
    public interface ILogger
    {
        bool IsVerbose { get; set; }

        void WriteLine(string str);

        void Write(string str);

        void Error(string str);
    }
}
