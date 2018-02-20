using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using Tesseract;

namespace Tesseract_OCR.Console
{
    public class DiagramPdfReader
    {
        private static DiagramPdfReader _current;
        private TesseractEngine _tesseractEngine;

        DiagramPdfReader()
        {
            var datapath = ConfigurationManager.AppSettings["DataPath"];
            _tesseractEngine = new TesseractEngine(datapath, "eng");
        }

        public static DiagramPdfReader Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new DiagramPdfReader();
                }

                return _current;
            }
            set { _current = value; }
        }

        public void Run()
        {
            var directory = ConfigurationManager.AppSettings["ImageFolderPath"];
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                if (string.IsNullOrEmpty(file))
                {
                    continue;
                }

                System.Console.WriteLine($"[FILE] - {Path.GetFileName(file)}\n//.._.._.._.._.._.._.._.._.._.._.._.._.._.._.._..");
                //ParseRect(file);
                Parse(file);
                System.Console.ReadLine();
            }

/*
            var path = ConfigurationManager.AppSettings["ImagePath"];
            Parse(path);
*/
        }

        public void Parse(string path)
        {
            try
            {
                // now add the following C# line in the code page  
                using (var image = new Bitmap(path))
                {
                    foreach (var mode in PageSegModes)
                    {
                        try
                        {
                            System.Console.WriteLine($"[{mode.ToString().ToUpper()}]:BEGIN\n//-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-");
                            var size = image.Size;
                            using (var page = _tesseractEngine.Process(image, mode))
                            {
                                var text = page?.GetText();
                                System.Console.WriteLine(text);
                            }
                            System.Console.WriteLine($"[{mode}]:END\n//-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-");
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("Error:");
                            System.Console.WriteLine(ex);
                            System.Console.WriteLine();
                            System.Console.WriteLine(ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error:");
                System.Console.WriteLine(ex);
                System.Console.WriteLine();
                System.Console.WriteLine(ex.Message);
            }

            System.Console.ReadKey();
        }
/*

        public void ParseRect(string path)
        {
            try
            {
                // now add the following C# line in the code page  
                using (var image = new Bitmap(path))
                {
                    foreach (var mode in PageSegModes)
                    {
                        try
                        {
                            System.Console.WriteLine($"[{mode}]:BEGIN\n//-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-");
                            var size = image.Size;
                            using (var page = _tesseractEngine.Process(image, new Rect(1770, 1512, 403, 64), mode))
                            {
                                var text = page?.GetText();
                                System.Console.WriteLine(text);
                            }
                            System.Console.WriteLine($"[{mode}]:END\n//-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-");
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("Error:");
                            System.Console.WriteLine(ex);
                            System.Console.WriteLine();
                            System.Console.WriteLine(ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error:");
                System.Console.WriteLine(ex);
                System.Console.WriteLine();
                System.Console.WriteLine(ex.Message);
            }

            System.Console.ReadKey();
        }
*/

        private List<PageSegMode> _pageSegModes;
        public List<PageSegMode> PageSegModes
        {
            get
            {
                return _pageSegModes ?? (_pageSegModes = new List<PageSegMode>()
                {
                    //PageSegMode.OsdOnly,
                    //PageSegMode.AutoOsd,
                    PageSegMode.AutoOnly,
                    PageSegMode.Auto,
                    PageSegMode.SingleColumn,
                    //PageSegMode.SingleBlockVertText,
                    PageSegMode.SingleBlock,
                    PageSegMode.SingleLine,
                    //PageSegMode.SingleWord,
                    //PageSegMode.CircleWord,
                    //PageSegMode.SingleChar,
                    PageSegMode.Count,
                });
            }
        }
    }
}