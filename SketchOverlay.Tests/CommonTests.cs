using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SketchOverlay.Messages;
using SketchOverlay.Tests.TestHelpers;

namespace SketchOverlay.Tests;

internal static class CommonTests
{
    public static void AssertSetterSetsInputValue<TValue>(object testTarget,
        string propertyName, TValue inputValue)
    {
        // Arrange
        testTarget.ThrowIfMatchingPropertyValue(propertyName, inputValue);

        // Act
        testTarget.SetPropertyValue(propertyName, inputValue);

        // Act
        Assert.Equal(inputValue, testTarget.GetPropertyValue<TValue>(propertyName));
    }

    public static void AssertSetterModifiesInputValue<TValue>(object testTarget,
        string propertyName, TValue inputValue, TValue outputValue)
    {
        // Arrange
        testTarget.ThrowIfMatchingPropertyValue(propertyName, outputValue);

        // Act
        testTarget.SetPropertyValue(propertyName, inputValue);

        // Act
        Assert.Equal(outputValue, testTarget.GetPropertyValue<TValue>(propertyName));
    }

    public static void AssertReceiveMessageUpdatesProperty<TMessage>(object testTarget, 
        Action<TMessage> receiveAction, TMessage message)
        where TMessage : SimpleSetPropertyMessage
    {
        // Arrange
        testTarget.ThrowIfMatchingPropertyValue(message.PropertyName, message.Value);

        // Act
        receiveAction.Invoke(message);

        // Assert
        Assert.Equal(message.Value, testTarget.GetPropertyValue(message.PropertyName));
    }

    public static TMessage AssertCommandSendsMessage<TMessage>(ICommand command, object? parameter = null) 
        where TMessage : class
    {
        // Arrange
        using MessageInbox inbox = new(Globals.Messenger);
        inbox.Register<TMessage>();

        // Act
        command.Execute(parameter);

        // Assert
        Assert.Equal(1, inbox.MessageCount);
        Assert.IsType<TMessage>(inbox.GetLastMessage());

        return (TMessage)inbox.GetLastMessage()!;
    }

    /// <summary>
    /// Invoke a command and wait a small delay before checking the message inbox.<br/>
    /// This is a workaround for commands that call async void methods.
    /// </summary>
    public static async Task<TMessage> AssertCommandSendsMessageAsync<TMessage>(ICommand command, object? parameter = null, int delayMs = 50)
        where TMessage : class
    {
        // Arrange
        using MessageInbox inbox = new(Globals.Messenger);
        inbox.Register<TMessage>();

        // Act
        command.Execute(parameter);
        await Task.Delay(delayMs);

        // Assert
        Assert.Equal(1, inbox.MessageCount);
        Assert.IsType<TMessage>(inbox.GetLastMessage());

        return (TMessage)inbox.GetLastMessage()!;
    }

    public static void AssertPropertyChangedSendsSimplePropertyChangedMessage<TMessage, TValue>(object testTarget,
        string propertyName, TValue newValue) 
        where TMessage : SimplePropertyChangedMessage
    {
        // Arrange
        testTarget.ThrowIfMatchingPropertyValue(propertyName, newValue);
        using MessageInbox inbox = new(Globals.Messenger);
        inbox.Register<TMessage>();

        // Act
        testTarget.SetPropertyValue(propertyName, newValue);
        var message = (TMessage)inbox.GetLastMessage()!;

        // Assert
        Assert.Equal(1, inbox.MessageCount);
        Assert.Equal(propertyName, message.PropertyName);
        Assert.Equal(newValue, message.Value);
    }

    public static void AssertSetterWithSameValueDoesNotSendMessage<TMessage, TValue>(object testTarget, string propertyName)
        where TMessage : class
    {
        // Arrange
        using MessageInbox inbox = new(Globals.Messenger);
        inbox.Register<TMessage>();

        // Act
        testTarget.SetPropertyValue(
            propertyName,
            testTarget.GetPropertyValue<TValue>(propertyName));

        // Assert
        Assert.Equal(0, inbox.MessageCount);
    }
}