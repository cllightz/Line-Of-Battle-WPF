using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ShootighLibrary.Messenger
{
    public class Mediator // : IMediator
    {
        private static Mediator _singleton = null;
        public static Mediator Singleton => _singleton ?? (_singleton = new Mediator());

        private Mediator() { }

        private Dictionary<Type, IChannel> _channels = new Dictionary<Type, IChannel>();

        public Mediator RegisterPublisher<TArgs>( Type publisherType )
        {
            try {
                ((IChannel<TArgs>)_channels[ typeof( TArgs ) ]).AddPublisher( publisherType );
            } catch ( KeyNotFoundException ) {
                // TArgs のチャンネルが初期化されていない場合
                var channel = new ImmediateFiringChannel<TArgs>();
                channel.AddPublisher( publisherType );
                _channels[ typeof( TArgs ) ] = channel;
            } catch ( InvalidCastException ) {
                Debug.WriteLine( $"{nameof( Mediator )}.{nameof( RegisterPublisher )}<{typeof( TArgs ).FullName}>() において、object→IChannel<{typeof( TArgs ).FullName}> のキャストに失敗しました。\nobject: {_channels[ typeof( TArgs ) ]}" );
                throw;
            }

            return this;
        }

        public Mediator Publish<TArgs>( Type publisherType, TArgs args )
        {
            try {
                ((IChannel<TArgs>)_channels[ typeof( TArgs ) ]).MulticastToSubscribers( new Message<TArgs>( publisherType, args ) );
            } catch ( KeyNotFoundException ) {
                // TArgs のチャンネルが初期化されていない場合
                // 何もしない
            }

            return this;
        }

        public Mediator Subscribe<TArgs>( Type subscriberType, Action<TArgs> callback )
        {
            try {
                ((IChannel<TArgs>)_channels[ typeof( TArgs ) ]).AddSubscriber( subscriberType, callback );
            } catch ( KeyNotFoundException ) {
                // TArgs のチャンネルが初期化されていない場合
                var channel = new ImmediateFiringChannel<TArgs>();
                channel.AddSubscriber( subscriberType, callback );
                _channels[ typeof( TArgs ) ] = channel;
            } catch ( InvalidCastException ) {
                Debug.WriteLine( $"{nameof( Mediator )}.{nameof( Subscribe )}<{typeof( TArgs ).FullName}>() において、object→IChannel<{typeof( TArgs ).FullName}> のキャストに失敗しました。\nobject: {_channels[ typeof( TArgs ) ]}" );
                throw;
            }

            return this;
        }

#if DEBUG
        public Mediator DebugOutput( Action<string> output )
        {
            output( string.Join( "\n",
                _channels.SelectMany( ch =>
                    (ch.Value as IChannel).EnumeratePublishers().Select( type => {
                        return "TArgs,PublisherType";
                        // IChannel<TArgs> の TArgs が何かを取得して、以下のような行を生成したい
                        // リフレクションは理想、
                        // デバッグ出力はできなくても、列挙まで出来てるところをアピールできればよい
                        // {TArgs},{PublisherType}
                    } )
                    // .Concat( (ch.Value as IChannel).EnumerateSubscribers() )
                )
            ) );

            return this;
        }
#endif
    }
}
