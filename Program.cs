using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SaleVeau
{
    class Program
    {
        static void Main(string[] args)
        {
            //init console
            Console.WindowHeight = (Console.LargestWindowHeight >= 60) ? 60 : Console.LargestWindowHeight;
            Console.WindowWidth = (Console.LargestWindowWidth >= 100) ? 100 : Console.LargestWindowWidth;

            InterfaceJoueur.AfficherDisclaimer();
            Demarrer();
        }

        /// <summary>
        /// Lancement du jeu
        /// </summary>
        public static void Demarrer()
        {
            //constante
            int[] tabTaillesBateaux = { 2, 3, 3, 4, 5 };

            // définition grilles
            int[,] grilleJoueurBateaux = new int[10, 10];
            int[,] grilleOrdiBateaux = new int[10, 10];
            int[,] grilleJoueurMissiles = new int[10, 10];
            int[,] grilleOrdiMissiles = new int[10, 10];


            //définition tableau
            int[] tabToucheBateauxJoueur = tabTaillesBateaux.ToArray();
            int[] tabToucheBateauxOrdi = tabTaillesBateaux.ToArray();

            //random instance
            Random rdn = new Random();

            //variable difficulté
            int diff = 0;

            //paramètres de l'application
            bool saveEachTurn = false;
            string nameSave = "quicksave";
            string path = Fichier.ObtenirPathSave();

            //chargement des paramètres à partir du fichier parametre
            path = Path.GetDirectoryName(path);
            Fichier.LireSaveParametre(path + "\\" + "parametre.txt", ref saveEachTurn);


            //init variable ordi
            int[][] preCoord = new int[5][] { new int[2], new int[2], new int[2], new int[2], new int[2] };
            int[] preDir = new int[] { -1, -1, -1, -1, -1 };
            bool[] goodDir = new bool[] { false, false, false, false, false };
            int[] typePrec = new int[] { -1, -1, -1, -1, -1 };

            //placement bateau aléatoire sur la grille de l'ordinateur
            InitialiserPreCoord(preCoord, grilleJoueurMissiles, rdn);

            //boucle principale
            int tour = 0;
            bool joueur;
            int tirsDispos;
            int[] coord;

            //menu principal
            DemarrerMenuPrincipal(ref grilleJoueurBateaux, grilleJoueurMissiles, ref grilleOrdiBateaux,
                grilleOrdiMissiles, tabTaillesBateaux, rdn, tabToucheBateauxJoueur, tabToucheBateauxOrdi
                , ref diff, ref saveEachTurn, ref preDir, ref typePrec, preCoord, goodDir, ref tour);

            do
            {

                //tour de qui?
                joueur = (tour % 2 == 0);

                //tour du joueur
                if (joueur)
                {
                    //sauvegarde auto
                    if (saveEachTurn)
                    {
                        Fichier.EcrireSavePartie(Fichier.ObtenirPathSave() + "\\" + nameSave,
                 new int[][,]{ grilleJoueurBateaux, grilleOrdiBateaux,
                        grilleJoueurMissiles, grilleOrdiMissiles },
                 new int[][] { tabToucheBateauxJoueur, tabToucheBateauxOrdi , preDir, typePrec}, preCoord, goodDir, diff, tour);
                    }

                    //affichage grilles
                    InterfaceJoueur.AfficherGrilles(grilleJoueurBateaux, grilleJoueurMissiles, grilleOrdiBateaux, grilleOrdiMissiles, 0, tabToucheBateauxJoueur, tabToucheBateauxOrdi, tour/2);

                    //nombre de tirs disponible
                    tirsDispos = 5 - GrilleBateaux.CompterNombreBateauxCoule(tabToucheBateauxJoueur);

                    //init coord des tirs
                    int[,] coordTirs = new int[tirsDispos, 2];

                    for (int i = 0; i < coordTirs.GetLength(0); i++)
                    {
                        coordTirs[i, 0] = -1;
                        coordTirs[i, 1] = -1;
                    }

                    //demander les coordonnées des tirs 
                    Action menu = () => DemarrerMenuContinue(grilleJoueurBateaux, grilleJoueurMissiles, grilleOrdiBateaux,
                        grilleOrdiMissiles, tabTaillesBateaux, rdn, tabToucheBateauxJoueur, tabToucheBateauxOrdi, diff,
                        preDir, typePrec, preCoord, goodDir, tour);

                    InterfaceJoueur.DemanderTirs(tirsDispos, 26, coordTirs
                        , menu);

                    //salve de tir
                    for (int i = 0; i < tirsDispos && tirsDispos < Grille.CompterNbCaseVide(grilleOrdiMissiles); i++)
                    {
                        coord = new int[2];
                        coord[0] = coordTirs[i, 0];
                        coord[1] = coordTirs[i, 1];
                        int result = GrilleMissile.Tirer(grilleOrdiBateaux, grilleOrdiMissiles, tabToucheBateauxOrdi, coord, tabTaillesBateaux);
                    }

                }


                //tour de l'ordi
                else
                {
                    //nombres de tirs dispo
                    tirsDispos = tabToucheBateauxOrdi.Length - GrilleBateaux.CompterNombreBateauxCoule(tabToucheBateauxOrdi);

                    //salve
                    for (int i = 0; i < tirsDispos && i < Grille.CompterNbCaseVide(grilleOrdiMissiles); i++)
                    {
                        //Facile
                        if (diff == 0)
                        {
                            Ordi_Facile.ObtenirProchainCoup(grilleJoueurBateaux, grilleJoueurMissiles,
                            tabToucheBateauxJoueur
                            , rdn, preCoord, preDir,
                            goodDir, typePrec, tabTaillesBateaux, i, tirsDispos);
                        }

                        //Difficile
                        else if (diff == 1)
                        {
                            Ordi_Difficile.ObtenirProchainCoup(grilleJoueurBateaux, grilleJoueurMissiles,
                            tabToucheBateauxJoueur
                            , rdn, preCoord, preDir,
                            goodDir, typePrec, tabTaillesBateaux, i, tirsDispos);
                        }

                    }
                }
                tour++;
            } while (Grille.VerifierVictoire(tabToucheBateauxJoueur, tabToucheBateauxOrdi) == 0);

            //Ecran de victoire
            InterfaceJoueur.AfficherVictoire(Grille.VerifierVictoire(tabToucheBateauxJoueur, tabToucheBateauxOrdi), grilleJoueurBateaux, grilleJoueurMissiles, grilleOrdiBateaux, grilleOrdiMissiles, tabToucheBateauxJoueur, tabToucheBateauxOrdi, tour/2);
        }


        /// <summary>
        /// Lancement du menu principal
        /// </summary>
        /// <param name="grilleJoueurBateaux"></param>
        /// <param name="grilleJoueurMissiles"></param>
        /// <param name="grilleOrdiBateaux"></param>
        /// <param name="grilleOrdiMissiles"></param>
        /// <param name="tabTaillesBateaux"></param>
        /// <param name="rdn"></param>
        /// <param name="tabToucheBateauxJoueur"></param>
        /// <param name="tabToucheBateauxOrdi"></param>
        /// <param name="diff"></param>
        /// <param name="saveEachTurn"></param>
        /// <param name="preDir"></param>
        /// <param name="typePrec"></param>
        /// <param name="preCoord"></param>
        /// <param name="goodDir"></param>
        /// <param name="tour"></param>
        public static void DemarrerMenuPrincipal(ref int[,] grilleJoueurBateaux, int[,] grilleJoueurMissiles
            , ref int[,] grilleOrdiBateaux, int[,] grilleOrdiMissiles, int[] tabTaillesBateaux
            , Random rdn, int[] tabToucheBateauxJoueur, int[] tabToucheBateauxOrdi, ref int diff, ref bool saveEachTurn,
            ref int[] preDir, ref int[] typePrec, int[][] preCoord, bool[] goodDir, ref int tour)
        {
            //menu principal
            string[] labels;

            //menu selection fichier
            string[] nameFiles;
            int selectFile;
            string pathSave = Fichier.ObtenirPathSave();

            //menu oui non
            string[] labelsOuiNon = { "Oui", "Non" };
            int selectOuiNon;

            //menu option
            string[] labelsOption;
            int selectOption;

            //menu difficulté
            string[] labelsDiff;

            //si on a des fichiers de sauvegarde existant
            if (Fichier.EtreDirectoryVide(pathSave))
            {
                labels = new string[] { "Nouvelle Partie", "Option", "Quitter" };
            }
            else
            {
                //ajout du labels charger partie
                labels = new string[] { "Nouvelle Partie", "Charger Partie", "Option", "Quitter" };
            }

            int selection = InterfaceJoueur.CreerMenu(labels, "--------- Menu Principal ------------", true);

            //Nouvelle Partie
            if (selection == 0)
            {
                //menu diff
                labelsDiff = new string[] { "Facile", "Difficile" };

                diff = InterfaceJoueur.CreerMenu(labelsDiff, "--------- Choix Difficulté ------------");

                //init grille bateaux
                GrilleBateaux.InitialiserGrilleBateaux(grilleOrdiBateaux, tabTaillesBateaux, rdn);
                do
                {
                    grilleJoueurBateaux = new int[10, 10];
                    GrilleBateaux.InitialiserGrilleBateaux(grilleJoueurBateaux, tabTaillesBateaux, rdn);
                    InterfaceJoueur.AfficherGrilles(grilleJoueurBateaux, grilleJoueurMissiles, grilleOrdiBateaux,
                        grilleOrdiMissiles, 0, tabToucheBateauxJoueur, tabToucheBateauxOrdi, 0);


                    selectOuiNon = InterfaceJoueur.CreerMenu(labelsOuiNon, "Voulez-vous charger une nouvelle grille?");
                }
                while (selectOuiNon == 0);

                return;
            }
            //Charger Partie
            else if (selection == 1 && !Fichier.EtreDirectoryVide(pathSave))
            {
                //obtenir les fichiers dans le directory //Save
                nameFiles = Fichier.ObtenirNomFichierdansSaveDirectory(pathSave);

                //menu selection fichier
                selectFile = InterfaceJoueur.CreerMenu(nameFiles, "--------- Menu Chargement ------------", true);

                //chargement de la sauvegarde
                Fichier.LireSavePartie(Fichier.ObtenirPathAvecNom(pathSave, nameFiles[selectFile]),
                     new int[][,]{ grilleJoueurBateaux, grilleOrdiBateaux,
                        grilleJoueurMissiles, grilleOrdiMissiles },
                     new int[][] { tabToucheBateauxJoueur, tabToucheBateauxOrdi , preDir, typePrec}, preCoord
                     , goodDir, ref diff, ref tour);

            }

            //Option
            else if (selection == 1 && Fichier.EtreDirectoryVide(pathSave) || selection == 2 && !Fichier.EtreDirectoryVide(pathSave))
            {
                while (true)
                {
                    //menu option
                    labelsOption = new string[] { String.Format("Sauvegarde à chaque tour : {0}", saveEachTurn ? "Oui" : "Non"), "Retour" };
                    selectOption = InterfaceJoueur.CreerMenu(labelsOption, "--------- Options ------------");

                    //sauvegarde auto
                    if (selectOption == 0)
                    {
                        selectOuiNon = InterfaceJoueur.CreerMenu(labelsOuiNon, "Voulez vous que le jeu sauvegarde automatiquement votre grille ?");
                        if (selectOuiNon == 0)
                        {
                            saveEachTurn = true;
                        }
                        else
                        {
                            saveEachTurn = false;
                        }

                        //sauvegarde paramètre
                        string path = Fichier.ObtenirPathSave();
                        path = Path.GetDirectoryName(path);
                        Fichier.EcrireSaveParametre(path + "\\" + "parametre.txt", saveEachTurn);
                    }
                    //retour
                    else
                    {
                        break;
                    }
                }

                //retour au menu principal
                DemarrerMenuPrincipal(ref grilleJoueurBateaux, grilleJoueurMissiles, ref grilleOrdiBateaux,
                grilleOrdiMissiles, tabTaillesBateaux, rdn, tabToucheBateauxJoueur, tabToucheBateauxOrdi, ref diff
                , ref saveEachTurn, ref preDir, ref typePrec, preCoord, goodDir, ref tour);

            }
            //Quitter
            else
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Lancement du menu continuer
        /// </summary>
        /// <param name="grilleJoueurBateaux"></param>
        /// <param name="grilleJoueurMissiles"></param>
        /// <param name="grilleOrdiBateaux"></param>
        /// <param name="grilleOrdiMissiles"></param>
        /// <param name="tabTaillesBateaux"></param>
        /// <param name="rdn"></param>
        /// <param name="tabToucheBateauxJoueur"></param>
        /// <param name="tabToucheBateauxOrdi"></param>
        /// <param name="diff"></param>
        /// <param name="preDir"></param>
        /// <param name="typePrec"></param>
        /// <param name="preCoord"></param>
        /// <param name="goodDir"></param>
        /// <param name="tour"></param>
        public static void DemarrerMenuContinue(int[,] grilleJoueurBateaux, int[,] grilleJoueurMissiles
            , int[,] grilleOrdiBateaux, int[,] grilleOrdiMissiles, int[] tabTaillesBateaux, Random rdn
            , int[] tabToucheBateauxJoueur, int[] tabToucheBateauxOrdi, int diff,
            int[] preDir, int[] typePrec, int[][] preCoord, bool[] goodDir, int tour)
        {
            //obtenir le path du directory //Save
            string pathSave = Fichier.ObtenirPathSave();

            //variable menu continue
            string[] labelsContinue = { "Continuer", "Sauvegarder", "Quitter" };
            int selectionContinue = 0;

            //variable menu sauvegarde
            string[] labelsFiles;
            int selectFile = 0;
            string name;
            string[] nameFiles;

            //Proposer de continuer
            selectionContinue = InterfaceJoueur.CreerMenu(labelsContinue, "--------- Menu Continuer ------------", false);

            //sauvegarder
            if (selectionContinue == 1)
            {
                nameFiles = Fichier.ObtenirNomFichierdansSaveDirectory(pathSave);
                labelsFiles = new string[nameFiles.Length + 2];
                nameFiles.CopyTo(labelsFiles, 0);
                labelsFiles[nameFiles.Length] = "Nouvelle sauvegarde";
                labelsFiles[nameFiles.Length + 1] = "Retour";

                selectFile = InterfaceJoueur.CreerMenu(labelsFiles, "--------- Menu Sauvegarde ------------", true);

                //sauvegarde dans un fichier existant
                if (selectFile < nameFiles.Length)
                {
                    Fichier.EcrireSavePartie(Fichier.ObtenirPathAvecNom(pathSave,
                        nameFiles[selectFile]),
                 new int[][,]{ grilleJoueurBateaux, grilleOrdiBateaux,
                        grilleJoueurMissiles, grilleOrdiMissiles },
                 new int[][] { tabToucheBateauxJoueur, tabToucheBateauxOrdi, preDir, typePrec },preCoord, goodDir,
                 diff, tour);
                }
                //sauvegarde dans un nouveau fichier
                else if (selectFile == nameFiles.Length)
                {
                    Console.WriteLine("--------- Nouveau Fichier ------------");

                    //demande ddu nom du fichier
                    name = InterfaceJoueur.DemanderNomNouveauFile();

                    //adaptation du nom du fichier
                    name = Fichier.CreerNouveauFichier(name, pathSave);


                    //sauvegarde
                    Fichier.EcrireSavePartie(pathSave + "\\" + name,
                 new int[][,]{ grilleJoueurBateaux, grilleOrdiBateaux,
                        grilleJoueurMissiles, grilleOrdiMissiles },
                 new int[][] { tabToucheBateauxJoueur, tabToucheBateauxOrdi, preDir, typePrec }, preCoord, goodDir,
                 diff, tour);
                }

                //retour au menu principal
                DemarrerMenuContinue(grilleJoueurBateaux, grilleJoueurMissiles, grilleOrdiBateaux,
                    grilleOrdiMissiles, tabTaillesBateaux, rdn, tabToucheBateauxJoueur, tabToucheBateauxOrdi, diff
                    ,preDir, typePrec, preCoord, goodDir, tour);

            }
            //Quitter
            else if (selectionContinue == 2)
            {
                Environment.Exit(0);
            }
            //Continuer
            else
            {
                return;
            }

            InterfaceJoueur.AfficherGrilles(grilleJoueurBateaux, grilleJoueurMissiles, grilleOrdiBateaux,
                        grilleOrdiMissiles, 0, tabToucheBateauxJoueur, tabToucheBateauxOrdi, 0);
        }

        /// <summary>
        /// Initialisation des coordonnées des tirs de l'ordinateur
        /// </summary>
        /// <param name="preCoord"></param>
        /// <param name="grilleJoueurMissiles"></param>
        /// <param name="rdn"></param>
        public static void InitialiserPreCoord(int[][] preCoord, int[,] grilleJoueurMissiles, Random rdn)
        {
            int[] coord;

            for (int i = 0; i < 5; i++)
            {
                do
                {
                    coord = Grille.ChoisirCaseVideAlea(grilleJoueurMissiles, rdn);
                } while (Grille.ContenirCoord(preCoord, coord));
                preCoord[i] = coord;
            }
        }
    }
}
/*
777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777 777777777
777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777 777777777
777777777777777777777777777777777............................................7777777777777 777777777
7777777777777777777777777................7777777777777777777777777777777777.....7777777777 777777777
7777777777777777777...........7777777777.....777777777777777..7777777777777777...777777777 777777777
*/