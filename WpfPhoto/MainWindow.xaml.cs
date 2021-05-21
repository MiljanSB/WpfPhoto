using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace WpfPhoto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string odabranaSlika = "";
        private int indeks = -1;
        private int unos = -1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool Validacija()
        {
            if (string.IsNullOrWhiteSpace(TextBoxNaziv.Text))
            {
                MessageBox.Show("Unesite naziv slike");
                TextBoxNaziv.Focus();
             
                return false;
            }

            if (DatePicker1.SelectedDate == null)
            {
                MessageBox.Show("Odaberi datum");

                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxOpis.Text))
            {
                MessageBox.Show("Unesite opis slike");
                
                TextBoxOpis.Focus();
                return false;
            }

            return true;
        }


        private void DozvoliIzmenu(bool dozvola)
        {
            GroupBox1.IsEnabled = dozvola;

            ButtonNovi.IsEnabled = !dozvola;
            ButtonPromeni.IsEnabled = !dozvola;
        }

        private void Resetuj()
        {
            odabranaSlika = "";
            indeks = -1;
            Image1.Source = null;
            TextBoxNaziv.Clear();
            TextBoxOpis.Clear();
            DatePicker1.SelectedDate = DateTime.Today;
        }

        private void ResetujBordere()
        {
            foreach (Border b in WrapPanel1.Children)
            {
                b.BorderBrush = new SolidColorBrush(Colors.Black);
                b.BorderThickness = new Thickness(1);
            }
        }

        private void SelektujBroder(Border b)
        {
            b.BorderBrush = new SolidColorBrush(Colors.Red);
            b.BorderThickness = new Thickness(2);
        }

        private void PrikaziFotografije()
        {
            WrapPanel1.Children.Clear();

            List<Border> listaBordera = SlikaHelper.VratiListuBordera();

            if (listaBordera != null)
            {
                foreach (Border b in listaBordera)
                {
                    WrapPanel1.Children.Add(b);
                    Image img2 = b.Child as Image;

                    img2.MouseDown += Img2_MouseDown;
                }               
            }        
          
        }

        private void Img2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            odabranaSlika = "";
            ResetujBordere();
            Image img = (Image)sender;
            Border selBorder = img.Parent as Border;
            SelektujBroder(selBorder);
            Fotografija selektovanaFotografija = selBorder.Tag as Fotografija;

            TextBoxNaziv.Text = selektovanaFotografija.Naziv;
            TextBoxOpis.Text = selektovanaFotografija.Opis;
            DatePicker1.SelectedDate = selektovanaFotografija.Datum;

            BitmapImage bmp = SlikaHelper.KreirajBitMapuIzMemorije(selektovanaFotografija.BinarniPodaci);
            Image1.Source = bmp;
            indeks = WrapPanel1.Children.IndexOf(selBorder);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DozvoliIzmenu(false);
            DatePicker1.SelectedDate = DateTime.Today;
            PrikaziFotografije();
        }

        private void ButtonOdaberi_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = @"C:\Slike";
            dlg.Filter = @"Slike|*.jpg;*.bmp;*.png;*.gif";
            
            if (dlg.ShowDialog()==true)
            {
                odabranaSlika = dlg.FileName;

                Uri adresa = new Uri(odabranaSlika, UriKind.Absolute);
                BitmapImage bmp = SlikaHelper.KreirajBitMapu(adresa);
                Image1.Source = bmp;
                TextBoxNaziv.Text = Path.GetFileName(odabranaSlika);
            }
        }

        private void ButtonNovi_Click(object sender, RoutedEventArgs e)
        {
            Resetuj();
            ResetujBordere();
            DozvoliIzmenu(true);
            unos = 1;

        }

        //PROMENI
        private void Promeni(int promeniSliku = 0)
        {
            if (indeks<0)
            {
                return;
            }

            if (!Validacija())
            {
                return;
            }

            Border b = WrapPanel1.Children[indeks] as Border;

            Fotografija selFotografija = b.Tag as Fotografija;

            selFotografija.Naziv = TextBoxNaziv.Text;
            selFotografija.Datum = DatePicker1.SelectedDate.Value;
            selFotografija.Opis = TextBoxOpis.Text;

            if (promeniSliku == 0)
            {
                int rez = FotografijaDal.PromeniFotografiju1(selFotografija);

                if (rez == 0)
                {
                    SelektujBroder(b);
                    MessageBox.Show("Podaci promenjeni");
                    DozvoliIzmenu(false);
                }
            }

            if (promeniSliku == 1)
            {
                Uri adresa = new Uri(odabranaSlika, UriKind.Absolute);
                BitmapImage bmp = SlikaHelper.KreirajBitMapu(adresa);
                selFotografija.BinarniPodaci = SlikaHelper.KreirajNizBajtova(bmp);

                int rez = FotografijaDal.PromeniFotografiju2(selFotografija);

                if (rez==0)
                {
                    PrikaziFotografije();
                    Border b1 = WrapPanel1.Children[indeks] as Border;
                    SelektujBroder(b1);

                    DozvoliIzmenu(false);
                    MessageBox.Show("Podaci promenjeni");
                }
                else
                {
                    MessageBox.Show("Greska pri promeni");
                }
            }
        }

        private void ButtonPromeni_Click(object sender, RoutedEventArgs e)
        {
            if (indeks>-1)
            {

                DozvoliIzmenu(true);
                unos = 0;


            }
            else
            {
                MessageBox.Show("Odaberi sliku");
            }
        }

        private void ButtonOdustani_Click(object sender, RoutedEventArgs e)
        {
            DozvoliIzmenu(false);
            Resetuj();
            ResetujBordere();
        }


        //UBACI
        private void Ubaci()
        {
            if (!Validacija())
            {
                DozvoliIzmenu(true);
                return;
            }

            if (string.IsNullOrWhiteSpace(odabranaSlika))
            {
                MessageBox.Show("Odaberi sliku");
                return;
            }

            Fotografija f = new Fotografija();
            f.Naziv = TextBoxNaziv.Text;
            f.Datum = DatePicker1.SelectedDate.Value;
            f.Opis = TextBoxOpis.Text;

            Uri adresa = new Uri(odabranaSlika, UriKind.Absolute);
            BitmapImage bmp = SlikaHelper.KreirajBitMapu(adresa);
            f.BinarniPodaci = SlikaHelper.KreirajNizBajtova(bmp);

            int rez = FotografijaDal.UbaciFotografiju(f);

            if (rez == 0)
            {
                PrikaziFotografije();
                odabranaSlika = "";
                indeks = WrapPanel1.Children.Count - 1;
                Border b = WrapPanel1.Children[indeks] as Border;

                SelektujBroder(b);
                DozvoliIzmenu(false);
                MessageBox.Show("Slika sacuvana");
                
            }
            else
            {
                MessageBox.Show("Greska pri cuvanju");
            }
        }

        private void ButtonSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            if (unos == 1)
            {
                Ubaci();
                DozvoliIzmenu(false);
            }

            if (unos == 0)
            {
                if (odabranaSlika != "")
                {
                    //menjamo sliku
                    Promeni(1);
                }
                else
                {
                    //ne menjamo sliku
                    Promeni();
                }
            }
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (indeks>-1)
            {
                Border b = WrapPanel1.Children[indeks] as Border;

                Fotografija f = b.Tag as Fotografija;

                int rez = FotografijaDal.ObrisiFotografiju(f.FotografijaId);

                if (rez == 0)
                {
                    PrikaziFotografije();
                    Resetuj();
                    ResetujBordere();
                    DozvoliIzmenu(false);
                    MessageBox.Show("Podaci obrisani");
                }
                else
                {
                    MessageBox.Show("Greska pri brisanju podataka");
                }

            }
        }
    }
}
