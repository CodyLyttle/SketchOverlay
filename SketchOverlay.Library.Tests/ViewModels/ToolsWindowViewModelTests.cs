using System.Drawing;
using CommunityToolkit.Mvvm.Messaging;
using FluentAssertions;
using FluentAssertions.Events;
using Moq;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Messages;
using SketchOverlay.Library.Models;
using SketchOverlay.Library.Tests.TestHelpers;
using SUT = SketchOverlay.Library.ViewModels.ToolsWindowViewModel<object, object, object>;

namespace SketchOverlay.Library.Tests.ViewModels;

public class ToolsWindowViewModelTests
{
    #region Setups

    private readonly SUT _sut;
    private readonly Mock<ICanvasProperties<object>> _mockCanvasProperties;
    private readonly Mock<IDrawingToolCollection<object, object, object>> _mockDrawingToolsCollection;
    private readonly Mock<ICanvasStateManager> _mockCanvasStateManager;
    private readonly IMessenger _messenger = new WeakReferenceMessenger();
    private readonly IMonitor<SUT> _sutMonitor;

    public ToolsWindowViewModelTests()
    {
        _mockCanvasProperties = new Mock<ICanvasProperties<object>>();
        _mockCanvasProperties.Setup(x => x.MinimumStrokeSize).Returns(1);
        _mockCanvasProperties.Setup(x => x.MaximumStrokeSize).Returns(10);

        _mockDrawingToolsCollection = new Mock<IDrawingToolCollection<object, object, object>>();
        _mockDrawingToolsCollection.Setup(x => x.SelectedToolInfo)
            .Returns(new DrawingToolInfo<object, object, object>(
                new Mock<IDrawingTool<object, object>>().Object, "iconSource", "mockTool"));

        _mockCanvasStateManager = new Mock<ICanvasStateManager>();
        _mockCanvasStateManager.SetupAllProperties();

        _sut = new SUT(
            _mockCanvasProperties.Object, 
            new Mock<IColorPalette<object>>().Object,
            _mockDrawingToolsCollection.Object,
            _mockCanvasStateManager.Object,
            _messenger);

        _sutMonitor = _sut.Monitor();
    }

    #endregion

    #region CanvasStateManager

    [Fact]
    public void CanvasStateManager_CanClearChanged_UpdatesCanClearProperty()
    {
        // Arrange
        bool expectedValue = !_sut.CanClear;

        // Act
        _mockCanvasStateManager.Raise(x => x.CanClearChanged += null, null, expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanClear);
    }


    [Fact]
    public void CanvasStateManager_CanRedoChanged_UpdatesCanRedoProperty()
    {
        // Arrange
        bool expectedValue = !_sut.CanRedo;

        // Act
        _mockCanvasStateManager.Raise(x => x.CanRedoChanged += null, null, expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanRedo);
    }

    [Fact]
    public void CanvasStateManager_CanUndoChanged_UpdatesCanUndoProperty()
    {
        // Arrange
        bool expectedValue = !_sut.CanUndo;

        // Act
        _mockCanvasStateManager.Raise(x => x.CanUndoChanged += null, null, expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanUndo);
    }

    #endregion

    #region ClearCommand 

    [Fact]
    public void ClearCommand_CallsClearOnCanvasStateManager()
    {
        // Act
        _sut.ClearCommand.Execute(null);

        // Assert
        _mockCanvasStateManager.Verify(x => x.Clear(), Times.Once);
    }

    [Fact]
    public void ClearCommand_CanClearIsFalse_CanExecuteIsFalse()
    {
        // Arrange
        Assert.False(_sut.CanClear);

        // Assert
        Assert.False(_sut.ClearCommand.CanExecute(null));
    }

    [Fact]
    public void ClearCommand_CanClearIsTrue_CanExecuteIsTrue()
    {
        // Arrange
        _mockCanvasStateManager.Raise(x => x.CanClearChanged += null, null, true);
        Assert.True(_sut.CanClear);

        // Assert
        Assert.True(_sut.ClearCommand.CanExecute(null));
    }

    #endregion

    #region RedoCommand 

    [Fact]
    public void RedoCommand_CallsRedoOnCanvasStateManager()
    {
        // Act
        _sut.RedoCommand.Execute(null);

        // Assert
        _mockCanvasStateManager.Verify(x => x.Redo(), Times.Once);
    }

    [Fact]
    public void RedoCommand_CanRedoIsFalse_CanExecuteIsFalse()
    {
        // Arrange
        Assert.False(_sut.CanRedo);

        // Assert
        Assert.False(_sut.RedoCommand.CanExecute(null));
    }

    [Fact]
    public void RedoCommand_CanRedoIsTrue_CanExecuteIsTrue()
    {
        // Arrange
        _mockCanvasStateManager.Raise(x => x.CanRedoChanged += null, null, true);
        Assert.True(_sut.CanRedo);

        // Assert
        Assert.True(_sut.RedoCommand.CanExecute(null));
    }

