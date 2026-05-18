using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace NivelUIWPF
{
    // Model pentru Produs (din baza de date)
    public class ProdusDisponibil
    {
        public string Nume { get; set; } = "";
        public bool NecesitaReteta { get; set; }
        public decimal Pret { get; set; } = 0; // Adăugăm preț pentru calcul total
    }

    // Model pentru un articol din Coș (temporar)
    public class ArticolCos
    {
        public string NumeProdus { get; set; }
        public int NrBucati { get; set; }
        public decimal PretUnitar { get; set; }
        public decimal TotalLinie => NrBucati * PretUnitar;
    }

    // Model pentru Comanda Finalizată (salvată în istoric)
    public class ComandaFinala
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string NumeClient { get; set; } = "";
        public string DetaliiProduse { get; set; } = ""; // Ex: "Paracetamol x2, Augmentin x1"
        public decimal TotalComanda { get; set; }
        public List<ArticolCos> Articole { get; set; } = new List<ArticolCos>();
    }

    public partial class PaginaComenzi : UserControl
    {
        private ObservableCollection<ArticolCos> cosCurent = new ObservableCollection<ArticolCos>();
        private ObservableCollection<ComandaFinala> listaComenzi = new ObservableCollection<ComandaFinala>();
        private List<ProdusDisponibil> listaProduse = new List<ProdusDisponibil>();
        private int nextId = 1;
        private string caleFisierDate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "comenzi_farmacie.json");

        public PaginaComenzi()
        {
            InitializeComponent();

            // Inițializare cu EXACT 10 MEDICAMENTE POPULARE
            listaProduse.Add(new ProdusDisponibil { Nume = "Paracetamol 500mg", NecesitaReteta = false, Pret = 15.50m });
            listaProduse.Add(new ProdusDisponibil { Nume = "Augmentin 625mg", NecesitaReteta = true, Pret = 45.00m });
            listaProduse.Add(new ProdusDisponibil { Nume = "Ibuprofen 400mg", NecesitaReteta = false, Pret = 12.00m });
            listaProduse.Add(new ProdusDisponibil { Nume = "Aspirină 500mg", NecesitaReteta = false, Pret = 10.00m });
            listaProduse.Add(new ProdusDisponibil { Nume = "Nurofen Forte", NecesitaReteta = false, Pret = 22.00m });
            listaProduse.Add(new ProdusDisponibil { Nume = "Omeprazol 20mg", NecesitaReteta = false, Pret = 18.00m });
            listaProduse.Add(new ProdusDisponibil { Nume = "Xanax 0.25mg", NecesitaReteta = true, Pret = 30.00m });
            listaProduse.Add(new ProdusDisponibil { Nume = "Rinofluimucil Spray", NecesitaReteta = false, Pret = 25.00m });
            listaProduse.Add(new ProdusDisponibil { Nume = "Vitamina C 1000mg", NecesitaReteta = false, Pret = 20.00m });
            listaProduse.Add(new ProdusDisponibil { Nume = "Diazepam 10mg", NecesitaReteta = true, Pret = 15.00m });

            CmbProduse.ItemsSource = listaProduse;
            CmbProduse.DisplayMemberPath = "Nume";

            DgCos.ItemsSource = cosCurent;
            DgComenzi.ItemsSource = listaComenzi;

            IncarcaDateDinFisier();
        }

        // 1. BUTON: Adaugă în Coș
        private void BtnAdaugaInCos_Click(object sender, RoutedEventArgs e)
        {
            ProdusDisponibil produsSelectat = null;

            // Verificăm dacă utilizatorul a selectat din listă SAU a scris un nume existent
            if (CmbProduse.SelectedItem is ProdusDisponibil selectatDinLista)
            {
                produsSelectat = selectatDinLista;
            }
            else
            {
                // Dacă nu e selectat din listă, verificăm dacă textul scris corespunde cu un produs din listă
                string textScris = CmbProduse.Text.Trim();
                produsSelectat = listaProduse.FirstOrDefault(p => p.Nume.ToLower() == textScris.ToLower());
            }

            // Dacă tot nu am găsit produsul, înseamnă că utilizatorul a inventat un nume
            if (produsSelectat == null)
            {
                MessageBox.Show("Produsul introdus nu există în baza de date!\nTe rog selectează unul din listă.", "Eroare Produs");
                return;
            }

            if (!int.TryParse(TxtCantitate.Text, out int cantitate) || cantitate <= 0)
            {
                MessageBox.Show("Introdu o cantitate validă!", "Eroare Cantitate");
                return;
            }

            // Adăugăm în coșul temporar
            var articol = new ArticolCos
            {
                NumeProdus = produsSelectat.Nume,
                NrBucati = cantitate,
                PretUnitar = produsSelectat.Pret
            };

            cosCurent.Add(articol);
            CalculeazaTotalCos();

            // Resetăm câmpurile pentru următoarea căutare
            TxtCantitate.Text = "1";
            CmbProduse.Text = ""; // Golim textul ca să poți scrie altceva imediat
            CmbProduse.Focus();   // Punem cursorul înapoi în combobox
        }

        // 2. BUTON: Șterge din Coș
        private void BtnStergeDinCos_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ArticolCos articol)
            {
                cosCurent.Remove(articol);
                CalculeazaTotalCos();
            }
        }

        // 3. BUTON: Finalizează Comanda (Salvează în Istoric și Fișier)
        private void BtnFinalizeazaComanda_Click(object sender, RoutedEventArgs e)
        {
            if (cosCurent.Count == 0)
            {
                MessageBox.Show("Coșul este gol!", "Atenție");
                return;
            }

            string numeClient = TxtNumeClient.Text.Trim();
            if (string.IsNullOrEmpty(numeClient))
            {
                MessageBox.Show("Introdu numele clientului înainte de finalizare!", "Eroare");
                return;
            }

            // Calculăm totalul general
            decimal totalGeneral = cosCurent.Sum(a => a.TotalLinie);

            // Creăm detalii text (ex: "Paracetamol x2, Augmentin x1")
            string detalii = string.Join(", ", cosCurent.Select(a => $"{a.NumeProdus} x{a.NrBucati}"));

            var comandaNoua = new ComandaFinala
            {
                Id = nextId++,
                Data = DateTime.Now,
                NumeClient = numeClient,
                DetaliiProduse = detalii,
                TotalComanda = totalGeneral,
                Articole = new List<ArticolCos>(cosCurent) // Copiem lista
            };

            // Salvăm în istoric
            listaComenzi.Insert(0, comandaNoua);

            // Golim coșul
            cosCurent.Clear();
            TxtNumeClient.Clear();
            CalculeazaTotalCos();

            // Salvăm în fișier
            SalveazaDateInFisier();

            MessageBox.Show($"Comandă finalizată!\nTotal: {totalGeneral:F2} RON", "Succes");
        }

        private void CalculeazaTotalCos()
        {
            decimal total = cosCurent.Sum(a => a.TotalLinie);
            TxtTotalGeneral.Text = $"{total:F2} RON";
            LblTotalCos.Text = $"({cosCurent.Count} articole)";
        }

        private void CmbProduse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Opțional: Afișăm prețul sau info despre rețetă într-un label dacă vrei
        }

        // --- SALVARE / ÎNCĂRCARE ---

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