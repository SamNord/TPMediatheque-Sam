using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediathequeTP.Classes
{
    public class Mediatheque
    {
        private string nom;
        List<Adherent> listeAdherent;
        List<Oeuvre> listeOeuvre;
        private int nbMax;
        public event Action<int> MaxOeuvreAtteint;
        public event Action Emprunte;
        public event Action OeuvreDispo;


        public List<Adherent> ListeAdherent { get => listeAdherent; set => listeAdherent = value; }
        public List<Oeuvre> ListeOeuvre { get => listeOeuvre; set => listeOeuvre = value; }
        public string Nom { get => nom; set => nom = value; }
        public int NbMax { get => nbMax; set => nbMax = 0; }

        public Mediatheque() { }

        public Mediatheque(string nom)
        {
            Nom = nom;
            if (File.Exists(Nom + "-adherent.json"))
            {
                LireAdherentJson();
            }
            else
            {
                ListeAdherent = new List<Adherent>();
            }
            if (File.Exists(Nom + "-oeuvre.json"))
            {
                LireOeuvreJson();
            }
            else
            {
                ListeOeuvre = new List<Oeuvre>();
            }
        }

        private void LireAdherentJson()
        {
            StreamReader reader = new StreamReader(File.Open(Nom + "-adherent.json", FileMode.Open));
            ListeAdherent = JsonConvert.DeserializeObject<List<Adherent>>(reader.ReadToEnd());
            reader.Dispose();
        }

        private void LireOeuvreJson()
        {
            StreamReader reader = new StreamReader(File.Open(Nom + "-oeuvre.json", FileMode.Open));
            ListeOeuvre = JsonConvert.DeserializeObject<List<Oeuvre>>(reader.ReadToEnd());
            reader.Dispose();
        }

        private void SauvegardeAdherent()
        {
            StreamWriter writer = new StreamWriter(File.Open(Nom + "-adherent.json", FileMode.Create));
            writer.WriteLine(JsonConvert.SerializeObject(ListeAdherent));
            writer.Dispose();
        }

        private void SauvegardeOeuvre()
        {
            StreamWriter writer = new StreamWriter(File.Open(Nom + "-oeuvre.json", FileMode.Create));
            writer.WriteLine(JsonConvert.SerializeObject(ListeOeuvre));
            writer.Dispose();
        }

        public void AjouterAdherent(Adherent a)
        {
            ListeAdherent.Add(a);
            SauvegardeAdherent();
        }

        public void AjouterOeuvre(Oeuvre o)
        {
            ListeOeuvre.Add(o);
            SauvegardeOeuvre();
        }

        public void SupprimerAdherent(Adherent a)
        {
            ListeAdherent.Remove(a);
            SauvegardeAdherent();
        }

        public void SupprimerOeuvre(Oeuvre o)
        {
            ListeOeuvre.Remove(o);
            SauvegardeOeuvre();
        }

        public Adherent GetAdherentById(int id)
        {
            Adherent a = null;
            foreach (Adherent ad in ListeAdherent)
            {
                if (ad.Id == id)
                {
                    a = ad;
                    break;
                }
            }
            return a;
        }

        public Oeuvre GetOeuvreById(int id)
        {
            //Oeuvre oeuvre = null;
            //foreach (Oeuvre o in ListeOeuvre)
            //{
            //    if (o.Id == id)
            //    {
            //        oeuvre = o;
            //        break;
            //    }
            //}
            //return oeuvre;
            return ListeOeuvre.Find(x => x.Id == id);

        }

        public void GetOeuvreDispo(string statut)
        {
            IHMMediatheque.ChangeText("*********Liste des oeuvres dispo ou empruntés", ConsoleColor.Yellow);
             foreach(Oeuvre o in ListeOeuvre)
            {
                if(o.Status == statut)
                {
                    Console.WriteLine(o);
                }
            }
        }

        public void Emprunter(int numero, string statut, DateTime de, DateTime dr)
        {
            if (NbMax <= 3)
            {
                foreach (Oeuvre o in ListeOeuvre)
                {
                    if (o.Id == numero)
                    {
                        o.Status = statut;
                        o.DateEmprunt = de;
                        o.DateRetour = dr;
                        NbMax++;
                        Emprunte?.Invoke();
                        break;
                    }
                }
            }
            else
            {
                MaxOeuvreAtteint?.Invoke(NbMax);
                IHMMediatheque.ChangeText("Vous avez déjà 3 livres empruntés", ConsoleColor.Red);
            }

            SauvegardeOeuvre();
        }

        public void Rendre(int id, DateTime dr, string statut)
        {
            foreach(Oeuvre o in ListeOeuvre)
            {
                if(o.Id == id)
                {
                    o.Status = statut;
                    o.DateRetour = DateTime.Now;
                    NbMax--;
                    OeuvreDispo?.Invoke();
                }
            }

        }


    }
}
