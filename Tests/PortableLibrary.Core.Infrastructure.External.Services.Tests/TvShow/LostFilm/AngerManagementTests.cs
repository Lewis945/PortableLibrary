using PortableLibrary.Core.External.Services.TvShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace PortableLibrary.Core.Infrastructure.External.Services.Tests.TvShow.LostFilm
{
    public class AngerManagementTests: LostFilmTestsBase
    {
        [Fact]
        public async Task Should_Extract_Anger_Management()
        {
            var model = await Service.ExtractTvShowAsync("https://www.lostfilm.tv/series/Anger_Management");

            #region Tv Show

            var genres = new List<string>
            {
                "Комедия",
            };

            string testDescription = "Чарли, перед возвращением в стан своей бейсбольной команды прошел курс " +
                                     "управления гневом, прежде чем доказать себе и окружающим, что он настоящий лидер команды. " +
                                     "В результате он приводит команду к победе, после чего покидает ее. Выходит, пока Чарли " +
                                     "борется со своим гневом, в его жизни процветает хаос. Всё осложняется его отношениями с " +
                                     "собственным терапевтом и лучшим другом, бывшей женой, чьи позитивные взгляды на будущее, " +
                                     "но при этом плохой выбор мужчин, расстраивают Чарли и их 13-летнюю дочь, имеющую " +
                                     "психические расстройства.";

            testDescription = Regex.Replace(testDescription, @"\t|\n|\r|\s", string.Empty);

            ValidateTvShow(model, title: "Управление гневом",
                originalTitle: "Anger Management",
                imageUri: "static.lostfilm.tv/Images/172/Posters/poster.jpg",
                status: TvShowStatus.CanceledOrEnded, genres: genres, description: testDescription, seasonsCount: 2);

            #endregion

            #region Season 1

            var season1 = model.Seasons.First(s => s.Index == 1);

            ValidateSeason(season1, 10);

            #region Episode 1

            var s1E1 = season1.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s1E1, title: "Чарли снова проходит терапию", originalTitle: "Charlie Goes Back to Therapy",
                dateReleased: new DateTime(2012, 7, 2, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2012, 6, 28, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 5

            var s1E5 = season1.Episodes.First(e => e.Index == 5);

            ValidateEpisode(s1E5, title: "Чарли доказывает, что терапия — штука честная",
                originalTitle: "Charlie Tries to Prove Therapy Is Legit",
                dateReleased: new DateTime(2012, 7, 22, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2012, 7, 19, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 10

            var s1E10 = season1.Episodes.First(e => e.Index == 10);

            ValidateEpisode(s1E10, title: "Чарли потянуло на романтику", originalTitle: "Charlie Gets Romantic",
                dateReleased: new DateTime(2012, 8, 25, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2012, 8, 23, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion

            #region Season 2

            var season2 = model.Seasons.First(s => s.Index == 2);

            ValidateSeason(season2, 90);

            #region Episode 1

            var s2E1 = season2.Episodes.First(e => e.Index == 1);

            ValidateEpisode(s2E1, title: "Как Чарли психанул на предрожденчике",
                originalTitle: "Charlie Loses it at a Baby Shower",
                dateReleased: new DateTime(2013, 1, 21, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2013, 1, 17, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 50

            var s2E50 = season2.Episodes.First(e => e.Index == 50);

            ValidateEpisode(s2E50, title: "Чарли и Шон соревнуются из-за девушки",
                originalTitle: "Charlie and Sean Fight Over a Girl",
                dateReleased: new DateTime(2014, 3, 4, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2014, 2, 27, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #region Episode 100

            var s2E90 = season2.Episodes.First(e => e.Index == 90);

            ValidateEpisode(s2E90, title: "Чарли и сотая серия", originalTitle: "Charlie & the 100th Episode",
                dateReleased: new DateTime(2015, 1, 19, 0, 0, 0, DateTimeKind.Utc),
                dateReleasedOrigianl: new DateTime(2014, 12, 22, 0, 0, 0, DateTimeKind.Utc));

            #endregion

            #endregion
        }
    }
}
