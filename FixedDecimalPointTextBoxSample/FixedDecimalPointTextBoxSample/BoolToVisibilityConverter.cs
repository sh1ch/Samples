using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FixedDecimalPointTextBoxSample
{
    /// <summary>
    /// <see cref="BoolToVisibilityConverter"/> クラスは、カスタム ロジックをバインディングに適用する方法を提供します。 
    /// <para>
    /// <seealso cref="bool"/> 型のデータを <seealso cref="System.Windows.Visibility"/> 型のデータに変換します。
    /// </para>
    /// </summary>
    /// <example>
    /// true  value = Visibility.Visible
    /// false value = FalseValue
    /// null  value = FalseValue
    /// </example>
    [ValueConversion(typeof(bool?), typeof(Visibility))]
    public class BoolToVisibilityConverter : DependencyObject, IValueConverter
    {
        #region DependencyProperties

        public static readonly DependencyProperty FalseValueProperty =
            DependencyProperty.Register(
                nameof(FalseValue),
                typeof(Visibility),
                typeof(BoolToVisibilityConverter),
                new PropertyMetadata(Visibility.Collapsed)
            );

        public static readonly DependencyProperty InverseProperty =
            DependencyProperty.Register(
                nameof(InverseValue),
                typeof(bool),
                typeof(BoolToVisibilityConverter),
                new PropertyMetadata(false)
            );

        /// <summary>
        /// 変換する値が false のとき、返却される <see cref="Visibility"/> 型の値を取得または設定します。
        /// </summary>
        public Visibility FalseValue
        {
            get { return (Visibility)GetValue(FalseValueProperty); }
            set { SetValue(FalseValueProperty, value); }
        }

        /// <summary>
        /// 変換する値を反転して評価するかどうかを決める値を取得または設定します。。
        /// </summary>
        public bool InverseValue
        {
            get { return (bool)GetValue(InverseProperty); }
            set { SetValue(InverseProperty, value); }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="BoolToVisibilityConverter"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public BoolToVisibilityConverter()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 値を変換します。
        /// <para>
        /// <see cref="bool"/> 型のデータを <see cref="Visibility"/> 型のデータに変換します。
        /// </para>
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値で、言語名を表すテキスト。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャー。</param>
        /// <returns>変換された値。　ture のとき、 <see cref="Visibility.Visible"/> それ以外は、 <see cref="FalseValue"/> 。</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = false;

            if (value == null)
            {
                return FalseValue;
            }
            else if (value is bool || value is bool?)
            {
                isVisible = (bool)value;
                if (InverseValue == true) isVisible = !isVisible;
            }
            else
            {
                // bool型ではないとき、テキスト値として処理する
                var canParse = bool.TryParse(value.ToString(), out isVisible);

                if (canParse == false)
                {
                    isVisible = false;
                }
                else
                {
                    if (InverseValue == true) isVisible = !isVisible;
                }
            }

            return (isVisible == true ? Visibility.Visible : FalseValue);
        }

        /// <summary>
        /// 値を変換します。
        /// <para>
        /// <see cref="Visibility"/> 型のデータを <see cref="bool"/> 型のデータに変換します。
        /// </para>
        /// </summary>
        /// <param name="value">バインディング ソースによって生成された値。</param>
        /// <param name="targetType">バインディング ターゲット プロパティの型。</param>
        /// <param name="parameter">使用するコンバーター パラメーター。</param>
        /// <param name="culture">コンバーターで使用するカルチャー。</param>
        /// <returns>変換された値。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = FalseValue;

            if (value == null)
            {
                return FalseValue;
            }
            else if (value is bool || value is bool?)
            {
                visibility = (Visibility)value;
            }
            else
            {
                // bool型ではないとき、テキスト値として処理する
                var canParse = Visibility.TryParse(value.ToString(), out visibility);

                if (canParse == false)
                {
                    visibility = FalseValue;
                }
            }

            return (visibility == Visibility.Visible ? true : false);
        }

        #endregion
    }
}
