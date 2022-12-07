using CommunityToolkit.Mvvm.Messaging;

namespace SketchOverlay.Library.Tests.TestHelpers;

internal static class MessengerAssertionExtensions
{
    public static void AssertIsRegistered<TMessage>(this IMessenger messenger, object sutInstance)
        where TMessage : class
    {
        MessengerAssertions.IsRegistered<TMessage>(messenger, sutInstance);
    }
}