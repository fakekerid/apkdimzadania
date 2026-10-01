using System;
using System.Windows;

namespace StoperDesktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Console.WriteLine("[App] OnStartup - Tworzenie MainWindow...");
        try
        {
            var win = new MainWindow();
            this.MainWindow = win;
            win.Show();
            Console.WriteLine("[App] MainWindow wyświetlone pomyślnie!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[App] Błąd podczas tworzenia MainWindow: {ex}");
        }
    }
}
