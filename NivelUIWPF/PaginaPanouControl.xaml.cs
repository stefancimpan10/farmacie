using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace NivelUIWPF
{
    // Definim o clasă simplă pentru a citi datele din JSON fără să complicăm lucrurile
    public class ComandaStatistica
    {
        public DateTime Data { get; set; }
        public decimal Total { get; set; }
        public string NumeProdus { get; set; } = "";
    }

    /// <summary>
    /// Interaction logic for PaginaPanouControl.xaml
    /// </summary>
    public partial class PaginaPanouControl : UserControl
    {
        private string caleFisierDate = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "comenzi_farmacie.json");

        public PaginaPanouControl()
        {
            InitializeComponent();

            // Calculăm statisticile imediat ce se deschide pagina
            CalculeazaStatistici();
        }

        private void CalculeazaStatistici()
        {
            // Verificăm dacă există fișierul cu comenzi
            if (!File.Exists(caleFisierDate))
            {
                TxtTotalAzi.Text = "0.00 RON";
                TxtNrComenzi.Text = "0";
                return;
            }

            try
            {
                // Citim tot conținutul fișierului JSON
                string json = File.ReadAllText(caleFisierDate);

                // Îl transformăm într-o listă de obiecte C#
                var toateComenzile = JsonConvert.DeserializeObject<List<ComandaStatistica>>(json);

                if (toateComenzile != null && toateComenzile.Any())
                {
                    // Filtrăm DOAR comenzile de AZI
                    var dataAzi = DateTime.Today;
                    var comenziAzi = toateComenzile.Where(c => c.Data.Date == dataAzi).ToList();

                    // 1. Calculăm suma totală a vânzărilor de azi
                    decimal totalSuma = comenziAzi.Sum(c => c.Total);
                    TxtTotalAzi.Text = $"{totalSuma:F2} RON";

                    // 2. Numărăm câte comenzi au fost
                    TxtNrComenzi.Text = comenziAzi.Count.ToString();

                    // 3. Pentru expirare, deocamdată punem 0 sau un număr fix până facem modulul de stoc
                    // În viitor, aici vei filtra produsele din inventar care au DataExpirare < DateTime.Now.AddDays(30)
                    TxtExpirate.Text = "0";
                }
                else
                {
                    TxtTotalAzi.Text = "0.00 RON";
                    TxtNrComenzi.Text = "0";
                }
            }
            catch (Exception )
            {
                // Dacă apare vreo eroare la citire, nu crăpă aplicația, doar lăsăm 0
                TxtTotalAzi.Text = "Eroare";
                TxtNrComenzi.Text = "-";
            }
        }
    }
}