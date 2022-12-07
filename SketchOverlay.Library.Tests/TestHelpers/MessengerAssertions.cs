using CommunityToolkit.Mvvm.Messaging;

namespace SketchOverlay.Library.Tests.TestHelpers;

internal static class MessengerAssertions
{
    public static void IsRegistered<TMessage>(IMessenger messenger, object sutInstance)
        where TMessage : class
    {
        Assert.True(messenger.IsRegistered<TMessage>(sutInstance));
    }
    
    public static void ReceivedSingleMessage<TMessage>(MessageInbox inbox, TMessage expectedMsg)
        where TMessage : class
    {
        Assert.Equal(1, inbox.MessageCount);
        Assert.Equivalent(expectedMsg, inbox.GetLastMessage());
    }

    public static void ReceivedNoMessages(MessageInbox inbox)
    {
        Assert.Equal(0, inbox.MessageCount);
    }
}