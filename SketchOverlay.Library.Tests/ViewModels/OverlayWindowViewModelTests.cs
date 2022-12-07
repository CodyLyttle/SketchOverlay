using CommunityToolkit.Mvvm.Messaging;
using Moq;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Messages;
using SketchOverlay.Library.Tests.TestHelpers;
using SUT = SketchOverlay.Library.ViewModels.OverlayWindowViewModel<object, object, object, object>;

namespace SketchOverlay.Library.Tests.ViewModels;

public class OverlayWindowViewModelTests
{
    #region Setups

    private readonly SUT _sut;
    private readonly IMessenger _messenger = WeakReferenceMessenger.Default;
    private readonly Mock<ICanvasManager<object>> _mockCanvas;

    public OverlayWindowViewModelTests()
    {
        _mockCanvas = new Mock<ICanvasManager<object>>();
        _mockCanvas.SetupAllProperties();
        _sut = new SUT(_mockCanvas.Object, _messenger);;
    }

    #endregion

    #region Messenger

    [Fact]
    public void DrawingWindowPropertyChangedMessage_OnInstantiation_MessageIsRegistered()
    {
        _messenger.AssertIsRegistered<DrawingWindowPropertyChangedMessage>(_sut);
    }

    [Fact]
    public void OverlayWindowCanvasActionMessage_OnInstantiation_MessageIsRegistered()
    {
        _messenger.AssertIsRegistered<OverlayWindowCanvasActionMessage>(_sut);
    }

    #endregion
}