    #endregion

    #region UndoCommand 

    [Fact]
    public void UndoCommand_CallsUndoOnCanvasStateManager()
    {
        // Act
        _sut.UndoCommand.Execute(null);

        // Assert
        _mockCanvasStateManager.Verify(x => x.Undo(), Times.Once);
    }

    [Fact]
    public void UndoCommand_CanUndoIsFalse_CanExecuteIsFalse()
    {
        // Arrange
        Assert.False(_sut.CanUndo);

        // Assert
        Assert.False(_sut.UndoCommand.CanExecute(null));
    }

    [Fact]
    public void UndoCommand_CanUndoIsTrue_CanExecuteIsTrue()
    {
        // Arrange
        _mockCanvasStateManager.Raise(x => x.CanUndoChanged += null, null, true);
        Assert.True(_sut.CanUndo);

        // Assert
        Assert.True(_sut.UndoCommand.CanExecute(null));
    }

    #endregion

    #region StrokeSize

    [Fact]
    public void MinimumStrokeSize_ReturnsCanvasPropertiesMinimumStrokeSize()
    {
        // Arrange
        Assert.Equal(_mockCanvasProperties.Object.MinimumStrokeSize, _sut.MinimumStrokeSize);
    }

    [Fact]
    public void MaximumStrokeSize_ReturnsCanvasPropertiesMaximumStrokeSize()
    {
        // Arrange
        _mockCanvasProperties.Setup(x => x.MaximumStrokeSize)
            .Returns(32);

        // Arrange
        Assert.Equal(_mockCanvasProperties.Object.MaximumStrokeSize, _sut.MaximumStrokeSize);
    }

    [Fact]
    public void StrokeSize_GetsCanvasPropertiesStrokeSize()
    {
        // Arrange
        _mockCanvasProperties.Object.StrokeSize = 
            _mockCanvasProperties.Object.MinimumStrokeSize + 1;

        // Assert
        Assert.Equal(_mockCanvasProperties.Object.StrokeSize, _sut.StrokeSize);
    }

    [Fact]
    public void StrokeSize_SetWithValueOutsideValidRange_DoesNothing()
    {
        // Arrange
        float belowMinimum = _mockCanvasProperties.Object.MinimumStrokeSize - 1;
        float aboveMaximum = _mockCanvasProperties.Object.MaximumStrokeSize + 1;

        // Act
        _sut.StrokeSize = belowMinimum;
        _sut.StrokeSize = aboveMaximum;

        // Assert
        _mockCanvasProperties.VerifySet(x => x.StrokeSize = belowMinimum, Times.Never);
        _mockCanvasProperties.VerifySet(x => x.StrokeSize = aboveMaximum, Times.Never);
        _sutMonitor.Should().NotRaisePropertyChangeFor(x => x.StrokeSize);
    }

    [Fact]
    public void StrokeSize_SetWithValueGreaterThanMaximumStrokeSize_DoesNothing()
    {
        // Arrange
        float expectedValue = _mockCanvasProperties.Object.MaximumStrokeSize + 1;

        // Act
        _sut.StrokeSize = expectedValue;

        // Assert
        _mockCanvasProperties.VerifySet(x => x.StrokeSize = expectedValue, Times.Never);
        _sutMonitor.Should().NotRaisePropertyChangeFor(x => x.StrokeSize);
    }

    [Fact]
    public void StrokeSize_SetWithValidStrokeSize_UpdatesStrokeSize()
    {
        // Arrange
        float expectedValue = _mockCanvasProperties.Object.MaximumStrokeSize - 1;

        // Act
        _sut.StrokeSize = expectedValue;

        // Assert
        _mockCanvasProperties.VerifySet(x => x.StrokeSize = expectedValue, Times.Once);
        _sutMonitor.Should().RaisePropertyChangeFor(x => x.StrokeSize);
    }

    #endregion

    #region FillColor

    [Fact]
    public void FillColor_GetsCanvasPropertiesFillColor()
    {
        // Arrange
        _mockCanvasProperties.Object.FillColor = "Blue";
        
        // Assert
        Assert.Equal(_mockCanvasProperties.Object.FillColor, _sut.FillColor);
    }

    [Fact]
    public void FillColor_SetWithNull_DoesNothing()
    {
        // Act
        // UI bindings have the ability to set non-nullable properties to null.
        _sut.FillColor = null!;

        // Assert
        _mockCanvasProperties.VerifySet(x => x.FillColor = null!, Times.Never);
    }

    [Fact]
    public void FillColor_SetWithValidValue_SetsCanvasPropertiesFillColor()
    {
        // Arrange
        object expectedValue = 16;

        // Act
        _sut.FillColor = expectedValue;

        // Assert
        _mockCanvasProperties.VerifySet(x => x.FillColor = expectedValue, Times.Once);
    }

