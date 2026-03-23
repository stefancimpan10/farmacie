using System;

namespace FarmacieApp
{
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

        public override string ToString()
        {
            return $"ID: {ID} | {DenumireComerciala} ({SubstantaActiva}) | Pret: {PretUnitar} RON | Stoc: {StocDisponibil}";
        }
    }
}