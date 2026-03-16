using System;
using System.Collections.Generic;

namespace FarmacieApp
{
    public enum StatusPlata { Cash, Card, Compensat }
    public enum FormaFarmaceutica { Tablete, Sirop, Unguent, Capsule }

    public class Produs
    {
        public int ID { get; set; }
        public string DenumireComerciala { get; set; }
        public string SubstantaActiva { get; set; }
        public FormaFarmaceutica Forma { get; set; }
        public string Concentratie { get; set; }
        public decimal PretUnitar { get; set; }
        public int StocDisponibil { get; set; }
        public bool NecesitaReteta { get; set; }
        public DateTime DataExpirarii { get; set; }

        public Produs() { }
    }

    // --- CLASA ARTICOL COMANDA ---
    public class ArticolComanda
    {
        public Produs ProdusComandat { get; set; }
        public int Cantitate { get; set; }
        public decimal PretTotalArticol { get; set; }

        public ArticolComanda() { }
    }

    // --- CLASA COMANDA ---
    public class Comanda
    {
        public int ID { get; set; }
        public string NumeClient { get; set; }
        public string PrenumeClient { get; set; }
        public string NumarTelefon { get; set; }
        public List<ArticolComanda> ProduseComandate { get; set; }
        public decimal PretTotal { get; set; }
        public DateTime DataOra { get; set; }
        public StatusPlata Status { get; set; }
        public string CodParafa { get; set; }

        public Comanda() { }
        public void AdaugaArticol(ArticolComanda articol) { throw new NotImplementedException(); }
    }

    // --- CLASA MANAGER ---
    public class Manager
    {
        private List<Produs> _stocMedicamente = new List<Produs>();
        private List<Comanda> _istoricVanzari = new List<Comanda>();
        private int _urmatorulIDProdus = 1;
        private int _urmatorulIDComanda = 1;

        public void AdaugaProdus() { throw new NotImplementedException(); }
        public void StergeProdus(int id) { throw new NotImplementedException(); }
        public void ModificaPret(int id, decimal pretNou) { throw new NotImplementedException(); }
        public void ActualizeazaStoc(int id, int cantitate) { throw new NotImplementedException(); }
        public void ObtineMeniu() { throw new NotImplementedException(); }
        public Produs CautaProdus(int id) { throw new NotImplementedException(); }
        public void CautaRapid(string text) { throw new NotImplementedException(); }
        public void VerificaExpirate() { throw new NotImplementedException(); }
        public void AdaugaComanda() { throw new NotImplementedException(); }
        public void ObtineComenzi() { throw new NotImplementedException(); }
        public void CautaComanda(int id) { throw new NotImplementedException(); }
        public void ModificaComanda(int id) { throw new NotImplementedException(); }
        public void StergeComanda(int id) { throw new NotImplementedException(); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Manager farmacie = new Manager();
            bool ruleaza = true;

            while (ruleaza)
            {
                Console.WriteLine("\n=== PROGRAM FARMACIE ===");
                Console.WriteLine("1. Vizualizare Catalog");
                Console.WriteLine("2. Adaugare Produs Nou");
                Console.WriteLine("3. Actualizare Stoc");
                Console.WriteLine("4. Verificare Produse Expirate");
                Console.WriteLine("5. Creare Comanda Noua");
                Console.WriteLine("6. Cautare Rapidă (Nume/Substanță)");
                Console.WriteLine("7. Vizualizare Istoric Vânzări");
                Console.WriteLine("8. Stergere Produs");
                Console.WriteLine("9. Modificare Pret");
                Console.WriteLine("X. Iesire");
                Console.Write("Alegeti o optiune: ");

                string optiune = Console.ReadLine().ToUpper();

                switch (optiune)
                {
                    case "1": farmacie.ObtineMeniu(); break;
                    case "2": farmacie.AdaugaProdus(); break;
                    case "3": /* Logica pentru ID si cantitate apoi farmacie.ActualizeazaStoc() */ break;
                    case "4": farmacie.VerificaExpirate(); break;
                    case "5": farmacie.AdaugaComanda(); break;
                    case "6": farmacie.CautaRapid("text"); break;
                    case "7": farmacie.ObtineComenzi(); break;
                    case "8": farmacie.StergeProdus(0); break;
                    case "9": farmacie.ModificaPret(0, 0); break;
                    case "X": ruleaza = false; break;
                    default: Console.WriteLine("Optiune invalida!"); break;
                }
            }
        }
    }
}