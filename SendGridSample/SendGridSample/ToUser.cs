using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendGridSample
{
    /// <summary>
    /// <see cref="ToUser"/> クラスは、送信先ユーザーを表現するクラスです。
    /// </summary>
    [DebuggerDisplay("Name={Name}, Company={Company}, Email={Email}")]
    public class ToUser
    {
        #region Properties

        /// <summary>
        /// メールアドレスを取得または設定します。
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 名前を取得または設定します。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 社名を取得または設定します。
        /// </summary>
        public string Company { get; set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="ToUser"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">名前。</param>
        /// <param name="company">社名。</param>
        /// <param name="email">メールアドレス。</param>
        public ToUser(string name, string company, string email)
        {
            Name = name;
            Company = company;
            Email = email;
        }

        #endregion

        #region Public Methods

        public EmailAddress GetEmailAddress()
        {
            return new EmailAddress(Email, Name);
        }

        #endregion
    }
}
