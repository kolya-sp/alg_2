using System;
using System.IO;
namespace la2
{
    class BTreeNode
    {
        public struct kv
        {
            public int key;
            public string value;

        }
        public kv[] keys; // Ключи узла
        public int MinDeg; // Минимальная степень узла B-дерева
        public BTreeNode[] children; // дочерний узел
        public int num; // Количество ключей узла
        public bool isLeaf; // Истина, если это листовой узел
        public static int chislo_proidenih_vuzliv;

        // Конструктор
        public BTreeNode(int deg, bool isLeaf)
        {

            this.MinDeg = deg;
            this.isLeaf = isLeaf;
            this.keys = new kv[2 * this.MinDeg - 1]; // Узел имеет не более 2 * MinDeg-1 ключей
            this.children = new BTreeNode[2 * this.MinDeg];
            this.num = 0;

        }

        //Находим индекс первой позиции, равный или больший, чем ключ
        public int findKey(int key)
        {

            int idx = 0;
            // Условия выхода из цикла: 1.idx == num, то есть сканировать все
            // 2.idx <num, т.е. найти ключ или больше ключа
            while (idx < num && keys[idx].key < key)
                ++idx;
            return idx;
        }
        public void remove(int key)
        {

            int idx = findKey(key);
            if (idx < num && keys[idx].key == key)
            { // Найди ключ
                if (isLeaf) // ключ находится в листовом узле
                    removeFromLeaf(idx);
                else // ключ отсутствует в листовом узле
                    removeFromNonLeaf(idx);
            }
            else
            {
                if (isLeaf)
                { // Если узел является листовым узлом, то этот узел не входит в B-дерево
                    Console.WriteLine($"The key {key} is does not exist in the tree");
                    return;
                }

                // В противном случае удаляемый ключ существует в поддереве с корнем в этом узле

                // Этот флаг указывает, существует ли ключ в поддереве с корнем в последнем дочернем узле узла
                // Когда idx равно num, сравнивается весь узел, и флаг равен true
                bool flag = idx == num;

                if (children[idx].num < MinDeg) // Когда дочерний узел узла не заполнен, сначала заполняем его
                    fill(idx);


                // Если последний дочерний узел был объединен, то он должен был быть объединен с предыдущим дочерним узлом, поэтому мы рекурсивно переходим к (idx-1) -ому дочернему узлу.
                // В противном случае мы рекурсивно переходим к (idx) -ому дочернему узлу, который теперь имеет как минимум ключи наименьшей степени
                if (flag && idx > num)
                    children[idx - 1].remove(key);
                else
                    children[idx].remove(key);
            }
        }

        public void removeFromLeaf(int idx)
        {

            // возвращаемся из idx
            for (int i = idx + 1; i < num; ++i)
                keys[i - 1] = keys[i];
            num--;
        }

        public void removeFromNonLeaf(int idx)
        {

            int key = keys[idx].key;

            // Если поддерево перед ключом (children [idx]) имеет не менее t ключей
            // Затем находим предшественника key'pred 'в поддереве с корнем в children [idx]
            // Заменить ключ на'pred ', рекурсивно удалить пред в дочерних [idx]
            if (children[idx].num >= MinDeg)
            {
                kv pred = getPred(idx);
                keys[idx].key = pred.key;
                keys[idx].value = pred.value;
                children[idx].remove(pred.key);
            }
            // Если у детей [idx] меньше ключей, чем у MinDeg, проверяем дочерние элементы [idx + 1]
            // Если дочерние элементы [idx + 1] имеют хотя бы ключи MinDeg, в поддереве с корнем дочерние элементы [idx + 1]
            // Находим преемника ключа 'ucc' для рекурсивного удаления succ в дочерних элементах [idx + 1]
            else if (children[idx + 1].num >= MinDeg)
            {
                kv succ = getSucc(idx);
                keys[idx].key = succ.key;
                keys[idx].value = succ.value;
                children[idx + 1].remove(succ.key);
            }
            else
            {
                // Если ключи children [idx] и children [idx + 1] меньше MinDeg
                // затем объединяем ключ и дочерние элементы [idx + 1] в дочерние элементы [idx]
                // Теперь children [idx] содержит ключ 2t-1
                // Освобождаем дочерние элементы [idx + 1], рекурсивно удаляем ключ в children [idx]
                merge(idx);
                children[idx].remove(key);
            }
        }

        public kv getPred(int idx)
        { // Узел-предшественник должен найти крайний правый узел из левого поддерева

            // Продолжаем двигаться к крайнему правому узлу, пока не достигнем листового узла
            BTreeNode cur = children[idx];
            kv rez;
            while (!cur.isLeaf)
                cur = cur.children[cur.num];
            rez.key = cur.keys[cur.num - 1].key;
            rez.value = cur.keys[cur.num - 1].value;
            return rez;
        }

