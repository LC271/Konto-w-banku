using System;
using Bank;

class Program
{
    static void Main(string[] args)
    {
        //var konto = new KontoPlus("Adam", 100, 150);
        //konto.Wyplata(150);
        //Console.WriteLine($"Klient: {konto.Nazwa}, Bilans: {konto.Bilans}, limit: {konto.Limit}, zablokowane: {(konto.Zablokowane ? "tak" : "nie")}");

        //var konto2 = new Konto("Jan", 100);

        //konto2.Wplata(50);
        //konto2.Wyplata(30);

        //Console.WriteLine($"Klient: {konto2.Nazwa}, Bilans: {konto2.Bilans}");

        Console.WriteLine("program - Konto w banku");
        string[] rodzajeKonta = { "Konto", "Kontoplus", "KontoLimit" };
        string[] operacje = { "Wpłata", "Wypłata", "Sprawdzenie bilansu", "Blokowanie konta", "Odblokowanie konta", "Wyjście" };

        static int WczytanieRodzaju(string komunikat)
        {
            int rodzaj;

            while (true)
            {
                Console.Write(komunikat);
                string wejscie = Console.ReadLine();
                if (int.TryParse(wejscie, out rodzaj))
                {
                    return rodzaj;
                }
                Console.WriteLine("Nieprawidłowy numer. Proszę spróbować użyć 1, 2 lub 3 do wyboru odpowiedniego rodzaju konta.");
            }
        }


        int RodzajKonta = WczytanieRodzaju("Podaj jaki rodzaj konta chcesz (np. normalne - 1, plus - 2, z limitem debetowym - 3): ");
        Console.WriteLine($"Wybrany rodzaj konta: {rodzajeKonta[RodzajKonta - 1]}");

        //operacje
        Console.Write("\n-----[Możliwe operacje]-----\n");


        static int WczytanieOperacji(string komunikat)
        {
            int rodzaj;

            while (true)
            {
                Console.Write(komunikat);
                string wejscie = Console.ReadLine();
                if (int.TryParse(wejscie, out rodzaj))
                {
                    return rodzaj;
                }
                Console.WriteLine("Nieprawidłowy numer. Proszę spróbować użyć 1, 2 lub 3 do wyboru odpowiedniego rodzaju konta.");
            }
        }
        int RodzajOperacji = WczytanieOperacji("1. Wpłata\n2. Wypłata\n3. Sprawdzenie bilansu\n4. Blokowanie konta\n5. Odblokowanie konta\n6. Wyjście\n\n");
        Console.WriteLine($"Wybrany operacji: {operacje[RodzajOperacji - 1]}");
    }
}