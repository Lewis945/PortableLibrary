using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PortableLibrary.Core.Database.Entities.Base;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Infrastructure.Templating.Libraries;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.Tests.Templating
{
    public class LibrariesPdfServiceTests
    {
        [Fact]
        public void Should_Generate_Libraries_Pdf()
        {
            var libraries = new List<BaseLibrary>
            {
                FeelLibrary(new BooksLibrary
                {
                    Name = "Books library",
                    Books = Enumerable.Range(0, 50).Select(n => new LibraryBook
                    {
                        Name = $"Book {n}"
                    }).ToList()
                }),
                FeelLibrary(new TvShowsLibrary
                {
                    Name = "TvShows library",
                    TvShows = Enumerable.Range(0, 100).Select(n => new LibraryTvShow
                    {
                        Name = $"Tv show {n}"
                    }).ToList()
                })
            };

            var librariesPdfService = new LibrariesPdfService();

            var bytes = librariesPdfService.GeneratePdf(libraries, false);

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

        private static BaseLibrary FeelLibrary(BaseLibrary library)
        {
            var random = new Random();

            if (library is BooksLibrary booksLibrary)
                Feel(booksLibrary.Books.Cast<BaseLibraryEntity>().ToList());
            else if (library is TvShowsLibrary tvShowsLibrary)
                Feel(tvShowsLibrary.TvShows.Cast<BaseLibraryEntity>().ToList());

            void Feel(List<BaseLibraryEntity> entities)
            {
                foreach (var entity in entities)
                {
                    entity.IsPublished = NextBool(random);
                    entity.IsFavourite = NextBool(random);
                    entity.IsProcessed = NextBool(random);
                    entity.IsProcessing = NextBool(random);
                    entity.IsProcessingPlanned = NextBool(random);
                    entity.IsWaitingToBecomeGlobal = NextBool(random);
                }
            }

            return library;
        }

        private static bool NextBool(Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100.0;
        }
    }
}