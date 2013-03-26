using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using DragonFiesta.Messages;
using DragonFiesta.Messages.Main;
using DragonFiesta.Util;

namespace DragonFiesta.InterNetwork
{
    public class InternMessageManager
    {
        #region .ctor

        private InternMessageManager()
        {
            _handlers = new Dictionary<Type, Action<IMessage>>();
            _callbacks = new Dictionary<Guid, IExpectAnAnswer>();
            _listeningQueues = new List<MessageQueueEx>();
        }

        #endregion

        #region Properties

        public static InternMessageManager Instance { get; private set; }
        private List<MessageQueueEx> _listeningQueues;
        private MessageQueueEx _myQueue;
        private Dictionary<Type, Action<IMessage>> _handlers;
        private Dictionary<Guid, IExpectAnAnswer> _callbacks; 

        #endregion

        #region Methods

        public static void Initialize()
        {
            Instance = new InternMessageManager();
            InitializeHandlers();
            InitializeQueues();
        }

        private static void InitializeQueues()
        {
            var qs = Configuration.Configuration.Instance.QueueSettings;

            Instance._myQueue = MessageQueueEx.CreateReceiverQueue(qs.MyQueue.Path, true);
            Instance._myQueue.MessageReceived += OnMessage;
            foreach(var q in qs.SubscribeTo)
            {
                var qu = MessageQueueEx.CreateSenderQueue(q.Path);
                var msg = new RegisterListener();
                msg.Id = new Guid();
                msg.QueuePath = qs.MyQueue.Path;
                qu.Send(msg);
            }
        }
        private static void InitializeHandlers()
        {
            var pairs =
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMethods()).
                    Select(
                        m =>
                        new Pair<MethodInfo, object[]>(m, m.GetCustomAttributes(typeof (InternMessageHandlerAttribute), false)))
                    .Where(p => p.Second.Length > 0).Select(
                        p =>
                        new Pair<MethodInfo, InternMessageHandlerAttribute>(p.First, p.Second[0] as InternMessageHandlerAttribute));
            foreach (var pair in pairs)
            {
                Pair<MethodInfo, InternMessageHandlerAttribute> p = pair;
                Action<IMessage>  del = (m) => p.First.Invoke(null, new object[] {m});
                Type t = pair.Second.Type;

                Instance._handlers.Add(t, del);
            }

            Instance._handlers.Add(typeof(RegisterListener), RegisterListener);

            Logs.InterNetwork.DebugFormat("Loaded {0} Handlers.", Instance._handlers.Count);
        }

        public void Send(IMessage msg)
        {
            Debug.WriteLine("Sending a message of type {0}", msg.GetType());
            foreach (var q in _listeningQueues)
            {
                q.Send(msg);
            }
            if(msg is IExpectAnAnswer)
            {
                _callbacks.Add(msg.Id, msg as IExpectAnAnswer);
            }
        }

        public static void StartListening()
        {
            Instance._myQueue.StartReceive();
        }

        protected virtual void RegisterQueue(string pPath)
        {
            this._listeningQueues.Add(MessageQueueEx.CreateSenderQueue(pPath));
            Logs.InterNetwork.InfoFormat("A queue under '{0}' is now listening to us", pPath);
        }
        protected static void OnMessage(object sender, MessageReceivedEventArgs pArgs)
        {
            var msg = pArgs.Message.Body as IMessage;
            Debug.WriteLine("Got message of type {0}", msg.GetType());
            if(Instance._callbacks.ContainsKey(msg.Id))
            {
                Instance._callbacks[msg.Id].Callback(msg);
            }
            if(Instance._handlers.ContainsKey(msg.GetType()))
            {
                Instance._handlers[msg.GetType()](msg);
            }
        }

        protected static void RegisterListener(IMessage pMessage)
        {
            var mes = pMessage as RegisterListener;
            if(mes == null)
                return;
            Instance.RegisterQueue(mes.QueuePath);
        }

        #endregion
    }
}