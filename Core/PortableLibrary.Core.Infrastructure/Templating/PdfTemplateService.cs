using System;
using System.IO;
using System.Threading.Tasks;
using DinkToPdf;
using HandlebarsDotNet;

namespace PortableLibrary.Core.Infrastructure.Templating
{
    public class PdfTemplateService
    {
        #region Fields

        

        #endregion
        
        #region .ctor

        /// <summary>
        /// https://github.com/rdvojmoc/DinkToPdf
        /// </summary>
        public PdfTemplateService()
        {
            
        }

        #endregion

        #region Public Methods

        public void GeneratePdf(string htmlContent, string outputPath)
        {
            var converter = new SynchronizedConverter(new PdfTools());
            
            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 }
                },
                Objects = {
                    new ObjectSettings {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };
            
//            converter.Convert(doc);
            
            byte[] pdf = converter.Convert(doc);
            
            File.WriteAllBytes(outputPath, pdf);
        }

        #endregion
        
        #region Private Methods

        

        #endregion
    }
}