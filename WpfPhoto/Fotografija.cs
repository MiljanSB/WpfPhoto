using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPhoto
{
    class Fotografija
    {
        public int FotografijaId { get; set; }
        public string Naziv { get; set; }
        public byte[] BinarniPodaci { get; set; }
        public DateTime Datum { get; set; }
        public string Opis { get; set; }

    }
}
