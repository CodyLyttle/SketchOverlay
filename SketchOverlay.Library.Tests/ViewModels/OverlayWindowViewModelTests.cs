using System.Drawing;
using CommunityToolkit.Mvvm.Messaging;
using Moq;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Messages;
using SketchOverlay.Library.Models;
using SketchOverlay.Library.Tests.TestHelpers;
using SUT = SketchOverlay.Library.ViewModels.OverlayWindowViewModel<object, object, object, object>;
using ToolsWindow = SketchOverlay.Library.ViewModels.ToolsWindowViewModel<object, object, object>;

namespace SketchOverlay.Library.Tests.ViewModels;

public class OverlayWindowViewModelTests
{
    #region Setups

    private readonly SUT _sut;
    private readonly IMessenger _messenger = WeakReferenceMessenger.Default;
    private readonly Mock<ICanvasDrawingManager<object>> _mockCanvas;

    public OverlayWindowViewModelTests()
    {
        _mockCanvas = new Mock<ICanvasDrawingManager<object>>();
        _mockCanvas.SetupAllProperties();
        _sut = new SUT(_mockCanvas.Object, _messenger);
    }

    #endregion

    [Fact]
    public void Constructor_RegistersForMessages()
    {
        _messenger.AssertIsRegistered<ToolsWindowPropertyChangedMessage>(_sut);
    }

    [Fact]
    public void ToggleCanvasVisibilityCommand_ToggleIsCanvasVisible()
    {
        for (var i = 0; i < 2; i++)
        {
            // Arrange
            bool initialValue = _sut.IsCanvasVisible;

            // Act
            _sut.ToggleCanvasVisibilityCommand.Execute(null);

            // Assert
            Assert.Equal(!initialValue, _sut.IsCanvasVisible);
        }
    }

    #region MouseDownCommand

    [Fact]
    public void MouseDownCommand_LeftButton_DisableToolsWindowHitTesting()
    {
        // Arrange
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowSetPropertyMessage>();
        ToolsWindowSetPropertyMessage expectedMsg = new(nameof(ToolsWindow.IsInputTransparent), true);

        // Act
        _sut.MouseDownCommand.Execute(new MouseActionInfo(MouseButton.Left, new PointF()));

        // Assert
        inbox.AssertReceivedSingleMessage(expectedMsg);
    }

