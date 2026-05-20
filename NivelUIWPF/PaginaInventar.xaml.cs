using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NivelStocareDate;

namespace NivelUIWPF
{
    // Clasă auxiliară pentru a calcula valoarea stocului
    public class ProdusInventar : MedicamentDB
    {
        public decimal ValoareStoc => Pret * Stoc;
    }

    public partial class PaginaInventar : UserControl
    {
        private List<ProdusInventar> listaInventar = new List<ProdusInventar>();

        public PaginaInventar()
        {
            InitializeComponent();
            IncarcaInventar();
        }

        private void IncarcaInventar()
        {
            var medicamente = ManagerMedicamente.IncarcaMedicamente();

            listaInventar = medicamente.Select(m => new ProdusInventar
            {
                Nume = m.Nume,
                Pret = m.Pret,
                NecesitaReteta = m.NecesitaReteta,
                Stoc = m.Stoc
            }).ToList();

            DgInventar.ItemsSource = listaInventar;
            CalculeazaTotal();
        }

        private void CalculeazaTotal()
        {
            decimal total = listaInventar.Sum(p => p.ValoareStoc);
            TxtValoareTotala.Text = $"{total:N2} RON";
        }

        private void TxtCautaInventar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = TxtCautaInventar.Text.ToLower();
            if (string.IsNullOrEmpty(text))
            {
                DgInventar.ItemsSource = listaInventar;
            }
            else
            {
                var filtrate = listaInventar.Where(p => p.Nume.ToLower().Contains(text)).ToList();
                DgInventar.ItemsSource = filtrate;
            }
        }
    }
}