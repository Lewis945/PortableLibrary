using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.Templating;
using PortableLibrary.Core.Infrastructure.Tests.Templating.Libraries;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.Tests.Templating
{
    public class PdfServiceTests
    {
        [Fact]
        public async Task Should_Generate_Pdf_File()
        {
            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "Templating", "Libraries");

            path = new DirectoryInfo(path).FullName;

            var templatePath = Path.Combine(path, "LibrariesTemplate.html");
            var pdfOutputPath = Path.Combine(path, "Output");
            if (!Directory.Exists(pdfOutputPath))
                Directory.CreateDirectory(pdfOutputPath);
            var pdfPath = Path.Combine(pdfOutputPath, $"Libraries.{DateTime.UtcNow:dd-MM-yyyy.HH-mm-ss}.pdf");

            var htmlTemplateService = new HtmlTemplateService(templatePath, true);

            var model = new LibrariesModel
            {
                Title = "Libraries",
                Libraries = new List<LibraryModel>
                {
                    new LibraryModel
                    {
                        Title = "Books library",
                        ItemsCount = 100
                    },
                    new LibraryModel
                    {
                        Title = "Tv Shows library",
                        ItemsCount = 50
                    }
                }
            };

            var html = htmlTemplateService.GetHtml(model);

            var pdfTemplateService = new PdfTemplateService();
            pdfTemplateService.GeneratePdf(html, pdfPath);

            var files = Directory.GetFiles(pdfOutputPath);
            Assert.Equal(1, files.Length);

//            if (Directory.Exists(pdfOutputPath))
//                Directory.Delete(pdfOutputPath);
        }
    }
}