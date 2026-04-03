using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    internal class Konto
    {
        private string klient;  //nazwa klienta
        private decimal bilans;  //aktualny stan środków na koncie
        private bool zablokowane = false; //stan konta

        //private Konto() { } //konstruktor bezparametrowy, który jest prywatny, aby uniemożliwić tworzenie kont bez podania klienta i bilansu

        public Konto(string klient, decimal bilansNaStart = 0) 
        {
            this.klient = klient;
            this.bilans = bilansNaStart;
        }

        public string NazwaKlienta
        {
            get { return klient; }
        }
        public decimal Bilans
        {
            get { return bilans; }
        }
        public bool Zablokowane
        {
            get { return zablokowane; }
        }
    }
}
