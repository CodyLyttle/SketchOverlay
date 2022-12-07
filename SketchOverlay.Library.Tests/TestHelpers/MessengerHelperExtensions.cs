using CommunityToolkit.Mvvm.Messaging;

namespace SketchOverlay.Library.Tests.TestHelpers;

internal static class MessengerHelperExtensions
{
    public static void AssertIsRegistered<TMessage>(this IMessenger messenger, object sutInstance)
        where TMessage : class
    {
        MessengerAssertions.IsRegistered<TMessage>(messenger, sutInstance);
    }

    public static MessageInbox RegisterInbox<TMessage>(this IMessenger messenger)
        where TMessage : class
    {
        MessageInbox inbox = new(messenger);
        inbox.Register<TMessage>();
        return inbox;
    }

    public static void AssertReceivedSingleMessage<TMessage>(this MessageInbox inbox, TMessage expectedMsg)
        where TMessage : class
    {
        MessengerAssertions.ReceivedSingleMessage(inbox, expectedMsg);
    }
}