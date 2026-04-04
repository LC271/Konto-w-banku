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
        string[] rodzajeKonta = { "Konto", "KontoPlus", "KontoLimit" };
        string[] operacje = {"Wpłata", "Wypłata", "Stan konta", "Blokuj konto", "Odblokuj konto", "Zmiana rodzaju konta", "Wyjście"};

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
                Console.WriteLine("Nieprawidłowy numer. Proszę spróbować ponownie.");
            }
        }

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
                Console.WriteLine("Nieprawidłowy numer. Proszę spróbować ponownie.");
            }
        }

        int RodzajKonta = WczytanieRodzaju("Podaj jaki rodzaj konta chcesz (1 - Konto, 2 - KontoPlus, 3 - KontoLimit): ");
        while (RodzajKonta < 1 || RodzajKonta > 3)
        {
            Console.WriteLine("Nieprawidłowy wybór. Wybierz 1, 2 lub 3.");
            RodzajKonta = WczytanieRodzaju("Podaj jaki rodzaj konta chcesz (1 - Konto, 2 - KontoPlus, 3 - KontoLimit): ");
        }

        Console.Write("Podaj nazwę klienta: ");
        string nazwa = Console.ReadLine();

        decimal poczatkowy = 0m;
        Console.Write("Podaj początkowy bilans (np. 100): ");
        decimal.TryParse(Console.ReadLine(), out poczatkowy);

        // utworzenie wybranego konta
        object account = null;
        if (RodzajKonta == 1)
        {
            account = new Konto(nazwa, poczatkowy);
        }
        else if (RodzajKonta == 2)
        {
            Console.Write("Podaj limit jednorazowego debetu (np. 150): ");
            decimal limit = 0m;
            decimal.TryParse(Console.ReadLine(), out limit);
            account = new KontoPlus(nazwa, poczatkowy, limit);
        }
        else
        {
            Console.Write("Podaj limit konta z delegacją (np. 150): ");
            decimal limit = 0m;
            decimal.TryParse(Console.ReadLine(), out limit);
            account = new KontoLimit(nazwa, poczatkowy, limit);
        }

        Console.WriteLine($"Utworzono: {rodzajeKonta[RodzajKonta - 1]}");

        // pętla operacji
        while (true)
        {
            Console.Write("\n-----[Możliwe operacje]-----\n");
            Console.WriteLine("1. Wpłata\n2. Wypłata\n3. Sprawdzenie stanu konta\n4. Blokowanie konta\n5. Odblokowanie konta\n6. Zmiana rodzaju konta\n7. Wyjście");
            int op = WczytanieOperacji("\nPodaj numer operacji: ");

            try
            {
                if (op == 1)
                {
                    Console.Write("Podaj kwotę wpłaty: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal kw))
                    {
                        if (account is Konto k1) k1.Wplata(kw);
                        else if (account is KontoPlus kp1) kp1.Wplata(kw);
                        else if (account is KontoLimit kl1) kl1.Wplata(kw);
                        Console.WriteLine("Wpłata wykonana.");
                    }
                }
                else if (op == 2)
                {
                    Console.Write("Podaj kwotę wypłaty: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal kw))
                    {
                        if (account is Konto k2) k2.Wyplata(kw);
                        else if (account is KontoPlus kp2) kp2.Wyplata(kw);
                        else if (account is KontoLimit kl2) kl2.Wyplata(kw);
                        Console.WriteLine("Wypłata wykonana.");
                    }
                }
                else if (op == 3)
                {
                    if (account is KontoPlus kp3)
                    {
                        Console.WriteLine($"Klient: {kp3.Nazwa}, Bilans: {kp3.Bilans}, Limit: {kp3.Limit}, Zablokowane: {(kp3.Zablokowane ? "tak" : "nie")}");
                    }
                    else if (account is KontoLimit kl3)
                    {
                        Console.WriteLine($"Klient: {kl3.Nazwa}, Bilans (z limitem): {kl3.Bilans}, Limit: {kl3.Limit}, Zablokowane: {(kl3.Zablokowane ? "tak" : "nie")}");
                    }
                    else if (account is Konto k3)
                    {
                        Console.WriteLine($"Klient: {k3.Nazwa}, Bilans: {k3.Bilans}, Zablokowane: {(k3.Zablokowane ? "tak" : "nie")}");
                    }
                }
                else if (op == 4)
                {
                    if (account is Konto k4) k4.BlokujKonto();
                    else if (account is KontoPlus kp4) kp4.BlokujKonto();
                    else if (account is KontoLimit kl4) kl4.BlokujKonto();
                    Console.WriteLine("Konto zablokowane.");
                }
                else if (op == 5)
                {
                    if (account is Konto k5) k5.OdblokujKonto();
                    else if (account is KontoPlus kp5) kp5.OdblokujKonto();
                    else if (account is KontoLimit kl5) kl5.OdblokujKonto();
                    Console.WriteLine("Konto odblokowane.");
                }
                else if (op == 6)
                {
                    int nowy = WczytanieRodzaju("Wybierz nowy rodzaj konta (1 - Konto, 2 - KontoPlus, 3 - KontoLimit): ");
                    while (nowy < 1 || nowy > 3)
                    {
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                        nowy = WczytanieRodzaju("Wybierz nowy rodzaj konta (1 - Konto, 2 - KontoPlus, 3 - KontoLimit): ");
                    }

                    if (nowy == 1)
                    {
                        if (account is KontoPlus toKP) account = toKP.ToKonto();
                        else if (account is KontoLimit toKL) account = toKL.ToKonto();
                        Console.WriteLine("Zmieniono na Konto.");
                    }
                    else if (nowy == 2)
                    {
                        Console.Write("Podaj limit dla KontoPlus: ");
                        decimal limit = 0m;
                        decimal.TryParse(Console.ReadLine(), out limit);
                        if (account is Konto k6) account = k6.ToKontoPlus(limit);
                        else if (account is KontoPlus kp6) kp6.Limit = limit; // pozostaje KontoPlus
                        else if (account is KontoLimit kl6) account = kl6.ToKontoPlus(limit);
                        Console.WriteLine("Zmieniono na KontoPlus.");
                    }
                    else if (nowy == 3)
                    {
                        Console.Write("Podaj limit dla KontoLimit: ");
                        decimal limit = 0m;
                        decimal.TryParse(Console.ReadLine(), out limit);
                        if (account is Konto k7) account = k7.ToKontoLimit(limit);
                        else if (account is KontoPlus kp7) account = kp7.ToKontoLimit(limit);
                        else if (account is KontoLimit kl7) kl7.Limit = limit; // pozostaje KontoLimit
                        Console.WriteLine("Zmieniono na KontoLimit.");
                    }
                }
                else if (op == 7)
                {
                    Console.WriteLine("Koniec programu.");
                    break;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy numer operacji.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}