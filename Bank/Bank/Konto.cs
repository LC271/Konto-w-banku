using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class Konto
    {
        private string klient;  //nazwa klienta
        private decimal bilans;  //aktualny stan środków na koncie
        private bool zablokowane = false; //stan konta


        public Konto(string klient, decimal bilansNaStart = 0) 
        {
            this.klient = klient;
            this.bilans = bilansNaStart;
        }

        public string Nazwa => klient;
        public decimal Bilans => bilans;
        public bool Zablokowane => zablokowane;

        public void Wplata(decimal kwota)
        {     if (zablokowane)
            {
                Console.WriteLine("Konto jest zablokowane. Nie można dokonać wpłaty.");
                return;
            }
            if (kwota <= 0)
            {
                Console.WriteLine("Kwota wpłaty musi być większa od zera.");
                return;
            }
            bilans += kwota;
            Console.WriteLine($"Wpłacono {kwota:C}. Nowy bilans: {bilans:C}");
        }
    }
}
