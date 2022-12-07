using CommunityToolkit.Mvvm.Messaging;

namespace SketchOverlay.Library.Tests.TestHelpers;

internal static class MessengerAssertions
{
    public static void IsRegistered<TMessage>(IMessenger messenger, object sutInstance)
        where TMessage : class
    {
        Assert.True(messenger.IsRegistered<TMessage>(sutInstance));
    }
}