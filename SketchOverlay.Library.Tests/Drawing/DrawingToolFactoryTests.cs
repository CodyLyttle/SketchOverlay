using Moq;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Library.Tests.Drawing;

public class DrawingToolFactoryTests
{
    #region Setups

    private class MockDrawingToolFactory : DrawingToolFactory<object, object, object>
    {
        public object ImageSourceReturnObject { get; set; } = new();
        public IEllipseTool<object, object>? EllipseTool { get; private set; }
        public ILineTool<object, object>? LineTool { get; private set; }
        public IPaintBrushTool<object, object>? PaintBrushTool { get; private set; }
        public IRectangleTool<object, object>? RectangleTool { get; private set; }

        public void WithMockEllipse()
            => EllipseTool = new Mock<IEllipseTool<object, object>>().Object;

        public void WithMockLine()
            => LineTool = new Mock<ILineTool<object, object>>().Object;

        public void WithMockPaintBrush()
            => PaintBrushTool = new Mock<IPaintBrushTool<object, object>>().Object;

        public void WithMockRectangle()
            => RectangleTool = new Mock<IRectangleTool<object, object>>().Object;

        // Call base method unless a mock tool has been set via the respective WithMock... method.
        protected override IEllipseTool<object, object>? CreateEllipseTool()
            => EllipseTool ?? base.CreateEllipseTool();

        protected override ILineTool<object, object>? CreateLineTool()
            => LineTool ?? base.CreateLineTool();

        protected override IPaintBrushTool<object, object>? CreatePaintBrushTool()
            => PaintBrushTool ?? base.CreatePaintBrushTool();

        protected override IRectangleTool<object, object>? CreateRectangleTool()
            => RectangleTool ?? base.CreateRectangleTool();

        protected override object CreateImageSource(string fileName) => ImageSourceReturnObject;
    }

    private readonly MockDrawingToolFactory _sut;

    public DrawingToolFactoryTests()
    {
        _sut = new MockDrawingToolFactory();
    }

    #endregion

    #region CreateDrawingToolCollection

    [Fact]
    public void CreateDrawingToolCollection_CreatesEllipseTool()
    {
        // Arrange
        _sut.WithMockEllipse();

        // Act
        IDrawingToolCollection<object, object, object> returnedTools = _sut.CreateDrawingToolCollection();

        // Assert
        Assert.NotNull(_sut.EllipseTool);
        Assert.Equal(_sut.EllipseTool, returnedTools.First().Tool);
    }

    [Fact]
    public void CreateDrawingToolCollection_CreatesLineTool()
    {
        // Arrange
        _sut.WithMockLine();

        // Act
        IDrawingToolCollection<object, object, object> returnedTools = _sut.CreateDrawingToolCollection();

        // Assert
        Assert.NotNull(_sut.LineTool);
        Assert.Equal(_sut.LineTool, returnedTools.First().Tool);
    }


    [Fact]
    public void CreateDrawingToolCollection_CreatesPaintBrushTool()
    {
        // Arrange
        _sut.WithMockPaintBrush();

        // Act
        IDrawingToolCollection<object, object, object> returnedTools = _sut.CreateDrawingToolCollection();

        // Assert
        Assert.NotNull(_sut.PaintBrushTool);
        Assert.Equal(_sut.PaintBrushTool, returnedTools.First().Tool);
    }

    [Fact]
    public void CreateDrawingToolCollection_CreatesRectangleTool()
    {
        // Arrange
        _sut.WithMockRectangle();

        // Act
        IDrawingToolCollection<object, object, object> returnedTools = _sut.CreateDrawingToolCollection();

        // Assert
        Assert.NotNull(_sut.RectangleTool);
        Assert.Equal(_sut.RectangleTool, returnedTools.First().Tool);
    }

    [Fact]
    public void CreateDrawingToolCollection_WithNullDrawingTools_CollectionDoesNotContainNullTools()
    {
        // Arrange
        _sut.WithMockLine();
        _sut.WithMockEllipse();

        // Act
        IDrawingToolCollection<object, object, object> returnedTools = _sut.CreateDrawingToolCollection();

        // Assert
        Assert.Equal(2, returnedTools.Count);
    }

    [Fact]
    public void CreateDrawingToolCollection_UsesCreateImageSourceOverride()
    {
        // Arrange
        _sut.ImageSourceReturnObject = "ImageSourceObj";
        _sut.WithMockEllipse();

        // Act
        IDrawingToolCollection<object, object, object> returnedTools = _sut.CreateDrawingToolCollection();

        // Assert
        Assert.Equal(_sut.ImageSourceReturnObject, returnedTools.First().IconSource);
    }

    #endregion
}