using System;
using System.Collections.Generic;
using System.Linq;

namespace ShootighLibrary.Messenger
{
    /// <summary>
    /// Publish() されたら即座に Subscriber のコールバックを呼び出すチャンネル
    /// </summary>
    /// <typeparam name="TArgs">メッセージの引数</typeparam>
    public class ImmediateFiringChannel<TArgs> : IChannel<TArgs>
    {
        private HashSet<Type> _publishers = new HashSet<Type>();
        private List<(Type SubscriberType, Action<TArgs> Callback)> _subscribers = new List<(Type, Action<TArgs>)>();

        public void AddPublisher( Type publisherType )
            => _publishers.Add( publisherType );

        public void AddSubscriber( Type subscriberType, Action<TArgs> callback )
            => _subscribers.Add( (subscriberType, callback) );

        public IEnumerable<Type> EnumeratePublishers()
            => _publishers;

        public IEnumerable<Type> EnumerateSubscribers()
            => _subscribers.Select( tuple => tuple.SubscriberType );

        public void MulticastToSubscribers( IMessage<TArgs> message )
        {
#if DEBUG
            if ( !_publishers.Contains( message.PublisherType ) ) {
                throw new PublisherNotRegisteredException( message.PublisherType, typeof( TArgs ) );
            }
#endif

            _subscribers.ForEach( tuple => tuple.Callback( message.Args ) );
        }
    }
}