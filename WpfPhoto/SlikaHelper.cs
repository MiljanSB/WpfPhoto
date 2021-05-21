using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfPhoto
{
    static class SlikaHelper
    {
        public static byte[] KreirajNizBajtova(BitmapImage bmp)
        {
            BitmapFrame bf = BitmapFrame.Create(bmp);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(bf);

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        public static BitmapImage KreirajBitMapu(Uri adresa)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = adresa;
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.DecodePixelWidth = 150;
            bmp.EndInit();
            return bmp;
        }

        public static BitmapImage KreirajBitMapuIzMemorije(byte[] podaci)
        {
            using (MemoryStream ms = new MemoryStream(podaci))
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.DecodePixelWidth = 150;
                bmp.EndInit();
                return bmp;
            }           
        }

        public static List<Border> VratiListuBordera()
        {
            List<Border> listaBordera = new List<Border>();
            List<Fotografija> listaFotografija = FotografijaDal.VratiFotografije();

            if (listaFotografija != null)
            {
                foreach (Fotografija f in listaFotografija)
                {
                    BitmapImage bmp = KreirajBitMapuIzMemorije(f.BinarniPodaci);
                    Image img1 = new Image();
                    img1.Source = bmp;
                    img1.Stretch = Stretch.Fill;

                    Border b = new Border {
                        Width = 80,
                        Height = 60,
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new System.Windows.Thickness(1),
                        Margin = new System.Windows.Thickness(5),
                        Tag = f
                    };

                    b.Child = img1;
                    listaBordera.Add(b);
                }
                return listaBordera;
            }
            else
            {
                return null;
            }
        }
    }
}