        public kv getSucc(int idx)
        { // Узел-преемник находится от правого поддерева к левому

            // Продолжаем перемещать крайний левый узел от дочерних [idx + 1], пока не достигнем конечного узла
            BTreeNode cur = children[idx + 1];
            kv rez;
            while (!cur.isLeaf)
                cur = cur.children[0];
            rez.key = cur.keys[0].key;
            rez.value = cur.keys[0].value;
            return rez;
        }

        // Заполняем дочерние элементы [idx], у которых меньше ключей MinDeg
        public void fill(int idx)
        {

            // Если предыдущий дочерний узел имеет несколько ключей MinDeg-1, заимствовать из них
            if (idx != 0 && children[idx - 1].num >= MinDeg)
                borrowFromPrev(idx);
            // Последний дочерний узел имеет несколько ключей MinDeg-1, заимствовать от них
            else if (idx != num && children[idx + 1].num >= MinDeg)
                borrowFromNext(idx);
            else
            {
                // объединить потомков [idx] и его брата
                // Если children [idx] - последний дочерний узел
                // затем объединить его с предыдущим дочерним узлом, иначе объединить его со следующим братом
                if (idx != num)
                    merge(idx);
                else
                    merge(idx - 1);
            }
        }

        // Заимствуем ключ у потомков [idx-1] и вставляем его в потомки [idx]
        public void borrowFromPrev(int idx)
        {

            BTreeNode child = children[idx];
            BTreeNode sibling = children[idx - 1];

            // Последний ключ из дочерних [idx-1] переходит к родительскому узлу
            // ключ [idx-1] из недополнения родительского узла вставляется как первый ключ в дочерних [idx]
            // Следовательно, sibling уменьшается на единицу, а children увеличивается на единицу
            for (int i = child.num - 1; i >= 0; --i) // дети [idx] продвигаются вперед
                child.keys[i + 1] = child.keys[i];

            if (!child.isLeaf)
            { // Если дочерний узел [idx] не является листовым, переместите его дочерний узел назад
                for (int i = child.num; i >= 0; --i)
                    child.children[i + 1] = child.children[i];
            }

            // Устанавливаем первый ключ дочернего узла на ключи текущего узла [idx-1]
            child.keys[0] = keys[idx - 1];
            if (!child.isLeaf) // Устанавливаем последний дочерний узел в качестве первого дочернего узла дочерних элементов [idx]
                child.children[0] = sibling.children[sibling.num];

            // Перемещаем последний ключ брата к последнему из текущего узла
            keys[idx - 1] = sibling.keys[sibling.num - 1];
            child.num += 1;
            sibling.num -= 1;
        }

        // Симметричный с заимствованиемFromPrev
        public void borrowFromNext(int idx)
        {

            BTreeNode child = children[idx];
            BTreeNode sibling = children[idx + 1];

            child.keys[child.num] = keys[idx];

            if (!child.isLeaf)
                child.children[child.num + 1] = sibling.children[0];

            keys[idx] = sibling.keys[0];

            for (int i = 1; i < sibling.num; ++i)
                sibling.keys[i - 1] = sibling.keys[i];

            if (!sibling.isLeaf)
            {
                for (int i = 1; i <= sibling.num; ++i)
                    sibling.children[i - 1] = sibling.children[i];
            }
            child.num += 1;
            sibling.num -= 1;
        }

        // объединить childre [idx + 1] в childre [idx]
        public void merge(int idx)
        {

            BTreeNode child = children[idx];
            BTreeNode sibling = children[idx + 1];

            // Вставляем последний ключ текущего узла в позицию MinDeg-1 дочернего узла
            child.keys[MinDeg - 1] = keys[idx];

            // ключи: children [idx + 1] скопированы в children [idx]
            for (int i = 0; i < sibling.num; ++i)
                child.keys[i + MinDeg] = sibling.keys[i];

            // children: children [idx + 1] скопированы в children [idx]
            if (!child.isLeaf)
            {
                for (int i = 0; i <= sibling.num; ++i)
                    child.children[i + MinDeg] = sibling.children[i];
            }

            // Перемещаем клавиши вперед, а не зазор, вызванный перемещением ключей [idx] к дочерним [idx]
            for (int i = idx + 1; i < num; ++i)
                keys[i - 1] = keys[i];
            // Перемещаем соответствующий дочерний узел вперед
            for (int i = idx + 2; i <= num; ++i)
                children[i - 1] = children[i];

            child.num += sibling.num + 1;
            num--;
        }


