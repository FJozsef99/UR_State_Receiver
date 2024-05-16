using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UR_State_Receiver
{
    public class DataSaver
    {
        public static void SaveToFile(string fileName, IEnumerable<string> linesToAdd ) 
        {
            File.AppendAllLines(fileName, linesToAdd);
        }
    }
}
