namespace Inventory.Maui;

public partial class App : Microsoft.Maui.Controls.Application  // ✅ Use full namespace
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}