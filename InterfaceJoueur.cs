using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleVeau
{
    class InterfaceJoueur
    {
        /// <summary>
        /// Gère la selection des cibles sur la grille et l'affichage du menu continue avec echap
        /// </summary>
        /// <param name="tirsDispos">nombre de tir</param>
        /// <param name="offsetGrille">décalage entre la grille et le bord gauche de l'écran</param>
        /// <param name="coordTirs"></param>
        /// <param name="menu">fonction lancant le menu continue</param>
        public static void DemanderTirs(int tirsDispos, int offsetGrille, int[,] coordTirs, Action menu)
        { 
            int[] coord = new int[2];

            for (int i = 0; i < tirsDispos; i++)
            {
                 coord = DemanderTir(offsetGrille, coordTirs, menu);

                 coordTirs[i, 0] = coord[0];
                 coordTirs[i, 1] = coord[1];
            }
        }

        /// <summary>
        /// Gère la selection d'une cible sur la grille et l'affichage du menu continue avec echap
        /// </summary>
        /// <param name="offsetGrille">décalage entre la grille et le bord gauche de l'écran</param>
        /// <param name="coordTirs"></param>
        /// <param name="menu">fonction lancant le menu continue</param>
        /// <returns></returns>
        public static int[] DemanderTir(int offsetGrille, int[,] coordTirs, Action menu)
        {
            int i = 0;
            int[] coord;

            if (coordTirs[0,0] == -1)
            {
                coord = new int[2] { 0, 0 };
            }
            else
            {
                for(i = 0; i<coordTirs.GetLength(0) && coordTirs[i, 0] != -1; i++)
                {
                    continue;
                }

                coord = new int[] { coordTirs[i-1, 0], coordTirs[i-1, 1] };
            }
            

            ConsoleKeyInfo ck;

            bool onOtherCurser = false;
            bool deletePrevCurser = false;
            
            int c = 0;
            int[] consolecur = new int[2];

            do
            {
                onOtherCurser = false;

                for (i = 0; i < coordTirs.GetLength(0); i++)
                {
                    if (coordTirs[i, 0] == -1)
                    {
                        break;
                    }
                    else if (coord[0] == coordTirs[i, 0] && coord[1] == coordTirs[i, 1])
                    {
                        onOtherCurser = true;
                    }
                }

                if (!deletePrevCurser)
                {
                    AfficherCurseur(coord, offsetGrille, false);
                }
                else
                {
                    AfficherCurseur(coord, offsetGrille, c != 0);
                }

                deletePrevCurser = !onOtherCurser;      
                
                ck = Console.ReadKey();

                if (ck.Key != ConsoleKey.Enter)
                    Console.Write("\b\u2502");

                if(ck.Key == ConsoleKey.UpArrow && coord[0]>0)
                {
                    coord[0] = coord[0] - 1;
                }

                else if (ck.Key == ConsoleKey.DownArrow && coord[0] <9)
                {
                    coord[0] = coord[0] + 1;
                }

                else if (ck.Key == ConsoleKey.RightArrow && coord[1] < 9)
                {
                    coord[1] = coord[1] + 1;
                }

                else if (ck.Key == ConsoleKey.LeftArrow && coord[1] > 0)
                {
                    coord[1] = coord[1] - 1;
                }
                else if (ck.Key == ConsoleKey.Escape)
                {
                    consolecur = new int[] { Console.CursorLeft, Console.CursorTop };

                    Console.CursorLeft = 0;
                    Console.CursorTop = 53;

                    menu();

                    Console.CursorLeft = consolecur[0];
                    Console.CursorTop = consolecur[1];
                }

                c++;

            } while (ck.Key != ConsoleKey.Enter);

            return coord;
        }

        /// <summary>
        /// Affichage du curseur de sélection sur la grille aux coordonnées <paramref name="coord"/>
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="offsetGrille">décalage entre la grille et le bord gauche de l'écran</param>
        /// <param name="deleteCursor">effacer le précedent curseur ?</param>
        public static void AfficherCurseur(int[] coord, int offsetGrille, bool deleteCursor)
        {
            //supprimer curseur
            if(deleteCursor)
            {
                Console.CursorLeft -= 2;
                Console.Write(" ");
                Console.CursorLeft -= 3;
                Console.Write(" ");
            }

            // afficher curseur
            Console.CursorTop = 4 + coord[0] * 2;
            Console.CursorLeft = offsetGrille+ 1 + coord[1] * 4;

            Console.Write("[");

            Console.CursorLeft += 1;

            Console.Write("]");
        }

        /// <summary>
        /// Affichage des grilles
        /// </summary>
        /// <param name="grilleJoueurBateaux"></param>
        /// <param name="grilleJoueurMissiles"></param>
        /// <param name="grilleOrdiBateaux"></param>
        /// <param name="grilleOrdiMissiles"></param>
        /// <param name="vainqueur">0 : personne, 1: ordinateur, 2:joueur</param>
        /// <param name="tabBateauxJoueur"></param>
        /// <param name="tabBateauxOrdi"></param>
        /// <param name="tour"></param>
        public static void AfficherGrilles(int[,] grilleJoueurBateaux, int[,] grilleJoueurMissiles
            , int[,] grilleOrdiBateaux, int[,] grilleOrdiMissiles, int vainqueur, int[] tabBateauxJoueur, int[] tabBateauxOrdi, int tour)
        {
            int statutOrdi = 0, statutJoueur = 0;
            Console.Clear();
            if (vainqueur == 2)
            {
                statutOrdi = -1;
                statutJoueur = 1;
            }
            if (vainqueur == 1)
            {
                statutOrdi = 1;
                statutJoueur = -1;
            }
            Affichage.AfficherGrille(grilleOrdiBateaux, grilleOrdiMissiles, false, statutOrdi, tabBateauxOrdi, tour);
            Console.WriteLine();

            Console.WriteLine();
            Affichage.AfficherGrille(grilleJoueurBateaux, grilleJoueurMissiles, true, statutJoueur, tabBateauxJoueur, tour);
            Console.WriteLine();

        }

        /// <summary>
        /// Création d'un menu interactif
        /// </summary>
        /// <param name="labels">contenu du menu</param>
        /// <param name="titre"></param>
        /// <param name="clear">effacer le contenu de la console avant d'afficher le menu</param>
        /// <returns></returns>
        public static int CreerMenu(string[] labels, string titre, bool clear = false)
        {

            if (clear)
            {
                Console.Clear();
            }


            int cursor = 0;
            ConsoleKeyInfo key;

            do
            {
                AfficherMenu(cursor, labels, titre);
                key = Console.ReadKey();

                if (key.Key == ConsoleKey.UpArrow && cursor > 0)
                {
                    cursor--;
                }

                else if (key.Key == ConsoleKey.DownArrow && cursor < labels.Length - 1)
                {
                    cursor++;
                }

                int cursorPosition = Console.CursorTop;

                EffacerLignes(cursorPosition - labels.Length - 1, cursorPosition);

            } while (key.Key != ConsoleKey.Enter);

            return cursor;
        }

        /// <summary>
        /// Afficher l'écran de victoire
        /// </summary>
        /// <param name="v">1: ordinateur, 2:joueur</param>
        /// <param name="grilleJoueurBateaux"></param>
        /// <param name="grilleJoueurMissiles"></param>
        /// <param name="grilleOrdiBateaux"></param>
        /// <param name="grilleOrdiMissiles"></param>
        /// <param name="tabToucheBateauxJoueur"></param>
        /// <param name="tabToucheBateauxOrdi"></param>
        /// <param name="tour"></param>
        public static void AfficherVictoire(int v, int[,] grilleJoueurBateaux, int[,] grilleJoueurMissiles
    , int[,] grilleOrdiBateaux, int[,] grilleOrdiMissiles, int[] tabToucheBateauxJoueur, int[] tabToucheBateauxOrdi, int tour)
        {
            InterfaceJoueur.AfficherGrilles(grilleJoueurBateaux, grilleJoueurMissiles, grilleOrdiBateaux, grilleOrdiMissiles, v, tabToucheBateauxJoueur, tabToucheBateauxOrdi, tour);
            string vainqueur = (v == 2) ? "Le joueur" : "L'ordinateur";
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                              " + vainqueur + " a remporté la partie !");
            Console.ReadKey();
        }

        /// <summary>
        /// Affichage du menu
        /// </summary>
        /// <param name="index"></param>
        /// <param name="labels"></param>
        /// <param name="titre"></param>
        public static void AfficherMenu(int index, string[] labels, string titre)
        {
            Console.WriteLine(titre);

            for(int i = 0; i<labels.Length; i++)
            {
                Console.WriteLine("{0} {1}", (i == index) ? "->" : "  ", labels[i]);
            }
        }

        /// <summary>
        /// Effacer la <paramref name="index"/>-ième ligne de la console
        /// </summary>
        /// <param name="index"></param>
        public static void EffacerLigne(int index)
        {
            Console.SetCursorPosition(0, index);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, index);
        }

        /// <summary>
        /// Effacer les lignes de stop à start
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        public static void EffacerLignes(int start, int stop)
        {
            for(int i=stop; i>=start; i--)
            {
                EffacerLigne(i);
            }
        }

        /// <summary>
        /// Demander à l'utilisateur le nom pour un nouveux fichier
        /// </summary>
        /// <returns>nom du fichier</returns>
        public static string DemanderNomNouveauFile()
        {
            Console.Clear();
            Console.WriteLine("Nom du nouveau fichier : ");
            string entre = Console.ReadLine();

            return entre;
        }

        /// <summary>
        /// Affichage d'un Disclaimer
        /// </summary>
        public static void AfficherDisclaimer()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-----------------------------------------DISCLAIMER ------------------------------------------------");
            Console.WriteLine(" 1) Police de la Console :");
            Console.WriteLine("    Nous avons utilisé des caractères unicode pour l'affichage.");
            Console.WriteLine("    Si vous ne voyez pas ce caractère : \u2566 ,");
            Console.WriteLine("    Veuillez changer la police en Lucida Console (ou autre police TrueType).");
            Console.WriteLine("    Clic droit sur la barre du haut -> Propriétés -> Police -> Lucida Console");
            Console.WriteLine("    Relancez l'application");
            Console.WriteLine();
            Console.WriteLine(" 2) Taille de la console");
            Console.WriteLine("    Si l'affichage est coupé à cause de la résolution de votre écran:");
            Console.WriteLine("    Veuillez diminuer la taille de la police.");
            Console.WriteLine("    Clic droit sur la barre du haut -> Propriétés -> Police -> Taille");
            Console.WriteLine();
            Console.WriteLine(" 3) Veuillez ne pas :");
            Console.WriteLine("    -Mettre la console en plein écran");
            Console.WriteLine("    -Changer la taille de la console");
            Console.WriteLine();
            Console.WriteLine("Appuyer sur une touche si tout fonctionne normalement.");
            Console.ReadKey();
        }
    }
}
/*
777...777.....7...7...............77777777777...77777.................7777.7777........7.. ..77777
77..777.777777777777777777.............7777777.........77777777777..777.777777777777777777 ...7777
7..77.777..777777777777777777777....77777777777.....777777...77777777777777..........77777 77..777
7..7.77.777......7777777777777777..7777777777777777777777777....7777777......777777....777 7.7..77
7...77777..........7777.777777777..777777777777777777777777777...........77777..77777...77 7.77...
*/
