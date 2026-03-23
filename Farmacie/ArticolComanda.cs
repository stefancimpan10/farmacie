namespace FarmacieApp
{
    public class ArticolComanda
    {
        public Produs ProdusComandat { get; set; }
        public int Cantitate { get; set; }
        public decimal PretTotalArticol { get; set; }

        public ArticolComanda(Produs p, int cantitate)
        {
            ProdusComandat = p;
            Cantitate = cantitate;
            PretTotalArticol = p.PretUnitar * cantitate;
        }
    }
}