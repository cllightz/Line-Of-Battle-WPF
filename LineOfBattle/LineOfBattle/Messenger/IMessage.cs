using System;

namespace LineOfBattle.Messenger
{
    internal interface IMessage<TArgs>
    {
        Type PublisherType { get; set; }
        TArgs Args { get; set; }
    }
}