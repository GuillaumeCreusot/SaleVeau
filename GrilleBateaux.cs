using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleVeau
{
    public static class GrilleBateaux
    {
        
        /// <summary>
        /// Initialise l'emplacement des bateaux sur <paramref name="grille"/>
        /// </summary>
        /// <param name="grille"></param>
        /// <param name="tabTaillesBateaux"></param>
        /// <param name="rdn"></param>
        public static void InitialiserGrilleBateaux(int[,] grille, int[] tabTaillesBateaux, Random rdn)
        {
            //placer bateaux
            for (int i = 0; i < tabTaillesBateaux.Length; i++)
            {
                CreerBateau(grille, tabTaillesBateaux[i], i + 1, rdn);
            }
        }
      
        /// <summary>
        /// Place un bateau aléatoirement sur <paramref name="grille"/> de type <paramref name="typeBateau"/>
        /// </summary>
        /// <param name="grille"></param>
        /// <param name="tailleBateau"></param>
        /// <param name="typeBateau"></param>
        /// <param name="rdn"></param>
        public static void CreerBateau(int[,] grille, int tailleBateau, int typeBateau, Random rdn)
        {
            int[] coord;
            int direction;
            do
            {
                coord = Grille.ChoisirCaseVideAlea(grille, rdn);
                direction = ChoisirRandomDirectionBateau(tailleBateau, coord, grille, rdn);
            } while (direction == -1);
            

            int incrementX, incrementY;

            Grille.IncrementerversDir(direction, out incrementX, out incrementY);

            for (int i = 0; i < tailleBateau; i++)
            {
                grille[coord[0] + i * incrementX, coord[1] + i * incrementY] = typeBateau;
            }


        }

        /// <summary>
        /// Choisit une direction aléatoire où il n'y a pas d'obstacle à une distance inférieure ou égale à <paramref name="tailleBateau"/>
        /// </summary>
        /// <param name="tailleBateau"></param>
        /// <param name="coord"></param>
        /// <param name="grille"></param>
        /// <param name="rdn"></param>
        /// <returns></returns>
        public static int ChoisirRandomDirectionBateau(int tailleBateau, int[] coord, int[,] grille, Random rdn)
        {
            bool[] tabDir = ObtenirBonnesDirection(tailleBateau, coord, grille, rdn);

            //on stocke les bonnes directions dans un tableau
            int[] goodDirection = new int[] { -1, -1, -1, -1 };
            int nbGoodDirection = 0;

            for (int i = 0; i < 4; i++)
            {
                if (tabDir[i])
                {
                    goodDirection[nbGoodDirection] = i;
                    nbGoodDirection++;
                }

            }

            if (EtreTabVide(tabDir))
            {
                return -1;
            }
            //choix direction aleatoire
            return goodDirection[rdn.Next(0, nbGoodDirection)];

        }

        /// <summary>
        /// Cherche les directions où il n'y a pas d'obstacle à une distance inférieure ou égale à <paramref name="tailleBateau"/>
        /// </summary>
        /// <param name="tailleBateau"></param>
        /// <param name="coord"></param>
        /// <param name="grille"></param>
        /// <param name="rdn"></param>
        /// <returns>retourne un tableau où chaque case correspond à une direction (0:ouest, 1:nord, 2:est, 3: sud</returns>
        public static bool[] ObtenirBonnesDirection(int tailleBateau,
            int[] coord, int[,] grille, Random rdn)
        {
            //  0 : ouest, 1: nord, 2 : est, 3: sud, -1 pas la place

            //place suffissante
            //tableau contenant les directions possibles pour poser un bateau
            bool[] tabDir = new bool[4];

            //nord
            if (coord[1] > tailleBateau)
            {
                tabDir[0] = !AvoirBateauDansDirection(grille, coord, 0, tailleBateau);
            }
            //est
            if (coord[0] < grille.GetLength(0) - tailleBateau)
            {
                tabDir[1] = !AvoirBateauDansDirection(grille, coord, 1, tailleBateau );
            }
            //sud
            if (coord[1] < grille.GetLength(1) - tailleBateau)
            {
                tabDir[2] = !AvoirBateauDansDirection(grille, coord, 2, tailleBateau);
            }
            //ouest
            if (coord[0] > tailleBateau)
            {
                tabDir[3] = !AvoirBateauDansDirection(grille, coord, 3, tailleBateau);
            }
            return tabDir;
        }

        /// <summary>
        /// Regarde s'il y a un bateau dans la direction <paramref name="dir"/> à une distance inférieure ou égale à <paramref name="tailleBateau"/>
        /// </summary>
        /// <param name="grille"></param>
        /// <param name="coord"></param>
        /// <param name="dir"></param>
        /// <param name="tailleBateau"></param>
        /// <returns></returns>
        public static bool AvoirBateauDansDirection(int[,] grille, int[] coord, int dir, int tailleBateau)
        {
            int incrementX, incrementY;

            Grille.IncrementerversDir(dir, out incrementX, out incrementY);

            bool result = false;

            for (int i = 1; i <= tailleBateau; i++)
            {
                if (grille[coord[0] + i * incrementX, coord[1] + i * incrementY] != 0 )
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Compte le nombre de bateaux coulés
        /// </summary>
        /// <param name="tabBateauxTouche"></param>
        /// <returns></returns>
        public static int CompterNombreBateauxCoule(int[] tabBateauxTouche)
        {
            int result = 0;
            foreach(int val in tabBateauxTouche)
            {
                if (val == 0) result++;
            }

            return result;
        }

        /// <summary>
        /// Regarde si la table <paramref name="tab"/> est vide (toute les cases = false)
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public static bool EtreTabVide(bool[] tab)
        {
            bool result = true;

            foreach (bool val in tab)
            {
                if (val)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
/*
77...7777.77..7...777.77777777...77777777777777.7...777777........7..7777777....77777..777 .77...7
777...77777.........777777777777......777777777777777........777777..7777......777777777.. 77..777
7777...7777..7..7......77777777777...77777777777........7777777777...7........7777777.7777 7...777
77777..7777....77..77........77777777..............77..7777777777.......77...7777777777... ..77777
77777..7777....77..7777....................7777777777..777777........7777...777777777777.. .777777
*/
