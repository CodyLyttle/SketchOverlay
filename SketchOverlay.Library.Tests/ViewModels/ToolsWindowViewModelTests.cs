using CommunityToolkit.Mvvm.Messaging;
using Moq;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SUT = SketchOverlay.Library.ViewModels.ToolsWindowViewModel<object, object, object>;

namespace SketchOverlay.Library.Tests.ViewModels;

public class ToolsWindowViewModelTests
{
    private readonly SUT _sut;
    private readonly Mock<ICanvasProperties<object>> _mockCanvasProperties;
    private readonly Mock<IDrawingToolCollection<object, object, object>> _mockDrawingToolsCollection;
    private readonly Mock<ICanvasStateManager> _mockCanvasStateManager;
    private readonly Mock<IColorPalette<object>> _mockColorPalette;
    private readonly IMessenger _messenger = WeakReferenceMessenger.Default;

    public ToolsWindowViewModelTests()
    {
        _mockCanvasProperties = new Mock<ICanvasProperties<object>>();
        _mockCanvasProperties.Setup(x => x.MinimumStrokeSize).Returns(1);
        _mockCanvasProperties.Setup(x => x.MaximumStrokeSize).Returns(10);

        _mockDrawingToolsCollection = new Mock<IDrawingToolCollection<object, object, object>>();
        _mockCanvasStateManager = new Mock<ICanvasStateManager>();
        _mockCanvasStateManager.SetupAllProperties();
        _mockColorPalette = new Mock<IColorPalette<object>>();

        _sut = new SUT(
            _mockCanvasProperties.Object,
            _mockColorPalette.Object,
            _mockDrawingToolsCollection.Object,
            _mockCanvasStateManager.Object,
            _messenger);
    }

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

    #region Undoommand 

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

    #region Properties

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
    public void StrokeSize_SetWithValueLessThanMinimumStrokeSize_DoesNothing()
    {
        // Arrange
        float expectedValue = _mockCanvasProperties.Object.MinimumStrokeSize - 1;

        // Act
        _sut.StrokeSize = expectedValue;

        // Assert
        _mockCanvasProperties.VerifySet(x => x.StrokeSize = expectedValue, Times.Never);
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
    }

    [Fact]
    public void StrokeSize_SetWithValidStrokeSize_DoesNothing()
    {
        // Arrange
        float expectedValue = _mockCanvasProperties.Object.MaximumStrokeSize - 1;

        // Act
        _sut.StrokeSize = expectedValue;

        // Assert
        _mockCanvasProperties.VerifySet(x => x.StrokeSize = expectedValue, Times.Once);
    }

    #endregion
}