    [Fact]
    public void MouseDownCommand_LeftButton_DrawAtMousePosition()
    {
        // Arrange
        PointF expectedPoint = new(123, 321);

        // Act
        _sut.MouseDownCommand.Execute(new MouseActionInfo(MouseButton.Left, expectedPoint));

        // Assert
        _mockCanvas.Verify(x => x.DoDrawing(expectedPoint), Times.Once);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MouseDownCommand_MiddleButton_ToggleToolsWindowVisibility(bool isVisible)
    {
        // Arrange
        _sut.SetField("_isToolsWindowVisible", isVisible);
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowSetPropertyMessage>();
        ToolsWindowSetPropertyMessage expectedMsg = new(nameof(ToolsWindow.IsVisible), !isVisible);

        // Act
        _sut.MouseDownCommand.Execute(new MouseActionInfo(MouseButton.Middle, Point.Empty));

        // Assert
        inbox.AssertReceivedSingleMessage(expectedMsg);
    }

    [Fact]
    public void MouseDownCommand_MiddleButtonWithToolWindowHidden_BeginToolsWindowDragAction()
    {
        // Arrange
        _sut.SetField("_isToolsWindowVisible", false);
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowDragEventMessage>();
        ToolsWindowDragEventMessage expectedMsg = new(DragAction.BeginDrag, new PointF(123, 456));

        // Act
        _sut.MouseDownCommand.Execute(new MouseActionInfo(MouseButton.Middle, expectedMsg.Value.position));


        // Assert
        inbox.AssertReceivedSingleMessage(expectedMsg);
    }

    #endregion

    #region MouseDragCommand

    [Fact]
    public void MouseDragCommand_LeftButton_DrawAtMousePosition()
    {
        // Arrange
        PointF expectedPoint = new(1, 2);

        // Act
        _sut.MouseDragCommand.Execute(new MouseActionInfo(MouseButton.Left, expectedPoint));

        // Assert
        _mockCanvas.Verify(x => x.DoDrawing(expectedPoint), Times.Once);
    }

    [Fact]
    public void MouseDragCommand_MiddleButtonWhileDragging_ContinueToolsWindowDragAction()
    {
        // Arrange
        _sut.SetField("_isToolsWindowDragInProgress", true);
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowDragEventMessage>();
        ToolsWindowDragEventMessage expectedMsg = new(DragAction.ContinueDrag, new PointF(33, 44));

        // Act
        _sut.MouseDragCommand.Execute(new MouseActionInfo(MouseButton.Middle, expectedMsg.Value.position));


        // Assert
        inbox.AssertReceivedSingleMessage(expectedMsg);
    }

    [Fact]
    public void MouseDragCommand_MiddleButtonWhileNotDragging_DoesNothing()
    {
        // Arrange
        _sut.SetField("_isToolsWindowDragInProgress", false);
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowDragEventMessage>();

        // Act
        _sut.MouseDragCommand.Execute(new MouseActionInfo(MouseButton.Middle, PointF.Empty));


        // Assert
        inbox.AssertReceivedNoMessages();
    }

    #endregion

    #region MouseUpCommand

    [Fact]
    public void MouseUpCommand_LeftButton_EnablesToolWindowHitTesting()
    {
        // Arrange
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowSetPropertyMessage>();
        ToolsWindowSetPropertyMessage expectedMsg = new(nameof(ToolsWindow.IsInputTransparent), false);

        // Act
        _sut.MouseUpCommand.Execute(new MouseActionInfo(MouseButton.Left, new PointF()));

        // Assert
        inbox.AssertReceivedSingleMessage(expectedMsg);
    }

    [Fact]
    public void MouseUpCommand_LeftButton_FinishesDrawing()
    {
        // Act
        _sut.MouseUpCommand.Execute(new MouseActionInfo(MouseButton.Left, PointF.Empty));

        // Assert
        _mockCanvas.Verify(x => x.FinishDrawing(), Times.Once);
    }

    [Fact]
    public void MouseUpCommand_MiddleButtonWhileDragging_EndToolWindowDragAction()
    {
        // Arrange
        _sut.SetField("_isToolsWindowDragInProgress", true);
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowDragEventMessage>();
        ToolsWindowDragEventMessage expectedMsg = new(DragAction.EndDrag, new PointF(0, 1));

        // Act
        _sut.MouseUpCommand.Execute(new MouseActionInfo(MouseButton.Middle, expectedMsg.Value.position));

        // Assert
        inbox.AssertReceivedSingleMessage(expectedMsg);
    }

    [Fact]
    public void MouseUpCommand_MiddleButtonWhileNotDragging_DoesNothing()
    {
        _sut.SetField("_isToolsWindowDragInProgress", false);
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowDragEventMessage>();

        // Act
        _sut.MouseUpCommand.Execute(new MouseActionInfo(MouseButton.Middle, PointF.Empty));

        // Assert
        inbox.AssertReceivedNoMessages();
    }

    #endregion

    #region Receive ToolsWindowPropertyChanged

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ReceiveToolsWindowPropertyChanged_IsVisible_SetsIsToolsWindowVisibleField(bool setterValue)
    {
        // Arrange
        const string fieldName = "_isToolsWindowVisible";
        _sut.SetField(fieldName, !setterValue);

        // Act
        _sut.Receive(new ToolsWindowPropertyChangedMessage(setterValue, nameof(ToolsWindow.IsVisible)));
        var actual = _sut.GetField<bool>(fieldName);

        // Assert
        Assert.Equal(setterValue, actual);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ReceiveToolsWindowPropertyChanged_IsVisible_SetsIsToolsWindowDragInProgressField(bool setterValue)
    {
        // Arrange
        const string fieldName = "_isToolsWindowDragInProgress";
        _sut.SetField(fieldName, !setterValue);

        // Act
        _sut.Receive(new ToolsWindowPropertyChangedMessage(setterValue, nameof(ToolsWindow.IsDragInProgress)));
        var actual = _sut.GetField<bool>(fieldName);

        // Assert
        Assert.Equal(setterValue, actual);
    }

    #endregion
}