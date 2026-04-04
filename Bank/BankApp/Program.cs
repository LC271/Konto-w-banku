using System;
using Bank;

class Program
{
    static void Main(string[] args)
    {
        //var konto = new Konto("Adam", 100);
        //Console.WriteLine($"Klient: {konto.Nazwa}, Bilans: {konto.Bilans}");

        //var konto2 = new Konto("Jan", 100);

        //konto2.Wplata(50);
        //konto2.Wyplata(30);

        //Console.WriteLine($"Klient: {konto2.Nazwa}, Bilans: {konto2.Bilans}");

        Console.WriteLine("program - Konto w banku");
        string[] rodzajeKonta = { "normalny", "plus", "z limitem debetowym" };
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
        Console.WriteLine($"Wybrany rodzaj konta: {rodzajeKonta[RodzajKonta-1]}");
    }
}