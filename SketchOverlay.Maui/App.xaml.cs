using SketchOverlay.Maui.Controls;

namespace SketchOverlay.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        WindowsGraphicsView.InitializeCustomHandlers();
		MainPage = new AppShell();
    }
}
