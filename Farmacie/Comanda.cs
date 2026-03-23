using System;
using System.Collections.Generic;

namespace FarmacieApp
{
    public class Comanda
    {
        public int ID { get; set; }
        public string NumeClient { get; set; }
        public List<ArticolComanda> ProduseComandate { get; set; } = new List<ArticolComanda>();
        public decimal PretTotal { get; set; }
        public DateTime DataOra { get; set; }

        public void CalculeazaTotal()
        {
            PretTotal = 0;
            foreach (var art in ProduseComandate)
                PretTotal += art.PretTotalArticol;
        }
    }
}