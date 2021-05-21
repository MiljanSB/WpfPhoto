using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WpfPhoto
{
    static class FotografijaDal
    {
        public static List<Fotografija> VratiFotografije()
        {
            List<Fotografija> listaFotografija = new List<Fotografija>();

            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnFotografija)) 
            {
                using (SqlCommand komanda = new SqlCommand("PrikaziFotografije", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        konekcija.Open();
                        using (SqlDataReader dr = komanda.ExecuteReader())
                        {

                            while (dr.Read())
                             {
                                Fotografija f = new Fotografija
                                {
                                    FotografijaId = dr.GetInt32(0),
                                    Naziv = dr.GetString(1),
                                    BinarniPodaci = (byte[])dr[2],
                                    Datum = dr.GetDateTime(3),
                                    Opis = dr.GetString(4)
                                };
                                listaFotografija.Add(f);
                            }
                            
                        }
                        return listaFotografija;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
        }

        public static int UbaciFotografiju(Fotografija f)
        {
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnFotografija))
            {
                using (SqlCommand komanda = new SqlCommand("UbaciFotografiju", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        komanda.Parameters.AddWithValue("@Naziv", f.Naziv);
                        komanda.Parameters.AddWithValue("@BinarniPodaci", f.BinarniPodaci);
                        komanda.Parameters.AddWithValue("@Datum", f.Datum);
                        komanda.Parameters.AddWithValue("@Opis", f.Opis);

                        konekcija.Open();

                        komanda.ExecuteNonQuery();
                        return 0;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
            }
        }

        public static int PromeniFotografiju1(Fotografija f)
        {
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnFotografija))
            {
                using (SqlCommand komanda = new SqlCommand("PromeniFotografiju1", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        komanda.Parameters.AddWithValue("@FotografijaId", f.FotografijaId);
                        komanda.Parameters.AddWithValue("@Naziv", f.Naziv);
                        komanda.Parameters.AddWithValue("@Datum", f.Datum);
                        komanda.Parameters.AddWithValue("@Opis", f.Opis);

                        konekcija.Open();

                        komanda.ExecuteNonQuery();
                        return 0;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }

                }
            }
        }

        public static int PromeniFotografiju2(Fotografija f)
        {
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnFotografija))
            {
                using (SqlCommand komanda = new SqlCommand("PromeniFotografiju2", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        komanda.Parameters.AddWithValue("@FotografijaId", f.FotografijaId);
                        komanda.Parameters.AddWithValue("@Naziv", f.Naziv);
                        komanda.Parameters.AddWithValue("@BinarniPodaci", f.BinarniPodaci);
                        komanda.Parameters.AddWithValue("@Datum", f.Datum);
                        komanda.Parameters.AddWithValue("@Opis", f.Opis);

                        konekcija.Open();

                        komanda.ExecuteNonQuery();
                        return 0;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }

                }
            }
        }

        public static int ObrisiFotografiju(int id)
        {
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnFotografija))
            {
                using (SqlCommand komanda = new SqlCommand("ObrisiFotografiju", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        komanda.Parameters.AddWithValue("@FotografijaId",id);

                        konekcija.Open();

                        komanda.ExecuteNonQuery();
                        return 0;
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
            }
        }
    }
}
