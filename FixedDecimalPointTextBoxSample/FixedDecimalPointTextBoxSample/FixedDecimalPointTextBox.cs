using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.AccessControl;
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

namespace FixedDecimalPointTextBoxSample
{
    public class FixedDecimalPointTextBox : TextBox
    {
        #region DependencyProperties

        /// <summary>
        /// 追加の表示値のプロパティ。
        /// </summary>
        public static readonly DependencyProperty SubTextProperty =
            DependencyProperty.Register(
                nameof(SubText),
                typeof(string),
                typeof(FixedDecimalPointTextBox),
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

        /// <summary>
        /// 小数の入力桁数のプロパティ。
        /// </summary>
        public static readonly DependencyProperty DecimalsProperty =
            DependencyProperty.Register(
                nameof(Decimals),
                typeof(int),
                typeof(FixedDecimalPointTextBox),
                new PropertyMetadata(2, (sender, e) => 
                {
                    var textbox = sender as FixedDecimalPointTextBox;

                    if (textbox == null) return;

                    textbox.AdjustDecimals(textbox);
                })
            );

        /// <summary>
        /// 小数の入力桁数を取得または設定します。
        /// </summary>
        public int Decimals
        {
            get { return (int)GetValue(DecimalsProperty); }
            set { SetValue(DecimalsProperty, value); }
        }

        #endregion

        static FixedDecimalPointTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FixedDecimalPointTextBox), new FrameworkPropertyMetadata(typeof(FixedDecimalPointTextBox)));
        }

        #region Command

        /*
        private SimpleCommand _DeleteCommand;

        public SimpleCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand = _DeleteCommand ?? new SimpleCommand(() =>
                {
                    Debug.WriteLine($"{nameof(DeleteCommand)} Called.");
                });
            }
        }
        */

        #endregion

        public FixedDecimalPointTextBox()
        {
            CommandManager.AddPreviewExecutedHandler(this, (sender, e) =>
            {
                if (e.Command == ApplicationCommands.Paste || e.Command == ApplicationCommands.Cut)
                {
                    e.Handled = true;
                }
            });

            // InputBindings.Add(new KeyBinding(DeleteCommand, Key.Delete, ModifierKeys.None));

            FocusManager.SetFocusedElement(this, null);

            SelectionChanged += (sender, e) =>
            {
                // Console.WriteLine($"\"{nameof(SelectionChanged)}\" event has occurred. {DateTime.Now}");

                var textbox = e.Source as TextBox;

                if (textbox == null) return;
                
                if (textbox.SelectionLength != 0)
                {
                    textbox.SelectionLength = 0;
                    e.Handled = true;
                }
            };

            PreviewMouseDown += (sender, e) =>
            {
                Console.WriteLine($"\"{nameof(PreviewMouseDown)}\" event has occurred. {DateTime.Now}");

                var textbox = e.Source as TextBox;

                if (textbox == null) return;

                textbox.Focus();

                // マウスの選択位置にキャレットが移動するのを GotFocus で指定した位置に固定する
                e.Handled = true;
            };

            GotFocus += (sender, e) =>
            {
                Console.WriteLine($"\"{nameof(GotFocus)}\" event has occurred. {DateTime.Now}");

                var textbox = e.Source as TextBox;

                if (textbox == null) return;

                var textStatus = new NumericalTextStatus(textbox.Text);

                if (textStatus.HasDecimal)
                {
                    textbox.SelectionStart = textStatus.IndexOfDecimal;
                }
                else
                {
                    textbox.SelectionStart = textStatus.Length;
                }
            };

            PreviewKeyDown += (sender, e) =>
            {
                Console.WriteLine($"\"{nameof(PreviewKeyDown)}\" event has occurred. {DateTime.Now}");

                var textbox = e.Source as TextBox;
                var isHandled = false;

                Input(textbox, e.Key, ref isHandled);

                e.Handled = isHandled;
            };

            TextChanged += (sender, e) =>
            {
                var textbox = e.Source as TextBox;

                if (textbox == null) return;

                AdjustDecimals(textbox);
            };
        }

        private void Input(TextBox textbox, Key key, ref bool isHandled)
        {
            if (textbox == null) return;

            var oldText = textbox.Text;
            var newText = "";

            // 例外
            if (key == Key.Enter)
            {
                return;
            }
            else if (key == Key.Tab)
            {
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                return;
            }
            else if (key == Key.Left || key == Key.Right)
            {
                return;
            }
            else if (key == Key.OemPeriod || key == Key.Decimal)
            {
                var decimalSelectionStart = Text.IndexOf('.');

                if (decimalSelectionStart > 0)
                {
                    textbox.SelectionStart = decimalSelectionStart + 1;
                }

                isHandled = true; // .は 2重に入力できない
                return;
            }

            var selectionStart = textbox.SelectionStart;
            var textStatus = new NumericalTextStatus(Text);

            if (key == Key.Delete)
            {
                newText = textStatus.Delete(selectionStart);

                textbox.Text = newText;
                textbox.SelectionStart = selectionStart;
            }
            else if (key == Key.Back)
            {
                if (selectionStart > 0)
                {
                    selectionStart -= 1;
                }

                newText = textStatus.Delete(selectionStart);

                textbox.Text = newText;
                textbox.SelectionStart = selectionStart;
            }
            else
            {
                // キーの数値入力を取得する
                var keyValue = GetKeyValue(key);

                if (keyValue < 0)
                {
                    isHandled = true;
                    return;
                }

                newText = textStatus.InsertOrReplace(keyValue, selectionStart);

                textbox.Text = newText;

                if (newText.Length != oldText.Length)
                {
                    textbox.SelectionStart = selectionStart + 1;
                }
                else
                {
                    if (textStatus.IndexOfDecimal + 1 <= selectionStart)
                    {
                        textbox.SelectionStart = selectionStart + 1;
                    }
                    else
                    {
                        textbox.SelectionStart = selectionStart;
                    }
                }
            }

            isHandled = true;
        }

        public void AdjustDecimals(TextBox textbox)
        {
            var textStatus = new NumericalTextStatus(textbox.Text);

            // 小数部を持っているか
            if (Decimals > 0)
            {
                if (!textStatus.HasDecimal || textStatus.DecimalPartText.Length != Decimals)
                {
                    var newText = textStatus.SetDecimals(Decimals);
                    textbox.Text = newText;
                }
            }
        }

        private int GetKeyValue(Key key)
        {
            int keyValue = (int)key;
            var selectedValue = -1;

            // 数値入力をフォロー
            if (keyValue >= (int)Key.D0 && keyValue <= (int)Key.D9)
            {
                selectedValue = keyValue - (int)Key.D0;
            }
            else if (keyValue >= (int)Key.NumPad0 && keyValue <= (int)Key.NumPad9)
            {
                selectedValue = keyValue - (int)Key.NumPad0;
            }

            return selectedValue;
        }

    }
}
