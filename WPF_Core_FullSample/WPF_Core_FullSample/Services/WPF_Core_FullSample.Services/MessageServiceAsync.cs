using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WPF_Core_FullSample.Services.Interfaces;

namespace WPF_Core_FullSample.Services
{
    public class MessageServiceAsync : IMessageServiceAsync
    {
        private readonly HttpClient _HttpClient;

        public MessageServiceAsync(HttpClient httpClient)
        {
            _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async ValueTask<string> GetMessageAsync()
        {
            using var jsonStream = await _HttpClient.GetStreamAsync("https://raw.githubusercontent.com/runceel/mockapi/master/message.json");

            // ２秒遅らせる
            await Task.Delay(6000);

            var result = await JsonSerializer.DeserializeAsync<MessageResult>(
                jsonStream,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

            return result.Message;
        }

        public class MessageResult
        {
            public string Message { get; set; }
        }
    }
}
