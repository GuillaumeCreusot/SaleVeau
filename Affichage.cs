using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleVeau
{
    class Affichage
    {   
        
        
        /// <summary>
        /// Affiche la grille du joueur ou de l'ordi
        /// </summary>
        /// <param name="grilleBateau">grille contenant les bateaux</param>
        /// <param name="grilleMissile"> grille contenant les missiles</param>
        /// <param name="isJoueur">booléen vrai si on affiche la grille du joueur, faux si ordi</param>
        /// <param name="aGagné">-1 si le joueur associé à la grille a perdu, 1 s'il a gagné, 0 si personne n'a encore gagné</param>
        /// <param name="tabBateauxTouches">tableau contenant les restes des bateaux du propriétaire de la grille</param>   
        public static void AfficherGrille(int[,] grilleBateau, int[,] grilleMissile, bool isJoueur, int aGagné, int[] tabBateauxTouches , int tour)
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (aGagné == -1)
                Console.ForegroundColor = ConsoleColor.DarkRed;
            if (aGagné == 1)
                Console.ForegroundColor = ConsoleColor.Green;

            if (!isJoueur)
                Console.WriteLine("-----------------------------------------Grille Ordi------------------------------------------------");
            else
                Console.WriteLine("-----------------------------------------Grille Joueur----------------------------------------------");

            int[] tabBateaux = { 2, 3, 3, 4, 5 };
            string Bateaux = "\u2560\u2550\u2550\u2550\u2563";


            int tailleY = grilleBateau.GetLength(0), tailleX = grilleBateau.GetLength(1);
            char x = ' ';
            Console.WriteLine("");
            for (int i = 0; i < 2 * tailleY + 1; i++)
            {

                //Affichage HUD Gauche (Bonne chance pour comprendre même moi j'y arrive plus)

                if (i == 0)
                    Console.Write("    {0} bateaux restants    ", 5 - GrilleBateaux.CompterNombreBateauxCoule(tabBateauxTouches));//Affichage bateaux restants
                else if (isJoueur)
                {
                    //Affichage statut des bateaux Joueur
                    bool afficheHUDBateau = false;
                    for (int j = 0; j < 5; j++)
                    {
                        if (i == 4 * j + 3)
                        {
                            Console.Write("          ");
                            for (int k = 0; k < 5; k++)
                            {
                                if (k < tabBateaux[j])
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    if (tabBateauxTouches[j] - k <= 0)
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                    if (tabBateauxTouches[j] == 0)
                                        Console.ForegroundColor = ConsoleColor.DarkRed;

                                    if (k == tabBateaux[j] - 1)
                                        Console.Write(Bateaux[4]);
                                    else
                                        Console.Write(Bateaux[k]);
                                }
                                else
                                    Console.Write(" ");
                            }
                            Console.Write("           ");
                            afficheHUDBateau = true;
                        }
                    }
                    if (!afficheHUDBateau)
                        Console.Write("                          ");
                }
                else
                {
                    //Affichage statut des bateaux Ordi
                    bool afficheHUDBateau = false;
                    for (int j = 0; j < 5; j++)
                    {
                        if (i == 4 * j + 3)
                        {
                            Console.Write("          ");
                            for (int k = 0; k < 5; k++)
                            {
                                if (k < tabBateaux[j] && tabBateauxTouches[j] == 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkRed;

                                    if (k == tabBateaux[j] - 1)
                                        Console.Write(Bateaux[4]);
                                    else
                                        Console.Write(Bateaux[k]);
                                }
                                else
                                    Console.Write(" ");
                            }
                            Console.Write("           ");
                            afficheHUDBateau = true;
                        }
                    }
                    if (!afficheHUDBateau)
                        Console.Write("                          ");
                }

                //Affichage de l'intérieur de la grille
                for (int j = 0; j < 4 * tailleX + 1; j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    if (aGagné == -1)
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    if (aGagné == 1)
                        Console.ForegroundColor = ConsoleColor.Green;

                    if (i % 2 == 0)
                    {
                        if (j % 4 == 0)
                            x = '\u253C';//Affichage de la grille
                        else
                            x = '\u2500';//Affichage de la grille
                    }
                    else
                    {
                        if (j % 4 == 0)
                            x = '\u2502';//Affichage de la grille
                        else
                        {
                            if (j % 2 == 0)
                                x = AfficherCaractere((i - 1) / 2, (j - 1) / 4, isJoueur, grilleBateau, grilleMissile); //Affiche l'intérieur des cases
                            else
                                x = ' ';
                        }
                    }
                    Console.Write(x);
                    Console.ForegroundColor = ConsoleColor.White;

                }

                //Affichage des lignes
                if (i % 2 == 1)
                    Console.Write("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[(i - 1) / 2]);


                //Affichage légende côté ordi

                Console.ForegroundColor = ConsoleColor.White;

                if (i == 0 && !isJoueur)
                    Console.Write("         TOUR :  {0}", tour + 1);
                if (i == 7 && !isJoueur)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("      O");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("      :  Bateau touché");
                }
                if (i == 12 && !isJoueur)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("    \u2560 \u2550 \u2550 \u2563");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("   :  Bateau coulé");
                }
                if (i == 17 && !isJoueur)
                    Console.Write("      \u00b7      :  Case vide");



                //Affichage légende côté joueur
                if (i == 11 && isJoueur)
                {
                    Console.Write("   \u2190 \u2191 \u2192 \u2193");
                    Console.Write("   :  Bouger curseur");
                }

                if (i == 1 && isJoueur)
                    Console.Write("      \u2566");
                if (i == 3 && isJoueur)
                    Console.Write("      \u2551");
                if (i == 5 && isJoueur)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("      \u2551");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("      :  Bateau touché");
                }
                if (i == 7 && isJoueur)
                    Console.Write("      \u2569");

                if (i == 15 && isJoueur)
                    Console.Write("    Enter    :  Selection");
                if (i == 19 && isJoueur)
                    Console.Write("    Echap    :  Menu");
                Console.Write('\n');


            }

            //Affichage des colonnes
            string str = "                          ";
            for (int i = 0; i < tailleX; i++)
                str += "  " + Convert.ToString(i + 1) + " ";
            Console.WriteLine(str);
        }
