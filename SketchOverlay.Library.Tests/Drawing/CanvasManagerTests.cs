using System.Drawing;
using FluentAssertions;
using FluentAssertions.Events;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.Tests.TestHelpers;
using Moq;
using SUT = SketchOverlay.Library.Drawing.Canvas.CanvasManager<object, object, object>;

namespace SketchOverlay.Library.Tests.Drawing;
public class CanvasManagerTests
{
    #region Setups

    private readonly SUT _sut;
    private readonly IMonitor<SUT> _sutMonitor;
    private readonly Stack<int> _drawStackItems;
    private readonly Mock<ICanvasProperties<object>> _mockCanvasProps;
    private readonly Mock<IDrawingStack<object, object>> _mockDrawStack;
    private readonly Mock<IDrawingToolRetriever<object, object>> _mockToolRetriever;
    private readonly Mock<IDrawingTool<object, object>> _mockDrawingTool;

    public CanvasManagerTests()
    {
        _drawStackItems = new Stack<int>();
        _mockDrawStack = new Mock<IDrawingStack<object, object>>();
        _mockDrawStack.Setup(x => x.Count)
            .Returns(() => _drawStackItems.Count);
        _mockDrawStack.Setup(x => x.Clear())
            .Callback(_drawStackItems.Clear);
        _mockDrawStack.Setup(x => x.PushDrawing(It.IsAny<object>()))
            .Callback(() => _drawStackItems.Push(Random.Shared.Next()));
        _mockDrawStack.Setup(x => x.PopDrawing())
            .Returns(() => _drawStackItems.Pop());

        _mockCanvasProps = new Mock<ICanvasProperties<object>>();
        _mockDrawingTool = new Mock<IDrawingTool<object, object>>();
        _mockToolRetriever = new Mock<IDrawingToolRetriever<object, object>>();
        _mockToolRetriever.Setup(x => x.SelectedTool).Returns(_mockDrawingTool.Object);

        _sut = new SUT(
            _mockCanvasProps.Object,
            _mockDrawStack.Object,
            _mockToolRetriever.Object);

        _sutMonitor = _sut.Monitor();
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

    private void AssertDoesNotInvokeCanvasActions()
    {
        _sutMonitor.Should().NotRaise(nameof(SUT.CanClearChanged));
        _sutMonitor.Should().NotRaise(nameof(SUT.CanRedoChanged));
        _sutMonitor.Should().NotRaise(nameof(SUT.CanUndoChanged));
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

    private void SetupCanUndoAndCanClear(int undoCount)
    {
        SetupWithDrawings(undoCount);
        Assert.True(_sut.CanUndo);
        Assert.True(_sut.CanClear);
    }

    private void SetupCanRedo(int redoCount)
    {
        SetupWithDrawings(redoCount);
        for (var i = 0; i < redoCount; i++)
        {
            _sut.Undo();
        }

        Assert.True(_sut.CanRedo);
    }

    #endregion

    [Fact]
    public void IsDrawing_OnInstantiation_IsFalse()
    {
        Assert.False(_sut.IsDrawing);
    }

    [Fact]
    public void DrawingOutput_ReturnsDrawStackOutput()
    {
        Assert.Equal(_mockDrawStack.Object.Output, _sut.DrawingOutput);
    }

    #region CanClear

    [Fact]
    public void CanClear_OnInstantiation_IsFalse()
    {
        Assert.False(_sut.CanClear);
    }

    [Fact]
    public void CanClear_SetWithSameValue_DoesNothing()
    {
        // Act
        _sut.SetPropertyValue(nameof(SUT.CanClear), _sut.CanClear);

        // Assert
        _sutMonitor.Should().NotRaise(nameof(SUT.CanClearChanged));
    }

    [Fact]
    public void CanClear_SetWithNewValue_SetsValueAndInvokesCanClearChanged()
    {
        // Arrange
        bool expectedValue = !_sut.CanClear;

        // Act
        _sut.SetPropertyValue(nameof(SUT.CanClear), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanClear);
        _sutMonitor.Should().Raise(nameof(SUT.CanClearChanged));
    }

    #endregion

    #region CanRedo

    [Fact]
    public void CanRedo_OnInstantiation_IsFalse()
    {
        Assert.False(_sut.CanRedo);
    }

    [Fact]
    public void CanRedo_SetWithSameValue_DoesNothing()
    {
        // Act
        _sut.SetPropertyValue(nameof(SUT.CanRedo), _sut.CanRedo);

        // Assert
        _sutMonitor.Should().NotRaise((nameof(SUT.CanRedoChanged)));
    }

    [Fact]
    public void CanRedo_SetWithNewValue_SetsValueAndInvokesCanRedoChanged()
    {
        // Arrange
        bool expectedValue = !_sut.CanRedo;

        // Act
        _sut.SetPropertyValue(nameof(SUT.CanRedo), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanRedo);
        _sutMonitor.Should().Raise(nameof(SUT.CanRedoChanged));
    }

    #endregion

    #region CanUndo

    [Fact]
    public void CanUndo_OnInstantiation_IsFalse()
    {
        Assert.False(_sut.CanUndo);
    }

    [Fact]
    public void CanUndo_SetWithSameValue_DoesNothing()
    {
        // Act
        _sut.SetPropertyValue(nameof(SUT.CanUndo), _sut.CanUndo);

        // Assert
        _sutMonitor.Should().NotRaise(nameof(SUT.CanRedoChanged));
    }

    [Fact]
    public void CanUndo_SetWithNewValue_SetsValueAndInvokesCanUndoChanged()
    {
        // Arrange
        bool expectedValue = !_sut.CanUndo;

        // Act
        _sut.SetPropertyValue(nameof(SUT.CanUndo), expectedValue);

        // Assert
        Assert.Equal(expectedValue, _sut.CanUndo);
        _sutMonitor.Should().Raise(nameof(SUT.CanUndoChanged));
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
    public void DoDrawing_WhileNotDrawing_InvokesRequestRedraw()
    {
        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        _sutMonitor.Should().Raise(nameof(SUT.RequestRedraw));
    }

    [Fact]
    public void DoDrawing_WhileNotDrawing_DoesNotInvokeCanvasActionEvents()
    {
        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        AssertDoesNotInvokeCanvasActions();
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
    public void DoDrawing_WhileDrawing_InvokesRequestRedraw()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        _sutMonitor.Should().Raise(nameof(SUT.RequestRedraw));
    }

    [Fact]
    public void DoDrawing_WhileDrawing_DoesNotInvokeCanvasActionEvents()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.DoDrawing(new PointF());

        // Assert
        AssertDoesNotInvokeCanvasActions();
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
    public void FinishDrawing_WhileDrawing_InvokesRequestRedraw()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.FinishDrawing();

        // Assert
        _sutMonitor.Should().Raise(nameof(SUT.RequestRedraw));
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
        _sut.DoDrawing(new PointF());
        _sutMonitor.Clear();

        // Act
        _sut.FinishDrawing();

        // Assert
        AssertDoesNotInvokeCanvasActions();
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
    public void CancelDrawing_WhileDrawing_InvokesRequestRedraw()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.CancelDrawing();

        // Assert
        _sutMonitor.Should().Raise(nameof(SUT.RequestRedraw));
    }

    [Fact]
    public void CancelDrawing_WhileDrawing_DoesNotInvokeCanvasActionEvents()
    {
        // Arrange
        SetupIsDrawing();

        // Act
        _sut.CancelDrawing();

        // Assert
        AssertDoesNotInvokeCanvasActions();
    }

    #endregion

    #region Clear

    [Fact]
    public void Clear_WithCanClearFalse_DoesNothing()
    {
        // Arrange
        Assert.False(_sut.CanClear);

        // Act
        _sut.Clear();

        // Assert
        AssertNoInvocations();
    }

    [Fact]
    public void Clear_WithCanClearTrue_ClearsDrawStack()
    {
        // Arrange
        SetupWithDrawings(3);

        // Act
        _sut.Clear();

        // Assert
        _mockDrawStack.Verify(x => x.Clear(), Times.Once);
    }

    [Fact]
    public void Clear_WithAllCanvasActionStatesTrue_SetsAllCanvasActionStatesToFalse()
    {
        // Arrange
        SetupWithDrawings(3);
        _sut.Undo();
        Assert.True(_sut.CanClear);
        Assert.True(_sut.CanRedo);
        Assert.True(_sut.CanUndo);

        // Act
        _sut.Clear();

        // Assert
        Assert.False(_sut.CanClear);
        Assert.False(_sut.CanRedo);
        Assert.False(_sut.CanUndo);
    }

    [Fact]
    public void Clear_WithCanClearTrue_InvokesRequestRedraw()
    {
        // Arrange
        SetupWithDrawings(3);

        // Act
        _sut.Clear();

        // Assert
        _sutMonitor.Should().Raise(nameof(SUT.RequestRedraw));
    }

    #endregion

    #region Redo

    [Fact]
    public void Redo_WithCanRedoFalse_DoesNothing()
    {
        // Arrange
        Assert.False(_sut.CanRedo);

        // Act
        _sut.Redo();

        // Assert
        AssertNoInvocations();
    }

    [Fact]
    public void Redo_WithCanRedoTrue_PushesLastPopToDrawStack()
    {
        // Arrange
        SetupWithDrawings(3);
        int expected = _drawStackItems.Last();
        _sut.Undo();

        // Act
        _sut.Redo();

        // Assert
        _mockDrawStack.Verify(x => x.PushDrawing(It.IsAny<object>()), Times.Once);
        Assert.Equal(expected, _drawStackItems.Last());
    }

    [Fact]
    public void Redo_WithSingleUndo_SetsCorrectCanvasActionStates()
    {
        // Arrange
        SetupCanRedo(1);

        // Act
        _sut.Redo();

        // Arrange
        Assert.True(_sut.CanClear);
        Assert.False(_sut.CanRedo);
        Assert.True(_sut.CanUndo);
    }

    [Fact]
    public void Redo_WithConsecutiveUndo_SetsCorrectCanvasActionStates()
    {
        // Arrange
        SetupCanRedo(3);

        // Act
        _sut.Redo();

        // Arrange
        Assert.True(_sut.CanClear);
        Assert.True(_sut.CanRedo);
        Assert.True(_sut.CanUndo);
    }

    [Fact]
    public void Redo_WithCanRedoTrue_InvokesRequestRedraw()
    {
        // Arrange
        SetupCanRedo(1);

        // Act
        _sut.Redo();

        // Assert
        _sutMonitor.Should().Raise(nameof(SUT.RequestRedraw));
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

    [Fact]
    public void Undo_WithCanUndoTrue_InvokesRequestRedraw()
    {
        // Arrange
        SetupCanUndoAndCanClear(1);

        // Act
        _sut.Undo();

        // Assert
        _sutMonitor.Should().Raise(nameof(SUT.RequestRedraw));
    }

    #endregion
}