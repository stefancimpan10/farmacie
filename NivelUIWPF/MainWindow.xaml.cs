using Newtonsoft.Json.Linq;
using System.Windows;
using System.IO;
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
            try
            {
                // Încercăm să creăm instanța paginii
                SchimbaPagina(new PaginaReceptie());
            }
            catch (Exception ex)
            {
                // Dacă apare vreo eroare la încărcarea paginii, o afișăm ca să știm ce se întâmplă
                MessageBox.Show($"Eroare la deschiderea paginii de Recepție:\n{ex.Message}", "Eroare Criticală");
            }
        }

        // 4. Inventar Stoc (Placeholder)
        private void BtnInventar_Click(object sender, RoutedEventArgs e)
        {
            SchimbaPagina(new PaginaInventar());
        }

        // 5. Alertă Expirare (Placeholder - o vom face detaliată data viitoare)
        private void BtnExpirare_Click(object sender, RoutedEventArgs e)
        {
            SchimbaPagina(new PaginaExpirare());
        }

        // 6. Rapoarte Financiare (Placeholder)
        private void BtnRapoarte_Click(object sender, RoutedEventArgs e)
        {
            SchimbaPagina(new PaginaRapoarte());
        }

        private void BtnAdministrare_Click(object sender, RoutedEventArgs e)
        {
            SchimbaPagina(new PaginaAdministrare());
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