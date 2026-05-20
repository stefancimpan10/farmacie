using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;

namespace NivelUIWPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // FORȚĂM APLICAȚIA SĂ FOLOSEASCĂ FORMATUL ENGLEZESC PENTRU NUMERE
        // Astfel, punctul (.) va fi recunoscut ca separator zecimal peste tot în aplicație
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

        base.OnStartup(e);
    }
}

