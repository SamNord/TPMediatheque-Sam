using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MediathequeTP.Classes
{
    public class Mediatheque
    {
        //mis à jour--> on utilise les locks et les tasks au niveau des lecture et écritures des fichiers
        static object _lock = new object();
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

        //Dans le constructeur, pour utiliser les task on appelle les méthodes définit tout en bas au lieu des méthodes habituelles avec json
        //on n'oublie pas d'intégrer le lock au sein des méthodes habituelles

        public Mediatheque(string nom)
        {
            Nom = nom;
            if (File.Exists(Nom + "-adherent.json"))
            {
                //LireAdherentJson();
                LireAdherentTask();
            }
            else
            {
                ListeAdherent = new List<Adherent>();
            }
            if (File.Exists(Nom + "-oeuvre.json"))
            {
                //LireOeuvreJson();
                LireOeuvreTask();
            }
            else
            {
                ListeOeuvre = new List<Oeuvre>();
            }
        }

        private void LireAdherentJson()
        {
            lock (_lock)
            {
                StreamReader reader = new StreamReader(File.Open(Nom + "-adherent.json", FileMode.Open));
                ListeAdherent = JsonConvert.DeserializeObject<List<Adherent>>(reader.ReadToEnd());
                reader.Dispose();
            }

        }

        private void LireOeuvreJson()
        {
            lock (_lock)
            {
                StreamReader reader = new StreamReader(File.Open(Nom + "-oeuvre.json", FileMode.Open));
                ListeOeuvre = JsonConvert.DeserializeObject<List<Oeuvre>>(reader.ReadToEnd());
                reader.Dispose();
            }

        }

        private void SauvegardeAdherent()
        {
            lock (_lock)
            {
                StreamWriter writer = new StreamWriter(File.Open(Nom + "-adherent.json", FileMode.Create));
                writer.WriteLine(JsonConvert.SerializeObject(ListeAdherent));
                writer.Dispose();
            }
        }

        private void SauvegardeOeuvre()
        {
            lock (_lock)
            {
                StreamWriter writer = new StreamWriter(File.Open(Nom + "-oeuvre.json", FileMode.Create));
                writer.WriteLine(JsonConvert.SerializeObject(ListeOeuvre));
                writer.Dispose();
            }

        }

        /******************Ajouter une oeuvre ******************************************************************/
        public void AjouterAdherent(Adherent a)
        {
            ListeAdherent.Add(a);
            //SauvegardeAdherent();
            SauvegardeAdherentTask();
        }

        /******************Ajouter une oeuvre ******************************************************************/
        public void AjouterOeuvre(Oeuvre o)
        {
            ListeOeuvre.Add(o);
            //SauvegardeOeuvre();
            SauvegardeOeuvreTask();
        }

        /******************Supprimer un adhérent ******************************************************************/
        public void SupprimerAdherent(Adherent a)
        {
            ListeAdherent.Remove(a);
            //SauvegardeAdherent();
            SauvegardeAdherentTask();
        }

        /******************Supprimer une oeuvre ******************************************************************/
        public void SupprimerOeuvre(Oeuvre o)
        {
            ListeOeuvre.Remove(o);
            //SauvegardeOeuvre();
            SauvegardeOeuvreTask();
        }

        /******************Rechercher un adhérent par son numéro --> id ********************************************/
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

        /******************Rechercher une oeuvre par son numéro --> id ********************************************/
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
            /*équivaut à :*/
            return ListeOeuvre.Find(x => x.Id == id);

        }

        /******************Rechercher les oeuvres par leur statut : disponible ou emprunté***********************************/
        public void GetOeuvreDispo(string statut)
        {

            foreach (Oeuvre o in ListeOeuvre)
            {
                if (o.Status == statut)
                {
                    Console.WriteLine(o);
                }
            }
        }

        /******************Emprunter une oeuvre avec événement intégré--> Emprunte => pou signaler que l'oeuvre est emprunté*/
        public void Emprunter(int numero, string statut, DateTime de, DateTime dr, int idAdherent)
        {
            Adherent ad = GetAdherentById(idAdherent);
            if (ad != null)
            {
                if (ad.OeuvreEmprunte.Count < 3)
                {
                    foreach (Oeuvre o in ListeOeuvre)
                    {
                        if (o.Id == numero)
                        {
                            o.Status = statut;
                            o.DateEmprunt = de;
                            o.DateRetour = dr;
                            ad.OeuvreEmprunte.Add(o);
                            //SauvegardeAdherent();
                            //SauvegardeOeuvre();
                            Emprunte?.Invoke();
                            break;
                        }
                    }
                }
                else
                {
                    MaxOeuvreAtteint?.Invoke(ad.OeuvreEmprunte.Count);
                    IHMMediatheque.ChangeText("Vous avez déjà 3 livres empruntés", ConsoleColor.Red);
                }

            }
            else
            {
                IHMMediatheque.ChangeText("Vous n'êtes pas inscrit", ConsoleColor.Red);
            }

            //SauvegardeOeuvre();
            SauvegardeOeuvreTask();
            //SauvegardeAdherent();
            SauvegardeAdherentTask();
        }

        /******************Rendre une oeuvre avec événement intégré déclarer tout en haut de type Action--> OeuvreDispo pour signaler que l'oeuvre est dispo**/
        public void Rendre(int id, DateTime dr, string statut, int idAdherent)
        {
            /*On demande d'abord l'identifiant de l'adhérent et on le cherche par son identifiant
             idAdherent...*/
            //Adherent ad = GetAdherentById(idAdherent);
            /*S'il est inscrit alors on procède au retour du livre*/
            //if (ad != null)
            //{
            /*On parcourt la liste des livres et on le cherche par son numéro*/
            //foreach (Oeuvre o in ListeOeuvre)
            //{
            /*si l'oeuvre appartient à la médiathèque alors on change son statut ainsi que 
             sa date de retour puis on l'enlève de la liste des livres emprunté de l'adhérent*/
            //if (o.Id == id)
            //{

            //    o.Status = statut;
            //    o.DateRetour = DateTime.Now;
            //    ad.OeuvreEmprunte.Remove(o);
            //    //SauvegardeAdherent();
            //    OeuvreDispo?.Invoke();
            //}
            //}
            //}
            /**********EXCEPTION intégrée********************/
            try
            {
                Adherent ad = GetAdherentById(idAdherent);
                if (ad != null)
                {
                    foreach (Oeuvre o in ad.OeuvreEmprunte)
                    {
                        if (o.Id == id)
                        {
                            o.Status = statut;
                            o.DateRetour = DateTime.Now;
                            ad.OeuvreEmprunte.Remove(o);
                            SauvegardeAdherent();
                            SauvegardeOeuvre();
                            OeuvreDispo?.Invoke();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                IHMMediatheque.ChangeText("Je suis une exception....Bizarre! ça marche quand même(..)!", ConsoleColor.Magenta);
            }

            /*on utilise les méthodes qui applique les tasks et lock définit tout en bas*/
            //SauvegardeAdherent();
            SauvegardeAdherentTask();
            //SauvegardeOeuvre();
            SauvegardeOeuvreTask();
        }

        /******************************Méthodes pour utiliser les Tasks*******************************************/
        /*********************************************************************************************************/
        public void LireAdherentTask()
        {
            Task t = new Task(() => LireAdherentJson());
            t.Start();
        }

        public void SauvegardeAdherentTask()
        {
            Task t = new Task(() => SauvegardeAdherent());
            t.Start();
        }

        public void LireOeuvreTask()
        {
            Task t = new Task(() => LireOeuvreJson());
            t.Start();
        }

        public void SauvegardeOeuvreTask()
        {
            Task t = new Task(() => SauvegardeOeuvre());
            t.Start();
        }


    }
}
