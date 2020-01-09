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

            adherent = new Adherent(nom, prenom, tel, mediatheque.ListeAdherent.Count < 1 ? 1 : mediatheque.ListeAdherent[mediatheque.ListeAdherent.Count - 1].Id + 1);

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
            if (adherent != null)
            {
                mediatheque.SupprimerAdherent(adherent);
                ChangeText("adhérent n°" + numero + " supprimé", ConsoleColor.Red);
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
                if (oeuvre.Status == "disponible")
                {
                    mediatheque.SupprimerOeuvre(oeuvre);
                    ChangeText("oeuvre n° " + numero + " supprimé", ConsoleColor.Red);
                }

                else
                {
                    ChangeText("L'oeuvre est emprunté !", ConsoleColor.Red);
                }

            }
            else
            {
                ChangeText("Pas d'article à ce numéro", ConsoleColor.Red);
            }
        }

        public void ListAdherent()
        {
            //si la liste n'est pas vide, on l'affiche
            if (mediatheque.ListeAdherent.Count > 0)
            {
                ChangeText("**********************Liste des Adhérents********************", ConsoleColor.Green);
                foreach (Adherent a in mediatheque.ListeAdherent)
                {
                    Console.WriteLine(a);
                }
                ChangeText("**********************Fin Liste********************", ConsoleColor.Green);
            }
            //sinon on affiche un message
            else
            {
                ChangeText("La liste est vide, veuillez ajouter un adhérent", ConsoleColor.Red);
            }

        }

        public void ListOeuvre()
        {
            //si la liste n'est pas vide, on l'affiche
            if (mediatheque.ListeOeuvre.Count > 0)
            {
                ChangeText("************************Liste des Oeuvres*********************", ConsoleColor.Magenta);
                foreach (Oeuvre o in mediatheque.ListeOeuvre)
                {
                    Console.WriteLine(o);
                }
                ChangeText("*********************Fin Liste****************", ConsoleColor.Magenta);
            }
            //sinon on affiche un message
            else
            {
                ChangeText("La liste est vide, veuillez ajouter une oeuvre", ConsoleColor.Red);
            }
        }

        public void EmprunterOeuvre()
        {
            //Affichage de la liste des oeuvres pour pouvoir choisir
            ListOeuvre();
            Console.WriteLine("votre identifiant : ");
            Int32.TryParse(Console.ReadLine(), out int identifiant);
            adherent = mediatheque.GetAdherentById(identifiant);

            if (adherent != null)
            {
                Console.WriteLine("Numéro de l'oeuvre à emprunter : ");
                Int32.TryParse(Console.ReadLine(), out int id);
                //on récupère l'oeuvre correspondante au numéro à emprunter
                oeuvre = mediatheque.GetOeuvreById(id);
                //si cette oeuvre existe, et si son status est disponible alors on procède à son emprunt
                if (oeuvre != null)
                {
                    if (oeuvre.Status == "disponible")
                    {
                        //méthode changer status de l'oeuvre en emprunté                  
                        DateTime dateEmprunt = DateTime.Now;
                        /*On ajoute 15 jours à la date d'emprunt via la méthode AddDays(nombre de jours à ajouter en int)*/
                        DateTime dateRetour = dateEmprunt.AddDays(15);
                        /*on abonne l'événement définit dans Mediatheque à une méthode sans paramètre qui envoit un message */
                        mediatheque.Emprunte += () => ChangeText("Livre n°" + id + " emprunté", ConsoleColor.Yellow);
                        /*j'ai abonné une autre méthode(voir plus bas) à lévénement qui déclenche un effet sonore*/
                        mediatheque.Emprunte += AlertSonore;
                        /*en appelant la méthode Emprunter décrit dans Mediatheque, on déclenche également l'événement qu'on a mis à l'intérieur de Emprunter*/
                        //dans la méthode emprunter on sauvegarde et on ajoute l'oeuvre empruntée au compte de l'adhérent
                        mediatheque.Emprunter(id, "emprunté", dateEmprunt, dateRetour, identifiant);

                    }
                    //sinon on affiche un message
                    else
                    {
                        ChangeText("Désolé, cet oeuvre n'est pas disponible", ConsoleColor.Magenta);
                    }
                }
                //si l'oeuvre n'appartient pas à la médiathèque, on affiche un message
                else
                {
                    ChangeText("Pas d'oeuvre à ce numéro", ConsoleColor.Red);
                }
            }

            else
            {
                ChangeText("Vous n'êtes pas inscrit, veuillez vous inscrire", ConsoleColor.Red);
            }
        }

        public void RendreOeuvre()
        {
            Console.WriteLine("votre identifiant : ");
            Int32.TryParse(Console.ReadLine(), out int identifiant);
            //on recherche l'adhérent
            adherent = mediatheque.GetAdherentById(identifiant);
            if (adherent != null)
            {
                Console.WriteLine(adherent);

                if (adherent.OeuvreEmprunte.Count > 0)
                {
                    Console.WriteLine("Numéro de l'oeuvre à rendre : ");
                    Int32.TryParse(Console.ReadLine(), out int id);
                    oeuvre = mediatheque.GetOeuvreById(id);
                    if (oeuvre != null)
                    {
                        mediatheque.OeuvreDispo += () => ChangeText("Livre n°" + id + " est disponible", ConsoleColor.Green);
                        mediatheque.OeuvreDispo += AlertSonore;
                        DateTime dateRendu = DateTime.Now;
                        mediatheque.Rendre(id, dateRendu, "disponible", identifiant);
                    }
                }
                else
                {
                    ChangeText("votre liste est vide!!", ConsoleColor.Red);
                }
            }

            else
            {
                ChangeText("Vous n'êtes pas inscrit, veuillez vous inscrire", ConsoleColor.Red);
            }
        }

        public void GetOeuvreByStatut()
        {
            Console.WriteLine("afficher liste des oeuvres : emprunté/disponible?");
            string stat = Console.ReadLine();
            if (stat == "disponible")
            {
                ChangeText("************Liste des oeuvres disponibles******", ConsoleColor.Cyan);
                mediatheque.GetOeuvreDispo("disponible");
            }
            else
            {
                ChangeText("**************Liste des oeuvres empruntées******", ConsoleColor.Blue);
                mediatheque.GetOeuvreDispo("emprunté");
            }

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

        public static void AlertSonore()
        {
            Console.Beep(659, 125);
            Console.Beep(659, 125);
            Console.Beep(32400, 125);
        }

    }
}
