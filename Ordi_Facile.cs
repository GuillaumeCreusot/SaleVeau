using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleVeau
{
    class Ordi_Facile
    {
        /// <summary>
        /// Tire sur la case de coordonnées <paramref name="coords"/>[<paramref name="index"/>]
        /// et choisit aléatoirement les prochaines coordonnées
        /// </summary>
        /// <param name="grilleBateauxJoueur"></param>
        /// <param name="grilleMissileJoueur"></param>
        /// <param name="tabToucheBateauxJoueur"></param>
        /// <param name="rdn"></param>
        /// <param name="coords"></param>
        /// <param name="dirs"></param>
        /// <param name="goodDirs"></param>
        /// <param name="typePrecs"></param>
        /// <param name="tabtaillesBateaux"></param>
        /// <param name="index"></param>
        /// <param name="tirDispo"></param>
        public static void ObtenirProchainCoup(int[,] grilleBateauxJoueur
            , int[,] grilleMissileJoueur, int[] tabToucheBateauxJoueur
            , Random rdn, int[][] coords, int[] dirs, bool[] goodDirs, int[] typePrecs, int[] tabtaillesBateaux, int index, int tirDispo)
        {
            int result = GrilleMissile.Tirer(grilleBateauxJoueur, grilleMissileJoueur
                , tabToucheBateauxJoueur, coords[index], tabtaillesBateaux);


            if (Grille.CompterNbCaseVide(grilleMissileJoueur) > tirDispo)
            {
                int[] coordInter;
                do
                {
                    //on en cherche une
                    coordInter = Grille.ChoisirCaseVideAlea(grilleMissileJoueur, rdn);
                } while (Grille.ContenirCoord(coords, coordInter));
                coords[index] = coordInter;
            }

        }
    }
}
/*
77777777777777777....77777777777777777777777777777777777777777777..7777777777777...7777777 777777777
77777777777777....7777.77......777777777777...77777777777777..777777.777777777777...777777 777777777
777777777777....7777777777777777777777777777777777777777777777777..7777.7777777777...77777 777777777
77777777777...777777777777.....77777777777777777777.7777777777...777.7777.777777777...7777 777777777
7777777777..77777777777.7777777777777777777777777.7777777777777777..777.7777.7777777...777 777777777
*/
