using System;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp
{
    public abstract class AsyncWampConnection<TMessage> : IWampConnection<TMessage>
    {
        private readonly ActionBlock<WampMessage<TMessage>> mSendBlock;

        protected AsyncWampConnection()
        {
            mSendBlock = new ActionBlock<WampMessage<TMessage>>(x => InnerSend(x));
        }

        public void Send(WampMessage<TMessage> message)
        {
            mSendBlock.Post(message);
        }

#if !NET40

        protected async Task InnerSend(WampMessage<TMessage> message)
        {
            if (IsConnected)
            {
                try
                {
                    Task sendAsync = SendAsync(message);

                    if (sendAsync != null)
                    {
                        await sendAsync.ConfigureAwait(false);                        
                    }
                }
                catch (Exception)
                {
                }
            }
        }

#else
        protected Task InnerSend(WampMessage<TMessage> message)
        {
            if (IsConnected)
            {
                Task sendAsync = SendAsync(message);
                
                Task result = sendAsync.ContinueWith(task =>
                {
                    var exception = task.Exception;
                });
                
                return result;
            }
            else
            {
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                tcs.SetResult(null);
                return tcs.Task;
            }
        }

#endif

        protected abstract bool IsConnected { get; }

        public event EventHandler ConnectionOpen;
        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        public event EventHandler ConnectionClosed;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
        protected abstract Task SendAsync(WampMessage<TMessage> message);

        protected virtual void RaiseConnectionOpen()
        {
            var handler = ConnectionOpen;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void RaiseMessageArrived(WampMessage<TMessage> message)
        {
            var handler = MessageArrived;
            if (handler != null) handler(this, new WampMessageArrivedEventArgs<TMessage>(message));
        }

        protected virtual void RaiseConnectionClosed()
        {
            var handler = ConnectionClosed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void RaiseConnectionError(Exception e)
        {
            var handler = ConnectionError;
            if (handler != null) handler(this, new WampConnectionErrorEventArgs(e));
        }
        
        async void IDisposable.Dispose()
        {
            mSendBlock.Complete();
            await mSendBlock.Completion;
            this.Dispose();
        }

        public abstract void Dispose();
    }
}