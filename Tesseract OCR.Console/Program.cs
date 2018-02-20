namespace Tesseract_OCR.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                DiagramPdfReader.Current.Run();
                System.Console.ReadKey();
            }
        }
    }
}
