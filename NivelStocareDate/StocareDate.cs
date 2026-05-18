using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // ESENȚIAL pentru Where, FirstOrDefault, Sum etc.
using Newtonsoft.Json;

namespace NivelStocareDate
{
    // ---------------------------------------------------------
    // PARTEA 1: PENTRU WPF (Baza de date JSON)
    // ---------------------------------------------------------

    public class MedicamentDB
    {
        public string Nume { get; set; } = "";
        public bool NecesitaReteta { get; set; }
        public decimal Pret { get; set; }
    }

    public static class ManagerMedicamente
    {
        private static string numeFisier = "medicamente.json";

        public static List<MedicamentDB> IncarcaMedicamente()
        {
            if (!File.Exists(numeFisier)) return new List<MedicamentDB>();

            try
            {
                string jsonContinut = File.ReadAllText(numeFisier);
                var lista = JsonConvert.DeserializeObject<List<MedicamentDB>>(jsonContinut);
                return lista ?? new List<MedicamentDB>();
            }
            catch { return new List<MedicamentDB>(); }
        }
    }

    // ---------------------------------------------------------
    // PARTEA 2: CLASELE AUXILIARE PENTRU CONSOLĂ (Manager)
    // Acestea trebuie să existe ca să meargă clasa Manager de mai jos
    // ---------------------------------------------------------

    public class Produs
    {
        public int ID { get; set; }
        public string DenumireComerciala { get; set; } = "";
        public string SubstantaActiva { get; set; } = "";
        public decimal PretUnitar { get; set; }
        public int StocDisponibil { get; set; }
        public DateTime DataExpirarii { get; set; }

        public override string ToString()
        {
            return $"{ID}. {DenumireComerciala} - {PretUnitar:F2} RON (Stoc: {StocDisponibil})";
        }
    }

    public class ArticolComanda
    {
        public Produs Produs { get; set; }
        public int Cantitate { get; set; }

        public ArticolComanda(Produs p, int c)
        {
            Produs = p;
            Cantitate = c;
        }
    }

    public class Comanda
    {
        public int ID { get; set; }
        public string NumeClient { get; set; } = "";
        public DateTime DataOra { get; set; }
        public List<ArticolComanda> ProduseComandate { get; set; } = new List<ArticolComanda>();
        public decimal PretTotal { get; set; }

        public void CalculeazaTotal()
        {
            PretTotal = ProduseComandate.Sum(a => a.Produs.PretUnitar * a.Cantitate);
        }
    }

    // ---------------------------------------------------------
    // PARTEA 3: CLASA MANAGER PENTRU CONSOLĂ
    // ---------------------------------------------------------

    public class Manager
    {
        private List<Produs> _stocMedicamente = new List<Produs>();
        private List<Comanda> _istoricVanzari = new List<Comanda>();
        private int _urmatorulIDProdus = 1;
        private int _urmatorulIDComanda = 1;

        public void ObtineMeniu()
        {
            Console.WriteLine("\n--- CATALOG PRODUSE ---");
            if (!_stocMedicamente.Any()) Console.WriteLine("Nu exista produse in stoc.");
            foreach (var p in _stocMedicamente) Console.WriteLine(p.ToString());
        }

        public void AdaugaProdus()
        {
            try
            {
                var p = new Produs { ID = _urmatorulIDProdus++ };
                Console.Write("Nume Comercial: "); p.DenumireComerciala = Console.ReadLine();
                Console.Write("Substanta Activa: "); p.SubstantaActiva = Console.ReadLine();
                Console.Write("Pret Unitar: "); p.PretUnitar = decimal.Parse(Console.ReadLine());
                Console.Write("Stoc Initial: "); p.StocDisponibil = int.Parse(Console.ReadLine());
                Console.Write("Zile pana la expirare: "); p.DataExpirarii = DateTime.Now.AddDays(int.Parse(Console.ReadLine()));
                _stocMedicamente.Add(p);
                Console.WriteLine("Produs adaugat!");
            }
            catch { Console.WriteLine("Date invalide!"); }
        }

        public void ActualizeazaStoc(int id, int cantitate)
        {
            var p = _stocMedicamente.FirstOrDefault(x => x.ID == id);
            if (p != null) { p.StocDisponibil += cantitate; Console.WriteLine("Stoc actualizat!"); }
            else Console.WriteLine("Produsul nu a fost gasit.");
        }

        public void VerificaExpirate()
        {
            var expirate = _stocMedicamente.Where(p => p.DataExpirarii < DateTime.Now).ToList();
            if (!expirate.Any()) Console.WriteLine("Nu exista produse expirate.");
            else expirate.ForEach(e => Console.WriteLine($"EXPIRAT: {e.DenumireComerciala} (Data: {e.DataExpirarii.ToShortDateString()})"));
        }

        public void AdaugaComanda()
        {
            Comanda c = new Comanda { ID = _urmatorulIDComanda++, DataOra = DateTime.Now };
            Console.Write("Nume Client: "); c.NumeClient = Console.ReadLine();

            Console.Write("ID Produs de cumparat: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var p = _stocMedicamente.FirstOrDefault(x => x.ID == id);
                if (p != null && p.StocDisponibil > 0)
                {
                    c.ProduseComandate.Add(new ArticolComanda(p, 1));
                    p.StocDisponibil--;
                    c.CalculeazaTotal();
                    _istoricVanzari.Add(c);
                    Console.WriteLine($"Vanzare reusita! Total: {c.PretTotal:F2} RON");
                }
                else Console.WriteLine("Produs indisponibil sau stoc zero.");
            }
        }

        public void CautaRapid(string text)
        {
            var rezultate = _stocMedicamente.Where(p => p.DenumireComerciala.ToLower().Contains(text.ToLower())).ToList();
            if (!rezultate.Any()) Console.WriteLine("Niciun rezultat.");
            else rezultate.ForEach(r => Console.WriteLine(r.ToString()));
        }

        public void ObtineComenzi()
        {
            Console.WriteLine("\n--- ISTORIC VANZARI ---");
            if (!_istoricVanzari.Any()) Console.WriteLine("Nicio vanzare inregistrata.");
            foreach (var c in _istoricVanzari)
                Console.WriteLine($"ID: {c.ID} | Client: {c.NumeClient} | Total: {c.PretTotal:F2} RON | Data: {c.DataOra}");
        }

        public void StergeProdus(int id)
        {
            var p = _stocMedicamente.FirstOrDefault(x => x.ID == id);
            if (p != null) { _stocMedicamente.Remove(p); Console.WriteLine("Produs eliminat."); }
        }

        public void ModificaPret(int id, decimal pretNou)
        {
            var p = _stocMedicamente.FirstOrDefault(x => x.ID == id);
            if (p != null) { p.PretUnitar = pretNou; Console.WriteLine("Pret modificat."); }
        }
    }
}