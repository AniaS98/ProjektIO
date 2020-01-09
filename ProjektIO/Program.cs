using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjektIO
{//Tworzę pomocniczą klasę, która przyda się do zapisu danych przetwarzanych w Tabu Search
    public class Tabu
    {
        int a;
        int b;
        int licznik;
        public int A { get => a; set => a = value; }
        public int B { get => b; set => b = value; }
        public int Licznik { get => licznik; set => licznik = value; }
        public Tabu()
        {
            a = 0;
            b = 0;
            licznik = 0;
        }
        public Tabu(int a, int b, int licznik)
        {
            this.a = a;
            this.b = b;
            this.licznik = licznik;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(a.ToString());
            sb.Append(b.ToString());
            sb.Append(licznik.ToString());
            return sb.ToString();
        }
    }

   
    class Program
    {
        static int[,] Losowanie(int[,] data, int[,] next)//Losowanie rozwiązania początkowego
        {
            int j = 0;
            int zamiana = 0;
            Random rnd = new Random();
            List<int> mieszalnik = new List<int>();
            List<int> lista = new List<int>();
            for (int i = 0; i < 200; i++)
                lista.Add(i);
            for (int i = 0; i < 200; i++)
            {
                zamiana = rnd.Next(0, lista.Count);
                mieszalnik.Add(lista[zamiana]);
                lista.RemoveAt(zamiana);
            }
            for (int i = 0; i < 200; i++)
            {
                for (j = 0; j < 3; j++)
                    next[mieszalnik[i], j] = data[i, j];
            }
            return next;
        }

        static void Main(string[] args)
        {
            //Zczytywanie danych +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            string text = System.IO.File.ReadAllText("Dane.csv");
            String[,] Tablica = new String[202, 3];
            int j = 0;
            int k = 0;
            int suma = 0;
            int[,] data = new int[200, 5];
            int czas = 0;
            for (int i = 0; i < 200; i++)
            {
                for (int e = 0; e < 3; e++)
                    Tablica[i, e] = "";
            }
            for (int i = 31; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    k++;
                    j = 0;
                }
                if (text[i] == ',')
                    j++;
                else
                    Tablica[k, j] += text[i];
            }
            for (int i = 0; i < 200; i++)
            {
                for (int e = 0; e < 3; e++)
                {
                    data[i, e] = Int32.Parse(Tablica[i, e]);
                }
                czas += data[i, 1];
                data[i, 3] = czas;
                data[i, 4] = (data[i, 2] - data[i, 3]) * (data[i, 2] - data[i, 3]);
                suma += data[i, 4];
            }
            //Koniec danych ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            string odp;
            Console.WriteLine("Wybierz numer opcji danej metody:");
            Console.WriteLine("1. Wyżarzanie");
            Console.WriteLine("2. Algorytm Wspinaczkowy");
            Console.WriteLine("3. Tabu Search");
            odp = Console.ReadLine();
            switch (odp)
            {
                case "1":
                    {
                        Console.WriteLine("Wyżarzanie");
                        Wyzarzanie(data, suma, czas);
                        break;
                    }
                case "2":
                    {
                        Console.WriteLine("Algorytm Wspinaczkowy");
                        Wspinaczka(data, suma, czas);
                        break;
                    }
                case "3":
                    {
                        Console.WriteLine("Tabu Search");
                        TabuSearch(data, suma, czas);
                        break;
                    }
            }
            Console.ReadKey();
        }
        static int Findmax(List<Tabu> tab)//Funkcja pomocnicza do TabuSearch znajdująca najgorszy element listy top najlepszych rozwiązań
        {
            int iter = 0;
            int max = tab[0].Licznik;
            for (int i=1;i<5;i++)
            {
                if (max < tab[i].Licznik)
                {
                    max = tab[i].Licznik;
                    iter = i;
                }     
            }
            return iter;
        }
        static void TabuSearch(int[,] data, int suma, int czas)
        {
            int j = 0;
            Tabu t = new Tabu();
            int iterator = 0;
            int[,] next = new int[200, 5];
            Random rnd = new Random();
            Queue<Tabu> lista = new Queue<Tabu>();
            int current = suma;
            List<Tabu> top = new List<Tabu>();
            next=Losowanie(data, next);//Losowanie rozwiązania początkowego
            suma = 0;
            czas = 0;
            for (int i = 0; i < 200; i++)//Obliczanie początkowej sumy odchyleń dla rozwiązania początkowego
            {
                for (j = 0; j < 3; j++)
                {
                    data[i, j] = next[i, j];
                }
                czas += data[i, 1];
                data[i, 3] = czas;
                data[i, 4] = (data[i, 2] - data[i, 3]) * (data[i, 2] - data[i, 3]);
                suma += data[i, 4];
            }
            current = suma;
            //Część główna
            for (int h = 0; h <= 2000; h++)//Zwiększenie wartości h polepszy końcowy wynik
            { //Usuwanie przedawnionych zamian z listy Tabu (od drugiej iteracji)
                top = new List<Tabu>();
                bool outcome = false;
                iterator = 0;
                Tabu result = new Tabu();
                if (h > 0)
                {
                    Tabu pomoc = lista.Peek();
                    if (pomoc.Licznik == 0)
                    {
                        lista.Dequeue();
                        pomoc = lista.Peek();
                    }
                }
                for (int x = 0; x < 200; x++)//Analiza poszczególnych przypadków
                {
                    iterator = 0;
                    for (int y = x + 1; y < 200; y++)
                    {
                        //przypisywanie wartości z pierwotnej tablicy do nowej tablicy
                        for (int i = 0; i < 200; i++)
                        {
                            for (j = 0; j < 3; j++)
                                next[i, j] = data[i, j];
                        }
                        //Zamiana wartości w nowej tablicy dla indeksów x i y
                        for (j = 0; j < 3; j++)
                        {
                            next[x, j] = data[y, j];
                            next[y, j] = data[x, j];
                        }
                        //Liczenie nowej wartości sumy odchyleń
                        suma = 0;
                        czas = 0;
                        for (int i = 0; i < 200; i++)
                        {
                            czas += next[i, 1];
                            next[i, 3] = czas;
                            next[i, 4] = (next[i, 2] - next[i, 3]) * (next[i, 2] - next[i, 3]);
                            suma += next[i, 4];
                        }
                        Console.WriteLine("Iteracja: " + h + " Pierwszy indeks: " + x + " Wynik pośredni: " + suma);
                        //Tworzenie listy top 5 najlepszych rozwiązań
                        if (iterator < 5)//Najpierw lista uzupełniana jest 5 pierwszymi rozwiązaniami
                        {
                            t = new Tabu(x, y, suma);
                            top.Add(t);
                            iterator++;
                        }
                        else
                        {
                            if (top[Findmax(top)].Licznik > suma)//Jeżeli obecna suma jest mniejsza od najgorszego rozwiązania z listy top to w miejsce najgorzego wyniku wpisywana jest obecna suma
                                top[Findmax(top)] = new Tabu(x, y, suma);
                                iterator++;
                        }
                    }
                }
                //Wybór rozwiązania
                if (lista.Count > 0)
                {//Sprawdzenie, czy najlepsze rozwiązanie z top nie pojawiło się na liście tabu
                    for (int i = 0; outcome == false || i < 5; i++)
                    {
                        t.A = top[i].A;
                        t.B = top[i].B;
                        iterator = 0;
                        for (j = 1; j <= 3; j++)
                        {
                            t.Licznik = j;
                            if (lista.Contains(t) == false)
                            {
                                iterator++;
                            }
                        }
                        if (iterator == 3)
                        {//Zapisanie najlepszego dozwolonego wyniku na listę Tabu
                            result = t;
                            result.Licznik = 4;
                            lista.Enqueue(result);
                            outcome = true;
                        }

                    }
                }
                else
                {//Zapisanie najlepszego wyniku na listę Tabu
                    result.A = top[0].A;
                    result.B = top[0].B;
                    result.Licznik = 4;
                    lista.Enqueue(result);
                    outcome = true;
                }
                //Zamiana indeksów w nowej tablicy na wartości z najlepszego wyniku
                for (j = 0; j < 3; j++)
                {
                    next[result.A, j] = data[result.B, j];
                    next[result.B, j] = data[result.A, j];
                    data[result.A, j] = next[result.A, j];
                    data[result.B, j] = next[result.B, j];
                }
                //Liczenie nowej wartości sumy odchyleń
                suma = 0;
                czas = 0;
                for (int i = 0; i < 200; i++)
                {
                    czas += data[i, 1];
                    data[i, 3] = czas;
                    data[i, 4] = (data[i, 2] - data[i, 3]) * (data[i, 2] - data[i, 3]);
                    suma += data[i, 4];
                }
                Console.WriteLine("Iteracja: " + h + "                      Wynik:          " + suma);
                foreach (Tabu tb in lista)//Pomniejszanie wartości liczącej ile razy zamiana nie może nastąpić przy poszczególnych indeksach
                {
                    tb.Licznik--;
                }
            }
            Console.WriteLine("Wynik końcowy:                                   " + suma);
            Zapis(data, suma);
        }

        

        static void Wspinaczka(int[,] data, int suma, int czas)
        {
            int j = 0;
            int current = suma;
            int[,] next = new int[200, 5];
            Random rnd = new Random();
            int zamiana = 0;
            Console.WriteLine(suma);
            next = Losowanie(data, next);//Losowanie rozwiązania początkowego
            suma = 0;
            czas = 0;
            for (int i = 0; i < 200; i++)//Obliczanie początkowej sumy odchyleń dla rozwiązania początkowego
            {
                for (j = 0; j < 3; j++)
                {
                    data[i, j] = next[i, j];
                }
                czas += data[i, 1];
                data[i, 3] = czas;
                data[i, 4] = (data[i, 2] - data[i, 3]) * (data[i, 2] - data[i, 3]);
                suma += data[i, 4];
            }
            current = suma;
            //while (min>current)
            for (int h = 0; h <= 10000; h++)
            {
                for (int i = 0; i < 200; i++)//przypisywanie wartości z pierwotnej tablicy do nowej tablicy
                {
                    for (j = 0; j < 3; j++)
                        next[i, j] = data[i, j];
                }
                //Losowanie dwóch indeksów do zamiany
                int i1 = rnd.Next(199) + 1;
                int i2 = rnd.Next(199) + 1;
                for (int i = 0; i < 3; i++)//Zamiana indeksów w nowej tablicy
                {
                    for (j = 0; j < 3; j++)
                    {
                        next[i2, j] = data[i1, j];
                        next[i1, j] = data[i2, j];
                    }
                }
                //Liczenie nowej wartości sumy odchyleń
                suma = 0;
                czas = 0;
                for (int i = 0; i < 200; i++)
                {
                    czas += next[i, 1];
                    next[i, 3] = czas;
                    next[i, 4] = (next[i, 2] - next[i, 3]) * (next[i, 2] - next[i, 3]);
                    suma += next[i, 4];
                }
                Console.WriteLine("Iteracja: " + h + " Poprzednia suma: " + current + " Obecna suma: " + suma + " Liczba zamian: " + zamiana);
                if (current > suma)//Jeżeli stara wartość sumy odchyleń jest większa od obecnej to następuje zamiana
                {
                    for (int i = 0; i < 200; i++)
                    {
                        for (int e = 0; e < 5; e++)
                            data[i, j] = next[i, j];
                    }
                    current = suma;
                    zamiana++;
                }
            }
            Console.WriteLine("Wynik końcowy: " + current);
            Zapis(data,current);
        }

        static void Wyzarzanie(int[,] data, int suma,int czas)
        {
            Console.WriteLine(suma);
            int j = 0;
            int[,] next = new int[200, 5];
            j = 0;
            double proba = 0.0;
            double alpha = 0.999;
            double temp = 10000000000.0; //Zmieniając te wartości
            double ep = 0.000001; //można dojść do polepszenia wyniku
            int delta;
            int current;
            Random rnd = new Random();
            next = Losowanie(data, next);//Losowanie rozwiązania początkowego
            current = suma;
            
            suma = 0;
            czas = 0;
            for (int i = 0; i < 200; i++)//Obliczanie początkowej sumy odchyleń dla rozwiązania początkowego
            {
                for (j = 0; j < 3; j++)
                {
                    data[i, j] = next[i, j];
                }
                czas += data[i, 1];
                data[i, 3] = czas;
                data[i, 4] = (data[i, 2] - data[i, 3]) * (data[i, 2] - data[i, 3]);
                suma += data[i, 4];
            }
            current = suma;
            while (temp > ep)//Dopóki temperatura jest większa od episilon, będą wykonywane kolejne iteracje polepszania wyniku
            {
                Random rnd2 = new Random();  
                for (int i = 0; i < 200; i++)//przypisywanie wartości z pierwotnej tablicy do nowej tablicy
                {
                    for (j = 0; j < 3; j++)
                        next[i, j] = data[i, j];
                }
                //Losowanie dwóch indeksów do zamiany
                int i1 = rnd.Next(199) + 1;
                int i2 = rnd.Next(199) + 1;
                for (int i = 0; i < 3; i++)//Zamiana indeksów w nowej tablicy
                {
                    for (j = 0; j < 3; j++)
                    {
                        next[i2, j] = data[i1, j];
                        next[i1, j] = data[i2, j];
                    }
                }
                suma = 0;
                czas = 0;
                for (int i = 0; i < 200; i++)//Liczenie nowej wartości sumy odchyleń
                {
                    czas += next[i, 1];
                    next[i, 3] = czas;
                    next[i, 4] = (next[i, 2] - next[i, 3]) * (next[i, 2] - next[i, 3]);
                    suma += next[i, 4];
                }
                delta = suma - current;//Liczenie delty dla obecnego rozwiązania
                Console.WriteLine("Poprzednia suma: " + suma + " Obecna suma: " + current);
                if (delta < 0)//Jeżeli stara wartość sumy odchyleń jest większa od obecnej to następuje zamiana
                {
                    for (int i = 0; i < 200; i++)
                    {
                        for (int e = 0; e < 5; e++)
                            data[i, j] = next[i, j];
                    }
                    current = delta + current;
                }
                else
                {
                    proba = (double)rnd2.Next(0, 1);//W przeciwnym przypadku zamiana następuje z prawdopodobienstwem exp(-delta/temp))
                    if (proba < Math.Exp(-delta / temp))
                    {
                        for (int i = 0; i < 200; i++)
                        {
                            for (int e = 0; e < 5; e++)
                                data[i, e] = next[i, e];
                        }
                        current = delta + current;
                    }
                } 
                temp *= alpha;//Następuje zmiana temperatury
            }
            Console.WriteLine("Wynik końcowy: " + current);
            Zapis(data,current);
        }
        //Funkcja pozwalająca na zapis obecnego ustawienia zadań
        static void Zapis(int[,] data,int suma)
        {
            string odp;
            Console.WriteLine("Czy chcesz zapisać to ustawienie? Wybierz numer opcji:");
            Console.WriteLine("1. TAK");
            Console.WriteLine("2. NIE");
            odp = Console.ReadLine();
            switch (odp)
            {
                case "1":
                    {
                        Console.WriteLine("Podaj nazwę pliku:");
                        string nazwa = Console.ReadLine();
                        nazwa += suma.ToString();
                        nazwa += ".csv";
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Zadanie,Czas wykonania,Termin,Czas Zakonczenia, Odchylenie\n");
                        for (int i = 0; i < 200; i++)
                        {
                            for (int j = 0; j < 5; j++)
                            {
                                sb.Append(data[i, j]);
                                sb.Append(",");
                            }
                            sb.Append("\n");
                        }
                        File.WriteAllText(nazwa, sb.ToString());
                        System.IO.File.WriteAllText(nazwa, sb.ToString());

                        Console.WriteLine("Plik znajduje się w: ProjektIO\\bin\\Debug\\netcoreapp2.2");
                        Console.WriteLine("Plik został zapisany, potwierdź zakończenie programu");
                        break;
                    }
                case "2":
                    {
                        Console.WriteLine("Potwierdź zakończenie programu");
                        break;
                    }
            }
        }
    }
}
