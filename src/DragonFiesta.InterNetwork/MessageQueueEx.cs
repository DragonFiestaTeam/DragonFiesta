using System;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;
using DragonFiesta.Util;

namespace DragonFiesta.InterNetwork
{
    public class MessageQueueEx
    {
        #region .ctor
        
        protected MessageQueueEx()        // dont allow to create by user
        {
            _receiveLoopThread = new Thread(ReceiveLoop);
            _receiveLoopThread.IsBackground = true;
        }

        #endregion
        #region Properties

        private MessageQueue _queue;
        private bool _isSendQueue;
        private bool _isReceiverQueue;
        private Thread _receiveLoopThread;

        public MessageQueue Queue { get { return _queue; } }
        public bool IsSendQueue { get { return _isSendQueue; } }
        public bool IsReceiverQueue { get { return _isReceiverQueue; } }

        #endregion
        #region Methods

        public static MessageQueueEx CreateSenderQueue(string pPath)
        {
            var q = new MessageQueueEx();

            q._isSendQueue = true;
            q._isReceiverQueue = false;
            q._queue = new MessageQueue(pPath);
            q._queue.Formatter = new BinaryMessageFormatter();
            q._receiveLoopThread.Name = "RecThread " + pPath;
            return q;
        }
        public static MessageQueueEx CreateReceiverQueue(string pPath, bool pCreateIfNonExistend)
        {
            var q = new MessageQueueEx();
            if(MessageQueue.Exists(pPath))
            {
                q._queue = new MessageQueue(pPath)
                               {Formatter = new BinaryMessageFormatter(),};
                q._isSendQueue = false;
                q._isReceiverQueue = true;
            }
            else
            {
                q._queue = MessageQueue.Create(pPath);
                q._queue.Formatter = new BinaryMessageFormatter();
                q._isSendQueue = false;
                q._isReceiverQueue = true;
            }

            return q;
        }

        public void StartReceive()
        {
            if (!_isReceiverQueue)
                return;
            _receiveLoopThread.Start();

        }
        public void Send(IMessage msg)
        {
            if(!_isSendQueue)
                throw new InvalidOperationException();
            _queue.Send(msg);
        }

        protected virtual void OnMessageReceived(MessageReceivedEventArgs pArgs)
        {
            if(MessageReceived != null)
            {
                MessageReceived(this, pArgs);
            }
        }
        
        protected virtual void ReceiveLoop()
        {
            while(true)
            {
                var m = _queue.Receive();
                var args = new MessageReceivedEventArgs() {Message = m,};
                OnMessageReceived(args);
            }
        }
        
        #endregion
        #region Events

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        #endregion
    }
}