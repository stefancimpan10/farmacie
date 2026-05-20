using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using NivelStocareDate;

namespace NivelUIWPF
{
    public class ProdusDisponibil
    {
        public string Nume { get; set; } = "";
        public bool NecesitaReteta { get; set; }
        public decimal Pret { get; set; } = 0;
    }

    public class ArticolCos
    {
        public string NumeProdus { get; set; } = "";
        public int NrBucati { get; set; }
        public decimal PretUnitar { get; set; }
        public decimal TotalLinie => NrBucati * PretUnitar;
    }

    public class ComandaFinala
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string NumeClient { get; set; } = "";
        public string DetaliiProduse { get; set; } = "";
        public decimal TotalComanda { get; set; }
        public List<ArticolCos> Articole { get; set; } = new List<ArticolCos>();
    }

    public partial class PaginaComenzi : UserControl
    {
        private ObservableCollection<ArticolCos> cosCurent = new ObservableCollection<ArticolCos>();
        private ObservableCollection<ComandaFinala> listaComenzi = new ObservableCollection<ComandaFinala>();
        private List<ProdusDisponibil> listaProduseCompleta = new List<ProdusDisponibil>();
        private int nextId = 1;
        private string caleFisierDate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "comenzi_farmacie.json");

        public PaginaComenzi()
        {
            InitializeComponent();

            // Se reîncarcă produsele de fiecare dată când intri pe pagină
            Loaded += (s, e) => ReincarcaProduse();

            DgCos.ItemsSource = cosCurent;
            DgComenzi.ItemsSource = listaComenzi;
            IncarcaDateDinFisier();
        }

        private void ReincarcaProduse()
        {
            listaProduseCompleta.Clear();
            var medicamenteDinDB = ManagerMedicamente.IncarcaMedicamente();

            foreach (var med in medicamenteDinDB)
            {
                listaProduseCompleta.Add(new ProdusDisponibil
                {
                    Nume = med.Nume,
                    NecesitaReteta = med.NecesitaReteta,
                    Pret = med.Pret
                });
            }

            CmbProduse.ItemsSource = listaProduseCompleta;
            CmbProduse.DisplayMemberPath = "Nume";

            // --- ADAUGĂ ASTA PENTRU VERIFICARE ---
            if (listaProduseCompleta.Count > 0)
            {
                // Dacă vrei, poți lăsa comentat sau șterge linia de mai jos după ce verifici
                System.Diagnostics.Debug.WriteLine($"SUCCES: Au fost încărcate {listaProduseCompleta.Count} medicamente din baza de date.");
            }
            else
            {
                MessageBox.Show("ATENȚIE: Baza de date este GOALĂ! Verifică fișierul medicamente.json.", "Eroare Bază Date");
            }
            // -------------------------------------
        }

        private void CmbProduse_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string textCautat = CmbProduse.Text.ToLower();
            if (string.IsNullOrEmpty(textCautat))
            {
                CmbProduse.ItemsSource = listaProduseCompleta;
            }
            else
            {
                var produseFiltrate = listaProduseCompleta
                    .Where(p => p.Nume.ToLower().Contains(textCautat))
                    .ToList();
                CmbProduse.ItemsSource = produseFiltrate;
                CmbProduse.IsDropDownOpen = true;
            }
        }

        private void BtnAdaugaInCos_Click(object sender, RoutedEventArgs e)
        {
            ProdusDisponibil produsSelectat = null;

            // 1. Găsim produsul selectat sau scris
            if (CmbProduse.SelectedItem is ProdusDisponibil selectatDinLista)
            {
                produsSelectat = selectatDinLista;
            }
            else
            {
                string textScris = CmbProduse.Text.Trim();
                produsSelectat = listaProduseCompleta.FirstOrDefault(p => p.Nume.ToLower() == textScris.ToLower());
            }

            // 2. Verificăm dacă produsul există în baza de date
            if (produsSelectat == null)
            {
                MessageBox.Show("EROARE: Produsul nu există în baza de date!\nTe rog selectează un produs valid din listă.", "Produs Invalid");
                return;
            }

            // 3. NOU: VERIFICĂM STOCUL
            // Trebuie să găsim produsul original din ManagerMedicamente pentru a vedea stocul real din JSON
            var produsDinBazaDeDate = ManagerMedicamente.IncarcaMedicamente()
                .FirstOrDefault(p => p.Nume == produsSelectat.Nume);

            if (produsDinBazaDeDate != null && produsDinBazaDeDate.Stoc <= 0)
            {
                MessageBox.Show($"STOC EPUIZAT!\nNu mai avem '{produsSelectat.Nume}' pe stoc.\nTe rugăm să faci o recepție de marfă.", "Stoc Insuficient", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Oprim funcția aici, nu adaugă în coș
            }

            // 4. Verificăm cantitatea introdusă
            if (!int.TryParse(TxtCantitate.Text, out int cantitate) || cantitate <= 0)
            {
                MessageBox.Show("Introdu o cantitate validă!", "Eroare Cantitate");
                return;
            }

            // 5. Verificăm dacă cantitatea cerută depășește stocul disponibil
            if (produsDinBazaDeDate != null && cantitate > produsDinBazaDeDate.Stoc)
            {
                MessageBox.Show($"STOC INSUFICIENT!\nAi cerut {cantitate} bucăți, dar avem doar {produsDinBazaDeDate.Stoc} în stoc.", "Eroare Stoc", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 6. Dacă totul e OK, adăugăm în coș
            var articol = new ArticolCos
            {
                NumeProdus = produsSelectat.Nume,
                NrBucati = cantitate,
                PretUnitar = produsSelectat.Pret
            };

            cosCurent.Add(articol);
            CalculeazaTotalCos();

            TxtCantitate.Text = "1";
            CmbProduse.Text = "";
            CmbProduse.Focus();
        }

        private void BtnStergeDinCos_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ArticolCos articol)
            {
                cosCurent.Remove(articol);
                CalculeazaTotalCos();
            }
        }

        private void BtnFinalizeazaComanda_Click(object sender, RoutedEventArgs e)
        {
            if (cosCurent.Count == 0) { MessageBox.Show("Coșul este gol!", "Atenție"); return; }
            string numeClient = TxtNumeClient.Text.Trim();
            if (string.IsNullOrEmpty(numeClient)) { MessageBox.Show("Introdu numele clientului!", "Eroare"); return; }

            decimal totalGeneral = cosCurent.Sum(a => a.TotalLinie);
            string detalii = string.Join(", ", cosCurent.Select(a => $"{a.NumeProdus} x{a.NrBucati}"));

            var comandaNoua = new ComandaFinala
            {
                Id = nextId++,
                Data = DateTime.Now,
                NumeClient = numeClient,
                DetaliiProduse = detalii,
                TotalComanda = totalGeneral,
                Articole = new List<ArticolCos>(cosCurent)
            };

            listaComenzi.Insert(0, comandaNoua);
            cosCurent.Clear();
            TxtNumeClient.Clear();
            CalculeazaTotalCos();
            SalveazaDateInFisier();
            MessageBox.Show($"Comandă finalizată!\nTotal: {totalGeneral:F2} RON", "Succes");
        }

        private void CalculeazaTotalCos()
        {
            decimal total = cosCurent.Sum(a => a.TotalLinie);
            TxtTotalGeneral.Text = $"{total:F2} RON";
            LblTotalCos.Text = $"({cosCurent.Count} articole)";
        }

        private void CmbProduse_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void SalveazaDateInFisier()
        {
            try
            {
                string json = JsonConvert.SerializeObject(listaComenzi, Formatting.Indented);
                File.WriteAllText(caleFisierDate, json);
            }
            catch (Exception ex) { MessageBox.Show($"Eroare salvare: {ex.Message}"); }
        }

        private void IncarcaDateDinFisier()
        {
            if (File.Exists(caleFisierDate))
            {
                try
                {
                    string json = File.ReadAllText(caleFisierDate);
                    var date = JsonConvert.DeserializeObject<ObservableCollection<ComandaFinala>>(json);
                    if (date != null)
                    {
                        listaComenzi.Clear();
                        foreach (var c in date) { listaComenzi.Add(c); if (c.Id >= nextId) nextId = c.Id + 1; }
                    }
                }
                catch { }
            }
        }
    }
}