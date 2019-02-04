using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCalendarTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            program.Run();
        }

        private void Run()
        {
            // OAuth 2.0 クライアント ID の json ファイルのパス
            var filePath = "client_id.json";
            var scopes = new[] { CalendarService.Scope.CalendarReadonly };

            // 通信設定
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)
                ).Result;

                // イベントの取得期間の指定
                var min = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                var max = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

                // メインコード
                var events = GetCalendarEvents(credential, "*****@gmail.com", min, max);

                if ((events?.Count() ?? 0) > 0)
                {
                    Console.WriteLine("今日の予定を表示します。");

                    foreach (var eventItem in events)
                    {
                        var whenText = eventItem.Start.DateTime.ToString();

                        if (string.IsNullOrEmpty(whenText))
                        {
                            whenText = eventItem.Start.Date;
                        }

                        Console.WriteLine($"{eventItem.Summary} - ({whenText})");
                    }
                }
                else
                {
                    Console.WriteLine("今日の予定は見つかりませんでした。");
                }

            }
        }

        /// <summary>
        /// 指定した期間のカレンダーのイベントのコレクションを取得します。
        /// </summary>
        /// <param name="credential">google 認証。</param>
        /// <param name="calendarId">google カレンダー ID を表すテキスト。</param>
        /// <param name="min">取得期間の開始。</param>
        /// <param name="max">取得期間の終了。</param>
        /// <param name="maxResults">最大の取得件数。</param>
        /// <returns>カレンダーのイベントのコレクション。取得に失敗したときは null を返却します。</returns>
        private IEnumerable<Google.Apis.Calendar.v3.Data.Event> GetCalendarEvents(ICredential credential, string calendarId, DateTime min, DateTime max, int maxResults = 10)
        {
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Calendar Sample",
            });

            EventsResource.ListRequest request = service.Events.List(calendarId);

            request.TimeMin = min;
            request.TimeMax = max;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = maxResults;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            var events = request.Execute()?.Items;

            return events;
        }


    }
}
