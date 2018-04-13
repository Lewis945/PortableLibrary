using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Infrastructure.External.Models.TvShow;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://newstudio.tv/
    /// </summary>
    public class NewStudioExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "http://newstudio.tv/";
        public override string ServiceName => "NewStudio";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public NewStudioExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion

        public async Task<NewStudioTvShowModel> ExtractTvShow(string uri)
        {
            var model = new NewStudioTvShowModel();

            var win1251 = Encoding.GetEncoding("windows-1251");
            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri, win1251));

            var divAccordionInner = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("accordion-inner"));

            var divsTopic = divAccordionInner?.SelectNodes(".//div")?.Where(n => n.HasClass("topic_id"));

            if (divsTopic == null)
                return model;

            var titleRegex =
                new Regex(
                    @"^(?<title>[\w\s\d_+!@-]+)\s\((?<season>[\w\s\d]+),(?<episode>[\w\s\d]+)\)\s/\s(?<engtitle>[\w\s\d_+!@-]+)\s.*$");

            foreach (var divTopic in divsTopic)
            {
                var aTopic = divTopic.SelectNodes(".//a")?.FirstOrDefault(n => n.HasClass("torTopic"));
                if (aTopic != null)
                {
                    //Шерлок Холмс (Сезон 1, Серия 1) / Sherlock (2010) BDRip | Первый
                    var text = aTopic.InnerText.ClearString();
                    var match = titleRegex.Match(text);
                }

                var divLastPost = divTopic.SelectNodes(".//div")?.FirstOrDefault(n => n.HasClass("lastpostt"));
                if (divLastPost != null)
                {
                    //2013-11-25 16:11
                    var text = divLastPost.InnerText.ClearString();
                    DateTime.TryParse(text, out var date);
                    var t = date;
                }
            }

            return model;
        }
    }
}