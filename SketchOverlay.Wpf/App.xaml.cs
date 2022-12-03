using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace SketchOverlay.Wpf;

public partial class App : Application
{
    public App()
    {
        Services = new ServiceCollection()
            .AddServices()
            .AddViewModels()
            .BuildServiceProvider();
    }

    public IServiceProvider Services { get; }
}