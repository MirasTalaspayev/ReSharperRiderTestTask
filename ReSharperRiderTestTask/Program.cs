using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace ReSharperRiderTestTask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string InputDir;
            string InputMasks;
            string OutputFile;

            Console.WriteLine("Input directory: ");
            InputDir = Console.ReadLine();
            Console.WriteLine("Input file masks: ");
            InputMasks = Console.ReadLine();
            Console.WriteLine("Input output file: ");
            OutputFile = Console.ReadLine();

            var masks = InputMasks.Split(' ');

            await PrintStatistics(InputDir, masks, OutputFile);
            
        }
        public static async Task PrintStatistics(string inputDir, string[] masks, string outputFile)
        {
            DirectoryParser dp = new DirectoryParser(masks, outputFile);
            await dp.ParseAsync(inputDir); 

            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
                var x = File.Create(outputFile);
                await x.DisposeAsync();
                x.Close();
            }
            using (var sw = File.AppendText(outputFile))
            {
                sw.WriteLine("File Formats: ");
                foreach (var kv in dp.dictFormat)
                {
                    await sw.WriteLineAsync("\t" + kv);
                }
                sw.WriteLine("File Structures: ");
                foreach (var kv in dp.dictStruct)
                {
                    await sw.WriteLineAsync("\t" + kv);
                }
            }
        }
    }
}
