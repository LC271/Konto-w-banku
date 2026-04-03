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
            if (string.IsNullOrWhiteSpace(klient))
            {
                throw new ArgumentException("Nazwa klienta nie może być pusta.", nameof(klient));
            }

            if (bilansNaStart < 0)
            {
                throw new ArgumentException("Bilans początkowy nie może być ujemny.", nameof(bilansNaStart));
            }

            this.klient = klient;
            this.bilans = bilansNaStart;
        }

        public string Nazwa => klient;
        public decimal Bilans => bilans;
        public bool Zablokowane => zablokowane;

        //public readonly (string Nazwa, decimal Bilans, bool Zablokowane) StanKonta = new (string.Empty, 0m, false);

        #region wpłata i wypłata
        public void Wplata(decimal kwota)
        {     
            if (zablokowane)
            {
                throw new ArgumentException("Konto jest zablokowane. Nie można dokonać wpłaty.");
            }

            if (kwota <= 0)
            {
                throw new ArgumentException("Kwota wpłaty musi być większa od zera.");
            }

            bilans += kwota;
        }
        public void Wyplata(decimal kwota)
        {
            if (zablokowane)
                throw new ArgumentException("Konto jest zablokowane. Nie można dokonać wypłaty.");

            if (kwota <= 0)
                throw new ArgumentException("Kwota wpłaty musi być większa od zera.");

            if (kwota > bilans)
                throw new ArgumentException("Niewystarczające środki na koncie.");

            bilans -= kwota;
        }
        #endregion

        #region blokada i odblokada

        public void BlokujKonto()
        {
            zablokowane = true;
        }
        public void OdblokujKonto()
        {
            zablokowane = false;
        }
        #endregion


    }
}
