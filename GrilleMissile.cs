using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleVeau
{
    public static class GrilleMissile
    {
        /// <summary>
        /// Tirer un missile aux coordonnées <paramref name="coord"/>
        /// </summary>
        /// <param name="grilleBateaux"></param>
        /// <param name="grilleMissile"></param>
        /// <param name="tabToucheBateaux"></param>
        /// <param name="coord"></param>
        /// <param name="tabTaillesBateaux"></param>
        /// <returns>résultat du tir: type du bateau touché, 0 si aucun bateau touché, -1 erreur on a déjà tiré ici</returns>
        public static int Tirer(int[,] grilleBateaux, int[,] grilleMissile
            , int[] tabToucheBateaux, int[] coord, int[] tabTaillesBateaux)
        {
            if (Grille.EtreVide(grilleMissile, coord))
            {
                //il n'y a pas de bateaux à coord
                if (Grille.EtreVide(grilleBateaux, coord))
                {
                    grilleMissile[coord[0], coord[1]] = 1;
                    return 0;
                }
                else
                {
                    grilleMissile[coord[0], coord[1]] = 2; //Bateau touché non coulé

                    //on décremente le nombre de case occupé par le bateau touché
                    tabToucheBateaux[grilleBateaux[coord[0], coord[1]] - 1] -= 1;

                    if (tabToucheBateaux[grilleBateaux[coord[0], coord[1]] - 1] == 0)
                        TransformerToucheEnCoule(grilleBateaux, grilleMissile, coord, tabTaillesBateaux);

                    return grilleBateaux[coord[0], coord[1]];
                }

            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Changer la valeur de la case aux coordonnées <paramref name="coord"/> sur la grille <paramref name="grilleMissile"/>
        /// en 3 si le bateau auquel appartient cette case coule
        /// </summary>
        /// <param name="grilleBateaux"></param>
        /// <param name="grilleMissile"></param>
        /// <param name="coord"></param>
        /// <param name="tabTaillesBateaux"></param>
        public static void TransformerToucheEnCoule(int[,] grilleBateaux, int[,] grilleMissile, int[] coord, int[] tabTaillesBateaux)
        {
            int x = coord[0], y = coord[1], i = 0, longueurBateau = tabTaillesBateaux[grilleBateaux[x, y] - 1], compteur = 0;

            //check à droite
            while (compteur <= longueurBateau && x + i < 10 && grilleBateaux[x + i, y] == grilleBateaux[x, y])
            {
                grilleMissile[x + i, y] = 3;
                compteur++;
                i++;
            }
            //check à gauche
            i = 1;
            while (compteur <= longueurBateau && x - i >= 0 && grilleBateaux[x - i, y] == grilleBateaux[x, y])
            {
                grilleMissile[x - i, y] = 3;
                compteur++;
                i++;
            }
            //check en haut
            i = 1;
            while (compteur <= longueurBateau && y - i >= 0 && grilleBateaux[x, y - i] == grilleBateaux[x, y])
            {
                grilleMissile[x, y - i] = 3;
                compteur++;
                i++;
            }
            //check en bas
            i = 1;
            while (compteur <= longueurBateau && y + i < 10 && grilleBateaux[x, y + i] == grilleBateaux[x, y])
            {
                grilleMissile[x, y + i] = 3;
                compteur++;
                i++;
            }
        }
    }
}
/*
7...77777.7777777........77777777..7777777777777777777777777777777777777777777..777777..77 7.77...
7...77.7777777..77....77777777....77777777777777777777777777777777777777777.....7777777..7 7.777..
7..7777.777777..77777777777....7777777777777777........7777777777777777.....777......77..7 7.777..
7....777..77....7777777777.....77777777777777777777..77777777777777......77777...7...7...7 7.77...
77..7..77777....7777777..77.....777777777.......777..7777777777.......77777777..777777..77 7777...
*/
