using SketchOverlay.Drawing.Canvas;

namespace SketchOverlay.Tests;

public class MauiProgramTests
{
    // Enforce test execution order by combining related tests.
    [Fact]
    public void GetService_CombinedTests()
    {
        // While _serviceProvider is null.
        Assert.Throws<NullReferenceException>(MauiProgram.GetService<object>);

        // Assign value to _serviceProvider.
        MauiProgram.CreateMauiApp();

        // Service doesn't exist.
        Assert.Throws<ArgumentOutOfRangeException>(MauiProgram.GetService<MauiProgramTests>);

        // Service exists.
        MauiProgram.GetService<IDrawingCanvas>();
    }
}