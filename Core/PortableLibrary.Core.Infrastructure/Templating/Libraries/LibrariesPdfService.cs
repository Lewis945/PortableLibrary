using System;
using System.Collections.Generic;
using PortableLibrary.Core.Database.Entities.Base;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Infrastructure.Templating.Table;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.Templating.Libraries
{
    public class LibrariesPdfService
    {
        #region Fields

        private const string LibrariesTemplateName =
            "PortableLibrary.Core.Infrastructure.Templating.Libraries.LibrariesTemplate.html";

        private readonly HtmlTemplateService _htmlTemplateService;
        private readonly PdfTemplateService _pdfTemplateService;

        #endregion

        #region .ctor

        /// <summary>
        /// Make it multi language
        /// </summary>
        public LibrariesPdfService()
        {
            _htmlTemplateService = new HtmlTemplateService(
                EmbeddedResourcesUtilities.GetEmbeddedResourceContent<LibrariesPdfService>(LibrariesTemplateName));
            _htmlTemplateService.RegisterPartial(Partials.Table);

            _pdfTemplateService = new PdfTemplateService(new PdfSettings
            {
                HeaderLeftContent = "Portable Library",
                HeaderRightContent = "Page [page] of [toPage]",
                FooterCenterContent = $"Copyright © {DateTime.UtcNow.Year} Portable Library"
            });
        }

        #endregion

        #region Public Methods

        public byte[] GeneratePdf(List<BaseLibrary> libraries, bool extended = false)
        {
            var model = new LibrariesModel
            {
                Title = "Libraries",
                Libraries = new List<LibraryModel>(),
                Table = new TableModel
                {
                    Headers = GetHeaders(extended),
                    Rows = new List<List<string>>()
                }
            };

            foreach (var library in libraries)
            {
                model.Libraries.Add(new LibraryModel
                {
                    Title = library.Name,
                });

                model.Table.Rows.Add(GetRow(library, extended));
            }

            var html = _htmlTemplateService.GetHtml(model);
            var bytes = _pdfTemplateService.GeneratePdf(model.Title, html);

            return bytes;
        }

        #endregion

        #region Private Methods

        private static List<string> GetHeaders(bool extended)
        {
            return extended
                ? new List<string>
                {
                    "Title",
                    "Type",
                    "Items",
                    "Published",
                    "Favourits",
                    "Processing",
                    "Processed",
                    "Planned",
                    "To global",
                    "Public"
                }
                : new List<string>
                {
                    "Title",
                    "Type",
                    "Items",
                    "Public"
                };
        }

        private static List<string> GetRow(BaseLibrary library, bool extended)
        {
            if (!extended)
                return new List<string>
                {
                    library.Name,
                    library.GetLibraryType().ToString(),
                    library.GetItemsCount().ToString(),
                    library.IsPublic ? "Yes" : "No"
                };

            var counts = library.GetCountByFlags();
            return new List<string>
            {
                library.Name,
                library.GetLibraryType().ToString(),
                counts.All.ToString(),
                counts.IsPublished.ToString(),
                counts.IsFavourite.ToString(),
                counts.IsProcessing.ToString(),
                counts.IsProcessed.ToString(),
                counts.IsProcessingPlanned.ToString(),
                counts.IsWaitingToBecomeGlobal.ToString(),
                library.IsPublic ? "Yes" : "No"
            };
        }

        #endregion
    }
}