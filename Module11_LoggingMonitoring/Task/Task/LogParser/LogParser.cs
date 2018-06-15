using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.MSUtil;

namespace LogParser
{
    public class LogParser
    {
        public string Path { get; set; }

        private COMTSVInputContextClass loggerContext;

        private readonly LogQueryClass logQueryClass;

        public ILogRecordset Error
        {
            get
            {
                return logQueryClass.Execute($@"SELECT Field1 AS Date, Field2 AS Level, Field3 AS Message FROM {Path} WHERE Field2='ERROR'", loggerContext);
            }
        }

        public ILogRecordset LevelsCount
        {
            get
            {
                return logQueryClass.Execute($@"SELECT Field2 AS Level, COUNT(*) AS LevelsCount FROM {Path} GROUP BY Field2", loggerContext);
            }
        }

        public LogParser(string path)
        {
            this.Path = path;
            
            this.loggerContext = new COMTSVInputContextClass
            {
                headerRow = false,
                iSeparator = " | ",
                nFields = 3
            };
            this.logQueryClass = new LogQueryClass();
        }
    }
}
