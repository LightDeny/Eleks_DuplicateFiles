using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace DuplicateFile
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Input path: ");
                string input = Console.ReadLine();
                DuplicateFile(input);
            }
            catch
            {
                Console.WriteLine("Invalid directory");
            }
        }

        static void DuplicateFile(string folder)
        {
            DirectoryInfo dir = new DirectoryInfo(folder);
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories).ToList();
            Console.WriteLine("Count: " + fileList.Count());

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var result = fileList.GroupBy(el => el.Length).Where(x => x.Count() > 1);

            foreach (var res in result)
            {
                try
                {
                    var hash = res.GroupBy(el => CalculateMD5(el.FullName)).ToList();

                    foreach (var x in hash)
                    {
                        if (x.Count() > 1)
                        {
                            Console.WriteLine($"File:  {x.First().FullName} --- {x.Count() - 1} Copies");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            sw.Stop();
            Console.WriteLine("\nTime: " + sw.Elapsed);
        }



        public static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

    }
}
