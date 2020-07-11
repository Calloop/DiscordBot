using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using DSharpPlus.Entities;
using Discord.Models;

namespace Discord.Commands
{
    public class Commands : BaseCommandModule
    {
        private static readonly string siteUrl = ""; // Сайт с БД

        #region [ КОМАНДЫ ]
        [Command("skillName")]
        [Description("Поиск и вывод способности по названию")]
        public async Task SkillName(CommandContext ctx,
            [RemainingText, Description("Название способности")] string key)
        {
            // Получение записи по названию
            string postRequest = PostRequest(KeyCollection(key, false));
            SkillEntry skillEntry = JsonConvert.DeserializeObject<SkillEntry>(postRequest);

            // Создание рамки с ответом, вывод ответа с записью
            DiscordEmbedBuilder embed = Embed
                (color: 0xFF0000, title: skillEntry.Name, description: skillEntry.Text,
                thumbnailUrl: $"Путь к иконке", footerText: $"ID: {skillEntry.Id}");

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
        }

        [Command("skillId")]
        [Description("Поиск и вывод способности по ID")]
        public async Task SkillId(CommandContext ctx,
            [Description("ID способности")] string key)
        {
            // Получение записи по ID
            string postRequest = PostRequest(KeyCollection(key, true));
            SkillEntry skillEntry = JsonConvert.DeserializeObject<SkillEntry>(postRequest);

            // Создание рамки с ответом, вывод ответа с записью
            DiscordEmbedBuilder embed = Embed
                (color: 0xFF0000, title: skillEntry.Name, description: skillEntry.Text,
                thumbnailUrl: $"Путь к иконке", footerText: $"ID: {skillEntry.Id}");

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
        }
        #endregion

        #region [ МЕТОДЫ ]
        // Создание рамки для ответа
        private DiscordEmbedBuilder Embed(int color, string title = null, string description = null,
                                          string thumbnailUrl = null, string footerText = null,
                                          string footerIconUrl = null)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                ThumbnailUrl = thumbnailUrl,
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = footerText,
                    IconUrl = footerIconUrl
                },
                Color = new DiscordColor(color)
            };

            return embed;
        }

        // Выбор ключа запроса (по ID или по названию)
        private NameValueCollection KeyCollection(string key, bool isIdKey)
        {
            NameValueCollection collection = new NameValueCollection();

            if (isIdKey)
            {
                collection.Add("action", "getSkillById");
                collection.Add("id", key);
            }
            else
            {
                collection.Add("action", "getSkillByName");
                collection.Add("name", key);
            }

            return collection;
        }

        // Отправка запроса на сервер
        private string PostRequest(NameValueCollection collection)
        {
            string url = $"{siteUrl}.html"; // Путь к БД на сайте
            byte[] response = null;

            using (WebClient webClient = new WebClient())
            {
                collection.Add("", ""); // Внутренний ключ для доступа к БД
                response = webClient.UploadValues(url, collection);
            }
            return Encoding.UTF8.GetString(response);
        }
        #endregion
    }
}