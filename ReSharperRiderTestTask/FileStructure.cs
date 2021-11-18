using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace ReSharperRiderTestTask
{
    class FileStructure
    {
        public SortedDictionary<string, string> Column_DataType { get; private set; }
        public FileStructure()
        {
            Column_DataType = new SortedDictionary<string, string>();
        }
        public override bool Equals(object obj)
        {
            FileStructure fs = obj as FileStructure;
            
            // Check for equal Column_DataType dictionary.
            if (Column_DataType.Count == fs.Column_DataType.Count && !Column_DataType.Except(fs.Column_DataType).Any())
            {
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var kv in Column_DataType)
            {
                sb.Append(kv);
            }
            return sb.ToString();
        }
    }
}
