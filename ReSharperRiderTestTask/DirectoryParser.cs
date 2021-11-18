using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ReSharperRiderTestTask
{
    class DirectoryParser
    {
        private string[] marks;
        private string output_path;
        public ConcurrentDictionary<FileStructure, int> dictStruct { get; private set; }
        public ConcurrentDictionary<FileFormat, int> dictFormat { get; private set; }

        public DirectoryParser(string[] masks, string output_path)
        {
            this.marks = Masks(masks);
            this.output_path = output_path;
            dictStruct = new ConcurrentDictionary<FileStructure, int>();
            dictFormat = new ConcurrentDictionary<FileFormat, int>();
        }
        private string[] Masks(string[] masks)
        {
            string[] result = new string[masks.Length];
            for (int i = 0; i < masks.Length; i++)
            {
                result[i] = "*" + masks[i] + "*.txt";
            }
            return result;
        }
        public async Task ParseAsync(string path)
        {
            var dir = new DirectoryInfo(path);
            var tasks = new List<Task>();
            var tasks_dir = new List<Task>();
            var files = new HashSet<string>();
            
            foreach(var mark in marks)
            {
                foreach(var file in dir.GetFiles(mark))
                {
                    if (files.Add(file.FullName))
                    {
                        tasks.Add(AddFileStructureAndFileFormat(file.FullName));
                    }
                }
            }
            foreach(var d in dir.GetDirectories()) // ParseAsync Inner Directories
            {
                tasks_dir.Add(ParseAsync(d.FullName));
            }

            await Task.WhenAll(tasks);
            await Task.WhenAll(tasks_dir);
        }
        private async Task AddFileStructureAndFileFormat(string path)
        {
            await Task.Run(() =>
            {
                var fsfs = new FileStructureFormatService(path);
                var fl = new FileFormat();
                var fs = fsfs.GetFileStructure(ref fl);
                if (fs != null)
                {
                    dictStruct.AddOrUpdate(fs, 1, (key, value) => value + 1);
                    dictFormat.AddOrUpdate(fl, 1, (key, value) => value + 1);
                }
            });
        }
    }
}
