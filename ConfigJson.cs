using Newtonsoft.Json;

namespace Discord
{
    public struct ConfigJson // Параметры конфигурации клиента
    {
        [JsonProperty("token")] // Токен бота для авторизации
        public string Token { get; private set; }

        [JsonProperty("prefix")] // Символ для начала ввода команды
        public string Prefix { get; private set; }
    }
}
