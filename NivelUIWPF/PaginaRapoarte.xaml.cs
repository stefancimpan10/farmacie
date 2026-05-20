using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NivelUIWPF
{
    public partial class PaginaRapoarte : UserControl
    {
        private string caleFisierDate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "comenzi_farmacie.json");

        public PaginaRapoarte()
        {
            InitializeComponent();

            // Setăm datele default: Luna curentă
            DpStart.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DpEnd.SelectedDate = DateTime.Now;

            Loaded += (s, e) => GenereazaRaport();
        }

        private void BtnGenereaza_Click(object sender, RoutedEventArgs e)
        {
            GenereazaRaport();
        }

        private void GenereazaRaport()
        {
            if (!File.Exists(caleFisierDate)) return;

            try
            {
                string json = File.ReadAllText(caleFisierDate);
                var listaRaw = JsonConvert.DeserializeObject<List<JObject>>(json);

                DateTime start = DpStart.SelectedDate ?? DateTime.MinValue;
                DateTime end = DpEnd.SelectedDate ?? DateTime.MaxValue;
                // Incluziv ziua de final până la 23:59
                end = end.Date.AddDays(1).AddTicks(-1);

                decimal totalInc = 0;
                int nrComenzi = 0;
                int totalBucati = 0;
                var tranzactiiFiltrate = new List<object>();

                foreach (var item in listaRaw)
                {
                    if (item["Data"] != null && item["TotalComanda"] != null)
                    {
                        DateTime dataComanda = item["Data"].Value<DateTime>();

                        // Filtrare după dată
                        if (dataComanda >= start && dataComanda <= end)
                        {
                            decimal suma = item["TotalComanda"].Value<decimal>();
                            totalInc += suma;
                            nrComenzi++;

                            // Calculăm numărul de produse (trebuie să parsăm articolele dacă sunt salvate)
                            // Pentru simplitate, folosim câmpul DetaliiProduse sau numărăm din lista de articole dacă există
                            if (item["Articole"] is JArray articole)
                            {
                                foreach (var art in articole)
                                {
                                    if (art["NrBucati"] != null) totalBucati += art["NrBucati"].Value<int>();
                                }
                            }

                            // Adăugăm în lista pentru tabel
                            tranzactiiFiltrate.Add(new
                            {
                                Data = dataComanda,
                                NumeClient = string.IsNullOrEmpty(item["NumeClient"]?.ToString()) ? "Anonim" : item["NumeClient"].ToString(),
                                DetaliiProduse = item["DetaliiProduse"]?.ToString(),
                                TotalComanda = suma
                            });
                        }
                    }
                }

                // Actualizăm UI
                TxtTotalInc.Text = $"{totalInc:N2} RON";
                TxtNrComenzi.Text = nrComenzi.ToString();
                TxtNrProduse.Text = $"{totalBucati} buc";
                TxtMedie.Text = nrComenzi > 0 ? $"{(totalInc / nrComenzi):N2} RON" : "0.00 RON";

                DgRapoarte.ItemsSource = tranzactiiFiltrate.OrderByDescending(x => ((dynamic)x).Data).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la generarea raportului: {ex.Message}", "Eroare");
            }
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            // Simulare export
            MessageBox.Show("Funcționalitatea de export Excel va fi disponibilă în versiunea completă.\n(Demo: Datele sunt gata de procesare)", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}