using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DragonFiesta.Util
{
    public class Worker
    {
        #region .ctor

        private Worker()
        {
            _taskQueue = new BlockingCollection<Task>();
        }
        public Worker(int pNumThreads) : this()
        {
            this._numThreads = pNumThreads;
            _threads = new Thread[pNumThreads];
            for(int i = 0; i < pNumThreads; i++)
            {
                Thread t = new Thread(Consume);
                t.IsBackground = true;
                t.Name = "DF_WORKER_" + i;
                _threads[i] = t;
            }
        }

        ~Worker()
        {
            _taskQueue.CompleteAdding();
            _taskQueue.Dispose();
        }
        #endregion

        #region Properties

        private int _numThreads;
        private Thread[] _threads;
        private BlockingCollection<Task> _taskQueue;

        public int NumberOfThreads { get { return _numThreads; } }

        #endregion

        #region Methods

        public void QueueTask(Task t)
        {
            _taskQueue.Add(t);
        }

        private void Consume()
        {
            var enumerable = _taskQueue.GetConsumingEnumerable();
            foreach (var task in enumerable)
            {
                task.RunSynchronously();
            }
        }

        #endregion
    }
}