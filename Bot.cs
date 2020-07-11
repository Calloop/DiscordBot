using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using System.IO;
using Newtonsoft.Json;

namespace Discord
{
    class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            // Загрузка данных для конфигурации клиента
            string json = string.Empty;

            using (FileStream fileStream = File.OpenRead("config.json"))
            using (StreamReader streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
                json = await streamReader.ReadToEndAsync().ConfigureAwait(false);

            ConfigJson configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            // Конфигурация клиента
            DiscordConfiguration config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            Client = new DiscordClient(config);
            Client.Ready += OnClientReady;

            // Конфигурация команд
            CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<Commands.Commands>();

            // Подключение к клиенту
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
