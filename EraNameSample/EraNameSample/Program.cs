using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EraNameSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            foreach (var era in program.GetEraNames())
            {
                Console.WriteLine($"{era}");
            }

            foreach (var era in program.GetAbbreviatedEraNames())
            {
                Console.WriteLine($"{era}");
            }

            Console.WriteLine("----");
            
            while (true)
            {
                Console.Write("元号を調べる年月日を入力してください。(入力例 2019-05-01):");
                var inputText = Console.ReadLine();

                if (inputText == "exit") break;

                DateTimeOffset datetime;
                if (DateTimeOffset.TryParseExact(inputText, "yyyy-MM-dd", null, DateTimeStyles.AssumeLocal, out datetime))
                {
                    var eraName = program.GetEra(datetime);
                    Console.WriteLine($"{inputText} の元号は {eraName} です。");
                }
                else
                {
                    Console.WriteLine($"{inputText} は、正しい年月日として認識できませんでした。");
                }

                Console.WriteLine("");
            }
            
        }

        /// <summary>
        /// 指定した <see cref="DateTimeOffset"/> の元号を表すテキストを取得します。
        /// </summary>
        /// <param name="datetime">元号を調べる時点。</param>
        /// <returns>元号を表すテキスト。</returns>
        public string GetEra(DateTimeOffset datetime)
        {
            var cultureInfo = new CultureInfo("ja-JP", true);
            cultureInfo.DateTimeFormat.Calendar = new JapaneseCalendar();

            return datetime.ToString("gg", cultureInfo);
        }

        /// <summary>
        /// レジストリに登録された元号の列挙子を取得します。
        /// </summary>
        /// <returns>元号のコレクション。</returns>
        public IEnumerable<string> GetEraNames()
        {
            var cultureInfo = new CultureInfo("ja-JP", true);
            cultureInfo.DateTimeFormat.Calendar = new JapaneseCalendar();

            var eras = cultureInfo.DateTimeFormat.Calendar.Eras;
            var eraNames = new List<string>();

            foreach (var era in eras)
            {
                eraNames.Add(cultureInfo.DateTimeFormat.GetEraName(era));
            }

            return eraNames;
        }

        /// <summary>
        /// レジストリに登録された元号 (省略形) の列挙子を取得します。
        /// </summary>
        /// <returns>元号 (省略形) のコレクション。</returns>
        public IEnumerable<string> GetAbbreviatedEraNames()
        {
            var cultureInfo = new CultureInfo("ja-JP", true);
            cultureInfo.DateTimeFormat.Calendar = new JapaneseCalendar();

            var eras = cultureInfo.DateTimeFormat.Calendar.Eras;
            var eraNames = new List<string>();

            foreach (var era in eras)
            {
                eraNames.Add(cultureInfo.DateTimeFormat.GetAbbreviatedEraName(era));
            }

            return eraNames;
        }

    }
}
