using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineOfBattle.Messenger
{
    internal class Broker // : IBroker
    {
        internal Broker Singleton = new Broker();

        internal void Subscribe<TSubscriber, TArgs>( Action<TArgs> callback )
        {
            if ( !_channels.ContainsKey( typeof(TArgs) ) ) {
                // Subscriberがいない場合
                _channels[typeof(TArgs)] = new ImmediateFiringChannel<TArgs>();
            }
        }

        // 追記する

        private Dictionary<Type, object> _channels = new Dictionary<Type, object>();
    }
}
