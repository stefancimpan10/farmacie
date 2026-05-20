using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NivelStocareDate;

namespace NivelUIWPF
{
    // Clasă auxiliară pentru o linie din coșul de recepție
    public class LinieIntrare
    {
        public string Nume { get; set; } = "";
        public int Cantitate { get; set; }
        public decimal PretAchizitie { get; set; }
        public string DataExpirare { get; set; } = "";
        public decimal TotalLinie => Cantitate * PretAchizitie;
    }

    public partial class PaginaReceptie : UserControl
    {
        private ObservableCollection<LinieIntrare> cosIntrare = new ObservableCollection<LinieIntrare>();
        private List<MedicamentDB> listaProduse = new List<MedicamentDB>();

        public PaginaReceptie()
        {
            InitializeComponent();
            Loaded += (s, e) => ReincarcaProduseReceptie();
            DgListaIntrare.ItemsSource = cosIntrare;

            // Setăm data default la +2 ani de azi (majoritatea medicamentelor au valabilitate 2-3 ani)
            if (DpExpirare != null)
                DpExpirare.SelectedDate = DateTime.Now.AddYears(2);
        }

        private void ReincarcaProduseReceptie()
        {
            listaProduse = ManagerMedicamente.IncarcaMedicamente();
            CmbProduseReceptie.ItemsSource = listaProduse;
        }

        // Căutare rapidă în ComboBox
        private void CmbProduseReceptie_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string text = CmbProduseReceptie.Text.ToLower();
            if (string.IsNullOrEmpty(text))
            {
                CmbProduseReceptie.ItemsSource = listaProduse;
            }
            else
            {
                var filtrate = listaProduse.Where(p => p.Nume.ToLower().Contains(text)).ToList();
                CmbProduseReceptie.ItemsSource = filtrate;
                CmbProduseReceptie.IsDropDownOpen = true;
            }
        }

        // Adaugă produsul în tabelul de jos
        private void BtnAdaugaInLista_Click(object sender, RoutedEventArgs e)
        {
            if (CmbProduseReceptie.SelectedItem is not MedicamentDB produsSelectat)
            {
                MessageBox.Show("Selectează un produs valid!", "Eroare");
                return;
            }

            if (!int.TryParse(TxtCantitateIntrare.Text, out int cantitate) || cantitate <= 0)
            {
                MessageBox.Show("Introdu o cantitate validă!", "Eroare");
                return;
            }

            if (!decimal.TryParse(TxtPretAchizitie.Text, out decimal pret))
            {
                MessageBox.Show("Introdu un preț valid!", "Eroare");
                return;
            }

            var linieNoua = new LinieIntrare
            {
                Nume = produsSelectat.Nume,
                Cantitate = cantitate,
                PretAchizitie = pret,
                DataExpirare = DpExpirare.SelectedDate?.ToString("dd.MM.yyyy") ?? "-"
            };

            cosIntrare.Add(linieNoua);
            CalculeazaTotalIntrare();

            // Reset câmpuri pentru următorul produs
            TxtCantitateIntrare.Text = "1";
            TxtPretAchizitie.Text = "0.00";
            CmbProduseReceptie.Text = "";
            CmbProduseReceptie.Focus();
        }

        // Șterge o linie din tabel
        private void BtnStergeLinie_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is LinieIntrare linie)
            {
                cosIntrare.Remove(linie);
                CalculeazaTotalIntrare();
            }
        }

        // SALVEAZĂ TOATE PRODUSELE DIN COȘ ÎN BAZA DE DATE
        private void BtnFinalizeazaIntrarea_Click(object sender, RoutedEventArgs e)
        {
            if (cosIntrare.Count == 0)
            {
                MessageBox.Show("Lista de intrare este goală!", "Atenție");
                return;
            }

            try
            {
                // 1. Încărcăm baza de date actuală
                var toateProdusele = ManagerMedicamente.IncarcaMedicamente();

                // 2. Iterăm prin coș și actualizăm stocurile
                foreach (var linie in cosIntrare)
                {
                    var prodInBD = toateProdusele.FirstOrDefault(p => p.Nume == linie.Nume);
                    if (prodInBD != null)
                    {
                        prodInBD.Stoc += linie.Cantitate; // Creștem stocul
                        // Notă: Prețul de vânzare (Pret) rămâne cel din BD. 
                        // Prețul de achiziție (linie.PretAchizitie) e doar informativ aici dacă nu ai câmp dedicat în BD.
                    }
                }

                // 3. Salvăm o singură dată tot fișierul
                ManagerMedicamente.SalveazaMedicamente(toateProdusele);

                MessageBox.Show($"INTRARE REUȘITĂ!\nAu fost adăugate {cosIntrare.Sum(l => l.Cantitate)} produse în stoc.\nValoare totală: {cosIntrare.Sum(l => l.TotalLinie):N2} RON",
                                "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                // 4. Golim coșul
                cosIntrare.Clear();
                CalculeazaTotalIntrare();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea în baza de date: {ex.Message}", "Eroare Criticală");
            }
        }

        private void CalculeazaTotalIntrare()
        {
            decimal total = cosIntrare.Sum(l => l.TotalLinie);
            TxtTotalIntrare.Text = $"{total:N2} RON";
        }
    }
}