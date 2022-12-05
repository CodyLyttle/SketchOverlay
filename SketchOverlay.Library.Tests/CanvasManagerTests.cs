using System.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Tests.TestHelpers;
using Moq;
using SUT = SketchOverlay.Library.Drawing.Canvas.CanvasManager<object, object, object>;

namespace SketchOverlay.Library.Tests;
public class CanvasManagerTests
{
    #region Setups

    private int _mockStackCount = 0;
    private readonly SUT _sut;
    private readonly Mock<ICanvasProperties<object>> _mockCanvasProps;
    private readonly Mock<IDrawingStack<object, object>> _mockDrawStack;
    private readonly Mock<IDrawingToolRetriever<object, object>> _mockToolRetriever;
    private readonly Mock<IDrawingTool<object, object>> _mockDrawingTool;

    public CanvasManagerTests()
    {
        _mockDrawStack = new Mock<IDrawingStack<object, object>>();
        _mockDrawStack.Setup(x => x.PushDrawing(It.IsAny<object>()))
            .Callback(() => _mockStackCount++);
        _mockDrawStack.Setup(x => x.PopDrawing())
            .Callback(() => _mockStackCount--);
        _mockDrawStack.Setup(x => x.Count)
            .Returns(() => _mockStackCount);

        _mockCanvasProps = new Mock<ICanvasProperties<object>>();
        _mockDrawingTool = new Mock<IDrawingTool<object, object>>();
        _mockToolRetriever = new Mock<IDrawingToolRetriever<object, object>>();
        _mockToolRetriever.Setup(x => x.SelectedTool).Returns(_mockDrawingTool.Object);

        _sut = new SUT(
            _mockCanvasProps.Object,
            _mockDrawStack.Object,
            _mockToolRetriever.Object);
    }

    private void ResetMockInvocations()
    {
        _mockCanvasProps.Invocations.Clear();
        _mockDrawStack.Invocations.Clear();
        _mockDrawingTool.Invocations.Clear();
        _mockToolRetriever.Invocations.Clear();
    }

    private void AssertNoInvocations()
    {
        _mockCanvasProps.VerifyNoOtherCalls();
        _mockDrawStack.VerifyNoOtherCalls();
        _mockDrawingTool.VerifyNoOtherCalls();
        _mockToolRetriever.VerifyNoOtherCalls();
    }

    private EventCatcher GetRequestRedrawEventCatcher()
    {
        EventCatcher eventCatcher = new();
        _sut.RequestRedraw += eventCatcher.OnReceived;

        return eventCatcher;
    }

    private EventCatcher<bool> GetCanvasActionsEventCatcher()
    {
        EventCatcher<bool> eventCatcher = new();
        _sut.CanClearChanged += eventCatcher.OnReceived;
        _sut.CanRedoChanged += eventCatcher.OnReceived;
        _sut.CanUndoChanged += eventCatcher.OnReceived;

        return eventCatcher;
    }

    private void SetupIsDrawing()
    {
        if (_sut.IsDrawing)
            return;

        _sut.DoDrawing(new PointF());
        ResetMockInvocations();
        Assert.True(_sut.IsDrawing);
    }

    private void SetupWithDrawings(int drawingCount)
    {
        Assert.True(drawingCount > 0);
        for (var i = 0; i < drawingCount; i++)
        {
            _sut.DoDrawing(new PointF());
            _sut.FinishDrawing();
        }

        ResetMockInvocations();
    }

    private void SetupCanUndoAndCanClear(int drawingCount)
    {
        SetupWithDrawings(drawingCount);
        Assert.True(_sut.CanUndo);
        Assert.True(_sut.CanClear);
    }

    #endregion

    #region Properties

    [Fact]
    public void IsDrawing_OnInstantiation_IsFalse()
    {
        Assert.False(_sut.IsDrawing);
    }

