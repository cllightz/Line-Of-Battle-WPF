using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LineOfBattle.Messenger
{
    internal class Mediator // : IMediator
    {
        private static Mediator _singleton = null;
        internal static Mediator Singleton => _singleton ?? (_singleton = new Mediator());

        private Mediator() { }

        private Dictionary<Type, object> _channels = new Dictionary<Type, object>();

        internal Mediator RegisterPublisher<TArgs>( Type publisherType )
        {
            try {
                ((IChannel<TArgs>)_channels[ typeof( TArgs ) ]).AddPublisher<TArgs>();
            } catch ( KeyNotFoundException ) {
                // TArgs のチャンネルが初期化されていない場合
                var channel = new ImmediateFiringChannel<TArgs>();
                channel.AddPublisher<TArgs>();
                _channels[ typeof( TArgs ) ] = channel;
            } catch ( InvalidCastException e ) {
                Debug.WriteLine( $"{nameof( Mediator )}.{nameof( RegisterPublisher )}<{publisherType.FullName}, {typeof( TArgs ).FullName}>() において、object→IChannel<{typeof( TArgs ).FullName}> のキャストに失敗しました。\nobject: {_channels[ typeof( TArgs ) ]}" );
                throw e;
            }

            return this;
        }

        internal Mediator Publish<TArgs>( Type publisherType, TArgs args )
        {
            try {
                ((IChannel<TArgs>)_channels[ typeof( TArgs ) ]).MulticastToSubscribers( new Message<TArgs>( publisherType, args ) );
            } catch ( KeyNotFoundException ) {
                // TArgs のチャンネルが初期化されていない場合
                // 何もしない
            }

            return this;
        }

        internal Mediator Subscribe<TArgs>( Type subscriberType, Action<TArgs> callback )
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

            return this;
        }
    }
}
