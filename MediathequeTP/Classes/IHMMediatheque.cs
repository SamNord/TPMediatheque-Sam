using System;
using System.Collections.Generic;
using System.Text;

namespace MediathequeTP.Classes
{
    public class IHMMediatheque
    {

        private Oeuvre oeuvre;
        private Adherent adherent;
        private Mediatheque mediatheque;


        public IHMMediatheque() { }

        /*****************************************DEBUT START******************************************/
        /***************************************************************************************/
        public void Start()
        {
            //Oeuvre oeuvre;
            //Adherent adherent;
            Console.WriteLine("Nom de la médiathèque : ");
            string n = Console.ReadLine();
            mediatheque = new Mediatheque(n);
            int choix;

            do
            {
                Menu();
                Int32.TryParse(Console.ReadLine(), out choix);

                switch (choix)
                {
                    case 1:
                        AddAdherent();
                        break;
                    case 2:
                        DeleteAdherent();
                        break;
                    case 3:
                        AddOeuvre();
                        break;
                    case 4:
                        DeleteOeuvre();
                        break;
                    case 5:
                        EmprunterOeuvre();
                        break;
                    case 6:
                        RendreOeuvre();
                        break;
                    case 7:
                        ListAdherent();
                        break;
                    case 8:
                        ListOeuvre();
                        break;
                    case 9:
                        GetOeuvreByStatut();
                        break;
                }

            } while (choix != 0);

        }

        /*****************************************METHODES******************************************/
        /***************************************************************************************/
        public Oeuvre AddOeuvre()
        {
            Console.Write("titre de l'oeuvre : ");
            string titre = Console.ReadLine();

            Console.Write("type de l'oeuvre : ");
            string type = Console.ReadLine();

            oeuvre = new Oeuvre(titre, type, mediatheque.ListeOeuvre.Count < 1 ? 1 : mediatheque.ListeOeuvre[mediatheque.ListeOeuvre.Count - 1].Id + 1);

            mediatheque.AjouterOeuvre(oeuvre);

            Console.WriteLine("oeuvre ajouté avec le numéro " + oeuvre.Id);

            return oeuvre;
        }

        public Adherent AddAdherent()
        {
            Console.Write("Nom  : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();
            Console.Write("Téléphone : ");
            string tel = Console.ReadLine();

            adherent = new Adherent(nom, prenom,tel, mediatheque.ListeAdherent.Count < 1 ? 1 : mediatheque.ListeAdherent[mediatheque.ListeAdherent.Count - 1].Id + 1);

            mediatheque.AjouterAdherent(adherent);

            ChangeText("Vous êtes inscrit sous le numéro " + adherent.Id, ConsoleColor.Cyan);

            return adherent;
        }

        public void DeleteAdherent()
        {
            ListAdherent();
            Console.WriteLine("numéro adhérent à supprimer : ");
            Int32.TryParse(Console.ReadLine(), out int numero);
            adherent = mediatheque.GetAdherentById(numero);
            if(adherent != null)
            {
                mediatheque.SupprimerAdherent(adherent);
                ChangeText("adhérent n°"+numero+" supprimé", ConsoleColor.Red);
            }
            else
            {
                ChangeText("Pas d'adhérent inscrit à ce numéro", ConsoleColor.Red);
            }

        }

        public void DeleteOeuvre()
        {
            ChangeText("***************Liste des Oeuvres****************", ConsoleColor.Magenta);
            ListOeuvre();
            Console.WriteLine("numéro oeuvre à supprimer : ");
            Int32.TryParse(Console.ReadLine(), out int numero);

            oeuvre = mediatheque.GetOeuvreById(numero);
            if (oeuvre != null)
            {
                mediatheque.SupprimerOeuvre(oeuvre);
                ChangeText("oeuvre n° " + numero + " supprimé", ConsoleColor.Red);
            }
            else
            {
                ChangeText("Pas d'article à ce numéro", ConsoleColor.Red);
            }
        }

        public void ListAdherent()
        {
            ChangeText("******************Liste des Adhérents****************", ConsoleColor.Green);
            foreach (Adherent a in mediatheque.ListeAdherent)
            {
                Console.WriteLine(a);
            }
            ChangeText("**********************Fin Liste********************", ConsoleColor.Green);
        }

        public void ListOeuvre()
        {
            ChangeText("***************Liste des Oeuvres****************", ConsoleColor.Magenta);
            foreach (Oeuvre o in mediatheque.ListeOeuvre)
            {
                Console.WriteLine(o);
            }
            ChangeText("*********************Fin Liste****************", ConsoleColor.Magenta);
        }

        public void EmprunterOeuvre()
        {
            ListOeuvre();
            Console.WriteLine("votre identifiant : ");
            Int32.TryParse(Console.ReadLine(), out int identifiant);
            Console.WriteLine("Numéro de l'oeuvre à emprunter : ");
            Int32.TryParse(Console.ReadLine(), out int id);
            oeuvre = mediatheque.GetOeuvreById(id);
            if(oeuvre != null)
            {
                if (oeuvre.Status == "disponible")
                {
                    //méthode changer status de l'oeuvre en emprunté
                    DateTime dateEmprunt = DateTime.Now;
                    DateTime dateRetour = dateEmprunt.AddDays(15);
                    mediatheque.Emprunte += () => ChangeText("Livre n°" + id + " emprunté", ConsoleColor.Yellow);
                    mediatheque.Emprunter(id, "emprunté", dateEmprunt, dateRetour, identifiant);
                   
                }
                else
                {
                    ChangeText("Désolé, cet oeuvre n'est pas disponible", ConsoleColor.Magenta);
                }
            }

            else
            {
                ChangeText("Pas d'oeuvre à ce numéro", ConsoleColor.Red);
            }
       
        }

        public void RendreOeuvre()
        {
            Console.WriteLine("Numéro de l'oeuvre à rendre : ");
            Int32.TryParse(Console.ReadLine(), out int id);
            oeuvre = mediatheque.GetOeuvreById(id);
            if(oeuvre != null)
            {
                mediatheque.OeuvreDispo += () => ChangeText("Livre n°" + id + " est disponible", ConsoleColor.Green);
                DateTime dateRendu = DateTime.Now;
                mediatheque.Rendre(id, dateRendu, "disponible");
            }
        }

        public void GetOeuvreByStatut()
        {         
            Console.WriteLine("Liste : disponible/emprunté");
            string stat = Console.ReadLine();
             mediatheque.GetOeuvreDispo(stat);
        }

        /*****************************************MENU******************************************/
        /***************************************************************************************/
        public static void Menu()
        {
            ChangeText("1- Ajouter un adhérent", ConsoleColor.Green);
            ChangeText("2- Supprimer un adhérent", ConsoleColor.Green);
            ChangeText("3- Ajouter une oeuvre", ConsoleColor.Yellow);
            ChangeText("4- Supprimer une oeuvre", ConsoleColor.Yellow);
            ChangeText("5- Emprunter une oeuvre", ConsoleColor.Cyan);
            ChangeText("6- Rendre une oeuvre", ConsoleColor.Cyan);
            ChangeText("7- Liste des adhérents", ConsoleColor.Green);
            ChangeText("8- Liste des oeuvres", ConsoleColor.Yellow);
            ChangeText("9- Liste des oeuvres par statut", ConsoleColor.Yellow);
        }

        /******************************Méthode pour changer la couleur du texte*************************************/
        public static void ChangeText(string m, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(m);
        }

    }
}