    [Fact]
    public void CanClear_SetWithSameValue_DoesNothing()
    {
        // Arrange
        EventCatcher<bool> eventCatcher = new();
        _sut.CanClearChanged += eventCatcher.OnReceived;
        bool expectedValue = _sut.CanClear;

        // Act
        _sut.SetPropertyValue(nameof(SUT.CanClear), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanClear);
        Assert.Empty(eventCatcher.Received);
    }

    [Fact]
    public void CanRedo_SetWithSameValue_DoesNothing()
    {
        // Arrange
        EventCatcher<bool> eventCatcher = new();
        _sut.CanRedoChanged += eventCatcher.OnReceived;
        bool expectedValue = _sut.CanRedo;

        // Act
        _sut.SetPropertyValue(nameof(SUT.CanRedo), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanRedo);
        Assert.Empty(eventCatcher.Received);
    }

    [Fact]
    public void CanUndo_SetWithSameValue_DoesNothing()
    {
        // Arrange
        EventCatcher<bool> eventCatcher = new();
        _sut.CanUndoChanged += eventCatcher.OnReceived;
        bool expectedValue = _sut.CanUndo;

        // Act
        _sut.SetPropertyValue(nameof(SUT.CanUndo), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanUndo);
        Assert.Empty(eventCatcher.Received);
    }

    [Fact]
    public void CanClear_PropertyChanged_InvokesCanClearChangedEvent()
    {
        // Arrange
        EventCatcher<bool> eventCatcher = new();
        _sut.CanClearChanged += eventCatcher.OnReceived;
        bool expectedValue = !_sut.CanClear;

        // Act
        _sut.SetPropertyValue(nameof(SUT.CanClear), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanClear);
        Assert.Single(eventCatcher.Received);
    }

    [Fact]
    public void CanRedo_PropertyChanged_InvokesCanRedoChangedEvent()
    {
        // Arrange
        EventCatcher<bool> eventCatcher = new();
        _sut.CanRedoChanged += eventCatcher.OnReceived;
        bool expectedValue = !_sut.CanRedo;

        // Act
        _sut.SetPropertyValue(nameof(SUT.CanRedo), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanRedo);
        Assert.Single(eventCatcher.Received);
    }

    [Fact]
    public void CanUndo_PropertyChanged_InvokesCanUndoChangedEvent()
    {
        // Arrange
        EventCatcher<bool> eventCatcher = new();
        _sut.CanUndoChanged += eventCatcher.OnReceived;
        bool expectedValue = !_sut.CanUndo;

        // Act
        _sut.SetPropertyValue(nameof(SUT.CanUndo), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanUndo);
        Assert.Single(eventCatcher.Received);
    }

    [Fact]
    public void DrawingOutput_ReturnsDrawStackOutput()
    {
        Assert.Equal(_mockDrawStack.Object.Output, _sut.DrawingOutput); 
    }

    #endregion

    #region DoDrawing

    [Fact]
    public void DoDrawing_WhileNotDrawing_SetsIsDrawingToTrue()
    {
        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.True(_sut.IsDrawing);
    }

    [Fact]
    public void DoDrawing_WhileNotDrawing_CreatesDrawingUsingCanvasProperties()
    {
        // Arrange
        PointF expectedPoint = new(123, 321);

        // Act
        _sut.DoDrawing(expectedPoint);

        // Assert
        _mockDrawingTool.Verify(x => x.CreateDrawing(_mockCanvasProps.Object, expectedPoint));
        _mockDrawingTool.VerifyNoOtherCalls();
    }

    [Fact]
    public void DoDrawing_WhileNotDrawing_PushesDrawingToDrawStack()
    {
        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        _mockDrawStack.Verify(x => x.PushDrawing(It.IsAny<object>()), Times.Once);
        _mockDrawStack.VerifyNoOtherCalls();
    }

    [Fact]
    public void DoDrawing_WhileNotDrawing_InvokesRequestsRedrawEvent()
    {
        // Arrange
        EventCatcher eventCatcher = GetRequestRedrawEventCatcher();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.Single(eventCatcher.Received);
    }

