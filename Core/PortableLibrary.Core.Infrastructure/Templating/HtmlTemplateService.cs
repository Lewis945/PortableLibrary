using System;
using System.IO;
using HandlebarsDotNet;

namespace PortableLibrary.Core.Infrastructure.Templating
{
    public class HtmlTemplateService
    {
        #region Fields

        private readonly Func<object, string> _compiledTemplate;

        #endregion

        #region .ctor

        /// <summary>
        /// https://handlebarsjs.com/
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isPath"></param>
        public HtmlTemplateService(string content, bool isPath = false)
        {
            if (isPath)
                content = File.ReadAllText(content);

            _compiledTemplate = GenerateHtmlTemplateCreator(content);
        }

        #endregion

        #region Public Methods

        public string GetHtml(object model)
        {
            return _compiledTemplate(model);
        }

        #endregion

        #region Private Methods

        private Func<object, string> GenerateHtmlTemplateCreator(string content)
        {
            var template = Handlebars.Compile(content);
            return template;
        }

        #endregion
    }
}