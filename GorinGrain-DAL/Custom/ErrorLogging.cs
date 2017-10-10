using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorinGrain_DAL.Custom
{
    class ErrorLogging
    {
        public static void LogError(Exception e)
        {
            //error message writes to same file that holds this program, bool at the end is to append, not overwrite
            using (StreamWriter fileWriter = new StreamWriter(@"C:\Users\Kyle\Documents\visual studio 2015\Projects\GorinGrain\Log files\errors.txt", true))
            {
                fileWriter.WriteLine(e.Message);
            }
        }
    }
}