    [Fact]
    public void DoDrawing_WhileNotDrawing_DoesNotInvokeCanvasActionEvents()
    {
        // Arrange
        EventCatcher<bool> eventCatcher = GetCanvasActionsEventCatcher();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.Empty(eventCatcher.Received);
    }

    [Fact]
    public void DoDrawing_WhileDrawing_IsDrawingRemainsTrue()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.True(_sut.IsDrawing);
    }

    [Fact]
    public void DoDrawing_WhileDrawing_DoesNotCreateDrawing()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        _mockDrawingTool.Verify(x =>
            x.CreateDrawing(It.IsAny<ICanvasProperties<object>>(), It.IsAny<PointF>()),
            Times.Never);
    }

    [Fact]
    public void DoDrawing_WhileDrawing_DoesNotModifyDrawStack()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        _mockDrawStack.VerifyNoOtherCalls();
    }

    [Fact]
    public void DoDrawing_WhileDrawing_UpdatesCurrentDrawingWithPointArgument()
    {
        // Arrange
        SetupIsDrawing();
        PointF expectedPoint = new(1, 2);

        // Act
        _sut.DoDrawing(expectedPoint);

        // Assert
        _mockDrawingTool.Verify(x => x.UpdateDrawing(expectedPoint), Times.Once);
        _mockDrawingTool.VerifyNoOtherCalls();
    }

    [Fact]
    public void DoDrawing_WhileDrawing_InvokesRequestRedrawEvent()
    {
        // Arrange
        SetupIsDrawing();
        EventCatcher eventCatcher = GetRequestRedrawEventCatcher();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.Single(eventCatcher.Received);
    }

    [Fact]
    public void DoDrawing_WhileDrawing_DoesNotInvokeCanvasActionEvents()
    {
        // Arrange
        SetupIsDrawing();
        EventCatcher<bool> eventCatcher = GetCanvasActionsEventCatcher();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        Assert.Empty(eventCatcher.Received);
    }

    #endregion

    #region FinishDrawing

    [Fact]
    public void FinishDrawing_WhileNotDrawing_DoesNothing()
    {
        // Act
        _sut.FinishDrawing();

        // Assert
        Assert.False(_sut.IsDrawing);
        AssertNoInvocations();
    }

    [Fact]
    public void FinishDrawing_WhileDrawing_SetsIsDrawingToFalse()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.FinishDrawing();

        // Assert
        Assert.False(_sut.IsDrawing);
    }

    [Fact]
    public void FinishDrawing_WhileDrawing_FinishesCurrentDrawing()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.FinishDrawing();

        // Assert
        _mockDrawingTool.Verify(x => x.FinishDrawing());
        _mockDrawingTool.VerifyNoOtherCalls();
    }

    [Fact]
    public void FinishDrawing_WhileDrawing_InvokesRequestRedrawEvent()
    {
        // Arrange
        SetupIsDrawing();
        EventCatcher eventCatcher = GetRequestRedrawEventCatcher();

        // Act
        _sut.FinishDrawing();

        // Assert
        Assert.Single(eventCatcher.Received);
    }

    [Fact]
    public void FinishDrawing_FirstDrawingInDrawStack_SetsCanClearToTrue()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.FinishDrawing();

        // Assert
        Assert.True(_sut.CanClear);
    }

    [Fact]
    public void FinishDrawing_FirstDrawingInDrawStack_SetsCanRedoToFalse()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.FinishDrawing();

        // Assert
        Assert.False(_sut.CanRedo);
    }

    [Fact]
    public void FinishDrawing_FirstDrawingInDrawStack_SetsCanUndoToTrue()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.FinishDrawing();

        // Assert
        Assert.True(_sut.CanUndo);
    }

    [Fact]
    public void FinishDrawing_OnConsecutiveDrawing_DoesNotInvokeCanvasActionEvents()
    {
        // Arrange
        SetupWithDrawings(1);
        EventCatcher<bool> eventCatcher = GetCanvasActionsEventCatcher();

        // Act
        _sut.DoDrawing(new PointF());
        _sut.FinishDrawing();

        // Assert
        Assert.Empty(eventCatcher.Received);
    }

    [Fact]
    public void FinishDrawing_WithCanRedoTrue_SetsCanRedoToFalse()
    {
        // Arrange
        SetupWithDrawings(2);
        _sut.Undo();
        Assert.True(_sut.CanRedo);

        // Act
        _sut.DoDrawing(new PointF());
        _sut.FinishDrawing();

        // Assert
        Assert.False(_sut.CanRedo);
    }

    #endregion

    #region CancelDrawing

    [Fact]
    public void CancelDrawing_WhileNotDrawing_DoesNothing()
    {
        // Act
        _sut.CancelDrawing();

        // Assert
        Assert.False(_sut.IsDrawing);
        AssertNoInvocations();
    }

    [Fact]
    public void CancelDrawing_WhileDrawing_SetsIsDrawingToFalse()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.CancelDrawing();

        // Assert
        Assert.False(_sut.IsDrawing);
    }

    [Fact]
    public void CancelDrawing_WhileDrawing_FinishesCurrentDrawing()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.CancelDrawing();

        // Assert
        _mockDrawingTool.Verify(x => x.FinishDrawing());
        _mockDrawingTool.VerifyNoOtherCalls();
    }

    [Fact]
    public void CancelDrawing_WhileDrawing_PopsDrawingFromDrawStack()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.CancelDrawing();

        // Assert
        _mockDrawStack.Verify(x => x.PopDrawing());
        _mockDrawStack.VerifyNoOtherCalls();
    }

    [Fact]
    public void CancelDrawing_WhileDrawing_InvokesRequestRedrawEvent()
    {
        // Arrange
        SetupIsDrawing();
        EventCatcher eventCatcher = new();
        _sut.RequestRedraw += eventCatcher.OnReceived;

        // Act
        _sut.CancelDrawing();

        // Assert
        Assert.Single(eventCatcher.Received);
    }

    [Fact]
    public void CancelDrawing_WhileDrawing_DoesNotInvokeCanvasActionEvents()
    {
        // Arrange
        EventCatcher<bool> eventCatcher = GetCanvasActionsEventCatcher();
        SetupIsDrawing();

        // Act
        _sut.CancelDrawing();

        // Assert
        Assert.Empty(eventCatcher.Received);
    }

    #endregion

    #region Undo

    [Fact]
    public void Undo_WithCanUndoFalse_DoesNothing()
    {
        // Arrange
        Assert.False(_sut.CanUndo);

        // Act
        _sut.Undo();

        // Assert
        AssertNoInvocations();
    }

    [Fact]
    public void Undo_WithCanUndoTrue_PopsDrawingFromDrawStack()
    {
        // Arrange
        SetupCanUndoAndCanClear(1);

        // Act
        _sut.Undo();

        // Assert
        _mockDrawStack.Verify(x => x.PopDrawing(), Times.Once);
    }

    [Fact]
    public void Undo_WithSingleDrawing_SetsCorrectCanvasActionStates()
    {
        // Arrange
        SetupCanUndoAndCanClear(1);

        // Act
        _sut.Undo();

        // Arrange
        Assert.False(_sut.CanClear);
        Assert.True(_sut.CanRedo);
        Assert.False(_sut.CanUndo);
    }

    [Fact]
    public void Undo_WithConsecutiveDrawings_SetsCorrectCanvasActionStates()
    {

        // Arrange
        SetupCanUndoAndCanClear(2);

        // Act
        _sut.Undo();

        // Arrange
        Assert.True(_sut.CanClear);
        Assert.True(_sut.CanRedo);
        Assert.True(_sut.CanUndo);
    }

    #endregion
}