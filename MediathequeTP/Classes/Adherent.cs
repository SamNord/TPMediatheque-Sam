using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediathequeTP.Classes
{
    public class Adherent
    {
        private static int cpt = 0;
        private int id;
        private string nom;
        private string prenom;
        private string telephone;
        private List<Oeuvre> oeuvreEmprunte;
        private Oeuvre oeuvre;
        private string titresEmprunte;

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public List<Oeuvre> OeuvreEmprunte { get => oeuvreEmprunte; set => oeuvreEmprunte = value; }
        public string TitresEmprunte { get => titresEmprunte; set => titresEmprunte = value; }

        public Adherent() { }

        public Adherent(string n, string p, string t, int id)
        {
            cpt++;
            if (File.Exists("adherent.txt"))
            {
                StreamReader reader = new StreamReader(File.Open("adherent.txt", FileMode.Open));
                Id = Convert.ToInt32(reader.ReadToEnd());
                reader.Dispose();
            }
            Id = id;
            StreamWriter writer = new StreamWriter(File.Open("adherent.txt", FileMode.Create));
            writer.WriteLine(Id);
            writer.Dispose();
            Nom = n;
            Prenom = p;
            Telephone = t;
            OeuvreEmprunte = new List<Oeuvre>();
        }

        public string TitresE()
        {
            TitresEmprunte = "";
            foreach (Oeuvre o in OeuvreEmprunte)
            {
                TitresEmprunte += o.Titre + " ";
            }
            return TitresEmprunte;
        }

        public override string ToString()
        {
            return $"Identifiant: {Id}\n\r Nom: {Nom}\n\r  Prénom : {Prenom}\n\r  Téléphone : {Telephone} \n\rListes des oeurvres empruntés : {TitresEmprunte}";
        }
    }
}
