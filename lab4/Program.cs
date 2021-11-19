using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace lab4
{
    class graf
    {
        public int n = 300; // kilkist vershin (300)
        public int m_min = 2; //stepin vershini
        public int m_max = 30;
        public int[,] mas; // matrisa sumijnosti
        public int p = 10; // rozmir populatsii (10)
        public int p1; // pershiy predok
        public int p2; // drugiy predok
        public List<int> stepin_uzlov;
        public List <int[]> spisok_hromosom;
        public int[] kilkist_vershin_v_hromosomah;
        public List<int[]> shilds; // spisok nashadkiv
        int[] bestshild;
        int rekord;
        int[,] mas_test ={{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,},
                          {1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0,},
                          {0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0,},
                          {0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0,},
                          {0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0,},
                          {0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0,},
                          {0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1,},
                          {0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1,},
                          {0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1,},
                          {0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1,},
                          {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0,}};

        public graf()
        {
            mas = new int[n, n];
            stepin_uzlov = new List<int>();
            for (int i = 0; i < n; i++) stepin_uzlov.Add(0);
            //gen_mas();
            load();
            //mas = mas_test;
            spisok_hromosom = new List<int[]>();
            for (int i=0; i<p; i++)
            {
                int[] mas = new int[n];
                spisok_hromosom.Add(mas);
            }
                
                //new int[p, n];
            kilkist_vershin_v_hromosomah = new int[p];
            gen_spisok_hromosom();
            shilds = new List<int[]>();
            rekord = 1;
        }
        public void gen_spisok_hromosom()
        {
            Random ran = new Random();
            for (int i=0; i<p; i++)
            {
                for (int j = 0; j < n; j++) spisok_hromosom[i][j] = 0;
                spisok_hromosom[i][ran.Next(n)] = 1;
                kilkist_vershin_v_hromosomah[i] = 1;
            }
        }
        public void gen_mas()
        {
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    mas[i, j] = 0;
                }
                int s1 = 0;
                int s2 = 0;
                for (int t = 0; t < i; t++)
                    if (mas[t, i] == 1) s1++;
                for (int t = i + 1; t < n; t++) if (stepin_uzlov[t]>= m_max) s2++;
                int k_sv = m_max - s1;
                if (k_sv > n - i - 1 - s2) k_sv = n - i - 1 - s2;
                if (k_sv >= m_min) k_sv = rnd.Next(m_min, k_sv);
                for (int k = 0; k < k_sv; k++)
                {
                    int r = rnd.Next(i + 1, n);
                    if (mas[i, r] == 0 && stepin_uzlov[i] < m_max && stepin_uzlov[r] < m_max)
                    {
                        mas[i, r] = 1;
                        stepin_uzlov[i]++;
                        stepin_uzlov[r]++;
                    }
                    else k--;
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    mas[j, i] = mas[i, j];
                }
            }
            for (int i = 0; i < n; i++) // rahuemo stepeni uzliv
            {
                int s = 0;
                for (int j = 0; j < n; j++)
                {
                    if (mas[i, j] == 1) s++;
                }
                stepin_uzlov[i] = s;
            }
        }
        public void vivod()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{ mas[i, j]}");
                }
                Console.WriteLine();
            }
        }
        public void vivod_hromosom()
        {
            Console.WriteLine("spisok hromosom:");
            for (int i = 0; i < p; i++)
            {
                Console.Write($"hromosoma {i} mae {kilkist_vershin_v_hromosomah[i]} vershin: ");
                for (int j = 0; j < n; j++) Console.Write( spisok_hromosom[i][j] + " ");
                Console.WriteLine("\n");
            }
        }
        public void vibir_batkiv_turnir(int k)
        {
            Random rnd = new Random();
            List<int> vibrani = new List<int>(); //turnirniy spisok
            int t;
            for (int i=0; i<k; i++) // zapovniaem k kandidatami
            {
                t = rnd.Next(p);
                if (vibrani.Contains(t)) i--;
                else vibrani.Add(t);
            }

            //Console.WriteLine("spisok turniru ");
            //for (int i = 0; i < k; i++) Console.Write(vibrani[i] + " ");
            //Console.WriteLine();

            int max1 = 0;
            for(int i=0; i<k; i++) 
            {
                if (max1 < kilkist_vershin_v_hromosomah[vibrani[i]]) max1 = vibrani[i];
            }
            int max2 = 0;
            for (int i = 0; i < k; i++)
            {
                if (max2 < kilkist_vershin_v_hromosomah[vibrani[i]] && vibrani[i]!=max1) max2 = vibrani[i];
            }
            p1 = max1;
            p2 = max2;
        }
        public int f(int[] hromosoma)
        {
            int rez = 0;
            for (int i = 0; i < hromosoma.Length; i++)
                if (hromosoma[i] == 1) rez++;
            return rez;
        }
        public bool chi_e_klika(int[] hromosoma)
        {
            bool rez = true;
            for (int i=0; i < n; i++)
            {
                for (int j=0; j<n; j++)
                {
                    if ((i != j) && (hromosoma[i] == 1) && hromosoma[j] == 1 && mas[i, j] == 0) 
                    {
                        rez = false;
                        return rez;
                    } 
                }
            }
            return rez;
        }
        public void skreshuvania1()
        {
            int[] shild1 = new int[n];
            int[] shild2 = new int[n];
            int t = n / 2;
            for (int i = 0; i < n; i++)
            {
                if (i < t)
                {
                    shild1[i] = spisok_hromosom[p1][i];
                    shild2[i] = spisok_hromosom[p2][i];
                }
                else
                {
                    shild1[i] = spisok_hromosom[p2][i];
                    shild2[i] = spisok_hromosom[p1][i];
                }
            }
            if (chi_e_klika(shild1)) shilds.Add(shild1);
            if (chi_e_klika(shild2)) shilds.Add(shild2);
            shilds.Sort((x, y) => f(y) - f(x));
            if(shilds.Count!=0) bestshild = shilds[0];
            shilds.Clear();
        }
        public void skreshuvania2()
        {
            //int[] shild1 = new int[n];
            int[] shild2 = new int[n];
            int[] shild3 = new int[n];
            int[] shild4 = new int[n];
            int[] shild5 = new int[n];
            int[] shild6 = new int[n];
            int[] shild7 = new int[n];
            //int[] shild8 = new int[n];
            int t1 = n / 3;
            int t2 = 2 * t1;
            for (int i = 0; i < n; i++)
            {
                if (i < t1)
                {
                    //shild1[i] = spisok_hromosom[p1, i];
                    shild2[i] = spisok_hromosom[p1][i];
                    shild3[i] = spisok_hromosom[p1][i];
                    shild4[i] = spisok_hromosom[p1][i];
                    shild5[i] = spisok_hromosom[p2][i];
                    shild6[i] = spisok_hromosom[p2][i];
                    shild7[i] = spisok_hromosom[p2][i];
                    //shild8[i] = spisok_hromosom[p2, i];
                }
                else if (i<t2)
                {
                    //shild1[i] = spisok_hromosom[p1, i];
                    shild2[i] = spisok_hromosom[p1][i];
                    shild3[i] = spisok_hromosom[p2][i];
                    shild4[i] = spisok_hromosom[p2][i];
                    shild5[i] = spisok_hromosom[p1][i];
                    shild6[i] = spisok_hromosom[p1][i];
                    shild7[i] = spisok_hromosom[p2][i];
                    //shild8[i] = spisok_hromosom[p2, i];
                }
                else 
                {
                    //shild1[i] = spisok_hromosom[p1, i];
                    shild2[i] = spisok_hromosom[p2][i];
                    shild3[i] = spisok_hromosom[p1][i];
                    shild4[i] = spisok_hromosom[p2][i];
                    shild5[i] = spisok_hromosom[p1][i];
                    shild6[i] = spisok_hromosom[p2][i];
                    shild7[i] = spisok_hromosom[p1][i];
                    //shild8[i] = spisok_hromosom[p2, i];
                }
            }
            if (chi_e_klika(shild2)) shilds.Add(shild2);
            if (chi_e_klika(shild3)) shilds.Add(shild3);
            if (chi_e_klika(shild4)) shilds.Add(shild4);
            if (chi_e_klika(shild5)) shilds.Add(shild5);
            if (chi_e_klika(shild6)) shilds.Add(shild6);
            if (chi_e_klika(shild7)) shilds.Add(shild7);
            if (shilds.Count != 0)
            {
                shilds.Sort((x, y) => f(y) - f(x));
                bestshild = shilds[0];
            }
            else bestshild = null;
            shilds.Clear();
        }
        public void skreshuvania3()
        {
            //int[] shild1 = new int[n];
            int[] shild2 = new int[n];
            int[] shild3 = new int[n];
            int[] shild4 = new int[n];
            int[] shild5 = new int[n];
            int[] shild6 = new int[n];
            int[] shild7 = new int[n];
            int[] shild8 = new int[n];
            int[] shild9 = new int[n];
            int[] shild10 = new int[n];
            int[] shild11 = new int[n];
            int[] shild12 = new int[n];
            int[] shild13 = new int[n];
            int[] shild14 = new int[n];
            int[] shild15 = new int[n];
            //int[] shild16 = new int[n];
            int t1 = n / 4;
            int t2 = 2 * t1;
            int t3 = 3 * t1;
            for (int i = 0; i < n; i++)
            {
                if (i < t1)
                {
                    //shild1[i] = spisok_hromosom[p1, i];
                    shild2[i] = spisok_hromosom[p1][i];
                    shild3[i] = spisok_hromosom[p1][i];
                    shild4[i] = spisok_hromosom[p1][i];
                    shild5[i] = spisok_hromosom[p1][i];
                    shild6[i] = spisok_hromosom[p1][i];
                    shild7[i] = spisok_hromosom[p1][i];
                    shild8[i] = spisok_hromosom[p1][i];
                    shild9[i] = spisok_hromosom[p2][i];
                    shild10[i] = spisok_hromosom[p2][i];
                    shild11[i] = spisok_hromosom[p2][i];
                    shild12[i] = spisok_hromosom[p2][i];
                    shild13[i] = spisok_hromosom[p2][i];
                    shild14[i] = spisok_hromosom[p2][i];
                    shild15[i] = spisok_hromosom[p2][i];
                    //shild16[i] = spisok_hromosom[p2, i];

                }
                else if (i < t2)
                {
                    //shild1[i] = spisok_hromosom[p1, i];
                    shild2[i] = spisok_hromosom[p1][i];
                    shild3[i] = spisok_hromosom[p1][i];
                    shild4[i] = spisok_hromosom[p1][i];
                    shild5[i] = spisok_hromosom[p2][i];
                    shild6[i] = spisok_hromosom[p2][i];
                    shild7[i] = spisok_hromosom[p2][i];
                    shild8[i] = spisok_hromosom[p2][i];
                    shild9[i] = spisok_hromosom[p1][i];
                    shild10[i] = spisok_hromosom[p1][i];
                    shild11[i] = spisok_hromosom[p1][i];
                    shild12[i] = spisok_hromosom[p1][i];
                    shild13[i] = spisok_hromosom[p2][i];
                    shild14[i] = spisok_hromosom[p2][i];
                    shild15[i] = spisok_hromosom[p2][i];
                    //shild16[i] = spisok_hromosom[p2, i];
                }
                else if (i < t3)
                {
                    //shild1[i] = spisok_hromosom[p1, i];
                    shild2[i] = spisok_hromosom[p1][i];
                    shild3[i] = spisok_hromosom[p2][i];
                    shild4[i] = spisok_hromosom[p2][i];
                    shild5[i] = spisok_hromosom[p1][i];
                    shild6[i] = spisok_hromosom[p1][i];
                    shild7[i] = spisok_hromosom[p2][i];
                    shild8[i] = spisok_hromosom[p2][i];
                    shild9[i] = spisok_hromosom[p1][i];
                    shild10[i] = spisok_hromosom[p1][i];
                    shild11[i] = spisok_hromosom[p2][i];
                    shild12[i] = spisok_hromosom[p2][i];
                    shild13[i] = spisok_hromosom[p1][i];
                    shild14[i] = spisok_hromosom[p1][i];
                    shild15[i] = spisok_hromosom[p2][i];
                    //shild16[i] = spisok_hromosom[p2, i];
                }
                else 
                {
                    //shild1[i] = spisok_hromosom[p1, i];
                    shild2[i] = spisok_hromosom[p2][i];
                    shild3[i] = spisok_hromosom[p1][i];
                    shild4[i] = spisok_hromosom[p2][i];
                    shild5[i] = spisok_hromosom[p1][i];
                    shild6[i] = spisok_hromosom[p2][i];
                    shild7[i] = spisok_hromosom[p1][i];
                    shild8[i] = spisok_hromosom[p2][i];
                    shild9[i] = spisok_hromosom[p1][i];
                    shild10[i] = spisok_hromosom[p2][i];
                    shild11[i] = spisok_hromosom[p1][i];
                    shild12[i] = spisok_hromosom[p2][i];
                    shild13[i] = spisok_hromosom[p1][i];
                    shild14[i] = spisok_hromosom[p2][i];
                    shild15[i] = spisok_hromosom[p1][i];
                    //shild16[i] = spisok_hromosom[p2, i];
                }
            }
            if (chi_e_klika(shild2)) shilds.Add(shild2);
            if (chi_e_klika(shild3)) shilds.Add(shild3);
            if (chi_e_klika(shild4)) shilds.Add(shild4);
            if (chi_e_klika(shild5)) shilds.Add(shild5);
            if (chi_e_klika(shild6)) shilds.Add(shild6);
            if (chi_e_klika(shild7)) shilds.Add(shild7);
            if (chi_e_klika(shild8)) shilds.Add(shild8);
            if (chi_e_klika(shild9)) shilds.Add(shild9);
            if (chi_e_klika(shild10)) shilds.Add(shild10);
            if (chi_e_klika(shild11)) shilds.Add(shild11);
            if (chi_e_klika(shild12)) shilds.Add(shild12);
            if (chi_e_klika(shild13)) shilds.Add(shild13);
            if (chi_e_klika(shild14)) shilds.Add(shild14);
            if (chi_e_klika(shild15)) shilds.Add(shild15);
            if (shilds.Count != 0)
            {
                shilds.Sort((x, y) => f(y) - f(x));
                bestshild = shilds[0];
            }
            else bestshild = null;
            shilds.Clear();
        }
        public void vivid_nashadkiv()
        {
            if (shilds.Count!=0) Console.WriteLine("nashadki: ");
            for (int i = 0; i < shilds.Count; i++)
            {                
                for(int j=0; j<n; j++)
                {
                    Console.Write(shilds[i][j] + " ");
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine($"bestshild: {rekord} vuzliv: ");
            bestshild = spisok_hromosom[0];
            for (int j = 0; j < n; j++)
            {
                Console.Write(bestshild[j] + " ");
            }
        }
        public void mutasia(int p) // p - vidsotok mutantiv
        {
            if (bestshild != null)
                for (int t = 0; t < 30; t++)
                {
                    Random ran = new Random();
                    if (ran.Next(101) <= p)
                    {
                        int i = ran.Next(n);
                        int[] shild1 = new int[n];
                        for (int j = 0; j < n; j++) shild1[j] = bestshild[j];
                        shild1[i] = ran.Next(2);
                        if (chi_e_klika(shild1)) bestshild = shild1;
                    }
                }
            //{
            //    int[] shild1 = new int[n];
            //    for (int j = 0; j < n; j++) shild1[j] = bestshild[j];
            //    for (int j = 0; j < n; j++)
            //    {
            //        if (shild1[j] == 1)
            //        {
            //            shild1[j] = 0;
            //            bestshild = shild1;
            //            return;
            //        }
            //    }
            //}

        }
        public void lokalne_pokrashenia(int k_pokrashen)
        {
            if (bestshild != null)
            {
                for (int t=0; t<k_pokrashen; t++)
                {
                    int[] shild1 = new int[n];
                    for (int i = 0; i < n; i++) shild1[i] = bestshild[i];
                    for (int i = 0; i < n; i++)
                    {
                        if (shild1[i] == 0)
                        {
                            shild1[i] = 1;
                            if (chi_e_klika(shild1))
                            {
                                bestshild = shild1;
                                break;
                            }
                            else shild1[i] = 0;
                        }
                    }
                }
                //if (k_pokrashen==1)
                //{
                //    int[] shild1 = new int[n];
                //    for (int i = 0; i < n; i++) shild1[i] = bestshild[i];
                //    for (int i = 0; i < n; i++)
                //    {
                //        if (shild1[i] == 0) 
                //        {
                //            shild1[i] = 1;
                //            if (chi_e_klika(shild1))
                //            {
                //                bestshild = shild1;
                //                return;
                //            }
                //            else shild1[i] = 0;
                //        }
                //    }
                //}
                //else if (k_pokrashen==2)
                //{
                //    int[] shild1 = new int[n];
                //    for (int i = 0; i < n; i++) shild1[i] = bestshild[i];
                //    for (int i = 0; i < n; i++)
                //        for (int j = 0; j < n; j++)
                //        {
                //            if (shild1[i] == 0 && shild1[j] == 0 && i != j) 
                //            { 
                //                shild1[i] = 1;
                //                shild1[j] = 1;
                //                if (chi_e_klika(shild1))
                //                {
                //                    bestshild = shild1;
                //                    return;
                //                }
                //                else
                //                {
                //                    shild1[i] = 0;
                //                    shild1[j] = 0;
                //                }
                //            }                          
                //        }
                //}
            }               
        }
        public void zmina_rekordu()
        {
            if (rekord < f(bestshild)) rekord = f(bestshild);
        }
        public void dodavania_nashadka_do_populatsii()
        {
            spisok_hromosom.Add(bestshild);
            spisok_hromosom.Sort((x, y) => f(y) - f(x));
            //Console.WriteLine($"vidaleno hromosomu z {f(spisok_hromosom[spisok_hromosom.Count - 1])} uzlami");
            spisok_hromosom.RemoveAt(spisok_hromosom.Count - 1);
        }
        public void evolutsia(int k)
        {
            for(int i=0; i<k; i++)
            {
                vibir_batkiv_turnir(5);
                skreshuvania1();
                mutasia(90);
                lokalne_pokrashenia(5);
                zmina_rekordu();
                dodavania_nashadka_do_populatsii();
            }
        }
        public void save()
        {
             string fname = "data.txt";
             using (StreamWriter sw = new StreamWriter(fname, false, System.Text.Encoding.Default))
             {
                string s = "";
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++) s += mas[i, j] + " ";
                }
                sw.WriteLine(s);
                //sw.WriteLine(JsonSerializer.Serialize<int[,]>(mas));
            }

        }
        public void load()
        {
            string fname = "data.txt";
            string s = "";
            using (StreamReader sr = new StreamReader(fname))
            {
                int k = 0;
                s = sr.ReadToEnd();
                string[] dataS = s.Split(" ");
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        mas[i, j] = Int32.Parse(dataS[k]);
                        k++;
                    }
                }
                //s = sr.ReadToEnd();
                //mas = JsonSerializer.Deserialize<int[,]>(s);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            graf g = new graf();
            g.vivod();
            //g.save();
            //g.load();
            //Console.WriteLine("load.save");
            //g.vivod();
            g.evolutsia(100);
            g.vivid_nashadkiv();

        }
    }
}
