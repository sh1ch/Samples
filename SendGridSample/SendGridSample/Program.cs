using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Mail;
using SendGrid.Helpers.Mail;
using System.Configuration;
using SendGrid;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace SendGridSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("-- SendGrid メール送信テストプログラム");
            Console.WriteLine("----------------------------------------");

            program.Run();
        }

        public void Run()
        {
            try
            {
                Console.WriteLine("送信するメールタイトルを入力してください。");
                var subject = Console.ReadLine();

                Console.WriteLine("ユーザーリストを読み込みます。 users.csv");
                var users = GetUsers("assets/users.csv");
                if (users == null) throw new ArgumentNullException("ユーザーリストが正しく読み込めませんでした。");

                Console.WriteLine("本文を読み込みます。 data.txt");
                var body = GetBody("assets/data.txt");
                if (string.IsNullOrEmpty(body)) throw new ArgumentNullException("本文が正しく読み込めませんでした。");

                SendMail(users, subject, body).Wait();

                Console.WriteLine("メールマガジンの配信に成功しました。");
            }
            catch (Exception e)
            {
                Console.WriteLine("メールマガジンの配信に失敗しました。");
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }

        public async Task SendMail(IEnumerable<ToUser> toUsers, string subject, string body)
        {
            var key = ConfigurationManager.AppSettings["API_KEY"];
            var fromAddress = ConfigurationManager.AppSettings["FROM"];
            var fromName = ConfigurationManager.AppSettings["FROM_NAME"];

            var message = new SendGridMessage();
            var client = new SendGridClient(key);
            var fromUser = new EmailAddress(fromAddress, fromName);

            message.SetFrom(fromUser);
            message.SetGlobalSubject(subject);

            // HTML 形式
            message.AddContent(MimeType.Html, body);

            var i = 0;
            foreach (var toUser in toUsers)
            {
                var personInfo = new Personalization();
                personInfo.Substitutions = new Dictionary<string, string>();

                personInfo.Substitutions.Add("%name%", toUser.Name);
                personInfo.Substitutions.Add("%company%", toUser.Company);

                message.AddTo(toUser.GetEmailAddress(), i++, personInfo);
            }

            var response = await client.SendEmailAsync(message);
        }

        private string GetBody(string filePath)
        {
            var text = "";

            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }

        private IEnumerable<ToUser> GetUsers(string filePath, string separator = ",")
        {
            var userTexts = GetUserTexts(filePath, separator);

            if (userTexts == null || userTexts.Count() <= 0) yield return null;

            foreach (var user in userTexts)
            {
                yield return new ToUser(user[0], user[1], user[2]);
            }
        }

        private IEnumerable<string[]> GetUserTexts(string filePath, string separator = ",")
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var parser = new TextFieldParser(stream, Encoding.UTF8, true, false))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.Delimiters = new[] { separator };
                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.TrimWhiteSpace = true;

                    while (parser.EndOfData == false)
                    {
                        string[] fields = parser.ReadFields();
                        yield return fields;
                    }
                }
            }
        }
    }
}
