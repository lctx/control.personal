using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace control.personal.Services
{
    public class TelegramService
    {
        public HttpClient Client { get; }
        public TelegramService(HttpClient client)
        {
            //client.BaseAddress = new Uri("http://api.telegram.org/bot1813881217:AAEktg5iiCsUBJ0SNZy6TpTQ_9D27n3BeKc");
            Client = client;
            //$"http://api.telegram.org/bot1813881217:AAEktg5iiCsUBJ0SNZy6TpTQ_9D27n3BeKc/sendMessage?chat_id=-500625102&text=La%20cámara%20{log.Camara}%20recibió%20un%20log%20de%20tipo%20{log.Level}%20que%20dice%20{log.Body}"
        }
        public async Task<string> SendText(string Message){
            var result=await Client.PostAsync(Client.BaseAddress+"/sendMessage?chat_id=-500625102&text="+Regex.Replace(Message,@"s","%20"),null);
            return result.StatusCode.ToString();
        }
    }
}