    #endregion

    #region StrokeColor

    [Fact]
    public void StrokeColor_GetsCanvasPropertiesStrokeColor()
    {
        // Arrange
        _mockCanvasProperties.Object.StrokeColor = "Red";

        // Assert
        Assert.Equal(_mockCanvasProperties.Object.StrokeColor, _sut.StrokeColor);
    }

    [Fact]
    public void StrokeColor_SetWithNull_DoesNothing()
    {
        // Act
        // UI bindings have the ability to set non-nullable properties to null.
        _sut.StrokeColor = null!;

        // Assert
        _mockCanvasProperties.VerifySet(x => x.StrokeColor = null!, Times.Never);
    }

    [Fact]
    public void StrokeColor_SetWithValidValue_SetsCanvasPropertiesStrokeColor()
    {
        // Arrange
        object expectedValue = 16;

        // Act
        _sut.StrokeColor = expectedValue;

        // Assert
        _mockCanvasProperties.VerifySet(x => x.StrokeColor = expectedValue, Times.Once);
    }

    #endregion

    #region SelectedToolInfo

    [Fact]
    public void SelectedToolInfo_GetsDrawingToolsSelectedToolInfo()
    {
        // Assert
        Assert.Equal(_mockDrawingToolsCollection.Object.SelectedToolInfo, _sut.SelectedToolInfo);
    }

    [Fact]
    public void SelectedToolInfo_SetWithNullOrSameValue_DoesNothing()
    {
        // Act
        _sut.SelectedToolInfo = null!;
        _sut.SelectedToolInfo = _sut.SelectedToolInfo;

        // Assert
        _mockDrawingToolsCollection.VerifySet(x => x.SelectedToolInfo = null!, Times.Never());
        _mockDrawingToolsCollection.VerifySet(x => x.SelectedToolInfo = _sut.SelectedToolInfo, Times.Never());
        _sutMonitor.Should().NotRaisePropertyChangeFor(x => x.SelectedToolInfo);
    }

    [Fact]
    public void SelectedToolInfo_SetWithNewValue_UpdatesSelectedDrawingToolInfo()
    {
        // Arrange
        DrawingToolInfo<object, object, object> expectedToolInfo = new(
            new Mock<IDrawingTool<object, object>>().Object, "icon2", "someOtherTool");

        // Act
        _sut.SelectedToolInfo = expectedToolInfo;

        // Assert
        _mockDrawingToolsCollection.VerifySet(x => x.SelectedToolInfo = expectedToolInfo, Times.Once);
        _sutMonitor.Should().RaisePropertyChangeFor(x => x.SelectedToolInfo);
    }

    #endregion

    #region IsDragInProgress

    [Fact]
    public void IsDragInProgress_SetWithSameValue_DoesNothing()
    {
        // Arrange
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowPropertyChangedMessage>();

        // Act
        _sut.SetPropertyValue(nameof(SUT.IsDragInProgress), _sut.IsDragInProgress);

        // Assert
        inbox.AssertReceivedNoMessages();
        _sutMonitor.Should().NotRaisePropertyChangeFor(x => x.IsDragInProgress);
    }

    [Fact]
    public void IsDragInProgress_SetWithNewValue_UpdatesIsDragInProgress()
    {
        // Arrange
        bool expectedValue = !_sut.IsDragInProgress;
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowPropertyChangedMessage>();
        ToolsWindowPropertyChangedMessage expectedMessage = new(expectedValue, nameof(SUT.IsDragInProgress));

        // Act
        _sut.SetPropertyValue(nameof(SUT.IsDragInProgress), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.IsDragInProgress);
        inbox.AssertReceivedSingleMessage(expectedMessage);
        _sutMonitor.Should().RaisePropertyChangeFor(x => x.IsDragInProgress);
    }

    #endregion

    #region IsVisible

    [Fact]
    public void IsVisible_SetWithSameValue_DoesNothing()
    {
        // Arrange
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowPropertyChangedMessage>();

        // Act
        _sut.SetPropertyValue(nameof(SUT.IsVisible), _sut.IsVisible);

        // Assert
        inbox.AssertReceivedNoMessages();
        _sutMonitor.Should().NotRaisePropertyChangeFor(x=> x.IsVisible);
    }

    [Fact]
    public void IsVisible_SetWithNewValue_UpdatesIsVisible()
    {
        // Arrange
        bool expectedValue = !_sut.IsVisible;
        using MessageInbox inbox = _messenger.RegisterInbox<ToolsWindowPropertyChangedMessage>();
        ToolsWindowPropertyChangedMessage expectedMessage = new(expectedValue, nameof(SUT.IsVisible));

        // Act
        _sut.SetPropertyValue(nameof(SUT.IsVisible), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.IsVisible);
        inbox.AssertReceivedSingleMessage(expectedMessage);
        _sutMonitor.Should().RaisePropertyChangeFor(x => x.IsVisible);
    }

