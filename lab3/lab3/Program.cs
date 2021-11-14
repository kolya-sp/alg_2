using System;
using System.Collections.Generic;

namespace lab3
{
    class uzel
    {
        public int stepin;
        public int color;
        public int konfl;
        public int nomer;
        public uzel (int s, int c, int k, int n)
        {
            stepin = s;
            color = c;
            konfl = k;
            nomer = n;
        }
    }
    class graf
    {
        public List<uzel> spisok_uzlov;
        public int n = 200; // kilkist vershin
        public int m = 50; // max stepin vershini
        public int[,] mas; // matrisa sumijnosti grafa
        public int max_color=1;
        public List<uzel> spisok_uzlov_min_record;
        public int max_color_record;
        public int k_rozvidnikiv=5;
        public int k_furjiriv=55;
        public graf()
        {
            mas = new int[n, n];
            spisok_uzlov = new List<uzel>();
            for(int i = 0; i<n; i++)
            {
                uzel u = new uzel(0, 0, 0, i);
                spisok_uzlov.Add(u);
            }
        }
        public void gen ()
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
                for (int t = i + 1; t < n; t++) if (spisok_uzlov[t].stepin >= m) s2++;
                int k_sv = m - s1;
                if (k_sv > n - i - 1 - s2) k_sv = n - i - 1 - s2;
                if(k_sv>0) k_sv = rnd.Next(1,k_sv);
                for (int k = 0; k < k_sv; k++)
                {
                    int r = rnd.Next(i+1,n);
                    if (mas[i, r] == 0 && spisok_uzlov[i].stepin<m && spisok_uzlov[r].stepin<m) { 
                        mas[i, r] = 1;
                        spisok_uzlov[i].stepin++;
                        spisok_uzlov[r].stepin++;
                    }
                    else k--;
                }                                     
            }
            for(int i=0; i<n; i++)
            {
                for (int j=i+1; j<n; j++)
                {
                    mas[j, i] = mas[i, j];
                }
            }
            for(int i=0; i<n; i++) // rahuemo stepeni uzliv
            {
                int s = 0;
                for(int j = 0; j < n; j++)
                {
                    if (mas[i, j] == 1) s++;
                }
                spisok_uzlov[i].stepin = s;
            }
        }
        public void vivod()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write( $"{ mas[i, j]}" );
                }
                Console.WriteLine();
            }
        }
        public void farbuemo()
        {
            List<uzel> rozf = new List<uzel>();
            List<uzel> nerozf = new List<uzel>();
            for (int i = 0; i < n; i++)
            {
                nerozf.Add(spisok_uzlov[i]);
            }
            for (int t=0; t<n; t++)
            {
                nerozf.Sort((u, v) => v.konfl - u.konfl); // znahodimo max konfl
                uzel v = nerozf[0];
                int i;
                int j=0;
                for(i=1; i<n && j<n; i++) // pidbiraemo kolir
                {
                    for (j = 0; j < n; j++)  // pereviraemo vershini sumijni na spibpadinnia koloru 
                        if (mas[v.nomer,j]==1 && i==spisok_uzlov[j].color && v.nomer!=j)
                        {
                            break;
                        }
                }
                v.color = i - 1;
                if (v.color > max_color) max_color = v.color;
                max_color_record = max_color;
                rozf.Add(v);
                nerozf.Remove(v);
                for (i = 0; i< nerozf.Count; i++) //rahuemo konflikti
                    for (j = 0; j<rozf.Count; j++) 
                    {
                        if (mas[nerozf[i].nomer, rozf[j].nomer] == 1) nerozf[i].konfl++;
                    }
            }
        }
        public void jadnefarbubania()
        {
            List<uzel> rozf = new List<uzel>();
            List<uzel> nerozf = new List<uzel>();
            for (int i = 0; i < n; i++)
            {
                nerozf.Add(spisok_uzlov[i]);
            }
            for (int t = 0; t < n; t++)
            {
                //nerozf.Sort((u, v) => v.konfl - u.konfl); // znahodimo max konfl
                uzel v = nerozf[0];
                int i;
                int j = 0;
                for (i = 1; i < n && j < n; i++) // pidbiraemo kolir
                {
                    for (j = 0; j < n; j++)  // pereviraemo vershini sumijni na spibpadinnia koloru 
                        if (mas[v.nomer, j] == 1 && i == spisok_uzlov[j].color && v.nomer != j)
                        {
                            break;
                        }
                }
                v.color = i - 1;
                if (v.color > max_color) max_color = v.color;
                max_color_record = max_color;
                rozf.Add(v);
                nerozf.Remove(v);
                for (i = 0; i < nerozf.Count; i++) //rahuemo konflikti
                    for (j = 0; j < rozf.Count; j++)
                    {
                        if (mas[nerozf[i].nomer, rozf[j].nomer] == 1) nerozf[i].konfl++;
                    }
            }
        }
        public void glupe_farbuvania()
        {
            int i;
            for ( i = 0; i < n; i++) spisok_uzlov[i].color = i + 1;
            max_color = i;
            max_color_record = max_color;
        }
        public List<int> rozvidka ()
        {
            List<int> rez = new List<int>();
            Random rnd = new Random();
            for (int k=0; k<k_rozvidnikiv; k++)
            {
                int n_dil = rnd.Next(n);
                if (!rez.Contains(n_dil)) rez.Add(n_dil);
                else k--;
            }

            //Console.Write($"rozvidka: ");
            //for (int k = 0; k < rez.Count; k++) { Console.Write($"{rez[k]} "); if (rez[k] == 199) Console.Write("!!!!!!!!!!!!!!!!!"); }
            //Console.WriteLine();

                return rez;
        }
        public List<int> rozpodil_furajiriv (List<int> nomera_tsiley)
        {
            List<int> rez = new List<int>();
            int s = 0;
            for (int i = 0; i < nomera_tsiley.Count; i++) s += spisok_uzlov[nomera_tsiley[i]].stepin;
            for (int i = 0; i < nomera_tsiley.Count; i++) rez.Add(1+ spisok_uzlov[nomera_tsiley[i]].stepin*(k_furjiriv-k_rozvidnikiv)/s);

            //Console.Write($"rozpodil_furajiriv: ");
            //for (int k = 0; k < rez.Count; k++) { Console.Write($"{rez[k]} ");}
            //Console.WriteLine();

            return rez;

        }
        public void poshuk_v_okolitsiah(List<int> tsili, List<int> rozpodileni_bdjoli)
        {
            //List<int> tsili=rozvidka();
            //List<int> rozpodileni_bdjoli = rozpodil_furajiriv(tsili);
            Random rnd = new Random();
            spisok_uzlov_min_record = spisok_uzlov;
            for (int i=0; i < tsili.Count; i++)
            {
                List < uzel > okil = new List<uzel>();
                for(int j=0;j<n;j++) //formuemo okil
                {
                    uzel u = new uzel(spisok_uzlov[j].stepin, spisok_uzlov[j].color, spisok_uzlov[j].konfl, spisok_uzlov[j].nomer);
                    okil.Add(u);
                }

                for (int k=0; k<rozpodileni_bdjoli[i]; k++) // zapusk furajiriv
                {
                    int tsil = tsili[i];
                    List<int> sumijni_vershini_z_tsillu = new List<int>();
                    for (int j=0; j<n; j++)
                    {
                        if (mas[tsil, j] == 1) sumijni_vershini_z_tsillu.Add(j);
                    }

                    int n2 = sumijni_vershini_z_tsillu[rnd.Next(sumijni_vershini_z_tsillu.Count)];
                    //Console.WriteLine($"n2={n2}");

                    List<int> sumijni_vershini_z_n2 = new List<int>();
                    for (int j = 0; j < n; j++)
                    {
                        if (mas[n2, j] == 1) sumijni_vershini_z_n2.Add(j);
                    }
                    bool flag = true;
                    for (int j=0; j<sumijni_vershini_z_n2.Count; j++)
                    {
                        if (spisok_uzlov[tsil].color == spisok_uzlov[sumijni_vershini_z_n2[j]].color && sumijni_vershini_z_n2[j] != tsil) flag = false;
                    }
                    for (int j = 0; j < sumijni_vershini_z_tsillu.Count; j++)
                    {
                        if (spisok_uzlov[n2].color == spisok_uzlov[sumijni_vershini_z_tsillu[j]].color && sumijni_vershini_z_tsillu[j]!=n2) flag = false;
                    }
                    if (flag) // zamina vuzliv
                    {
                        int tmp = okil[n2].color;
                        okil[n2].color = okil[tsil].color;
                        okil[tsil].color = tmp;
                        //Console.WriteLine("zamina !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"); // test

                        //okil[n2].color = 0; // zmenshuemo kolori;
                        int j1;
                        int t1=0;
                        for (j1 = 1; j1 <= max_color && t1 < sumijni_vershini_z_n2.Count; j1++)
                        {
                            for (t1 = 0; t1 < sumijni_vershini_z_n2.Count; t1++)
                            {
                                if (okil[sumijni_vershini_z_n2[t1]].color == j1) break;
                            }
                        }
                        okil[n2].color = j1 - 1;
                        //Console.WriteLine($"v {n2} kolir {j1 - 1}"); //test
                        //okil[tsil].color = 0;
                        t1 = 0;
                        for (j1 = 1; j1 <= max_color && t1 < sumijni_vershini_z_tsillu.Count; j1++)
                        {
                            for (t1 = 0; t1 < sumijni_vershini_z_tsillu.Count; t1++)
                            {
                                if (okil[sumijni_vershini_z_tsillu[t1]].color == j1) break;
                            }
                        }
                        okil[tsil].color = j1 - 1;
                        //Console.WriteLine($"v {tsil} kolir {j1 - 1}"); //test
                        if (otsinka_korisnosti_dilanki(okil) < otsinka_korisnosti_dilanki(spisok_uzlov_min_record)) // onovlenia rekordu
                        {
                            spisok_uzlov_min_record = okil;
                            //Console.WriteLine($"korisnist rekordu {otsinka_korisnosti_dilanki(spisok_uzlov_min_record)}");
                        }


                    }                    
                }
            }
            spisok_uzlov = spisok_uzlov_min_record;
        }
        public int otsinka_korisnosti_dilanki( List<uzel> dilianka)
        {
            int rez = 0;
            for (int i = 0; i < dilianka.Count; i++) rez += dilianka[i].color;
            //Console.WriteLine($"otsinka_korisnosti_dilanki = {rez}");
            return rez;
        }
        public bool test()
        {
            bool flag = true;
            for(int i=0; i<n; i++)
            {
                for(int j=0; j<n; j++)
                {
                    if (mas[i,j]==1 && spisok_uzlov[i].color==spisok_uzlov[j].color)
                    {
                        flag = false;
                        return flag;
                    }
                }
            }
            return flag;
        }
        public void roi (int k = 10)
        {
            for (int i=1; i<=k; i++ )
            {
                List<int> tsili = rozvidka();
                poshuk_v_okolitsiah(tsili, rozpodil_furajiriv(tsili));
                max_color_record = 0;
                for (int j=0; j< spisok_uzlov_min_record.Count; j++)
                {
                    if (spisok_uzlov_min_record[j].color > max_color_record) max_color_record = spisok_uzlov_min_record[j].color;
                }
                if(i%20==0) Console.Write ($"№ {i} koloriv {max_color_record} \t");
                if (i % 100 == 0) Console.WriteLine();
            }
        }
        public void vivod_rozkraski()
        {
            Console.WriteLine("rozkraska: ");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"v[{i}].col={spisok_uzlov[i].color} \t");
                if ((i+1) % 5 == 0) Console.WriteLine();
            }
            Console.WriteLine($"vikoristano {max_color_record} koloriv");
        }
    }

    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            graf g = new graf();
            g.gen();
            g.vivod();
            g.jadnefarbubania();
            //g.glupe_farbuvania();
            g.vivod_rozkraski();
            g.roi(1000);
            g.vivod_rozkraski();
            Console.WriteLine("perevirka vidsutnosti konfliktiv koloriv: "+g.test());
        }
    }
}
