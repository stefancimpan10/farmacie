using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using NivelStocareDate;

namespace NivelUIWPF
{
    public class ProdusExpirat
    {
        public string Nume { get; set; } = "";
        public DateTime DataExpirareSimulata { get; set; }
        public int ZileRamase { get; set; }
        public int Stoc { get; set; }
    }

    public partial class PaginaExpirare : UserControl
    {
        private List<ProdusExpirat> toateProduseleCuExp = new List<ProdusExpirat>();

        public PaginaExpirare()
        {
            InitializeComponent();

            // Folosim evenimentul Loaded pentru a ne asigura că DgExpirare există înainte de a-l folosi
            Loaded += (s, e) =>
            {
                GenereazaDateSimulate();
                FiltreazaExpirari(30); // Default: arată produsele care expiră în 30 de zile
            };
        }

        private void GenereazaDateSimulate()
        {
            var medicamente = ManagerMedicamente.IncarcaMedicamente();
            Random rand = new Random();

            foreach (var med in medicamente)
            {
                // Simulăm o dată de expirare aleatorie între acum 10 zile și peste 2 ani (730 zile)
                int zileAdaugate = rand.Next(-10, 730);
                var dataExp = DateTime.Now.AddDays(zileAdaugate);

                toateProduseleCuExp.Add(new ProdusExpirat
                {
                    Nume = med.Nume,
                    DataExpirareSimulata = dataExp,
                    ZileRamase = (dataExp - DateTime.Now).Days,
                    Stoc = med.Stoc
                });
            }
        }

        private void CmbPerioada_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verificăm dacă ComboBox-ul este inițializat pentru a evita erori la pornire
            if (CmbPerioada == null) return;

            if (CmbPerioada.SelectedIndex == 0) FiltreazaExpirari(30);   // 30 Zile
            else if (CmbPerioada.SelectedIndex == 1) FiltreazaExpirari(90);  // 3 Luni
            else if (CmbPerioada.SelectedIndex == 2) FiltreazaExpirari(180); // 6 Luni
            else if (CmbPerioada.SelectedIndex == 3) FiltreazaExpirari(365); // 1 An
        }

        private void FiltreazaExpirari(int zileMaxime)
        {
            // Filtrăm produsele care expiră între azi și data limită selectată
            var produseCritice = toateProduseleCuExp
                .Where(p => p.ZileRamase >= 0 && p.ZileRamase <= zileMaxime)
                .OrderBy(p => p.ZileRamase) // Le ordonăm de la cele care expiră cel mai repede
                .ToList();

            // Atribuim lista filtrată DataGrid-ului
            if (DgExpirare != null)
            {
                DgExpirare.ItemsSource = produseCritice;
            }
        }
    }
}