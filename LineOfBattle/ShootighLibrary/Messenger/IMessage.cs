using System;

namespace ShootighLibrary.Messenger
{
    public interface IMessage<TArgs>
    {
        Type PublisherType { get; set; }
        TArgs Args { get; set; }
    }
}