using System;
using System.Collections.Generic;
using System.Text;
namespace lab6
{
    class karta
    {
        public int nominal;
        public int mast;
        public string nazva;
        public int tsinnist;
        public bool joker;
        public karta(int mast_, int nominal_)
        {
            mast = mast_;
            nominal = nominal_;
            gen_nazvi();
            if (nominal == 15) joker = true;
            else joker = false;
        }
        public void gen_nazvi()
        {
            if ((nominal > 1) && (nominal < 11))
            {
                nazva += nominal;
            }
            else
                switch (nominal)
                {
                    case 11:
                        nazva = "В";
                        break;
                    case 12:
                        nazva = "Д";
                        break;
                    case 13:
                        nazva = "К";
                        break;
                    case 14:
                        nazva = "Т";
                        break;
                    case 15:
                        nazva = "Дж";
                        break;
                }
            switch (mast)
            {
                case 1:
                    nazva += "♥"; // '\u2665'
                    break;
                case 2:
                    nazva += "♦";
                    break;
                case 3:
                    nazva += "♣";
                    break;
                case 4:
                    nazva += "♠";
                    break;
            }
        }
        public void vivid()
        {
            Console.Write(nazva + " ");
        }
        public void rozstanovka_tsinnostey(int kilkist_kart_gravtsa)
        {
            switch (nominal)
            {
                case 2: tsinnist = 1;
                    break;
                case 3:
                    tsinnist = 2;
                    break;
                case 4:
                    tsinnist = 4;
                    break;
                case 5:
                    tsinnist = 3;
                    break;
                case 6:
                    tsinnist = 5;
                    break;
                case 7:
                    tsinnist = 5;
                    break;
                case 8:
                    tsinnist = 6;
                    break;
                case 9:
                    tsinnist = 5;
                    break;
                case 10:
                    tsinnist = 7;
                    break;
                case 11:
                    tsinnist = 8;
                    break;
                case 12:
                    tsinnist = 7;
                    break;
                case 13:
                    tsinnist = 7;
                    break;
                case 14:
                    tsinnist = 8;
                    break;
                case 15:
                    tsinnist = 4;
                    break;
            }
            if (kilkist_kart_gravtsa <= 2)
            {
                switch (nominal)
                {
                    case 15:
                        tsinnist = 10;
                        break;
                    case 8:
                        tsinnist = 9;
                        break;
                }
            }
        }
    }
    class gra
    {
        public int skladnist;
        public List<karta> koloda;
        public List<karta> ruka_kompa;
        public List<karta> ruka_gravtsa;
        public List<int> spetsefekti = new List<int> {2,3,4,8,11,15};
        public karta karta_na_stoli;
        public bool osoblivi_pravila=false;
        public bool end_game = false;
        public int ochki_gravtsa = 0;
        public int ochki_compa = 0;
        public gra()
        {
            ochki_compa = 0;
            ochki_gravtsa = 0;
            vibir_skladnosti();
            while (ochki_gravtsa<30 && ochki_compa<30)
            {

                osoblivi_pravila = false;
                generuem_kolodu();
                pochatkova_rozdacha();
                karta_na_stoli = null;
                end_game = false;
                while (!end_game)
                {
                    hid_gravtsa();
                    hid_kompa();
                }
                pidrahunok_ochkiv();
                Console.WriteLine($"результат гри: очки гравця {ochki_gravtsa}, очки компа {ochki_compa}");
            }
            Console.WriteLine("кінець гри");
        }
        public void vibir_skladnosti()
        {
            Console.Write("Вибери складність (введи число, скільки карт компа закрито від тебе) 0-10:");
            skladnist = int.Parse(Console.ReadLine());
        }
        public void pidrahunok_ochkiv()
        {
            for (int i = 0; i < ruka_gravtsa.Count; i++)
            {
                if (ruka_gravtsa[i].nominal >= 10 && ruka_gravtsa[i].nominal <= 13) ochki_gravtsa += 10;
                if (ruka_gravtsa[i].nominal == 14 ) ochki_gravtsa += 15;
                if (ruka_gravtsa[i].nominal == 8  ) ochki_gravtsa += 25;
                if (ruka_gravtsa[i].nominal == 15 ) ochki_gravtsa += 40;
            }
            for (int i = 0; i < ruka_kompa.Count; i++)
            {
                if (ruka_kompa[i].nominal >= 10 && ruka_kompa[i].nominal <= 13) ochki_compa += 10;
                if (ruka_kompa[i].nominal == 14) ochki_compa += 15;
                if (ruka_kompa[i].nominal == 8) ochki_compa += 25;
                if (ruka_kompa[i].nominal == 15) ochki_compa += 40;
            }
        }
        public void generuem_kolodu()
        {
            koloda = new List<karta>();
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 2; j <= 14; j++)
                {
                    karta k1 = new karta(i, j);
                    koloda.Add(k1);
                }
            }
            karta k2 = new karta(5, 15);
            koloda.Add(k2);
            k2 = new karta(5, 15);
            koloda.Add(k2);
        }
        public void vivid_spisku(List<karta> spisok, int k=0)
        {
            for (int i = 0; i < spisok.Count; i++)
            {
                if (i >= k) spisok[i].vivid();
                else Console.Write("?? ");
            }
        }
        
        public void pochatkova_rozdacha()
        {
            ruka_gravtsa = new List<karta>();
            ruka_kompa = new List<karta>();
            for (int i = 0; i < 4; i++)
            {
                igrok_bere_kartu();
                komp_beret_kartu();
            }
        }
        public void vivid_stanu()
        {
            Console.Write("\n\nрука компа: ");
            vivid_spisku(ruka_kompa, skladnist);
            Console.Write("\nкарта на столі: ");
            if (karta_na_stoli != null) karta_na_stoli.vivid();
            Console.Write("\nрука гравця: ");
            vivid_spisku(ruka_gravtsa);
            //Console.Write("\nколода: ");
            //vivid_spisku(koloda);
        }
        public void igrok_bere_kartu()
        {
            Random ran = new Random();
            int n = ran.Next(koloda.Count);
            karta kar1 = koloda[n];
            ruka_gravtsa.Add(kar1);
            koloda.RemoveAt(n);
        }
        public void komp_beret_kartu()
        {
            Random ran = new Random();
            int n = ran.Next(koloda.Count);
            karta kar1 = koloda[n];
            ruka_kompa.Add(kar1);
            koloda.RemoveAt(n);
        }
        public void hid_gravtsa_kartoi(int i)
        {
            if (karta_na_stoli.joker) karta_na_stoli = new karta(5, 15);
            koloda.Add(karta_na_stoli);
            karta_na_stoli = ruka_gravtsa[i];
            ruka_gravtsa.RemoveAt(i);
        }

        public void hid_gravtsa()
        {
            if (ruka_gravtsa.Count == 0)
            {
                end_game = true;
                Console.WriteLine("Переміг гравець");
                return;
            }
            if (ruka_kompa.Count == 0)
            {
                end_game = true;
                Console.WriteLine("Переміг комп");
                return;
            }
            vivid_stanu();
            List<int> mojlivi_hodi_gravtsa = new List<int>();
            int hid;
            if (karta_na_stoli == null)
            {
                Console.Write($"\nвибери номер карти від 1 до {ruka_gravtsa.Count}: ");
                hid = int.Parse(Console.ReadLine()) - 1;
                karta_na_stoli = ruka_gravtsa[hid];
                ruka_gravtsa.RemoveAt(hid);
                if (karta_na_stoli.nominal == 15)
                {
                    Console.Write("\nвведи масть джокера 1-♥ 2-♦ 3-♣ 4-♠ : ");
                    karta_na_stoli.mast = int.Parse(Console.ReadLine());
                    Console.Write("\nвведи номінал джокера 2 3 4 5 6 7 8 9 10 11-В 12-Д 13-К 14-Т: ");
                    karta_na_stoli.nominal = int.Parse(Console.ReadLine());

                    string s =""+karta_na_stoli.nominal;
                    switch (karta_na_stoli.mast)
                    {
                        case 1:
                            s += "♥"; // '\u2665'
                            break;
                        case 2:
                            s += "♦";
                            break;
                        case 3:
                            s += "♣";
                            break;
                        case 4:
                            s += "♠";
                            break;
                    }
                    karta_na_stoli.nazva += s;
                }
                if (spetsefekti.Contains(karta_na_stoli.nominal))
                {
                    osoblivi_pravila = true;
                }
                if (karta_na_stoli.nominal == 3)
                {
                    hid_gravtsa();
                }
                if (karta_na_stoli.nominal == 8)
                {
                    Console.Write("\nвибери масть: 1-♥ 2-♦ 3-♣ 4-♠ :");
                    karta_na_stoli.mast = int.Parse(Console.ReadLine());
                    osoblivi_pravila = false;
                }                
            }
            else
            {
                if (osoblivi_pravila==false)
                {
                    for (int i = 0; i < ruka_gravtsa.Count; i++)
                    {
                        if (
                            ruka_gravtsa[i].nominal == 15 || ruka_gravtsa[i].nominal == 8 ||
                            ruka_gravtsa[i].nominal!=4 && (ruka_gravtsa[i].nominal == karta_na_stoli.nominal || ruka_gravtsa[i].mast == karta_na_stoli.mast) ||
                            ruka_gravtsa[i].nominal == 4 && ruka_gravtsa[i].mast == karta_na_stoli.mast && (karta_na_stoli.nominal==5 || karta_na_stoli.nominal == 6 || karta_na_stoli.nominal == 7)
                            )
                        {
                            mojlivi_hodi_gravtsa.Add(i);
                        }
                    }
                }
                else
                {
                    if (karta_na_stoli.nominal == 4)
                    {
                        for (int i = 0; i < ruka_gravtsa.Count; i++)
                        {
                            if (ruka_gravtsa[i].nominal==15 || ruka_gravtsa[i].nominal==5 && ruka_gravtsa[i].mast == karta_na_stoli.mast)
                            {
                                mojlivi_hodi_gravtsa.Add(i);
                            }
                        }
                    }
                    if (karta_na_stoli.nominal==3)
                    {
                        for (int i = 0; i < ruka_gravtsa.Count; i++) mojlivi_hodi_gravtsa.Add(i);
                        osoblivi_pravila = false;
                    }
                }

                if (mojlivi_hodi_gravtsa.Count == 0)
                    if (karta_na_stoli.nominal==4 && osoblivi_pravila == true)
                    {
                        igrok_bere_kartu();
                        igrok_bere_kartu();
                        igrok_bere_kartu();
                        igrok_bere_kartu();
                        osoblivi_pravila = false;
                        return;
                    } 
                    else
                    {
                        Console.WriteLine("Гравцю нічим ходити, він бере карту і пропускає хід");
                        igrok_bere_kartu();
                        return;
                    }
                hid = -1;
                while (!mojlivi_hodi_gravtsa.Contains(hid))
                {
                    Console.Write($"\nвибери номер карти зі списку ");
                    for (int i = 0; i < mojlivi_hodi_gravtsa.Count; i++) Console.Write((mojlivi_hodi_gravtsa[i]+1) + " ");
                    Console.Write(": ");
                    hid = int.Parse(Console.ReadLine()) - 1;
                }
                if (new List<int> { 2, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14 }.Contains(ruka_gravtsa[hid].nominal))
                {
                    hid_gravtsa_kartoi(hid);
                    if (new List<int> { 2, 4, 11 }.Contains(karta_na_stoli.nominal)) osoblivi_pravila = true;
                    return;
                }
                if (ruka_gravtsa[hid].nominal == 3)
                {
                    hid_gravtsa_kartoi(hid);
                    osoblivi_pravila = true;
                    hid_gravtsa();
                    // dopisat
                    return;
                }
                if (ruka_gravtsa[hid].nominal == 8)
                {
                    hid_gravtsa_kartoi(hid);
                    Console.Write("\nвибери масть: 1-♥ 2-♦ 3-♣ 4-♠ :");
                    karta_na_stoli.mast = int.Parse(Console.ReadLine());
                    return;
                }
                if (ruka_gravtsa[hid].nominal == 15)
                {
                    if (karta_na_stoli.nominal == 4)
                    {
                        int mast = karta_na_stoli.mast;
                        hid_gravtsa_kartoi(hid);
                        karta_na_stoli.nominal = 5;
                        karta_na_stoli.mast = mast;
                        string s = "-5";
                        switch (mast)
                        {
                            case 1:
                                s += "♥"; // '\u2665'
                                break;
                            case 2:
                                s += "♦";
                                break;
                            case 3:
                                s += "♣";
                                break;
                            case 4:
                                s += "♠";
                                break;
                        }
                        karta_na_stoli.nazva += s;
                    }
                    else
                    {
                        Console.Write("\nвведи масть джокера 1-♥ 2-♦ 3-♣ 4-♠ : ");
                        ruka_gravtsa[hid].mast = int.Parse(Console.ReadLine());
                        Console.Write("\nвведи номінал джокера 2 3 4 5 6 7 8 9 10 11-В 12-Д 13-К 14-Т: ");
                        ruka_gravtsa[hid].nominal = int.Parse(Console.ReadLine());
                        string s = "-"+ ruka_gravtsa[hid].nominal;
                        switch (ruka_gravtsa[hid].mast)
                        {
                            case 1:
                                s += "♥"; // '\u2665'
                                break;
                            case 2:
                                s += "♦";
                                break;
                            case 3:
                                s += "♣";
                                break;
                            case 4:
                                s += "♠";
                                break;
                        }
                        ruka_gravtsa[hid].nazva += s;


                        hid_gravtsa();
                    }
                }
            }
        }
        public void hid_kompa_kartoi(int i)
        {
            if (karta_na_stoli.joker) karta_na_stoli = new karta(5, 15);
            koloda.Add(karta_na_stoli);
            karta_na_stoli = ruka_kompa[i];
            ruka_kompa.RemoveAt(i);
        }
        public int vibor_masti()
        {
            List<int> mast = new List<int> { 0, 0, 0, 0 };
            for (int i=0; i < ruka_kompa.Count; i++)
            {
                if (ruka_kompa[i].mast<5)
                mast[ruka_kompa[i].mast-1]++;
            }
            int max_i_mast = 0;
            for (int i = 0; i < 4; i++) if (mast[i] > mast[max_i_mast]) max_i_mast = i;
            Console.Write("\nКомп вибрав масть ");
            switch (max_i_mast + 1)
            {
                case 1:
                    Console.WriteLine("♥"); // '\u2665'
                    break;
                case 2:
                    Console.WriteLine("♦");
                    break;
                case 3:
                    Console.WriteLine("♣");
                    break;
                case 4:
                    Console.WriteLine("♠");
                    break;
            }

            return max_i_mast+1;
        }
        public void hid_kompa_jokerom(int i)
        {
            if(karta_na_stoli.nominal==5 || karta_na_stoli.nominal == 6 || karta_na_stoli.nominal == 7)
            {
                int mast = karta_na_stoli.mast;
                hid_kompa_kartoi(i);
                karta_na_stoli.nominal = 4;
                karta_na_stoli.mast = mast;
                osoblivi_pravila = true;
                string s="-4";
                switch (mast)
                {
                    case 1:
                        s+="♥"; // '\u2665'
                        break;
                    case 2:
                        s+="♦";
                        break;
                    case 3:
                        s += "♣";
                        break;
                    case 4:
                        s+="♠";
                        break;
                }
                karta_na_stoli.nazva += s;
            }            
            else
            {
                int mast = karta_na_stoli.mast;
                hid_kompa_kartoi(i);
                karta_na_stoli.nominal = 3;
                karta_na_stoli.mast = mast;
                osoblivi_pravila = true;
                string s = "-3";
                switch (mast)
                {
                    case 1:
                        s += "♥"; // '\u2665'
                        break;
                    case 2:
                        s += "♦";
                        break;
                    case 3:
                        s += "♣";
                        break;
                    case 4:
                        s += "♠";
                        break;
                }
                karta_na_stoli.nazva += s;
                hid_kompa();
            }
        }
        public void hid_kompa()
        {
            vivid_stanu();
            if (ruka_gravtsa.Count == 0)
            {
                end_game = true;
                Console.WriteLine("Переміг гравець");
                return;
            }
            if (ruka_kompa.Count == 0)
            {
                end_game = true;
                Console.WriteLine("Переміг комп");
                return;
            }
            for (int i = 0; i < ruka_kompa.Count; i++)
            {
                ruka_kompa[i].rozstanovka_tsinnostey(ruka_gravtsa.Count);
            }
            ruka_kompa.Sort((x, y) => y.tsinnist - x.tsinnist);
            //vivid_stanu();
            if (osoblivi_pravila == false)
            {
                int i = 0;
                for (; i < ruka_kompa.Count; i++)
                {
                    if ((ruka_kompa[i].mast == karta_na_stoli.mast || ruka_kompa[i].nominal == karta_na_stoli.nominal)&&(!spetsefekti.Contains(ruka_kompa[i].nominal)))
                    {
                        hid_kompa_kartoi(i);
                        return;
                    }
                    if(ruka_kompa[i].nominal==2 && (ruka_kompa[i].mast == karta_na_stoli.mast || ruka_kompa[i].nominal == karta_na_stoli.nominal))
                    {
                        hid_kompa_kartoi(i);
                        igrok_bere_kartu();
                        hid_kompa();
                        return;
                    }
                    if (ruka_kompa[i].nominal == 3 && (ruka_kompa[i].mast == karta_na_stoli.mast || ruka_kompa[i].nominal == karta_na_stoli.nominal))
                    {
                        hid_kompa_kartoi(i);
                        osoblivi_pravila = true;
                        hid_kompa();
                        return;
                    }
                    if (ruka_kompa[i].nominal == 4 && (karta_na_stoli.nominal==5 || karta_na_stoli.nominal == 6 || karta_na_stoli.nominal == 7) && ruka_kompa[i].mast == karta_na_stoli.mast)
                    {
                        hid_kompa_kartoi(i);
                        osoblivi_pravila = true;
                        return;
                    }
                    if (ruka_kompa[i].nominal == 8)
                    {
                        hid_kompa_kartoi(i);
                        karta_na_stoli.mast=vibor_masti();
                        //karta_na_stoli.nazva +="-"+karta_na_stoli.mast;
                        return;
                    }
                    if (ruka_kompa[i].nominal == 11 && (ruka_kompa[i].mast == karta_na_stoli.mast || ruka_kompa[i].nominal == karta_na_stoli.nominal))
                    {
                        hid_kompa_kartoi(i);
                        hid_kompa();
                        return;
                    }
                    if (ruka_kompa[i].nominal == 15)
                    {
                        hid_kompa_jokerom(i);
                        return;
                    }
                }
                if (i>=ruka_kompa.Count)
                {
                    komp_beret_kartu();
                }
            }
            else
            {
                osoblivi_pravila = false;
                if (karta_na_stoli.nominal == 2)
                {
                    komp_beret_kartu();
                    return;
                }
                if (karta_na_stoli.nominal == 3)
                {
                    if (!spetsefekti.Contains(ruka_kompa[0].nominal))
                    {
                        hid_kompa_kartoi(0);
                        return;
                    }
                    else
                    {
                        if(ruka_kompa[0].nominal==2)
                        {
                            hid_kompa_kartoi(0);
                            igrok_bere_kartu();
                            hid_kompa();
                            return;
                        }
                        if (ruka_kompa[0].nominal == 3)
                        {
                            hid_kompa_kartoi(0);
                            osoblivi_pravila = true;
                            hid_kompa();
                            return;
                        }
                        if (ruka_kompa[0].nominal == 4)
                        {
                            hid_kompa_kartoi(0);
                            osoblivi_pravila = true;
                            return;
                        }
                        if (ruka_kompa[0].nominal == 8)
                        {
                            hid_kompa_kartoi(0);
                            karta_na_stoli.mast = vibor_masti();
                            return;
                        }
                        if (ruka_kompa[0].nominal == 11)
                        {
                            hid_kompa_kartoi(0);
                            hid_kompa();
                            return;
                        }
                        if (ruka_kompa[0].nominal == 15)
                        {
                            hid_kompa_jokerom(0);
                            return;
                        }
                        // treba dopisat
                    }
                }
                if (karta_na_stoli.nominal == 4)
                {
                    for (int i = 0; i < ruka_kompa.Count; i++)
                    {
                        if (ruka_kompa[i].nominal==5&&ruka_kompa[i].mast==karta_na_stoli.mast)
                        {
                            hid_kompa_kartoi(i);
                            return;
                        }
                        else if (ruka_kompa[i].nominal == 15)
                        {
                            int mast = karta_na_stoli.mast;
                            hid_kompa_kartoi(i);
                            karta_na_stoli.mast = mast;
                            karta_na_stoli.nominal = 5;
                            string s = "-5";
                            switch (mast)
                            {
                                case 1:
                                    s += "♥"; // '\u2665'
                                    break;
                                case 2:
                                    s += "♦";
                                    break;
                                case 3:
                                    s += "♣";
                                    break;
                                case 4:
                                    s += "♠";
                                    break;
                            }
                            karta_na_stoli.nazva += s;
                            return;
                        }                        
                    }
                    for (int j = 0; j < 4; j++) komp_beret_kartu();
                    osoblivi_pravila = false;
                    return;
                }
                if (karta_na_stoli.nominal == 11)
                {
                    return;
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.OutputEncoding = Encoding.UTF8;
            gra g = new gra();
        }
    }
}
