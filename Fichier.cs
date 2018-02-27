using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SaleVeau
{
    public static class Fichier
    {

        /// <summary>
        /// Obtenir le chemain d'accés du directory Save
        /// </summary>
        /// <returns></returns>
        public static string ObtenirPathSave()
        {
            //emplacement de l'exe
            string path = Environment.CurrentDirectory;

            //retour en arrière
            path = Path.GetDirectoryName(path);
            path = Path.GetDirectoryName(path);

            //emplacement fichier sauvegarde
            path += "\\Save";

            return path;
        }

        /// <summary>
        /// Chargement des données relatives à la partie
        /// </summary>
        /// <param name="path"></param>
        /// <param name="grilles"></param>
        /// <param name="tabsInt"></param>
        /// <param name="coords"></param>
        /// <param name="goodDir"></param>
        /// <param name="diff"></param>
        /// <param name="tour"></param>
        public static void LireSavePartie(string path, int[][,] grilles, int[][] tabsInt
            , int[][] coords, bool[] goodDir, ref int diff, ref int tour)
        {
            //ouverture du StreamReader
            StreamReader sr = new StreamReader(path);

            string line;
            int c = 0;

            //grilles
            do
            {
                line = sr.ReadLine();

                for (int i = 0; i < grilles[c].GetLength(0); i++)
                {
                    for (int j = 0; j < grilles[c].GetLength(1); j++)
                    {
                        grilles[c][i, j] = Convert.ToInt32(line[i * 10 + j]) - 48;
                    }
                }

                c++;

            } while (c < grilles.GetLength(0));

            //tableaux int
            int i2 = 0;

            c = 0;
            do
            {
                line = sr.ReadLine();

                i2 = 0;
                for (int i = 0; i < tabsInt[c].GetLength(0); i++)
                {
                    if(line[i2] == '-')
                    {
                        tabsInt[c][i] = Convert.ToInt32(line[i2]+line[i2+1]-95);
                        i2++;
                    }
                    else
                    {
                        tabsInt[c][i] = Convert.ToInt32(line[i]) - 48;
                    }
                    i2++;
                }

                c++;

            } while (c < tabsInt.Length);

            //coords
            i2 = 0;
            line = sr.ReadLine();

            for(int i=0; i < coords.GetLength(0); i++)
            {
                coords[i][0] = line[i2]-48;
                coords[i][1] = line[i2 + 1]-48;

                i2 += 2;
            }

            //bool[]
            line = sr.ReadLine();

            for(int i =0; i<goodDir.Length; i++)
            {
                if(line[4*i] == 'F')
                {
                    goodDir[i] = false;
                }
                else
                {
                    goodDir[i] = true;
                }
                
            }

            //paramètres
            diff = Convert.ToInt32(sr.ReadLine());
            tour = Convert.ToInt32(sr.ReadLine());



            //fermeture
            sr.Close();
        }

        /// <summary>
        /// Sauvegarde des données relatives à la partie
        /// </summary>
        /// <param name="path"></param>
        /// <param name="grilles"></param>
        /// <param name="tabsInt"></param>
        /// <param name="coords"></param>
        /// <param name="goodDir"></param>
        /// <param name="diff"></param>
        /// <param name="tour"></param>
        public static void EcrireSavePartie(string path, int[][,] grilles, int[][] tabsInt, int[][] coords, bool[] goodDir
            , int diff, int tour)
        {
            //ouverture du StreamWriter
            StreamWriter sw = new StreamWriter(path);

            int c = 0;
            string line;

            //grilles
            do
            {
                line = "";
                for (int i = 0; i < grilles[c].GetLength(0); i++)
                {
                    for (int j = 0; j < grilles[c].GetLength(1); j++)
                    {
                        line += grilles[c][i, j];
                    }
                }

                sw.WriteLine(line);

                c++;

            } while (c < grilles.GetLength(0));

            c = 0;
            //tableaux
            do
            {
                line = "";

                for (int i = 0; i < tabsInt[c].GetLength(0); i++)
                {
                    line += tabsInt[c][i];
                }

                sw.WriteLine(line);

                c++;
            } while (c < tabsInt.Length);

            //coords
            line = "";
            for(int i = 0; i<coords.GetLength(0); i++)
            {
                line += "" + coords[i][0] + coords[i][1];
            }

            sw.WriteLine(line);

            //bool[]
            line = "";
            for(int i = 0; i<goodDir.Length; i++)
            {
                line += goodDir[i];
            }
            sw.WriteLine(line);

            //paramètre
            sw.WriteLine(diff);
            sw.WriteLine(tour);

            sw.Close();
        }

        /// <summary>
        /// Sauvegarde des paramètres
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveEachTurn"></param>
        public static void EcrireSaveParametre(string path, bool saveEachTurn)
        {
            //ouverture StreamWriter
            StreamWriter sw = new StreamWriter(path);

            //saveEachTurn
            sw.WriteLine(saveEachTurn);

            sw.Close();
        }

        /// <summary>
        /// chargement des paramètres
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveEachTurn"></param>
        public static void LireSaveParametre(string path, ref bool saveEachTurn)
        {
            //ouverture StreamReader
            StreamReader sr = new StreamReader(path);

            //saveEachTurn
            saveEachTurn = Convert.ToBoolean(sr.ReadLine());

            sr.Close();
        }

        /// <summary>
        /// Obtenir les noms des fichiers dans le directory situé à <paramref name="pathSave"/>
        /// </summary>
        /// <param name="pathSave"></param>
        /// <returns></returns>
        public static string[] ObtenirNomFichierdansSaveDirectory(string pathSave)
        {
            FileInfo[] files = ObtenirFichierDansSaveDirectory(pathSave);

            string[] result = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                result[i] = files[i].Name;
            }

            return result;
        }

        /// <summary>
        /// Obtenir le chemin d'accés à partir du nom du fichier <paramref name="name"/> et du chemin d'accés du dossier <paramref name="pathSave"/>
        /// </summary>
        /// <param name="pathSave"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ObtenirPathAvecNom(string pathSave, string name)
        {
            return pathSave + "\\" + name;

        }

        public static FileInfo[] ObtenirFichierDansSaveDirectory(string pathSave)
        {
            DirectoryInfo dir = new DirectoryInfo(pathSave);
            FileInfo[] files = dir.GetFiles();

            return files;
        }

        /// <summary>
        /// Vérifie si le dossier est vide
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool EtreDirectoryVide(string path)
        {
            return (Fichier.ObtenirFichierDansSaveDirectory(path).Length == 0);
        }

        /// <summary>
        /// Création d'un nouveau fichier
        /// s'il existe déjà un fichier portant le nom <paramref name="name"/> ajout d'un "-" à la fin du nom
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pathSave"></param>
        /// <returns></returns>
        public static string CreerNouveauFichier(string name, string pathSave)
        {
            string[] nameFiles = Fichier.ObtenirNomFichierdansSaveDirectory(pathSave);

            while (nameFiles.Contains(name + ".txt"))
            {
                name += "-";
            }

            name += ".txt";

            Console.WriteLine(name);

            FileStream f = File.Create(pathSave + "\\" + name);
            f.Close();

            return name;
        }
    }
}
/*
77777..77777..7...........................7..7777777777...77777....777777777777777...77777 7777777
77777..777777..7..77..777..77777...77777777..77777777777..777....777777777777777...7777777 7777777
77777..777777......7...777..77777..77777777..777777777777......7777777777777777...77777777 7777777
77777..7777777....777...77...7777..77777777..777777777......7777777.77777.777...7777777777 7777777
77777..777777777.........77..7777...7777777..77.........77777777.77777..777....77777777777 7777777
*/
