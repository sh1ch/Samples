using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DependencyCustomControlSample
{
    public class DataGridCustomTextBoxColumn : TextBox
    {
        #region DependencyProperties

        /// <summary>
        /// 追加の表示値のプロパティ。
        /// </summary>
        public static readonly DependencyProperty SubTextProperty =
            DependencyProperty.Register(
                nameof(SubText),
                typeof(string),
                typeof(DataGridCustomTextBoxColumn),
                new UIPropertyMetadata(null)
            );

        /// <summary>
        /// 追加の表示するテキストを取得または設定します。
        /// </summary>
        public string SubText
        {
            get { return (string)GetValue(SubTextProperty); }
            set { SetValue(SubTextProperty, value); }
        }

        #endregion

        static DataGridCustomTextBoxColumn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridCustomTextBoxColumn), new FrameworkPropertyMetadata(typeof(DataGridCustomTextBoxColumn)));
        }

        public DataGridCustomTextBoxColumn()
        {
            LostFocus += ForceUpdateBinding;
        }

        /// <summary>
        /// コントロールからフォーカスが失われたときに強制的にバインディングの更新を実行します。
        /// </summary>
        /// <param name="sender">イベントが発生したオブジェクト(未使用)。</param>
        /// <param name="e">フォーカスイベントの情報を示すイベント引数(未使用)。</param>
        private void ForceUpdateBinding(object sender, RoutedEventArgs e)
        {
            if (IsKeyboardFocusWithin == true) return;

            var binding = BindingOperations.GetBinding(this, TextProperty);

            if (binding != null && (binding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus || binding.UpdateSourceTrigger == UpdateSourceTrigger.Default))
            {
                var bindingExpression = GetBindingExpression(TextProperty);
                if (bindingExpression == null) return;

                if (bindingExpression != null && (bindingExpression.BindingGroup?.Owner ?? null) is DataGrid)
                {
                    Debug.WriteLine($"BindingGroup が {bindingExpression.BindingGroup.Owner.GetType()} に設定されています。");
                    Debug.WriteLine($"未反映状態 = {bindingExpression.BindingGroup.IsDirty}");
                }

                bindingExpression.UpdateSource();
                bindingExpression.UpdateTarget();
            }
        }
    }
}
