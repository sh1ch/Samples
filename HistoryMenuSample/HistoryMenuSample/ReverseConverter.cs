using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HistoryMenuSample
{
    /// <summary>
    /// <see cref="ReverseConverter"/> クラスは、カスタム ロジックをバインディングに適用する方法を提供します。
    /// <para>
    /// リストの並びを逆転させます。
    /// </para>
    /// </summary>
    public class ReverseConverter : IValueConverter
    {
        #region Fields

        private ObservableCollection<object> _Items = new ObservableCollection<object>();

        #endregion

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rawItems = value as IEnumerable<object>;

            if (rawItems == null) return null;

            if (rawItems is INotifyCollectionChanged)
            {
                (value as INotifyCollectionChanged).CollectionChanged += (sender, args) => 
                {
                    Update(rawItems);
                };
            }

            Update(rawItems);

            return _Items;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Private Methods

        private void Update(IEnumerable<object> items)
        {
            _Items.Clear();

            foreach (var item in items.Reverse())
            {
                _Items.Add(item);
            }
        }

        #endregion
    }
}