    #endregion

    #region ReceiveSetProperty

    [Fact]
    public void ReceiveSetIsVisible_SetsIsVisibleProperty()
    {
        // Arrange
        bool expectedValue = !_sut.IsVisible;

        // Act
        _messenger.Send(new ToolsWindowSetPropertyMessage(nameof(SUT.IsVisible), expectedValue));

        // Assert
        Assert.Equal(expectedValue, _sut.IsVisible);
    }

    [Fact]
    public void ReceiveSetIsInputTransparent_SetsIsInputTransparentProperty()
    {
        // Arrange
        bool expectedValue = !_sut.IsInputTransparent;

        // Act
        _messenger.Send(new ToolsWindowSetPropertyMessage(nameof(SUT.IsInputTransparent), expectedValue));

        // Assert
        Assert.Equal(expectedValue, _sut.IsInputTransparent);
    }

    [Fact]
    public void ReceiveUnhandledMessage_DoesNotThrowException()
    {
        _messenger.Send(new ToolsWindowSetPropertyMessage("SomeProperty", 32));
    }

    #endregion

    #region ReceiveDragAction

    [Fact]
    public void ReceiveBeginDragMessage_WhileDragging_ThrowsInvalidOperationException()
    {
        // Arrange
        _sut.SetPropertyValue(nameof(SUT.IsDragInProgress), true);

        // Act/Assert
        Assert.Throws<InvalidOperationException>(() => _messenger.Send(
            new ToolsWindowDragEventMessage(DragAction.BeginDrag, new PointF(100, 200))));
    }

    [Fact]
    public void ReceiveBeginDragMessage_WhileNotDragging_SetsWindowState()
    {
        // Arrange
        LibraryThickness oldWindowMargin = _sut.WindowMargin;
        _sut.SetPropertyValue(nameof(SUT.IsDragInProgress), false);
        _sut.SetPropertyValue(nameof(SUT.IsInputTransparent), false);

        // Act
        _messenger.Send(new ToolsWindowDragEventMessage(DragAction.BeginDrag, new PointF(100, 200)));

        // Assert
        Assert.True(_sut.IsDragInProgress);
        Assert.True(_sut.IsInputTransparent);
        Assert.NotEqual(oldWindowMargin, _sut.WindowMargin);
    }

    [Fact]
    public void ReceiveContinueDragMessage_WhileNotDragging_ThrowsInvalidOperationException()
    {
        // Arrange
        _sut.SetPropertyValue(nameof(SUT.IsDragInProgress), false);

        // Act/Assert
        Assert.Throws<InvalidOperationException>(() => _messenger.Send(
            new ToolsWindowDragEventMessage(DragAction.ContinueDrag, new PointF(100, 200))));
    }

    [Fact]
    public void ReceiveContinueDragMessage_WhileDragging_SetsWindowState()
    {
        // Arrange
        LibraryThickness oldWindowMargin = _sut.WindowMargin;
        _sut.SetPropertyValue(nameof(SUT.IsDragInProgress), true);

        // Act
        _messenger.Send(new ToolsWindowDragEventMessage(DragAction.ContinueDrag, new PointF(100, 200)));

        // Assert
        Assert.NotEqual(oldWindowMargin, _sut.WindowMargin);
    }


    [Fact]
    public void ReceiveEndDragMessage_WhileNotDragging_ThrowsInvalidOperationException()
    {
        // Arrange
        _sut.SetPropertyValue(nameof(SUT.IsDragInProgress), false);

        // Act/Assert
        Assert.Throws<InvalidOperationException>(() => _messenger.Send(
            new ToolsWindowDragEventMessage(DragAction.EndDrag, new PointF(100, 200))));
    }

    [Fact]
    public void ReceiveEndDragMessage_WhileDragging_SetsWindowState()
    {
        // Arrange
        LibraryThickness oldWindowMargin = _sut.WindowMargin;
        _sut.SetPropertyValue(nameof(SUT.IsDragInProgress), true);
        _sut.SetPropertyValue(nameof(SUT.IsInputTransparent), true);

        // Act
        _messenger.Send(new ToolsWindowDragEventMessage(DragAction.EndDrag, new PointF(100, 200)));

        // Assert
        Assert.False(_sut.IsDragInProgress);
        Assert.False(_sut.IsInputTransparent);
        Assert.NotEqual(oldWindowMargin, _sut.WindowMargin);
    }

    [Fact]
    public void ReceiveUnhandledDragAction_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _messenger.Send(
            new ToolsWindowDragEventMessage((DragAction)999, new PointF())));
    }

    #endregion
}