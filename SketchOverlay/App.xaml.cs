using SketchOverlay.Controls;

namespace SketchOverlay;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        WindowsGraphicsView.InitializeCustomHandlers();
		MainPage = new AppShell();
    }
}
