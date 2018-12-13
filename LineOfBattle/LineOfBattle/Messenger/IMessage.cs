using System;

namespace LineOfBattle.Messenger
{
    internal interface IMessage<TArgs>
    {
        object Publisher { get; set; }
        Type PublisherType { get; set; }
        TArgs Args { get; set; }
    }
}