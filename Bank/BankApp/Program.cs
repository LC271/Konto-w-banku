using System;
using Bank;

class Program
{
    static void Main(string[] args)
    {
        var konto = new Konto("Adam", 100);
        Console.WriteLine($"Klient: {konto.Nazwa}, Bilans: {konto.Bilans}");

        var konto2 = new Konto("Jan", 100);

        konto2.Wplata(50);
        konto2.Wyplata(30);

        Console.WriteLine($"Klient: {konto2.Nazwa}, Bilans: {konto2.Bilans}");
    }
}