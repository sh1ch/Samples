using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComPortDetectionSample
{
    /// <summary>
    /// <see cref="WindowMessageReceiver"/> クラスは、WndProc のメッセージの転送をサポートするクラスです。
    /// </summary>
    public class WindowMessageReceiver : System.Windows.Forms.NativeWindow, IDisposable
    {
        #region Fields

        private ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private SynchronizationContext _SynchronizationContext = new SynchronizationContext();

        #endregion

        #region Properties

        private List<int> _Messages = new List<int>();

        /// <summary>
        /// 捕捉している Window メッセージを取得します。
        /// </summary>
        public IEnumerable<int> Messages
        {
            get { return _Messages; }
        }

        private List<Action<Message>> _Events = new List<Action<Message>>();

        public IEnumerable<Action<Message>> Events
        {
            get { return _Events; }
        }

        /// <summary>
        /// <see cref="WindowMessageReceiver"/> オブジェクトが破棄されたかどうか示している値を取得します。
        /// </summary>
        public bool IsDisposed { get; private set; } = true;

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="WindowMessageReceiver"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public WindowMessageReceiver()
        {
            Initialize();
        }

        #endregion

        #region Events

        /// <summary>
        /// <see cref="Messages"/> プロパティに登録された Window メッセージを捕捉したときに発生するイベントです。
        /// </summary>
        public event Action<Message> EventReceived;

        #endregion

        #region Public Methods

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public void Initialize()
        {
            var param = new CreateParams();

            param.Caption = GetType().FullName;
            base.CreateHandle(param);

            IsDisposed = false;
        }

        /// <summary>
        /// 使用したリソースをすべて開放します。
        /// </summary>
        /// <param name="runGcCollect">ガベージコレクションを実行します。</param>
        public void Dispose()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException($"{nameof(WindowMessageReceiver)} オブジェクトのリソースは、すでに削除されています。");
            }

            ClearMessages();
            ClearEvents();
            DestroyHandle();

            IsDisposed = true;
        }

        /// <summary>
        /// 使用したリソースをすべて開放します。
        /// </summary>
        /// <param name="runGcCollect">ガベージコレクションを実行します。</param>
        public void Dispose(bool runGcCollect)
        {
            Dispose();

            if (runGcCollect)
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// <see cref="Messages"/> に要素を登録します。
        /// </summary>
        /// <param name="message">登録するメッセージ。</param>
        public void RegisterMessage(int message)
        {
            if (Messages == null) throw new NullReferenceException($"{nameof(Messages)} が null です。");

            _Lock.EnterWriteLock();
            _Messages.Add(message);
            _Lock.ExitWriteLock();
        }

        /// <summary>
        /// <see cref="Messages"/> の要素を削除します。
        /// </summary>
        /// <param name="message">削除するメッセージ。</param>
        public void RemoveMessage(int message)
        {
            if (Messages == null) throw new NullReferenceException($"{nameof(Messages)} が null です。");

            _Lock.EnterWriteLock();
            _Messages.Remove(message);
            _Lock.ExitWriteLock();
        }

        /// <summary>
        /// <see cref="Messages"/> の要素をすべて削除します。
        /// </summary>
        public void ClearMessages()
        {
            if (Messages == null) throw new NullReferenceException($"{nameof(Messages)} が null です。");

            _Lock.EnterWriteLock();
            _Messages.Clear();
            _Lock.ExitWriteLock();
        }

        /// <summary>
        /// <see cref="Events"/> の要素を登録します。
        /// </summary>
        /// <param name="action">登録するイベント。</param>
        public void RegisterEvent(Action<Message> action)
        {
            if (Events == null) throw new NullReferenceException($"{nameof(Events)} が null です。");

            _Lock.EnterWriteLock();

            EventReceived += action;
            _Events.Add(action);

            _Lock.ExitWriteLock();
        }

        /// <summary>
        /// <see cref="Events"/> の要素を削除します。
        /// </summary>
        /// <param name="action">削除するイベント。</param>
        public void RemoveEvent(Action<Message> action)
        {
            if (Events == null) throw new NullReferenceException($"{nameof(Events)} が null です。");

            _Lock.EnterWriteLock();

            EventReceived -= action;
            _Events.Remove(action);

            _Lock.ExitWriteLock();
        }

        /// <summary>
        /// <see cref="Events"/> の要素をすべて削除します。
        /// </summary>
        public void ClearEvents()
        {
            if (Events == null) throw new NullReferenceException($"{nameof(Events)} が null です。");

            _Lock.EnterWriteLock();
            _Events.Clear();
            _Lock.ExitWriteLock();
        }



        #endregion

        #region Protected Methods

        protected override void WndProc(ref Message m)
        {
            _Lock.EnterReadLock();
            var isContained = Messages.Contains(m.Msg);
            _Lock.ExitReadLock();

            if (isContained)
            {
                _SynchronizationContext.Post((state) =>
                {
                    EventReceived?.Invoke((Message)state);
                }, m);
            }

            base.WndProc(ref m);
        }

        #endregion
    }
}
