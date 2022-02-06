using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_OblsugaPlikow.Models;
using WPF_OblsugaPlikow.Services;

namespace WPF_OblsugaPlikow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IOsobaService _osobaService;
        public MainWindow()
        {
            InitializeComponent();
            _osobaService = new OsobaService();
            ZaladujDane();
        }

        private void BT_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            OsobaDTO osoba = PobierzDaneZFormularza();
            if (osoba != null)
            {
                _osobaService.StworzOsobe(osoba);
                WyczcyscFormularz();
                ZaladujDane();
            }

        }

        private void WyczcyscFormularz()
        {
            TB_ImieNazwisko.Text = string.Empty;
            TB_Wiek.Text = string.Empty;
        }

        private OsobaDTO PobierzDaneZFormularza()
        {
            try
            {
                string imieNazwisko = TB_ImieNazwisko.Text;
                int wiek = int.Parse(TB_Wiek.Text);
                OsobaDTO o = new OsobaDTO
                {
                    ImieNazwisko = imieNazwisko,
                    Wiek = wiek
                };
                return o;
            }
            catch (Exception)
            {
                MessageBox.Show("bledne dane");
            }
            return null;

        }

        private void ZaladujDane()
        {
            DG_dane.ItemsSource = null;
            DG_dane.ItemsSource = _osobaService.GetOsoby();
        }


        private void BT_Restet_Click(object sender, RoutedEventArgs e)
        {
            _osobaService.ResetDanych();
            WyczcyscFormularz();
            ZaladujDane();

        }

        private void BT_Usun_Click(object sender, RoutedEventArgs e)
        {
            var osoba = DG_dane.SelectedItem as OsobaDTO;
            var rezultat = _osobaService.UsunOsobe(osoba.Id);
            if (!rezultat) MessageBox.Show("blad usuniecia");
            ZaladujDane();
        }


        int _id;
        private void BT_Edycja_Click(object sender, RoutedEventArgs e)
        {
            var osoba = DG_dane.SelectedItem as OsobaDTO;
            _id = osoba.Id;
            EdycjaDanych(osoba);
        }

        private void EdycjaDanych(OsobaDTO osoba)
        {
            WyczcyscFormularz();
            TB_ImieNazwisko.Text = osoba.ImieNazwisko;
            TB_Wiek.Text = osoba.Wiek.ToString();
            BT_Akcja.Content = "Edytuj";
            BT_Akcja.Click -= BT_Dodaj_Click;
            BT_Akcja.Click += EdytujDoPliku;
        }

        private void EdytujDoPliku(object sender, RoutedEventArgs e)
        {
            var osoba = PobierzDaneZFormularza();
            var rezultat = _osobaService.AktualizujOsobe(_id, osoba);
            if (!rezultat) MessageBox.Show("blad edycji");
            ZaladujDane();
            WyczcyscFormularz();
            BT_Akcja.Content = "Dodaj";
            BT_Akcja.Click -= EdytujDoPliku;
            BT_Akcja.Click += BT_Dodaj_Click;

        }

        private void ValidationTextBoxOnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }
    }
}
