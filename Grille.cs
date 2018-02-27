using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleVeau
{
    static class Grille
    {
        /// <summary>
        /// Donne l'incrément necessaire pour se déplacer horizontalement(Y)(<paramref name="incrementY"/> )
        /// et verticalement(X)(<paramref name="incrementX"/>) selon la direction <paramref name="direction"/>
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="incrementX"></param>
        /// <param name="incrementY"></param>
        public static void IncrementerversDir(int direction, out int incrementX, out int incrementY)
        {
            incrementX = 0;
            incrementY = 0;

            switch (direction)
            {
                //nord
                case 0:
                    incrementY = -1;
                    break;
                //est
                case 1:
                    incrementX = 1;
                    break;
                //sud
                case 2:
                    incrementY = 1;
                    break;
                //ouest
                case 3:
                    incrementX = -1;
                    break;
                default:
                    break;

            }
        }
        
        /// <summary>
        /// Regarde si la case aux coordonnées <paramref name="coord"/> est vide (case = 0)
        /// </summary>
        /// <param name="grille"></param>
        /// <param name="coord"></param>
        /// <returns></returns>
        public static bool EtreVide(int[,] grille, int[] coord)
        {


            return grille[coord[0], coord[1]] == 0;
        }

        /// <summary>
        /// Choisit une case vide aléatoire dans la grille
        /// </summary>
        /// <param name="grille"></param>
        /// <param name="rdn"></param>
        /// <returns></returns>
        public static int[] ChoisirCaseVideAlea(int[,] grille, Random rdn)
        {
            int[] coord = new int[2];


            ///listage des coordonnées des cases vides
            int[][] casesVide = new int[CompterNbCaseVide(grille)][];

            int c = 0;

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    if (EtreVide(grille, new int[] { i, j }))
                    {
                        casesVide[c] = new int[] { i, j };
                        c++;

                    }
                }
            }



            //choix d'une case vide aléatoirement
            int[] choix = casesVide[rdn.Next(0, CompterNbCaseVide(grille))];

            return choix;
        }

        /// <summary>
        /// Vérifie s'il y a victoire
        /// </summary>
        /// <param name="tabToucheJoueur"></param>
        /// <param name="tabToucheOrdi"></param>
        /// <returns>0:personne, 1:ordi, 2:joueur</returns>
        public static int VerifierVictoire(int[] tabToucheJoueur, int[] tabToucheOrdi)
        {
            if (EtreTabVide(tabToucheJoueur))
            {
                return 1;
            }

            else if (EtreTabVide(tabToucheOrdi))
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Vérifie si le tableau <paramref name="tab"/> est vide (tt cases = 0)
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public static bool EtreTabVide(int[] tab)
        {
            bool result = true;

            foreach (int val in tab)
            {
                if (val != 0)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Vérifie si le tableau <paramref name="tab"/> contient la coordonnée <paramref name="coord"/>
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="coord"></param>
        /// <returns></returns>
        public static bool ContenirCoord(int[][] tab, int[] coord)
        {
            bool result = false;

            foreach (int[] c in tab)
            {
                if (c[0] == coord[0] && c[1] == coord[1])
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Compte le nombre de cases vides dans <paramref name="grille"/>
        /// </summary>
        /// <param name="grille"></param>
        /// <returns></returns>
        public static int CompterNbCaseVide(int[,] grille)
        {
            int result = 0;

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(0); j++)
                {
                    if (grille[i, j] == 0)
                    {
                        result += 1;
                    }
                }
            }

            return result;
        }
    }
}
/*
77777..7777....77..777...7777777..7777777..7777777777...7.........7..777...77777777777...7 7777777
77777..7777........777..77777777..7777777..777777777...........7777..77...77777777777...77 7777777
77777..7777.................................................7777777......777777777777..777 7777777
77777..7777...........................................7..77777777777...7777777777777...777 7777777
77777..77777....................................7777777..777777777...77777777777777...7777 7777777
*/