        public void insertNotFull(int key, string value)
        {

            int i = num - 1; // Инициализируем i индексом самого правого значения

            if (isLeaf)
            { // Когда это листовой узел
              // Находим, куда нужно вставить новый ключ
                while (i >= 0 && keys[i].key > key)
                {
                    keys[i + 1] = keys[i]; // клавиши возвращаются
                    i--;
                }
                keys[i + 1].key = key;
                keys[i + 1].value = value;
                num = num + 1;
            }
            else
            {
                // Находим позицию дочернего узла, который нужно вставить
                while (i >= 0 && keys[i].key > key)
                    i--;
                if (children[i + 1].num == 2 * MinDeg - 1)
                { // Когда дочерний узел заполнен
                    splitChild(i + 1, children[i + 1]);
                    // После разделения ключ в середине дочернего узла перемещается вверх, а дочерний узел разделяется на два
                    if (keys[i + 1].key < key)
                        i++;
                }
                children[i + 1].insertNotFull(key, value);
            }
        }


        public void splitChild(int i, BTreeNode y)
        {

            // Сначала создаем узел, содержащий ключи MinDeg-1 y
            BTreeNode z = new BTreeNode(y.MinDeg, y.isLeaf);
            z.num = MinDeg - 1;

            // Передаем все атрибуты y в z
            for (int j = 0; j < MinDeg - 1; j++)
                z.keys[j] = y.keys[j + MinDeg];
            if (!y.isLeaf)
            {
                for (int j = 0; j < MinDeg; j++)
                    z.children[j] = y.children[j + MinDeg];
            }
            y.num = MinDeg - 1;

            // Вставляем новый дочерний узел в дочерний узел
            for (int j = num; j >= i + 1; j--)
                children[j + 1] = children[j];
            children[i + 1] = z;

            // Перемещаем ключ по y к этому узлу
            for (int j = num - 1; j >= i; j--)
                keys[j + 1] = keys[j];
            keys[i] = y.keys[MinDeg - 1];

            num = num + 1;
        }


        public void traverse()
        {
            int i;
            for (i = 0; i < num; i++)
            {
                if (!isLeaf)
                    children[i].traverse();
                Console.Write($" {keys[i].key}-{keys[i].value}");
            }

            if (!isLeaf)
            {
                children[i].traverse();
            }
        }

        public string tree_to_string()
        {            
            string s="";
            int i;
            for (i = 0; i < num; i++)
            {
                if (!isLeaf)
                    s+=children[i].tree_to_string();
                s+= keys[i].key + "@" + keys[i].value + "@";
            }

            if (!isLeaf)
            {
                s+=children[i].tree_to_string();
            }
            return s;
        }
        public BTreeNode search(int key)
        {
            int i = 0;
            while (i < num && key > keys[i].key)
                i++;

            if (keys[i].key == key)
                return this;
            if (isLeaf)
                return null;
            return children[i].search(key);
        }
        public string poshuk(int key)
        {
            chislo_proidenih_vuzliv++;
            int left = -1;
            int right = num;
            while (left < right - 1)
            {
                int mid = (left + right) / 2;
                if (keys[mid].key < key)
                {
                    left = mid;
                }
                else
                {
                    right = mid;
                }
            }
            if (right < keys.Length)
                if (keys[right].key == key) return keys[right].value;
            if (isLeaf) return key + " не знайдено";
            else return children[right].poshuk(key);
        }
    }


    class BTree
    {
        public BTreeNode root;
        int MinDeg;

        // Конструктор
        public BTree(int deg)
        {
            this.root = null;
            this.MinDeg = deg;
        }

        public void traverse()
        {
            if (root != null)
            {
                root.traverse();
            }
        }

        // Функция для поиска ключа
        public BTreeNode search(int key)
        {
            BTreeNode.chislo_proidenih_vuzliv = 0;
            return root == null ? null : root.search(key);           
        }

        public void insert(int key, string value)
        {

            if (root == null)
            {

                root = new BTreeNode(MinDeg, true);
                root.keys[0].key = key;
                root.keys[0].value = value;
                root.num = 1;
            }
            else
            {
                // Когда корневой узел заполнится, дерево станет выше
                if (root.num == 2 * MinDeg - 1)
                {
                    BTreeNode s = new BTreeNode(MinDeg, false);
                    // Старый корневой узел становится дочерним узлом нового корневого узла
                    s.children[0] = root;
                    // Отделяем старый корневой узел и даем ключ новому узлу
                    s.splitChild(0, root);
                    // Новый корневой узел имеет 2 дочерних узла, переместите туда старый корневой узел
                    int i = 0;
                    if (s.keys[0].key < key)
                        i++;
                    s.children[i].insertNotFull(key,value);

                    root = s;
                }
                else
                    root.insertNotFull(key,value);
            }
        }

