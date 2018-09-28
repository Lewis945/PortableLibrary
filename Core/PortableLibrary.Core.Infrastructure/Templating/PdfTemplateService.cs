using System;
using System.Runtime.InteropServices;
using DinkToPdf;

namespace PortableLibrary.Core.Infrastructure.Templating
{
    public class PdfTemplateService
    {
        #region Fields

        private readonly PdfSettings _settings;

        #endregion

        #region .ctor

        /// <summary>
        /// https://github.com/rdvojmoc/DinkToPdf
        /// </summary>
        public PdfTemplateService(PdfSettings settings)
        {
            _settings = settings;
        }

        #endregion

        #region Public Methods

        public byte[] GeneratePdf(string title, string htmlContent)
        {
            using (var pdfTools = new PdfTools())
            {
                var converter = new SynchronizedConverter(pdfTools);

                var doc = new HtmlToPdfDocument
                {
                    GlobalSettings = new GlobalSettings
                    {
                        DocumentTitle = title,
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                        Margins = new MarginSettings
                        {
                            Top = 6,
                            Bottom = 6
                        }
                    },
                    Objects =
                {
                    new ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = new WebSettings
                        {
                            DefaultEncoding = "utf-8"
                        },
                        HeaderSettings = new HeaderSettings
                        {
                            FontSize = 8,
                            Line = true,
                            Spacing = 2,
                            Left = _settings.HeaderLeftContent,
                            Right = _settings.HeaderRightContent
                        },
                        FooterSettings = new FooterSettings
                        {
                            FontSize = 8,
                            Line = true,
                            Spacing = 2,
                            Center = _settings.FooterCenterContent
                        }
                    }
                }
                };

                byte[] pdf = converter.Convert(doc);
                return pdf;
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}