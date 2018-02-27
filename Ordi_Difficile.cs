using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleVeau
{
    public static class Ordi_Difficile
    {

        /// <summary>
        /// Tire sur la case de coordonnées <paramref name="coords"/>[<paramref name="index"/>]
        /// et choisit "intelligement" les prochaines coordonnées
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
            , Random rdn, int[][] coords, int[] dirs, bool[] goodDirs, int[] typePrecs
            , int[] tabtaillesBateaux, int index, int tirDispo)
        {
            int result = GrilleMissile.Tirer(grilleBateauxJoueur, grilleMissileJoueur
                , tabToucheBateauxJoueur, coords[index], tabtaillesBateaux);

            if (Grille.CompterNbCaseVide(grilleMissileJoueur) > tirDispo)
            {
                //choix de la prochaine coords[index]
                int incx, incy;

                int[] coordInter;

                bool test;

                //si on ne touche rien ou que l'on touche un autre navire que le navire recherché
                if (result == 0 || (result != typePrecs[index] && typePrecs[index] != -1))
                {

                    //si l'on n'a pas encore trouvé de cible
                    if (typePrecs[index] == -1)
                    {
                        do
                        {
                            //on en cherche une
                            coordInter = Grille.ChoisirCaseVideAlea(grilleMissileJoueur, rdn);
                        } while (Grille.ContenirCoord(coords, coordInter));
                        coords[index] = coordInter;


                    }
                    else if (tabToucheBateauxJoueur[typePrecs[index] - 1] != 0)
                    {
                        //on retourne dans la direction opposée
                        RetournerArr(coords[index], ref dirs[index], grilleMissileJoueur, grilleBateauxJoueur, typePrecs[index]);

                        //si on est pas sur le bon axe
                        if (!goodDirs[index])
                        {
                            //on choisis une direction aléatoire
                            dirs[index] = GrilleBateaux.ChoisirRandomDirectionBateau(1, coords[index],
                                grilleMissileJoueur, rdn);
                        }
                        Grille.IncrementerversDir(dirs[index], out incx, out incy);

                        // on avance d'une case dans la direction dirs[index]
                        coords[index] = new int[] { coords[index][0] + incx, coords[index][1] + incy };


                    }
                    else
                    {
                        dirs[index] = -1;
                        typePrecs[index] = -1;
                        goodDirs[index] = false;
                        do
                        {
                            //on en cherche une
                            coordInter = Grille.ChoisirCaseVideAlea(grilleMissileJoueur, rdn);
                        } while (Grille.ContenirCoord(coords, coordInter));
                        coords[index] = coordInter;
                    }
                }

                else
                {
                    //debug
                    if (result == -1)
                    {
                        Console.WriteLine("erreur");
                    }

                    if (typePrecs[index] == -1)
                    {
                        typePrecs[index] = result;
                    }


                    //si la cible n'est pas coulée

                    if (tabToucheBateauxJoueur[typePrecs[index] - 1] != 0)
                    {
                        //si on suit une dirs[index]ection
                        if (dirs[index] != -1 && result == typePrecs[index])
                        {
                            goodDirs[index] = true;
                        }

                        do
                        {
                            //si on n est pas dans la bonne direction
                            if (!goodDirs[index])
                            {
                                dirs[index] = GrilleBateaux.ChoisirRandomDirectionBateau(1, coords[index],
                                    grilleMissileJoueur, rdn);

                            }

                            Grille.IncrementerversDir(dirs[index], out incx, out incy);
                            coords[index] = new int[] { coords[index][0] + incx, coords[index][1] + incy };
                            if (coords[index][0] >= 0 && coords[index][0] < 10
                                && coords[index][1] >= 0 && coords[index][1] < 10)
                            {
                                //si la case a déjà été touchée et la case n'est pas déjà ciblée
                                test = grilleMissileJoueur[coords[index][0], coords[index][1]] != 0 && !coords.Contains(coords[index]);
                                if (test)
                                {
                                    //on retourne dans la direction opposée
                                    RetournerArr(coords[index], ref dirs[index], grilleMissileJoueur, grilleBateauxJoueur, typePrecs[index]);
                                }
                            }
                            else
                            {
                                test = true;
                            }


                        } while (test);
                    }

                    else
                    {
                        dirs[index] = -1;
                        typePrecs[index] = -1;
                        goodDirs[index] = false;
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

        /// <summary>
        /// Retourne en arrière depuis la coordonnée <paramref name="coord"/> dans la direction inverse à <paramref name="dir"/>
        /// jusqu'à arriver à une case de type <paramref name="typePrec"/> située avant une case d'un type différent
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="dir"></param>
        /// <param name="grilleMissileJoueur"></param>
        /// <param name="grilleBateauxJoueur"></param>
        /// <param name="typePrec"></param>
        public static void RetournerArr(int[] coord, ref int dir, int[,] grilleMissileJoueur,
            int[,] grilleBateauxJoueur, int typePrec)
        {
            int incx, incy;

            dir = (dir + 2) % 4;


            Grille.IncrementerversDir(dir, out incx, out incy);

            int c = 1;

            while (coord[0] + c * incx >= 0
                && coord[0] + c * incx < grilleMissileJoueur.GetLength(0)
                && coord[1] + c * incy >= 0
                && coord[1] + c * incy < grilleMissileJoueur.GetLength(1)
                && !(grilleMissileJoueur[coord[0] + c * incx,
                coord[1] + c * incy] == 0)
                && grilleBateauxJoueur[coord[0] + c * incx,
            coord[1] + c * incy] == typePrec
               )
            {
                c++;
            }

            coord[0] = coord[0] + (c - 1) * incx;
            coord[1] = coord[1] + (c - 1) * incy;
        }
    }
}
/*
7777777777..777777777.777777777777.7777777777777777777777777777777777.77.7777.7777777..777 777777777
7777777777..77777777.77777777777777777777777777777777777777777777777777.77.77777777777..77 777777777
777777777..777777777777777777777777777777777777777777..............7777777777777777777...7 777777777
77777777...777777777777........7777777777777777777.....777............77777777777777777... 777777777
7777....77777777777..............7777777777777....777777........77...777777777777777777... 7777777
*/
