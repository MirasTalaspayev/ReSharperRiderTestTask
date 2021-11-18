using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ReSharperRiderTestTask
{
    class FileParser
    {
        static char[] delims = { ',', ';', '\t' };
        public char Delimiter { get; private set; }
        public FileParser() { }
        public string[][] Parse(string path)
        {
            var lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                return null;
            }
            string[][] datatable = new string[lines.Length][];
            for (int i = 0; i < delims.Length; i++)
            {
                char c = delims[i];
                datatable[0] = lines[0].Split(c);
                if (datatable[0].Length != 1)
                {
                    Delimiter = delims[i];
                    break;
                }
            }
            for (int i = 1; i < lines.Length; i++)
            {
                datatable[i] = lines[i].Split(Delimiter);
            }
            return datatable;
        }
    }
}
