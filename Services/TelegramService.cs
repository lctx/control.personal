using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using control.personal.Models;

namespace control.personal.Services
{
    public class TelegramService
    {
        public HttpClient Client { get; }
        public TelegramService(HttpClient client)
        {
            //client.BaseAddress = new Uri("http://api.telegram.org/bot1813881217:AAEktg5iiCsUBJ0SNZy6TpTQ_9D27n3BeKc");
            Client = client;
        }
        public async Task<string> SendMessage(string Message)
        {
            var messageBody = new StringContent(
                JsonSerializer.Serialize(
                    new TelegramBodyMessaje()
                    {
                        chat_id = -500625102,
                        text = Message,
                        parse_mode = "Markdown",
                    }, new JsonSerializerOptions(){}),
                Encoding.UTF8,
                "application/json"
            );
            Console.WriteLine(messageBody.ReadAsStringAsync().Result);
            var result = await Client.PostAsync(Client.BaseAddress + "/sendMessage", messageBody);
            Console.WriteLine("-------------- \n" + result.Content.ReadAsStringAsync().Result);
            return result.StatusCode.ToString();
        }

        public async Task<string> SendMessage(TelegramBodyMessaje Message)
        {
            //telegram no acepta como url valida localhost, por eso debo cambiar a 127.0.0.1
            var messageBody = new StringContent(
                JsonSerializer.Serialize(
                    Message),
                Encoding.UTF8,
                "application/json"
            );
            var result = await Client.PostAsync(Client.BaseAddress + "/sendMessage", messageBody);
            return result.StatusCode.ToString();
        }
    }

}