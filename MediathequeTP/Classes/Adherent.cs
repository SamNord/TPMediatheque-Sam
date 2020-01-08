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
        private List<Oeuvre> oeuvreEmprunte; public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public List<Oeuvre> OeuvreEmprunte { get => oeuvreEmprunte; set => oeuvreEmprunte = value; }
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
        public string TitresEmprunte
        {
            get
            {
                string titres = "";
                foreach (Oeuvre o in oeuvreEmprunte)
                {
                    titres += $"\n{o.Titre} - {o.Type} - A rendre avant {o.DateRetour.ToString()}";
                }
                return titres;
            }
        }
        public override string ToString()
        {
            return $"Identifiant: {Id}\n\r Nom: {Nom}\n\r  Prénom : {Prenom}\n\r  Téléphone : {Telephone} \n\rListes des oeurvres empruntés : {TitresEmprunte}";
        }
    }
}