/*
77777..77777777777777...............................777777777..7777..7777.... 777777777777777
7777...77777777.777777777777777777777777777777777777777777.77777..7777.....77 77777777777777
7777..7777777777.777777777777777777777777777777777777..777777.77777.....77777 77777777777777
7777..777777777777.777777777777777777777777777777..777777..77777.....77777777 77777777777777
77..77777..77777777...77777777777..........777777..7777777......7777777 777777777777
*/

        /// <summary>
        /// Renvoie en fonction des coordonées de l'emplacement le caractère associé (Bateau/Vide, Rien/Touché/Coulé)
        /// </summary>
        /// <param name="coordX">coordonnée en x du caractère à afficher</param>
        /// <param name="coordY">coordonnée en y du caractère à afficher</param>
        /// <param name="isJoueur">-1 si le joueur associé à la grille a perdu, 1 s'il a gagné, 0 si personne n'a encore gagné</param>
        /// <param name="grilleBateau">grille contenant les bateaux</param>
        /// <param name="grilleMissile"></param>
        /// <returns>Caractère à afficher</returns>
        public static char AfficherCaractere(int coordX, int coordY, bool isJoueur, int[,] grilleBateau, int[,] grilleMissile)
        {
            
            int valeurBateau = grilleBateau[coordX, coordY], valeurMissile = grilleMissile[coordX, coordY];

            if (valeurBateau < 0 || valeurBateau > 5) return 'V'; //Erreur
            if (valeurMissile < 0 || valeurMissile > 3) return 'W'; //Erreur

            //Grille Joueur, bateaux affichés
            if (isJoueur) 
            {
                if (valeurBateau == 0)
                {
                    if (valeurMissile == 0)
                        return ' ';
                    else
                        return '\u00b7';
                }
                else
                {
                    if (valeurMissile == 2)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    if (valeurMissile == 3)
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    return AfficherCaractereBateau(grilleBateau, grilleMissile, coordX, coordY);

                }

            }

            //Grille adverse, les bateaux ne sont pas affichés (sauf coulés)
            else
            {
                if (valeurBateau == 0)
                {
                    if (valeurMissile == 0)
                        return ' ';
                    else
                        return '\u00b7';
                }
                else
                {
                    if (valeurMissile == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        return 'O';
                    }
                    if (valeurMissile == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        return AfficherCaractereBateau(grilleBateau, grilleMissile, coordX, coordY);
                    }

                }
            }
            return ' ';
        }


        /// <summary>
        /// Si le caractère à afficher est un bateau, renvoie le caractère précis (orientation, bout du bateau ou non)
        /// </summary>
        /// <param name="grilleBateau">grille contenant les bateaux</param>
        /// <param name="grilleMissile">grille contenant les missiles</param>
        /// <param name="coordX">coordonnée en x du caractère à afficher</param>
        /// <param name="coordY">coordonnée en y du caractère à afficher</param>
        /// <returns>Caractère de bateau à afficher</returns>
        public static char AfficherCaractereBateau(int[,] grilleBateau, int[,] grilleMissile, int coordX, int coordY)
        {
            //Check orientation et si intérieur de bateau
            if (coordX > 0 && coordX < 9 && grilleBateau[coordX, coordY] == grilleBateau[coordX + 1, coordY] && grilleBateau[coordX, coordY] == grilleBateau[coordX - 1, coordY])
                return '\u2551';
            if (coordY > 0 && coordY < 9 && grilleBateau[coordX, coordY] == grilleBateau[coordX, coordY + 1] && grilleBateau[coordX, coordY] == grilleBateau[coordX, coordY - 1])
                return '\u2550';

            //Check orientation et si bout de bateau
            if (coordX > 0 && grilleBateau[coordX, coordY] == grilleBateau[coordX - 1, coordY])
                return '\u2569';
            if (coordX < 9 && grilleBateau[coordX, coordY] == grilleBateau[coordX + 1, coordY])
                return '\u2566';
            if (coordY > 0 && grilleBateau[coordX, coordY] == grilleBateau[coordX, coordY - 1])
                return '\u2563';
            if (coordY < 9 && grilleBateau[coordX, coordY] == grilleBateau[coordX, coordY + 1])
                return '\u2560';

            return '\u256C';
        }
    }
}
/*
7..777777777.7777777777777777777777777...777777777777.....777777 77777777
7...7777777777...............777777777777777777777.....777777 7777
7...777777777777777777777777777777777777777777.....777777 77
77...77777777777777777777777777777777777..7.....7777777 
777....7777777777777777777777777777..........77777777 
7777.....7777777777777777777.........77777777777777 
7777777........................77777777777777777777
*/
