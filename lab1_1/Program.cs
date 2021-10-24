using System;
using System.Collections.Generic;

namespace lab1_1
{
    class Node
    {
        public int[,] State;
        public Node Parent_Node;
        public string action;
        public int Path_Cost;
        public int Depth;
        public int f;
        public Node(int[,] State, Node Parent)
        {
            this.State = new int[3, 3];
            this.State = State;
            Parent_Node = Parent;
            if (Parent == null)
            {
                this.Depth = 0;
                this.Path_Cost = 0;
            }
            else
            {
                this.Depth = Parent.Depth + 1;
                this.Path_Cost = Path_Cost + 1;            
            }
        }
    }
    class Program
    {
        public static int[,] matr = new int[3, 3]{
            {7,3,6},
            {1,4,8},
            {5,2,0}
            };
        public static int[,] matr2 = new int[3, 3]{
            {5,4,2},
            {3,0,6},
            {1,7,8}
            };
        public static int[,] matr3 = new int[3, 3]{
            {1,2,3},
            {4,8,0},
            {7,6,5}
            };
        public static int kilk_staniv;
        public static int kilk_iterasiy;
        public static List<Node> Qp;
        public static int[,] Goal = new int[3, 3]{
            {1,2,3},
            {4,5,6},
            {7,8,0}
            };
        public static Queue<Node> fringe = new Queue<Node>();
        public static Node Up(Node per)
        {
            int l = -1, m = -1;
            int[,] NewState = copy(per.State);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (NewState[i, j] == 0)
                    {
                        l = i;
                        m = j;
                    };
                }
            }
            if (l == 0) return null;
            NewState[l, m] = NewState[l - 1, m];
            NewState[l - 1, m] = 0;
            Node shild = new Node(NewState, per);
            shild.action = "Up";
            return shild;
        }
        public static Node Dawn(Node per)
        {
            int l = -1, m = -1;
            int[,] NewState = copy(per.State);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (NewState[i, j] == 0)
                    {
                        l = i;
                        m = j;
                    };
                }
            }
            if (l == 2) return null;
            NewState[l, m] = NewState[l + 1, m];
            NewState[l + 1, m]=0;
            Node shild = new Node(NewState, per);
            shild.action = "Dawn";
            return shild;
        }
        public static Node Left(Node per)
        {
            int l = -1, m = -1;
            int[,] NewState = copy(per.State);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (NewState[i, j] == 0)
                    {
                        l = i;
                        m = j;
                    };
                }
            }
            if (m == 2) return null;
            NewState[l, m] = NewState[l, m + 1];
            NewState[l, m + 1] = 0;
            Node shild = new Node(NewState, per);
            shild.action = "Left";
            return shild;
        }
        public static Node Right(Node per)
        {
            int l = -1, m = -1;
            int[,] NewState = copy(per.State);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (NewState[i, j] == 0)
                    {
                        l = i;
                        m = j;
                    };
                }
            }
            if (m == 0) return null;
            NewState[l, m] = NewState[l, m - 1];
            NewState[l, m - 1] = 0;
            Node shild = new Node(NewState, per);
            shild.action = "Right";
            return shild;
        }
        public static List<Node> Expand(Node per )
        {
            List<Node> successors = new List<Node>();
            Node shild;
            shild=Up(per);
            if (shild != null) successors.Add(shild);
            shild = Left(per);
            if (shild != null) successors.Add(shild);
            shild = Dawn(per);
            if (shild != null) successors.Add(shild);
            shild = Right(per);
            if (shild != null) successors.Add(shild);
            return successors;
        }
        public static void vivod (Node n)
        {
            Console.WriteLine("matr:");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(n.State[i,j]+"\t");
                }
                Console.WriteLine();
            }
        }
        public static bool Goal_Test(Node n)
        {
            for(int i=0; i < 3; i++)
            {
                for(int j=0; j < 3; j++)
                {
                    if (n.State[i, j] != Goal[i, j]) return false;
                }
            }
            return true;
        }
        public static Node tree_search_BFS(Node problem, int d=15)
        {
            kilk_staniv = 1;
            kilk_iterasiy = 0;
            Node n;
            fringe.Enqueue(problem);
            while (fringe.Count != 0)
            {
                kilk_iterasiy++;
                if (fringe.Count == 0) return null;
                n = fringe.Dequeue();
                if (Goal_Test(n)) return n;
                List<Node> shild = Expand(n);
                if (shild!=null) kilk_staniv += shild.Count;
                if (shild != null) for (int i = 0; i < shild.Count; i++)
                    {
                        fringe.Enqueue(shild[i]);
                    }
                if (n.Depth==d)
                {
                    Console.WriteLine($"glubuna dereva poshuku BFS {d}, rishennia ne znaishov");
                    return null;
                }
            }
            return null;
        }
        public static int[,] copy(int[,] mas) 
        {
            int[,] rez = new int[3,3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rez[i, j] = mas[i, j];
                }
            }
            return rez;
        }
        public static void Vivod_solution(Node solution)
        {
            if (solution == null) return;
            Stack<Node> rez = new Stack<Node>(); 
            while (solution.Parent_Node != null)
            {
                rez.Push(solution);
                solution = solution.Parent_Node;
            }
            while (rez.Count!=0)
            {
                vivod(rez.Pop());
            }
        }
        public static int h(Node n)
        {
            int h = 0;
            for (int i=0; i<3; i++)
            {
                for (int j=0; j<3; j++)
                {
                    int x = n.State[i, j];
                    switch (x)
                    {
                        case 1: h += Math.Abs(0 - i) + Math.Abs(0 - j); break;
                        case 2: h += Math.Abs(0 - i) + Math.Abs(1 - j); break;
                        case 3: h += Math.Abs(0 - i) + Math.Abs(2 - j); break;
                        case 4: h += Math.Abs(1 - i) + Math.Abs(0 - j); break;
                        case 5: h += Math.Abs(1 - i) + Math.Abs(1 - j); break;
                        case 6: h += Math.Abs(1 - i) + Math.Abs(2 - j); break;
                        case 7: h += Math.Abs(2 - i) + Math.Abs(0 - j); break;
                        case 8: h += Math.Abs(2 - i) + Math.Abs(1 - j); break;
                        case 0: h += Math.Abs(2 - i) + Math.Abs(2 - j); break;
                        default: break;
                    }                   
                }
            }
            return h;
        }
        public static bool Contain (List<Node> l, Node n)
        {
            if (l == null) return false;
            if (l.Count == 0) return false;
            for(int k=0; k<l.Count; k++)
            {
                bool rez = true;
                for (int i=0; i<3; i++)
                {
                    for (int j=0; j<3; j++)
                    {
                        if (l[k].State[i, j] != n.State[i, j]) rez = false;
                    }
                }
                if (rez) return rez;
            }
            return false;
        }
        public static Node A_star(Node Start, int max_it=5000)
        {
            //    Q — множество вершин, которые требуется рассмотреть,
            //    U — множество рассмотренных вершин,
            //    f[x] — значение эвристической функции "расстояние + стоимость" для вершины x,
            //    g[x] — стоимость пути от начальной вершины до x,
            //    h(x) — эвристическая оценка расстояния от вершины x до конечной вершины.
            kilk_staniv = 1;
            kilk_iterasiy = 0;
            List<Node> U=new List<Node>();
            List<Node> Q=new List<Node>();
            Q.Add(Start);
            Start.Path_Cost = 0;
            Start.f = Start.Path_Cost + h(Start);
            while (Q.Count != 0)
            {
                kilk_iterasiy++;
                if (kilk_iterasiy > max_it)
                {
                    Console.WriteLine($"kilk_iterasiy={kilk_iterasiy} розвязок не знайдено. ");
                    return null;
                }
                Q.Sort((x, y) => x.f - y.f);
                Node current = Q[0];
                if (Goal_Test(current)) return current;
                Q.Remove(current);
                U.Add(current);
                List<Node> vl = Expand(current);
                if (vl != null) kilk_staniv += vl.Count;
                foreach (Node v in vl)
                {
                    int tentativeScore = current.Path_Cost + 1;
                    if ((Contain(U,v))&&(tentativeScore >= v.Path_Cost)) continue;
                    if ((!(Contain(U,v))) || (tentativeScore < v.Path_Cost)) {
                        v.Parent_Node = current;
                        v.Path_Cost = tentativeScore;
                        v.f = v.Path_Cost + h(v);
                        if (!(Contain(Q,v))) Q.Add(v);
                    }
                }
            }
            return null;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Node tree = new Node(matr3, null);
            Console.WriteLine("BFS:");
            vivod(tree);
            Vivod_solution(tree_search_BFS(tree));
            Console.WriteLine($"killist_staniv={kilk_staniv}");
            Console.WriteLine($"kilk_iterasiy={kilk_iterasiy}");
            Console.WriteLine("A*:");
            vivod(tree);
            Vivod_solution(A_star(tree));
            Console.WriteLine($"killist_staniv={kilk_staniv}");
            Console.WriteLine($"kilk_iterasiy={kilk_iterasiy}");
        }
    }
}
