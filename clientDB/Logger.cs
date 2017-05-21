using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace clientDB
{
    public class Logger
    {
        private static Logger instance;

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }

        public void Log(string message)
        {
            File.AppendAllText("log.txt", $"{DateTime.Now} - {message}" + Environment.NewLine);
        }

        private Logger()
        {
        }

    }
}
