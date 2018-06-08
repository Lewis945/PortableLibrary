using System;
using System.Collections.Generic;
using System.IO;
using PortableLibrary.Core.Enums;
using PortableLibrary.Core.Infrastructure.Templating.Libraries;
using PortableLibrary.Core.SimpleServices.Models;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.Tests.Templating
{
    public class LibrariesPdfServiceTests
    {
        [Fact]
        public void Should_Generate_Libraries_Pdf()
        {
            var libraries = new List<LibraryListModel>
            {
                FeelLibrary(new LibraryListModel
                {
                    Title = "Books library",
                    Type = LibraryType.Book,
                    Public = true
                }),
                FeelLibrary(new LibraryListModel
                {
                    Title = "TvShows library",
                    Type = LibraryType.TvShow
                })
            };

            var librariesPdfService = new LibrariesPdfService();

            var bytes = librariesPdfService.GeneratePdf(libraries, true);

            Assert.True(bytes.Length > 1000);

            #region Create file test

            var path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "Templating", "Output");

            path = new DirectoryInfo(path).FullName;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var pdfPath = Path.Combine(path, $"Libraries.{DateTime.UtcNow:dd-MM-yyyy.HH-mm-ss}.pdf");
            // var pdfPath = Path.Combine(path, $"Libraries.pdf");

            File.WriteAllBytes(pdfPath, bytes);

            #endregion
        }

        private static LibraryListModel FeelLibrary(LibraryListModel library)
        {
            var random = new Random();

            library.Items = random.Next(100);
            library.Published = random.Next(library.Items);
            library.Favourits = random.Next(library.Items);
            library.Processed = random.Next(library.Items);
            library.Processing = random.Next(library.Items);
            library.Planned = random.Next(library.Items);
            library.AreWaitingToBecomeGlobal = random.Next(library.Items);

            return library;
        }
    }
}