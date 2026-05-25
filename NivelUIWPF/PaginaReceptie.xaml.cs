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

            // Setăm data default la +2 ani
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

        // --- METODA CARE DA EROARE LA TINE ---
        // Asigură-te că aceasta este în interiorul clasei PaginaReceptie, dar NU în altă metodă
        private void BtnAdaugaInLista_Click(object sender, RoutedEventArgs e)
        {
            // 1. Verificare Produs
            if (CmbProduseReceptie.SelectedItem is not MedicamentDB produsSelectat)
            {
                MessageBox.Show("Te rog selectează un produs din listă!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Validare Cantitate
            if (!int.TryParse(TxtCantitateIntrare.Text, out int cantitate) || cantitate <= 0)
            {
                MessageBox.Show("Cantitatea trebuie să fie un număr pozitiv!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                TxtCantitateIntrare.Focus();
                return;
            }

            // 3. Validare Preț
            if (!decimal.TryParse(TxtPretAchizitie.Text, out decimal pret))
            {
                MessageBox.Show("Preț invalid!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                TxtPretAchizitie.Focus();
                return;
            }

            if (pret < 0)
            {
                MessageBox.Show("Prețul nu poate fi negativ!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 4. Validare Dată Expirare
            DateTime dataExp = DpExpirare.SelectedDate ?? DateTime.MinValue;
            if (dataExp.Date < DateTime.Today)
            {
                MessageBox.Show("Data expirării nu poate fi în trecut!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Adăugare în listă
            var linieNoua = new LinieIntrare
            {
                Nume = produsSelectat.Nume,
                Cantitate = cantitate,
                PretAchizitie = pret,
                DataExpirare = dataExp.ToString("dd.MM.yyyy")
            };

            cosIntrare.Add(linieNoua);
            CalculeazaTotalIntrare();

            // Reset
            TxtCantitateIntrare.Text = "1";
            TxtPretAchizitie.Text = "0.00";
            CmbProduseReceptie.Text = "";
            CmbProduseReceptie.Focus();
        }

        private void BtnStergeLinie_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is LinieIntrare linie)
            {
                cosIntrare.Remove(linie);
                CalculeazaTotalIntrare();
            }
        }

        private void BtnFinalizeazaIntrarea_Click(object sender, RoutedEventArgs e)
        {
            if (cosIntrare.Count == 0) return;

            try
            {
                var toateProdusele = ManagerMedicamente.IncarcaMedicamente();

                foreach (var linie in cosIntrare)
                {
                    var prodInBD = toateProdusele.FirstOrDefault(p => p.Nume == linie.Nume);
                    if (prodInBD != null)
                    {
                        prodInBD.Stoc += linie.Cantitate;
                    }
                }

                ManagerMedicamente.SalveazaMedicamente(toateProdusele);
                MessageBox.Show("Intrare finalizată cu succes!", "Succes");
                cosIntrare.Clear();
                CalculeazaTotalIntrare();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}", "Eroare");
            }
        }

        private void CalculeazaTotalIntrare()
        {
            decimal total = cosIntrare.Sum(l => l.TotalLinie);
            TxtTotalIntrare.Text = $"{total:N2} RON";
        }

        // Metode pentru a bloca tastele invalide (opțional, dar recomandat)
        private void NumaiCifre_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void NumarDecimal_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            bool isDigit = char.IsDigit(e.Text, 0);
            bool isDot = e.Text == ".";
            if (!isDigit && !isDot) { e.Handled = true; return; }
            if (isDot)
            {
                var textBox = sender as TextBox;
                if (textBox != null && textBox.Text.Contains(".")) e.Handled = true;
            }
        }
    }
}