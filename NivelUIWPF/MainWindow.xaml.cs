using System.Windows;
using System.Windows.Controls;
using System.Windows.Media; // Adaugă asta sus de tot

namespace NivelUIWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // La pornire, afișăm Panoul de Control
            SchimbaPagina(new PaginaPanouControl());
        }

        // Metoda universală pentru schimbarea paginii
        private void SchimbaPagina(UserControl paginaNoua)
        {
            ZonaContinut.Content = paginaNoua;
        }

        // 1. Panou Control (Dashboard)
        private void BtnPanou_Click(object sender, RoutedEventArgs e)
        {
            SchimbaPagina(new PaginaPanouControl());
        }

        // 2. Vânzări / Comenzi (Pagina pe care am făcut-o anterior)
        private void BtnVanzari_Click(object sender, RoutedEventArgs e)
        {
            SchimbaPagina(new PaginaComenzi());
        }

        // 3. Recepție Marfă (Placeholder deocamdată)
        private void BtnReceptie_Click(object sender, RoutedEventArgs e)
        {
            var lbl = new Label
            {
                Content = "📦 Modul Recepție Marfă\n(Aici vei adăuga produse noi în stoc)",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            SchimbaPagina(new UserControl { Content = lbl });
        }

        // 4. Inventar Stoc (Placeholder)
        private void BtnInventar_Click(object sender, RoutedEventArgs e)
        {
            var lbl = new Label
            {
                Content = "📋 Modul Inventar\n(Lista completă cu medicamente și prețuri)",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            SchimbaPagina(new UserControl { Content = lbl });
        }

        // 5. Alertă Expirare (Placeholder - o vom face detaliată data viitoare)
        private void BtnExpirare_Click(object sender, RoutedEventArgs e)
        {
            var lbl = new Label
            {
                Content = "⚠️ Alertă Expirare\n(Lista produselor care expiră în < 30 zile)",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.Red
            };
            SchimbaPagina(new UserControl { Content = lbl });
        }

        // 6. Rapoarte Financiare (Placeholder)
        private void BtnRapoarte_Click(object sender, RoutedEventArgs e)
        {
            var lbl = new Label
            {
                Content = "💰 Rapoarte Financiare\n(Total încasări, TVA, Profit)",
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center, 
                VerticalAlignment = VerticalAlignment.Center     
            };
            SchimbaPagina(new UserControl { Content = lbl });
        }

        // 7. Configurare
        private void BtnConfig_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aici vei configura utilizatorii, imprimanta și setările generale.", "Configurare");
        }

        // 8. Ieșire
        private void BtnIesire_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}