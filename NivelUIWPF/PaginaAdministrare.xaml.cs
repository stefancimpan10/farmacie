using System;
using System.Collections.Generic;
using System.Text;
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
    public partial class PaginaAdministrare : UserControl
    {
        private List<MedicamentDB> listaProduseAdmin = new List<MedicamentDB>();

        public PaginaAdministrare()
        {
            InitializeComponent();
            IncarcaDate();
        }

        private void IncarcaDate()
        {
            listaProduseAdmin = ManagerMedicamente.IncarcaMedicamente();
            DgAdmin.ItemsSource = listaProduseAdmin;
        }

        // Se apelează când editezi o celulă (schimbi preț sau stoc)
        private void DgAdmin_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Nu facem nimic aici, lăsăm utilizatorul să editeze liber.
            // Salvarea se face la buton.
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Salvăm lista modificată înapoi în fișierul JSON
                ManagerMedicamente.SalveazaMedicamente(listaProduseAdmin);
                MessageBox.Show("Modificările au fost salvate cu succes!", "Succes");

                // Reîncărcăm datele ca să fim siguri
                IncarcaDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvare: {ex.Message}", "Eroare");
            }
        }
    }
}
