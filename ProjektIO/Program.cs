using System;
using System.IO;

namespace ProjektIO
{

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

            //wyzarzanie();
            //WSPINACZKA



            Console.ReadKey();
        }     

        static void wyzarzanie()
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
            double temp = 100.0;
            double ep = 0.001;
            long delta;
            long current = suma;

            //int[] pomoc = new int[5];
            while (temp > ep)
            {
                Random rnd = new Random();
                Random rnd2 = new Random();

                //przypisywanie wartości z pierwotnej tabelicy do nowej tabelicy
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


