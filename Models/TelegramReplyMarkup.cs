using System.Collections.Generic;

namespace control.personal.Models
{
    public class TelegramReplyMarkup
    {
        public List<List<TelegramInlineKeyboard>> inline_keyboard { get; set; }
    }
}