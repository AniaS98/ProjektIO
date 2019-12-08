using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjektIO
{
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


    }


    class Program
    {

        static void Main(string[] args)
        {
            //Zczytywanie danych +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            string text = System.IO.File.ReadAllText("D:\\adm\\Documents\\STUDIA\\V Semestr\\Inteligencja Obliczeniowa\\ProjektIO\\ProjektIO\\Dane.csv");

            Console.WriteLine(text);

            //Console.WriteLine(text[31]);
            
            String[,] Tablica = new String[202,3];
            
            for(int i=0;i<200;i++)
            {
                for (int e = 0; e < 3; e++)
                    Tablica[i,e] = "";
            }

            int j = 0;
            int k = 0;
            for(int i=31;i<text.Length;i++)
            {
                if (text[i] == '\n')
                {
                    k++;
                    j = 0;
                }        
                if (text[i] == ',')
                {
                    j++;
                    
                }
                else
                {
                    Tablica[k,j] +=text[i];
                }
            }


            int suma = 0;
            Console.WriteLine('\n');
            int[,] data = new int[200, 5];
            int czas = 0;
            for (int i = 0; i < 200; i++)
            {
                for (int e = 0; e < 3; e++)
                {
                    data[i, e] = Int32.Parse(Tablica[i, e]);

                    Console.WriteLine(data[i, e]);
                }

                czas += data[i, 1];
                data[i, 3] = czas;
                data[i, 4] = (data[i, 2] - data[i, 3]) * (data[i, 2] - data[i, 3]);
                suma += data[i, 4];
            }
            //Koniec danych ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            //Wyzarzanie();
            //Wspinaczka();
            //TABU SEARCH
            
            Tabu t = new Tabu();
            int iterator=0;
            int[,] next = new int[200, 5];
            Random rnd = new Random();
            Queue<Tabu> lista=new Queue<Tabu>();
            int current = suma;
            List<Tabu> top = new List<Tabu>();
            for (int h=0;h<100;h++)
            { 
                if (h>0)
                {
                    Tabu pomoc = lista.Peek();
                    while (pomoc.Licznik == 0)
                    {
                        lista.Dequeue();
                        pomoc = lista.Peek();
                    }
                        
                }
                for(int x=0;x<200;x++)
                {
                    for(int y=x+1;y<200;y++)
                    {
                        //przypisywanie wartości z pierwotnej tablicy do nowej tablicy
                        for (int i = 0; i < 200; i++)
                        {
                            for (j = 0; j < 3; j++)
                                next[i, j] = data[i, j];
                        }
                        //Zamiana indeksów w nowej tablicy
                        for (int i = 0; i < 3; i++)
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
                        if (iterator < 5)
                        {
                            t = new Tabu(x, y, suma);
                            top.Add(t);
                        }
                        else
                        {
                            top[Findmax(top)]=new Tabu(x,y,suma);
                            //Tworzenie listy top 5 najlepszych rozwiązanń
                        }
                    }
                }

                //Wybór rozwiązania
                bool outcome=false;
                iterator = 0;
                Tabu result = new Tabu();
                if(lista.Count > 0)
                {
                    //Tu jest źle
                    for (int i=0; outcome==false|| i<5;i++)
                    {
                        //Console.WriteLine("dziala");
                        t.A = top[i].A;
                        t.B = top[i].B;
                        iterator = 0;
                        for (j=1;j<=3 || outcome==false;j++)
                        {
                            t.Licznik = j;
                            if(lista.Contains(t)==false)
                            {
                                iterator++;
                            }
                        }
                        if(iterator==3)
                        {
                            result = t;
                            result.Licznik = 3;
                            lista.Enqueue(result);
                            outcome = true;
                        }
                    
                    }
                }
                else
                {
                    result = t;
                    result.Licznik = 3;
                    lista.Enqueue(result);
                    outcome = true;

                }


                //Zamiana indeksów w nowej tablicy
                for (int i = 0; i < 3; i++)
                {
                    next[t.A, j] = data[t.B, j];
                    next[t.B, j] = data[t.A, j];
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
                Console.WriteLine("     " + suma);
                foreach(Tabu tb in lista)
                {
                    tb.Licznik--;
                }





            }
            

            




            

            Console.ReadKey();
        }     

        static int Findmax(List<Tabu> tab)
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


        static void Wspinaczka()
        {
            //Zczytywanie danych +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            string text = System.IO.File.ReadAllText("D:\\adm\\Documents\\STUDIA\\V Semestr\\Inteligencja Obliczeniowa\\ProjektIO\\ProjektIO\\Dane.csv");

            Console.WriteLine(text);

            //Console.WriteLine(text[31]);

            String[,] Tablica = new String[202, 3];

            for (int i = 0; i < 200; i++)
            {
                for (int e = 0; e < 3; e++)
                    Tablica[i, e] = "";
            }

            int j = 0;
            int k = 0;
            for (int i = 31; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    k++;
                    j = 0;
                }
                if (text[i] == ',')
                {
                    j++;

                }
                else
                {
                    Tablica[k, j] += text[i];
                }
            }


            int suma = 0;
            Console.WriteLine('\n');
            int[,] data = new int[200, 5];
            int czas = 0;
            for (int i = 0; i < 200; i++)
            {
                for (int e = 0; e < 3; e++)
                {
                    data[i, e] = Int32.Parse(Tablica[i, e]);

                    Console.WriteLine(data[i, e]);
                }

                czas += data[i, 1];
                data[i, 3] = czas;
                data[i, 4] = (data[i, 2] - data[i, 3]) * (data[i, 2] - data[i, 3]);
                suma += data[i, 4];
            }
            //Koniec danych ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            int current = suma;
            int[] newset = Losowanie();
            int[,] next = new int[200, 5];
            //long solution = suma;
            Console.WriteLine(suma);
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
                {
                    next[mieszalnik[i], j] = data[i, j];
                }
                Console.WriteLine(mieszalnik[i]);
            }

            suma = 0;
            czas = 0;
            for (int i = 0; i < 200; i++)
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
            int size = 2000000;
            for (int h = 0; h < size; h++)
            {


                //przypisywanie wartości z pierwotnej tablicy do nowej tablicy
                for (int i = 0; i < 200; i++)
                {
                    for (j = 0; j < 3; j++)
                        next[i, j] = data[i, j];
                }
                //Losowanie dwóch indeksów do zamiany
                int i1 = rnd.Next(199) + 1;
                int i2 = rnd.Next(199) + 1;
                //Zamiana indeksów w nowej tablicy
                for (int i = 0; i < 3; i++)
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
                }//Jeżeli stara wartość sumy odchyleń jest większa od obecnej to następuje zamiana
                if (current > suma)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        for (int e = 0; e < 5; e++)
                            data[i, j] = next[i, j];
                    }
                    current = suma;
                    zamiana++;
                }
                //Console.WriteLine(suma + " " + current);
                Console.WriteLine(h + " " + suma + " " + current + " " + zamiana);

            }
            Console.WriteLine(current);
        }

        static int[] Losowanie()
        {
            int n = 200;
            Random rnd = new Random();
            int[] newset = new int[200];
            for (int i = 0; i < n; i++)
            {
                newset[i] = i + 1;
            }
            for (int i = 0; i < 200; i++)
            {
                int r = rnd.Next(n);
                newset[r] = newset[n - 1];
                n--;
            }

            return newset;
        }


        static void Wyzarzanie()
        {
            //Zczytywanie danych +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            string text = System.IO.File.ReadAllText("D:\\adm\\Documents\\STUDIA\\V Semestr\\Inteligencja Obliczeniowa\\ProjektIO\\ProjektIO\\Dane.csv");

            Console.WriteLine(text);

            //Console.WriteLine(text[31]);

            String[,] Tablica = new String[202, 3];

            for (int i = 0; i < 200; i++)
            {
                for (int e = 0; e < 3; e++)
                    Tablica[i, e] = "";
            }

            int j = 0;
            int k = 0;
            for (int i = 31; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    k++;
                    j = 0;
                }
                if (text[i] == ',')
                {
                    j++;

                }
                else
                {
                    Tablica[k, j] += text[i];
                }
            }


            long suma = 0;
            Console.WriteLine('\n');
            int[,] data = new int[200, 5];
            int czas = 0;
            for (int i = 0; i < 200; i++)
            {
                for (int e = 0; e < 3; e++)
                {
                    data[i, e] = Int32.Parse(Tablica[i, e]);

                    Console.WriteLine(data[i, e]);
                }

                czas += data[i, 1];
                data[i, 3] = czas;
                data[i, 4] = (data[i, 2] - data[i, 3]) * (data[i, 2] - data[i, 3]);
                suma += data[i, 4];
            }
            //Koniec danych ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            Console.WriteLine(suma);

            ///<summary>
            /// Algorytm wspinaczkowy
            ///</summary>

            //Deklaracja początkowych wartości danych
            int[,] next = new int[200, 5];
            j = 0;
            double proba = 0.0;
            double alpha = 0.999;
            double temp = 100000000.0; //Tu można się pobawić
            double ep = 0.000001; //z wartościami
            long delta;
            long current = suma;

            StringBuilder sb = new StringBuilder();

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
                {
                    next[mieszalnik[i], j] = data[i, j];
                }
                
            }
            
            suma = 0;
            czas = 0;
            for (int i = 0; i < 200; i++)
            {
                sb.Append("\n");
                sb.Append(next[i, 0]);
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
            File.WriteAllText("D:\\adm\\Documents\\STUDIA\\V Semestr\\Inteligencja Obliczeniowa\\ProjektIO\\ProjektIO\\RandomOutput", sb.ToString());
            //ZAPIS DO TXT

            //int[] pomoc = new int[5];
            while (temp > ep)
            {
                Random rnd2 = new Random();

                //przypisywanie wartości z pierwotnej tablicy do nowej tablicy
                for (int i = 0; i < 200; i++)
                {
                    for (j = 0; j < 3; j++)
                        next[i, j] = data[i, j];
                }
                //Losowanie dwóch indeksów do zamiany
                int i1 = rnd.Next(199) + 1;
                int i2 = rnd.Next(199) + 1;
                //Zamiana indeksów w nowej tablicy
                for (int i = 0; i < 3; i++)
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
                delta = suma - current;
                Console.WriteLine(suma + " " + current);
                //Console.WriteLine(suma + " " + current);

                //Jeżeli stara wartość sumy odchyleń jest większa od obecnej to następuje zamiana
                if (delta < 0)
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
                    //W przeciwnym przypadku zamiana następuje z prawdopodobienstwem exp(-delta/temp))
                    proba = (double)rnd2.Next(0, 1);
                    if (proba < Math.Exp(-delta / temp))
                    {
                        for (int i = 0; i < 200; i++)
                        {
                            for (int e = 0; e < 5; e++)
                                data[i, j] = next[i, j];
                        }
                        current = delta + current;
                    }
                }


                //Następuje zmiana temperatury
                temp *= alpha;





            }
            Console.WriteLine(current);
        }
    }
}


