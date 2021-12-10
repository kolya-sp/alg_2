using System;
using System.Collections.Generic;

namespace lab5
{
    class state
    {
        public static int i = 0;
        public static int n = 8;
        public static int k = 1;
        public int[,] mas;
        public int hid;
        public state(int hid1) 
        {
            hid = hid1;
            mas = new int[n, n];
            for (int i=0; i<n; i++)
            {
                for (int j=0; j<n; j++)
                {
                    mas[i, j] = 0;
                }
            }


            i++;
        }
        public int[,] clone ()
        {
            int[,] rez = new int[n,n];
            for(int i=0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    rez[i, j] = mas[i, j];
                }
            }
            return rez;
        }
        public void vivod()
        {
            Console.Write("  ");
            for (int i = 0; i < n; i++) Console.Write(i);
            Console.WriteLine();
            for (int i = 0; i < n; i++)
            {
                Console.Write(i+" ");
                for (int j = 0; j < n; j++)
                {                    
                    if (mas[i, j] == 0) Console.Write(mas[i, j]);
                    else 
                    {
                        Console.Write("#"); 
                    }
                    //Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }

        }
        public List<state> Successors()
        {
            List<state> rez = new List<state>();
            if (hid == 1)
            {
                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (mas[i, j] == 0 && mas[i + 1, j] == 0)
                        {
                            state st_add = new state(2);
                            st_add.mas = clone();
                            st_add.mas[i, j] = 2;
                            st_add.mas[i + 1, j] = 4;
                            rez.Add(st_add);
                            //if (state.i % 10000000 == 0) st_add.vivod(); // test ------------------
                        }
                    }
                }
            }
            else if (hid == 2)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n - 1; j++)
                    {
                        if (mas[i, j] == 0 && mas[i, j + 1] == 0)
                        {
                            state st_add = new state(1);
                            st_add.mas = clone();
                            st_add.mas[i, j] = 1;
                            st_add.mas[i, j + 1] = 3;
                            rez.Add(st_add);
                            //if (state.i % 10000000 == 0) st_add.vivod(); // test ------------------
                        }
                    }
                }
            }
            else Console.WriteLine("Error hid!=1,2");
            return rez;
        }
        public bool terminal_test()
        {
            return Successors().Count == 0;
        }
    }
    class tree
    {
        public static state top;
        public static state current;
        public static bool flag = true;
        public tree()
        {
            top = new state(1);
            current = top;
        }
        public static state mini_max_desidion(state current_state)
        {
            int rez_i = -1;
            int max = 0;
            state.i = 1;
            List<state> list_nashadkiv = current_state.Successors();
            if (list_nashadkiv.Count == 0) { flag = false; Console.WriteLine("your win"); return null; }
            else
            {
                for (int i = 0; i < list_nashadkiv.Count; i++)
                {
                    if (max < min_value(list_nashadkiv[i]))
                    {
                        max = min_value(list_nashadkiv[i]);
                        rez_i = i;
                    }
                    if (state.i > 1000000* state.k)
                    {
                        state.i = 1;
                        return otsinka(current_state);
                    }
                }
                return list_nashadkiv[rez_i];
            }
            return null;
        }
        public static int max_value(state st)
        {
            if (state.i > 1000000 * state.k) return 0;
            List<state> list_nashadkiv = st.Successors();
            if (list_nashadkiv.Count==0) return st.hid;
            int rez = 0;
            for (int i=0; i< list_nashadkiv.Count; i++)
            {
                if (rez < min_value(list_nashadkiv[i])) rez = min_value(list_nashadkiv[i]);
            }
            return rez;
        }
        public static int min_value(state st)
        {
            if (state.i > 1000000 * state.k) return 0;
            List<state> list_nashadkiv = st.Successors();
            if (list_nashadkiv.Count==0) return st.hid;
            int rez = 3;
            for (int i = 0; i < list_nashadkiv.Count; i++)
            {
                if (rez > max_value(list_nashadkiv[i])) rez = max_value(list_nashadkiv[i]);
            }
            return rez;
        }
        public static void hid()
        {
            List<state> list_nashadkiv = top.Successors();
            current = null;
            int t = top.hid;
            if (t == 1)
            {
                if (list_nashadkiv.Count > 0)
                {
                    Console.Write("i=");
                    int i = int.Parse(Console.ReadLine());
                    Console.Write("j=");
                    int j = int.Parse(Console.ReadLine());

                    for (int k = 0; k < list_nashadkiv.Count; k++)
                    {
                        if (list_nashadkiv[k].mas[i, j] == 2)
                        {
                            current = list_nashadkiv[k];
                        }
                    }
                }
                else { Console.WriteLine("game over"); flag = false; return; }
            }
            else
            {
                if (list_nashadkiv.Count > 0)
                {
                    Console.Write("i=");
                    int i = int.Parse(Console.ReadLine());
                    Console.Write("j=");
                    int j = int.Parse(Console.ReadLine());
                    for (int k = 0; k < list_nashadkiv.Count; k++)
                    {
                        if (list_nashadkiv[k].mas[i, j] == 1)
                        {
                            current = list_nashadkiv[k];
                        }
                    }
                }
                else { Console.WriteLine("game over"); flag = false; return; }
            }
            if (current != null) top = current;
            else hid();
        }
        public static state ab_vidsikania(state current_state)
        {
            int rez_i = -1;
            int max = 0;
            state.i = 1;
            List<state> list_nashadkiv = current_state.Successors();
            if (list_nashadkiv.Count == 0) { flag = false; Console.WriteLine("your win"); return null; }
            for (int i = 0; i < list_nashadkiv.Count; i++)
            {
                if (max < ab_min_value(list_nashadkiv[i],1,2))
                {
                    max = ab_min_value(list_nashadkiv[i],1,2);
                    rez_i = i;
                }
                if (state.i > 1000000 * state.k)
                {
                    state.i = 1;
                    return otsinka(current_state);
                }
            }
            return list_nashadkiv[rez_i];
        }
        public static int ab_max_value(state st, int a, int b )
        {
            if (state.i > 1000000 * state.k) return 0;
            List<state> list_nashadkiv = st.Successors();
            if (list_nashadkiv.Count == 0) return st.hid;
            int v = 0;
            for (int i = 0; i < list_nashadkiv.Count; i++)
            {
                if (v < ab_min_value(list_nashadkiv[i],a,b)) v = ab_min_value(list_nashadkiv[i],a,b);
                if (v >= b) return v;
                if (v > a) a = v;
            }
            return v;
        }

        public static int ab_min_value(state st, int a, int b)
        {
            if (state.i > 1000000 * state.k) return 0;
            List<state> list_nashadkiv = st.Successors();
            if (list_nashadkiv.Count == 0) return st.hid;
            int v = 3;
            for (int i = 0; i < list_nashadkiv.Count; i++)
            {
                if (v > ab_max_value(list_nashadkiv[i],a,b)) v = ab_max_value(list_nashadkiv[i],a,b);
                if (v <= a) return v;
                if (b > v) b = v;
            }
            return v;
        }

        public static state otsinka (state st)
        {
            List<state> list_nashadkiv = st.Successors();
            int min = 56;
            int min_i=-1;
            for (int i = 0; i < list_nashadkiv.Count; i++)
            {
                if (min > list_nashadkiv[i].Successors().Count) 
                { 
                    min = list_nashadkiv[i].Successors().Count;
                    min_i = i;
                }
            }
            if (min_i != -1)
            {
                return list_nashadkiv[min_i];
            }
            else return null;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Vvedit skladnist");
            tree t1 = new tree();
            state.k= int.Parse(Console.ReadLine());
            while (tree.flag)
            {
                                             
                if (tree.flag)
                {
                    Console.WriteLine("stan:");
                    tree.top.vivod();
                    tree.top = tree.mini_max_desidion(tree.top);
                    Console.WriteLine("stan:");
                    if(tree.top != null) tree.top.vivod();
                }
                if (tree.top != null) tree.hid();
            }
        }
    }
}
