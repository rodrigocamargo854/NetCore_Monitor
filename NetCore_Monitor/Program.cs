using System;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Newtonsoft.Json;

namespace NetCore_Monitor
{
    class Program
    {
        private static FileSystemWatcher _monitorar;

        public static void MonitorarArquivos(string path, string filtro)
        {

            _monitorar = new FileSystemWatcher(path, filtro)
            {
                IncludeSubdirectories = true
            };

            _monitorar.Changed += OnFileChanged;
            _monitorar.EnableRaisingEvents = true;

            Console.WriteLine($"Convertendo arquivo {filtro} em {path}");


        }
        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"O Arquivo {e.Name} {e.ChangeType}");
        }
        private static void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"O Arquivo {e.OldName} {e.ChangeType} para {e.Name}");
        }

        static string ExtraiPdf(string nomedoarquivo)
        {

            string result = null;
            PdfReader pdfReader = new PdfReader(nomedoarquivo);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string conteudo = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
                result += conteudo;
            }
            pdfDoc.Close();
            pdfReader.Close();

            return result;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Monitorando o sistema com : FileSystemWatcher");
            string path = @"C:\Users\rodrigo.godoy\OneDrive - Anheuser-Busch InBev\Attachments";
            string file = @"C:\Users\rodrigo.godoy\OneDrive - Anheuser-Busch InBev\Attachments\teste.pdf";
            string filtro = "*.pdf";
            MonitorarArquivos(path, filtro);
            var result2 = ExtraiPdf(file);
            string ans = JsonConvert.SerializeObject(result2, Formatting.Indented);
            Console.WriteLine(result2);

        }

    }

}











