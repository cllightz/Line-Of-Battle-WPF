using System;

namespace ShootighLibrary.Messenger
{
    internal class Message<TArgs> : IMessage<TArgs>
    {
        internal Message( Type publisherType, TArgs args )
        {
            PublisherType = publisherType;
            Args = args;
        }

        public Type PublisherType { get; set; }
        public TArgs Args { get; set; }
    }
}
