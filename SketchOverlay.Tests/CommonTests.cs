using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SketchOverlay.Tests.TestHelpers;

namespace SketchOverlay.Tests;

internal static class CommonTests
{
    public static void AssertSetterWithNewValueUpdatesPropertyValue<TValue>(object testTarget, string propertyName, TValue newValue)
    {
        // Arrange
        testTarget.ThrowIfMatchingPropertyValue(propertyName, newValue);

        // Act
        testTarget.SetPropertyValue(propertyName, newValue);

        // Assert
        Assert.Equal(newValue, testTarget.GetPropertyValue<TValue>(propertyName));
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

    public static void AssertPropertyChangedSendsMessageWithNewValue<TMessage, TValue>(object testTarget, string propertyName, TValue newValue)
        where TMessage : ValueChangedMessage<TValue>
    {
        // Arrange
        testTarget.ThrowIfMatchingPropertyValue(propertyName, newValue, out TValue currentValue);
        using MessageInbox inbox = new(Globals.Messenger);
        inbox.Register<TMessage>();

        // Act
        testTarget.SetPropertyValue(propertyName, newValue);

        // Assert
        Assert.Equal(1, inbox.MessageCount);
        Assert.IsType<TMessage>(inbox.GetLastMessage());
        Assert.Equal(newValue, ((TMessage)inbox.GetLastMessage()!).Value);
    }

    // Use this overload when the ValueChangedMessage generic type parameter is different than the property type.
    public static void AssertPropertyChangedSendsMessageWithNewValue<TMessage, TMessageValue, TPropertyValue>(object testTarget, string propertyName,
        TPropertyValue newPropertyValue, TMessageValue expectedMessageValue)
        where TMessage : ValueChangedMessage<TMessageValue>
    {
        // Arrange
        testTarget.ThrowIfMatchingPropertyValue(propertyName, newPropertyValue, out TPropertyValue currentValue);
        using MessageInbox inbox = new(Globals.Messenger);
        inbox.Register<TMessage>();

        // Act
        testTarget.SetPropertyValue(propertyName, newPropertyValue);

        // Assert
        Assert.Equal(1, inbox.MessageCount);
        Assert.IsType<TMessage>(inbox.GetLastMessage());
        Assert.Equal(expectedMessageValue, ((TMessage)inbox.GetLastMessage()!).Value);
    }

    public static TMessage AssertCommandSendsMessage<TMessage>(ICommand command, object? parameter = null) where TMessage : class
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
}