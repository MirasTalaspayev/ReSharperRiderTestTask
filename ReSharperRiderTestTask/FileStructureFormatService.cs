using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ReSharperRiderTestTask
{
    class FileStructureFormatService
    {
        private string[][] dataTable;
        private char Delimiter;
        public FileStructureFormatService(string path)
        {
            FileParser fileParser = new FileParser();
            dataTable = fileParser.Parse(path);
            Delimiter = fileParser.Delimiter;
        }
        public FileStructure GetFileStructure(ref FileFormat fl)
        {
            if (dataTable == null)
            {
                fl = null;
                return null;
            }
            FileStructure fileStructure = new FileStructure();
            fl.Delimiter = Delimiter;

            for (int i = 0; i < dataTable[1].Length; i++)
            {
                object obj;
                fileStructure.Column_DataType[dataTable[0][i]] = DataType(dataTable[1][i], i, out obj);
                if (fileStructure.Column_DataType[dataTable[0][i]] == nameof(Int32))
                {
                    var x = obj as char[];
                    fl.NumberSeperatorThousand = x[0];
                    fl.NumberSeperatorDigital = x[1];
                }
                else if (fileStructure.Column_DataType[dataTable[0][i]] == nameof(DateTime))
                {
                    fl.DateFormat = obj as string;
                }
            }
            return fileStructure;
        }
        public string DataType(string s, int column, out object obj)
        {
            obj = null;
            Match match = Regex.Match(s, "((\\d{2}(/|.)\\d{2}(/|.)\\d{4})|(\\d{4}(/|.)\\d{2}(/|.)\\d{2}))"); // dates
            if (match.Success)
            {
                if (match.Groups[2].Success)
                {
                    obj = DateFormat(column);
                }
                else if (match.Groups[5].Success)
                {
                    obj = match.Groups[5].Value;
                }
                return nameof(DateTime);
            }
            match = Regex.Match(s, "^\\d{1,3}(( \\d{3})*([\\.,]\\d+)?|(,\\d{3})*(\\.\\d+)?)$"); // numbers
            if (match.Success)
            {
                char[] values = new char[2];
                if (match.Groups[2].Success)
                {
                    values[0] = match.Groups[2].Value[0]; // thousand
                }
                if (match.Groups[3].Success)
                {
                    values[1] = match.Groups[2].Value[0]; // float delimiter
                }
                if (match.Groups[4].Success)
                {
                    values[0] = match.Groups[4].Value[0]; // thousand
                }
                if (match.Groups[5].Success)
                {
                    values[1] = match.Groups[5].Value[0]; // float delimiter
                }
                obj = values;
                return nameof(Int32);
            }
            match = Regex.Match(s, "\\w+"); // strings
            if (match.Success)
            {
                return nameof(String);
            }
            return null;
        }
        private string DateFormat(int column)
        {
            string date = "";
            for (int i = 1; i < dataTable.Length; i++)
            {
                date = dataTable[i][column];
                var numbers = date.Split(date[2]);
                if (numbers[0].CompareTo("12") == 1)
                {
                    return String.Format("DD{0}MM{0}YYYY", date[2]);
                }
                else if (numbers[1].CompareTo("12") == 1)
                {
                    return String.Format("MM{0}DD{0}YYYY", date[2]);
                }
            }
            return String.Format("DD{0}MM{0}YYYY", date[2]);
        }
    }
}
