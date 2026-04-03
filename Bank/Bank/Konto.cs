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
        {     
            if (zablokowane)
            {
                throw new Exception("Konto jest zablokowane. Nie można dokonać wpłaty.");
            }

            if (kwota <= 0)
            {
                throw new Exception("Kwota wpłaty musi być większa od zera.");
            }

            bilans += kwota;
        }
        public void Wyplata(decimal kwota)
        {
            if (zablokowane)
                throw new Exception("Konto jest zablokowane. Nie można dokonać wypłaty.");

            if (kwota <= 0)
                throw new Exception("Kwota wpłaty musi być większa od zera.");

            if (kwota > bilans)
                throw new Exception("Niewystarczające środki na koncie.");

            bilans -= kwota;
        }
    }
}