        public void remove(int key)
        {
            if (root == null)
            {
                Console.WriteLine("The tree is empty");
                return;
            }

            root.remove(key);

            if (root.num == 0)
            { // Если у корневого узла 0 ключей
              // Если у него есть дочерний узел, используйте его первый дочерний узел как новый корневой узел,
              // В противном случае установите корневой узел в ноль
                if (root.isLeaf)
                    root = null;
                else
                    root = root.children[0];
            }
        }
        public string poshuk(int pkey)
        {
            BTreeNode.chislo_proidenih_vuzliv = 0;
            if (root != null) return root.poshuk(pkey);
            else return "не знайдено";

        }
        public void redakt(int pkey, string val)
        {
            if (search(pkey) != null)
            {
                remove(pkey);
                insert(pkey, val);
                Console.WriteLine($"Змiнено ключ {pkey}: value = {poshuk(pkey)}");
            }
            else Console.WriteLine($"key={pkey} not found! ");       
        }
        public void save()
        {
            if (root != null)
            {
                string fname = "data.txt";                
                using (StreamWriter sw = new StreamWriter(fname, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(root.tree_to_string());
                }
            }
        }
        public void load()
        {
            string fname = "data.txt";
            string s = "";
            using (StreamReader sr = new StreamReader(fname))
            {
                s=sr.ReadToEnd();
                string[] dataS = s.Split("@");
                root = null;
                for (int i = 0; i < dataS.Length-1; i += 2) insert(Int32.Parse(dataS[i]), dataS[i + 1]);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            BTree t = new BTree(25); // A B-Tree with minium degree 2
            //t.insert(1, "v1");
            //t.insert(3, "v3");
            //t.insert(7, "v7");
            //t.insert(10, "v10");
            //t.insert(11, "v11");
            //t.insert(13, "v13");
            //t.insert(14, "v14");
            //t.insert(15, "v15");
            //t.insert(18, "v18");
            //t.insert(16, "v16");
            //t.insert(19, "v19");
            //t.insert(24, "v24");
            //t.insert(25, "v25");
            //t.insert(26, "v26");
            //t.insert(21, "v21");
            //t.insert(4, "v4");
            //t.insert(5, "v5");
            //t.insert(20, "v20");
            //t.insert(22, "v22");
            //t.insert(2, "v2");
            //t.insert(17, "v17");
            //t.insert(12, "v12");
            //t.insert(6, "v6");

            //Console.WriteLine("Traversal of tree constructed is");
            //t.traverse();
            //Console.WriteLine();

            //t.remove(6);
            //Console.WriteLine("Traversal of tree after removing 6");
            //t.traverse();
            //Console.WriteLine();

            //t.remove(13);
            //Console.WriteLine("Traversal of tree after removing 13");
            //t.traverse();
            //Console.WriteLine();

            //t.remove(7);
            //Console.WriteLine("Traversal of tree after removing 7");
            //t.traverse();
            //Console.WriteLine();

            //t.remove(4);
            //Console.WriteLine("Traversal of tree after removing 4");
            //t.traverse();
            //Console.WriteLine();

            //t.remove(2);
            //Console.WriteLine("Traversal of tree after removing 2");
            //t.traverse();
            //Console.WriteLine();

            //t.remove(16);
            //Console.WriteLine("Traversal of tree after removing 16");
            //t.traverse();
            //Console.WriteLine();

            //int pkey = 15;
            //Console.WriteLine($"Пошук ключ {pkey}: value = {t.poshuk(pkey)}");

            //t.redakt(15, "hi");

            //Console.WriteLine("Traversal of tree after removing 16");
            //t.traverse();
            //Console.WriteLine();
            ////t.save();
            ////t.load();
            //t.traverse();
            //Console.WriteLine();
            var rand = new Random();
            Console.WriteLine("Додав в бi-дерево (з t=25) 10 000 ключiв i значень. Пошук 10 випадкових ключив:");
            for (int i = 0; i < 10000; i++) t.insert(i, "v" + i);
            for (int i = 0; i < 10; i++) {
                int k = rand.Next(10000);
                Console.WriteLine("Пошук ключа "+k + " Знайшов значенння: " + t.poshuk(k) + " Число пройдених вузлiв: " + BTreeNode.chislo_proidenih_vuzliv); 
            }
        }
    }
}
