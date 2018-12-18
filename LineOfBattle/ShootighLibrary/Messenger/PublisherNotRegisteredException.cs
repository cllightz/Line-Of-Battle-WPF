using System;

namespace ShootighLibrary.Messenger
{
#if DEBUG
    internal class PublisherNotRegisteredException : Exception
    {
        internal Type PublisherType;
        internal Type MessageArgsType;

        internal PublisherNotRegisteredException( Type publisherType, Type messageArgsType )
            : base( $"{publisherType.FullName} が {messageArgsType.FullName} のパブリッシャーとして登録されていないにも関わらずメッセージが発行されました。" )
        {
            PublisherType = publisherType;
            MessageArgsType = messageArgsType;
        }
    }
#endif
}
