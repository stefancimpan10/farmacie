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
    // Clasă simplă doar pentru a ține datele în memorie
    public class StatComanda
    {
        public DateTime Data { get; set; }
        public decimal Total { get; set; }
        public string NumeClient { get; set; } = "";
    }

    public partial class PaginaPanouControl : UserControl
    {
        private string caleFisierDate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "comenzi_farmacie.json");
        private List<StatComanda> toateComenzile = new List<StatComanda>();

        public PaginaPanouControl()
        {
            InitializeComponent();

            // Când se încarcă pagina, citim datele
            Loaded += (s, e) => IncarcaSiCalculeaza("Azi");
        }

        private void BtnFiltru_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                IncarcaSiCalculeaza(tag);
            }
        }

        private void IncarcaSiCalculeaza(string tipFiltru)
        {
            IncarcaDateDinFisier();
            CalculeazaStatistici(tipFiltru);
        }

        private void IncarcaDateDinFisier()
        {
            toateComenzile.Clear();

            if (!File.Exists(caleFisierDate))
            {
                return;
            }

            try
            {
                string json = File.ReadAllText(caleFisierDate);
                if (string.IsNullOrWhiteSpace(json)) return;

                // Citim ca listă de obiecte JSON generice (JObject)
                var listaRaw = JsonConvert.DeserializeObject<List<JObject>>(json);

                if (listaRaw != null)
                {
                    foreach (var item in listaRaw)
                    {
                        var comanda = new StatComanda();

                        // Extragem data
                        if (item["Data"] != null)
                            comanda.Data = item["Data"].Value<DateTime>();

                        // Extragem Totalul (încercăm mai multe nume posibile din JSON)
                        if (item["TotalComanda"] != null)
                            comanda.Total = item["TotalComanda"].Value<decimal>();
                        else if (item["Total"] != null)
                            comanda.Total = item["Total"].Value<decimal>();
                        else if (item["PretTotal"] != null)
                            comanda.Total = item["PretTotal"].Value<decimal>();

                        // Extragem Numele Clientului
                        if (item["NumeClient"] != null)
                            comanda.NumeClient = item["NumeClient"].Value<string>();
                        else if (item["Client"] != null)
                            comanda.NumeClient = item["Client"].Value<string>();

                        toateComenzile.Add(comanda);
                    }
                }
            }
            catch (Exception ex)
            {
                // Dacă apare eroare, o afișăm discret sau o ignorăm ca să nu crape aplicația
                System.Diagnostics.Debug.WriteLine($"Eroare citire stats: {ex.Message}");
            }
        }

        private void CalculeazaStatistici(string tipFiltru)
        {
            DateTime acum = DateTime.Now;
            var comenziFiltrate = new List<StatComanda>();

            switch (tipFiltru)
            {
                case "Azi":
                    comenziFiltrate = toateComenzile.Where(c => c.Data.Date == acum.Date).ToList();
                    LblPerioada.Text = "(pentru data de azi)";
                    break;
                case "Saptamana":
                    int diff = DayOfWeek.Monday - acum.DayOfWeek;
                    if (diff > 0) diff -= 7;
                    DateTime start = acum.AddDays(diff).Date;
                    comenziFiltrate = toateComenzile.Where(c => c.Data >= start).ToList();
                    LblPerioada.Text = "(din această săptămână)";
                    break;
                case "Luna":
                    comenziFiltrate = toateComenzile.Where(c => c.Data.Year == acum.Year && c.Data.Month == acum.Month).ToList();
                    LblPerioada.Text = "(în această lună)";
                    break;
                case "An":
                    comenziFiltrate = toateComenzile.Where(c => c.Data.Year == acum.Year).ToList();
                    LblPerioada.Text = "(în anul curent)";
                    break;
            }

            // Calculăm valorile
            decimal totalSuma = comenziFiltrate.Sum(c => c.Total);
            int nrComenzi = comenziFiltrate.Count;
            decimal medie = nrComenzi > 0 ? totalSuma / nrComenzi : 0;

            // Afișăm în interfață
            TxtTotal.Text = $"{totalSuma:F2} RON";
            TxtNrComenzi.Text = nrComenzi.ToString();
            TxtMedie.Text = $"{medie:F2} RON";

            // Actualizăm lista recentă
            LstActivitateRecents.Items.Clear();
            if (comenziFiltrate.Any())
            {
                foreach (var c in comenziFiltrate.OrderByDescending(c => c.Data).Take(5))
                {
                    LstActivitateRecents.Items.Add($"[{c.Data:HH:mm:ss}] Client: {c.NumeClient} - {c.Total:F2} RON");
                }
            }
            else
            {
                LstActivitateRecents.Items.Add("Nu există comenzi în această perioadă.");
            }
        }
    }
}