using System;
using System.Collections.Generic;
using System.IO;
using HandlebarsDotNet;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.Templating
{
    public class HtmlTemplateService
    {
        #region Constants

        private const string TableTemplateName =
            "PortableLibrary.Core.Infrastructure.Templating.Table.TableTemplate.html";

        #endregion

        #region Fields

        private Func<object, string> _compiledTemplate;
        private List<Partials> _registeredPartials;

        private string _content;

        #endregion

        #region .ctor

        /// <summary>
        /// https://handlebarsjs.com/
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isPath"></param>
        public HtmlTemplateService(string content, bool isPath = false)
        {
            _content = isPath ? File.ReadAllText(content) : content;
        }

        #endregion

        #region Public Methods

        public bool RegisterPartial(Partials partial)
        {
            if (partial == Partials.Table)
            {
                AddTable();
                _registeredPartials = _registeredPartials ?? new List<Partials>();
                _registeredPartials.Add(partial);
                return true;
            }

            return false;
        }

        public string GetHtml(object model)
        {
            if (!Compile())
                return null;

            return _compiledTemplate(model);
        }

        #endregion

        #region Private Methods

        private static Func<object, string> GenerateHtmlTemplateCreator(string content)
        {
            var template = Handlebars.Compile(content);
            return template;
        }

        private static void AddTable()
        {
            var table = EmbeddedResourcesUtilities.GetEmbeddedResourceContent<HtmlTemplateService>(TableTemplateName);
            Handlebars.RegisterTemplate("table", table);
        }

        private bool Compile()
        {
            if (_compiledTemplate != null || string.IsNullOrWhiteSpace(_content))
                return false;

            _compiledTemplate = GenerateHtmlTemplateCreator(_content);
            _content = null;
            return true;
        }

        #endregion
    }
}