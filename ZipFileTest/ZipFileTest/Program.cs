using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            program.Run();
        }

        public void Run()
        {
            Sample1();
            Sample2();
        }

        private void Sample1()
        {
            var filename = "data.zip";
            var files = EnumerateFiles(filename);

            Console.WriteLine("--");

            Console.WriteLine(ReadToEnd(filename, "data/data1.txt"));

            Console.WriteLine("--");

            var lines = ReadLine(filename, "data/data2.txt");
            foreach (var line in lines) Console.WriteLine(line);

            Console.WriteLine("--");

            if (Exists(filename, "data/data3.txt"))
            {
                Delete(filename, "data/data3.txt");
            }

            WriteLine(filename, "data/data3.txt", $"testdata1 {DateTime.Now}", true);
            WriteLine(filename, "data/data3.txt", $"testdata2 {DateTime.Now}", true);
        }

        private void Sample2()
        {
            var simple = new SimpleZip("simpledata.zip");

            simple.WriteLine("data1.log", $"testdata1 {DateTime.Now}", true);
            simple.WriteLine("data1.log", $"testdata2 {DateTime.Now}", true);
            simple.WriteLine("data1.log", $"testdata3 {DateTime.Now}", true);

            simple.MakeDirectory(@"data");
        }

        #region Zip Sample

        private void Create(string zipPath)
        {
            if (File.Exists(zipPath))
            {
                throw new System.IO.IOException("すでに同じ名前のファイル、または、フォルダーが存在しています。");
            }

            var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            zip.Dispose();
        }

        private void Write(string zipPath, string name, string text, bool overwrite = false)
        {
            var beforeText = "";

            if (Exists(zipPath, name))
            {
                if (overwrite == false) throw new System.IO.IOException("すでに同じ名前のファイル、または、フォルダーが存在しています。");

                beforeText = ReadToEnd(zipPath, name);

                Delete(zipPath, name);
            }

            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                var newFile = zip.CreateEntry(name);

                using (var writer = new StreamWriter(newFile.Open(), System.Text.Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(beforeText))
                    {
                        writer.Write(beforeText);
                    }

                    writer.Write(text);
                }
            }
        }

        private void WriteLine(string zipPath, string name, string text, bool overwrite = false)
        {
            var beforeText = "";

            if (Exists(zipPath, name))
            {
                if (overwrite == false) throw new System.IO.IOException();

                beforeText = ReadToEnd(zipPath, name);

                Delete(zipPath, name);
            }

            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                var newFile = zip.CreateEntry(name);

                using (var writer = new StreamWriter(newFile.Open(), System.Text.Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(beforeText)) 
                    {
                        writer.Write(beforeText); 
                    }

                    writer.WriteLine(text);
                }
            }
        }

        private IEnumerable<string> EnumerateFiles(string zipPath)
        {
            var files = new List<string>();

            using (var zip = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in zip.Entries)
                {
                    files.Add(entry.FullName);
                    Console.WriteLine(entry.FullName);
                }
            }

            return files;
        }

        private string ReadToEnd(string zipPath, string name)
        {
            using (var zip = ZipFile.OpenRead(zipPath))
            {
                var selectedFile = zip.Entries.FirstOrDefault(p => p.FullName == name);

                if (selectedFile == null) throw new System.IO.FileNotFoundException();

                using (var reader = new StreamReader(selectedFile.Open()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private IEnumerable<string> ReadLine(string zipPath, string name)
        {
            var lines = new List<string>();

            using (var zip = ZipFile.OpenRead(zipPath))
            {
                var selectedFile = zip.Entries.FirstOrDefault(p => p.FullName == name);

                if (selectedFile == null) throw new System.IO.FileNotFoundException();

                using (var reader = new StreamReader(selectedFile.Open()))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }

            return lines;
        }

        private bool Exists(string zipPath, string name)
        {
            using (var zip = ZipFile.OpenRead(zipPath))
            {
                var selectedFile = zip.Entries.FirstOrDefault(p => p.FullName == name);

                return selectedFile != null;
            }
        }

        private void Delete(string zipPath, string name)
        {
            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                var selectedFile = zip.Entries.FirstOrDefault(p => p.FullName == name);

                if (selectedFile == null) throw new System.IO.FileNotFoundException();

                selectedFile.Delete();
            }
        }

        #endregion

    }
}
