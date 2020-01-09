using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediathequeTP.Classes
{
    public class Oeuvre
    {
        private static int cpt = 0;
        private int id;
        private string titre;
        private DateTime dateEmprunt;
        private DateTime dateRetour;
        private string type;
        private string status;

        public int Id { get => id; set => id = value; }
        public string Titre { get => titre; set => titre = value; }
        public DateTime DateEmprunt { get => dateEmprunt; set => dateEmprunt = value; }
        public DateTime DateRetour { get => dateRetour; set => dateRetour = value; }
        public string Type { get => type; set => type = value; }
        public string Status { get => status; set => status = value; }

        public Oeuvre()
        {
            
        }

        public Oeuvre(string t, string typ, int id)
        {
            cpt++;
            if (File.Exists("oeuvre.txt"))
            {
                StreamReader reader = new StreamReader(File.Open("oeuvre.txt", FileMode.Open));
                Id = Convert.ToInt32(reader.ReadToEnd());
                reader.Dispose();
            }
            Id = id;
            StreamWriter writer = new StreamWriter(File.Open("oeuvre.txt", FileMode.Create));
            writer.WriteLine(Id);
            writer.Dispose();
            Titre = t;
            Type = typ;
            Status = "disponible";
        }

        public override string ToString()
        {
            //return $"Identifiant: {Id}\n\r Titre: {Titre}\n\r Type: {Type}\n\r Statut : {Status}\n\r  Date d'emprunt : {DateEmprunt}\n\r Date retour : {DateRetour}";
            return $"--------------Identifiant: {Id}----------------\n\r Titre: {Titre}\n\r Type: {Type}\n\r" +
              $" Statut : {Status}\n\r" +
              $"  Date d'emprunt : {DateEmprunt.Day}/{DateEmprunt.Month}/{DateEmprunt.Year}\n\r" +
              $" Date retour : {DateRetour.Day}/{DateRetour.Month}/{DateRetour.Year}";
        }
    }
}
