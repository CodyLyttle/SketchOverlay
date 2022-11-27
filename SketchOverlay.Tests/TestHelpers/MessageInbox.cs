using CommunityToolkit.Mvvm.Messaging;

namespace SketchOverlay.Tests.TestHelpers;

internal sealed class MessageInbox : IDisposable
{
    private readonly IMessenger _messenger;

    private readonly Stack<Action> _unregisterActions = new();
    private readonly Stack<object> _receivedMessages = new();

    public MessageInbox(IMessenger messenger)
    {
        _messenger = messenger;
    }

    public int MessageCount => _receivedMessages.Count;

    public void Register<TMessage>() where TMessage : class
    {
        _messenger.Register<TMessage>(this, (_, msg) => { _receivedMessages.Push(msg); });
        _unregisterActions.Push(() => _messenger.Unregister<TMessage>(this));
    }

    public object? GetLastMessage()
    {
        return _receivedMessages.FirstOrDefault();
    }

    public IEnumerable<object> GetMessagesOrderReceived()
    {
        return _receivedMessages.Reverse();
    }

    public IEnumerable<object> GetMessagesOrderRecent()
    {
        return _receivedMessages;
    }

    public void Dispose()
    {
        foreach (Action action in _unregisterActions)
        {
            action.Invoke();
        }
    }
}