using System.Text.Json.Serialization;

namespace control.personal.Models
{
    public class TelegramBodyMessaje
    {
        public int chat_id { get; set; }
        public string text { get; set; }
        public string parse_mode { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]

        public TelegramReplyMarkup reply_markup { get; set; }
    }
}