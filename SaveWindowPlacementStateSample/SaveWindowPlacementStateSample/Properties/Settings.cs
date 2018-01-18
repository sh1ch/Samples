using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveWindowPlacementStateSample.Properties
{
    /// <summary>
    /// <see cref="Settings"/> クラスは、アプリケーション設定を管理することができます。
    /// </summary>
    internal sealed partial class Settings
    {
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public WINDOWPLACEMENT? WindowPlacement
        {
            get
            {
                return ((WINDOWPLACEMENT?)(this["WindowPlacement"]));
            }
            set
            {
                this["WindowPlacement"] = value;
            }
        }
    }
}
