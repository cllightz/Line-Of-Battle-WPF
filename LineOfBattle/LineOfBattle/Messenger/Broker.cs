using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LineOfBattle.Messenger
{
    internal class Broker // : IBroker
    {
        internal Broker Singleton = new Broker();

        private Dictionary<Type, object> _channels = new Dictionary<Type, object>();

        internal void RegisterPublisher<TArgs>( Type publisherType )
        {
            try {
                ((IChannel<TArgs>)_channels[ typeof( TArgs ) ]).AddPublisher<TArgs>();
            } catch ( KeyNotFoundException ) {
                // TArgs のチャンネルが初期化されていない場合
                var channel = new ImmediateFiringChannel<TArgs>();
                channel.AddPublisher<TArgs>();
                _channels[ typeof( TArgs ) ] = channel;
            } catch ( InvalidCastException e ) {
                Debug.WriteLine( $"Broker.RegisterPublisher<{publisherType.FullName}, {typeof( TArgs ).FullName}>() において、object→IChannel<{typeof( TArgs ).FullName}> のキャストに失敗しました。\nobject: {_channels[ typeof( TArgs ) ]}" );
                throw e;
            }
        }

        internal void Publish<TArgs>( Type publisherType, TArgs args )
        {
            try {
                ((IChannel<TArgs>)_channels[ typeof( TArgs ) ]).MulticastToSubscribers( new Message<TArgs>( publisherType, args ) );
            } catch ( KeyNotFoundException ) {
                // TArgs のチャンネルが初期化されていない場合
                // 何もしない
            }
        }

        internal void Subscribe<TArgs>( Type subscriberType, Action<TArgs> callback )
        {
            try {
                ((IChannel<TArgs>)_channels[ typeof( TArgs ) ]).AddSubscriber<TArgs>( callback );
            } catch ( KeyNotFoundException ) {
                // TArgs のチャンネルが初期化されていない場合
                var channel = new ImmediateFiringChannel<TArgs>();
                channel.AddPublisher<TArgs>();
                _channels[ typeof( TArgs ) ] = channel;
            } catch ( InvalidCastException e ) {
                Debug.WriteLine( $"Broker.RegisterPublisher<{subscriberType.FullName}, {typeof( TArgs ).FullName}>() において、object→IChannel<{typeof( TArgs ).FullName}> のキャストに失敗しました。\nobject: {_channels[ typeof( TArgs ) ]}" );
                throw e;
            }
        }
    }
}
