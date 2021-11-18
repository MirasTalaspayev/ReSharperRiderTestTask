using System;
using System.Collections.Generic;
using System.Text;

namespace ReSharperRiderTestTask
{
    class FileFormat
    {
        public char Delimiter { get; set; }
        public char NumberSeperatorDigital { get; set; }
        public char NumberSeperatorThousand { get; set; }
        public string DateFormat { get; set; }
        public override bool Equals(object obj)
        {
            FileFormat o = obj as FileFormat;
            if (Delimiter == o.Delimiter &&
                NumberSeperatorDigital == o.NumberSeperatorDigital &&
                NumberSeperatorThousand == o.NumberSeperatorThousand &&
                DateFormat.Equals(o.DateFormat)) 
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", Delimiter, NumberSeperatorDigital, NumberSeperatorThousand, DateFormat);
        }
    }
}
