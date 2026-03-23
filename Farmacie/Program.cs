using System;

namespace FarmacieApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager farmacie = new Manager();
            bool ruleaza = true;

            while (ruleaza)
            {
                Console.WriteLine("\n=== PROGRAM GESTIUNE FARMACIE ===");
                Console.WriteLine("1. Vizualizare Catalog");
                Console.WriteLine("2. Adaugare Produs Nou");
                Console.WriteLine("3. Actualizare Stoc (Intrari/Iesiri)");
                Console.WriteLine("4. Verificare Produse Expirate");
                Console.WriteLine("5. Creare Comanda Noua (Vanzare)");
                Console.WriteLine("6. Cautare Rapida (Nume)");
                Console.WriteLine("7. Vizualizare Istoric Vanzari");
                Console.WriteLine("8. Stergere Produs");
                Console.WriteLine("9. Modificare Pret");
                Console.WriteLine("X. Iesire");
                Console.Write("\nAlegeti o optiune: ");

                string optiune = Console.ReadLine().ToUpper();

                switch (optiune)
                {
                    case "1": 
                        farmacie.ObtineMeniu(); 
                        break;
                    case "2": 
                        farmacie.AdaugaProdus(); 
                        break;
                    case "3":
                        Console.Write("ID Produs: "); int idS = int.Parse(Console.ReadLine());
                        Console.Write("Cantitate de adaugat/scas (+/-): "); int cantS = int.Parse(Console.ReadLine());
                        farmacie.ActualizeazaStoc(idS, cantS);
                        break;
                    case "4": 
                        farmacie.VerificaExpirate(); 
                        break;
                    case "5": 
                        farmacie.AdaugaComanda(); 
                        break;
                    case "6":
                        Console.Write("Introduceti textul pentru cautare: ");
                        string cauta = Console.ReadLine();
                        farmacie.CautaRapid(cauta);
                        break;
                    case "7": 
                        farmacie.ObtineComenzi(); 
                        break;
                    case "8":
                        Console.Write("ID Produs de sters: ");
                        int idDel = int.Parse(Console.ReadLine());
                        farmacie.StergeProdus(idDel);
                        break;
                    case "9":
                        Console.Write("ID Produs: "); int idP = int.Parse(Console.ReadLine());
                        Console.Write("Pret nou: "); decimal prN = decimal.Parse(Console.ReadLine());
                        farmacie.ModificaPret(idP, prN);
                        break;
                    case "X": 
                        ruleaza = false; 
                        break;
                    default: 
                        Console.WriteLine("Optiune invalida! Incearca din nou."); 
                        break;
                }
            }
        }
    }
}