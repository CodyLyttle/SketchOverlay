using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.Messages;
using SketchOverlay.ViewModels;

namespace SketchOverlay.Tests.ViewModels;

public class DrawingToolWindowViewModelTests
{
    private static readonly IMessenger TestMessenger = WeakReferenceMessenger.Default;
    private readonly DrawingToolWindowViewModel _sut;

    public DrawingToolWindowViewModelTests()
    {
        _sut = new DrawingToolWindowViewModel();
    }

    [Fact]
    public void UndoCommand_SendsCanvasActionMessageWithValueUndo()
    {
        // Arrange
        const CanvasAction expected = CanvasAction.Undo;
        CanvasAction? actual = null;
        TestMessenger.Register<CanvasActionMessage>(this, (_, message) => actual = message.Value);

        // Act
        _sut.UndoCommand.Execute(null);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RedoCommand_SendsCanvasActionMessageWithValueRedo()
    {
        // Arrange
        const CanvasAction expected = CanvasAction.Redo;
        CanvasAction? actual = null;
        TestMessenger.Register<CanvasActionMessage>(this, (_, message) => actual = message.Value);

        // Act
        _sut.RedoCommand.Execute(null);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ClearCommand_SendsCanvasActionMessageWithValueClear()
    {
        // Arrange
        const CanvasAction expected = CanvasAction.Clear;
        CanvasAction? actual = null;
        TestMessenger.Register<CanvasActionMessage>(this, (_, message) => actual = message.Value);

        // Act
        _sut.ClearCommand.Execute(null);

        // Assert
        Assert.Equal(expected, actual);
    }
}