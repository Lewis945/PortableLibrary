using System;
using System.Collections.Generic;
using System.Linq;
using PortableLibrary.Core.Infrastructure.Templating.Table;
using PortableLibrary.Core.SimpleServices.Models;
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

        public byte[] GeneratePdf(List<LibraryListModel> libraries, bool extended = false)
        {
            var model = new LibrariesModel
            {
                Title = "Libraries",
                Table = new TableModel
                {
                    Headers = GetHeaders(extended),
                    Rows = new List<List<string>>()
                }
            };

            foreach (var library in libraries)
            {
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

        private static List<string> GetRow(LibraryListModel library, bool extended)
        {
            return extended
                ? new List<string>
                {
                    library.Title,
                    library.Type.ToString(),
                    library.Items.ToString(),
                    library.Published?.ToString(),
                    library.Favourits?.ToString(),
                    library.Processing?.ToString(),
                    library.Processed?.ToString(),
                    library.Planned?.ToString(),
                    library.AreWaitingToBecomeGlobal?.ToString(),
                    library.Public.ToString()
                }
                : new List<string>
                {
                    library.Title,
                    library.Type.ToString(),
                    library.Items.ToString(),
                    library.Public.ToString()
                };
        }

        #endregion
    }
}