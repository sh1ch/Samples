using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryMenuSample
{
    /// <summary>
    /// <see cref="ObservableFixedQueue"/> クラスは、オブジェクトの先入れ先出し型の固定長データ・コレクションを提供するクラスです。
    /// <para>
    /// データ・コレクションは、項目が追加または削除されたとき、あるいは、リスト全体が更新されたときに動的な通知を行います。
    /// </para>
    /// </summary>
    public class ObservableFixedQueue<T> : ObservableQueue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// データ・コレクションのデータ長を取得または設定します。
        /// </summary>
        public int Length { get; set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="ObservableFixedQueue"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="length"></param>
        public ObservableFixedQueue(int length) 
        {
            Length = length;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="ObservableQueue{T}"/> の末尾にデータを追加します。
        /// </summary>
        /// <param name="item">追加するオブジェクト。</param>
        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            while (base.Count > Length)
            {
                base.Dequeue();
            }
        }

        /// <summary>
        /// テキストデータにした <see cref="ObservableFixedQueue{T}"/> のコレクションを取得します。
        /// </summary>
        /// <returns></returns>
        public StringCollection ToStringCollection()
        {
            var collection = new StringCollection();

            foreach (var item in this)
            {
                collection.Add(item.ToString());
            }

            return collection;
        }

        #endregion
    }
}
