using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HistoryMenuSample
{
    /// <summary>
    /// <see cref="ObservableQueue"/> クラスは、オブジェクトの先入れ先出し型のデータ・コレクションを提供するクラスです。
    /// <para>
    /// データ・コレクションは、項目が追加または削除されたとき、あるいは、リスト全体が更新されたときに動的な通知を行います。
    /// </para>
    /// </summary>
    public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Fields

        private object _Sync = new object();

        #endregion

        #region Overload

        /// <summary>
        /// 指定したインデックスにある要素を取得します。
        /// </summary>
        /// <param name="index">取得する要素の０から始まるインデックス。</param>
        /// <returns>取得した <see cref="T"/> 型の要素。</returns>
        public T this[int index]
        {
            get
            {
                return ToArray()[index];
            }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="ObservableQueue"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ObservableQueue() 
        {
            BindingOperations.EnableCollectionSynchronization(this, _Sync);
        }

        /// <summary>
        /// <see cref="ObservableQueue"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="capacity"><see cref="ObservableQueue{T}"/> が格納できる要素数の初期値。</param>
        public ObservableQueue(int capacity) : base(capacity)
        {
            BindingOperations.EnableCollectionSynchronization(this, _Sync);
        }

        public ObservableQueue(IEnumerable<T> collection) : base(collection)
        {
            BindingOperations.EnableCollectionSynchronization(this, _Sync);

            RaiseCollectionChanged(NotifyCollectionChangedAction.Replace);
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="ObservableQueue{T}"/> からすべてのオブジェクトを削除します。
        /// </summary>
        public new void Clear()
        {
            base.Clear();

            RaiseCollectionChanged(NotifyCollectionChangedAction.Reset, default);
        }

        /// <summary>
        /// <see cref="ObservableQueue{T}"/> の末尾にデータを追加します。
        /// </summary>
        /// <param name="item">追加するオブジェクト。</param>
        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            RaiseCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }

        /// <summary>
        /// <see cref="ObservableQueue{T}"/> の先頭にあるオブジェクトを（キューから削除して）返却します。
        /// </summary>
        /// <exception cref="InvalidOperationException">要素数が存在しない（０件）ときに発生する例外です。</exception>
        public new T Dequeue()
        {
            var item = base.Dequeue();

            RaiseCollectionChanged(NotifyCollectionChangedAction.Remove, item);

            return item;
        }

        /// <summary>
        /// <see cref="ObservableQueue{T}"/> の先頭にあるオブジェクトを（キューから削除せずに）返却します。
        /// </summary>
        /// <exception cref="InvalidOperationException">要素数が存在しない（０件）ときに発生する例外です。</exception>
        public new T Peek()
        {
            return base.Peek();
        }

        #endregion

        #region Private Methods

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void RaiseCollectionChanged(NotifyCollectionChangedAction action, object changedItem)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
            
            RaisePropertyChanged(nameof(Count));
            RaisePropertyChanged("");
        }

        protected void RaiseCollectionChanged(NotifyCollectionChangedAction action)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action));

            RaisePropertyChanged(nameof(Count));
            RaisePropertyChanged("");
        }

        #endregion
    }
}
