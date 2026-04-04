using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class Konto
    {
        protected string klient;  //nazwa klienta
        protected decimal bilans;  //aktualny stan środków na koncie
        protected bool zablokowane = false; //stan konta


        public Konto(string klient, decimal bilansNaStart = 0)
        {
            if (string.IsNullOrWhiteSpace(klient))
            {
                throw new ArgumentException("Nazwa klienta nie może być pusta.");
            }

            if (bilansNaStart < 0)
            {
                throw new ArgumentException("Bilans początkowy nie może być ujemny.");
            }

            this.klient = klient;
            this.bilans = bilansNaStart;
        }

        public string Nazwa => klient;
        public virtual decimal Bilans => bilans;
        public bool Zablokowane => zablokowane;

        #region wpłata i wypłata

        public virtual void Wplata(decimal kwota)
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
        public virtual void Wyplata(decimal kwota)
        {
            if (zablokowane)
                throw new ArgumentException("Konto jest zablokowane. Nie można dokonać wypłaty.");

            if (kwota <= 0)
                throw new ArgumentException("Kwota wypłaty musi być większa od zera.");

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
        internal void ZmianaBilansu(decimal delta)
        {
            bilans += delta;
        }
        #endregion

        #region konwersje
        // Konwersje ułatwiające tworzenie nowych obiektów różnych typów kont
        public virtual KontoPlus ToKontoPlus(decimal limit = 0m)
        {
            var kp = new KontoPlus(this.klient, this.bilans, limit);
            if (this.zablokowane)
                kp.BlokujKonto();
            return kp;
        }

        public virtual KontoLimit ToKontoLimit(decimal limit = 0m)
        {
            var kl = new KontoLimit(this.klient, this.bilans, limit);
            if (this.zablokowane)
                kl.BlokujKonto();
            return kl;
        }


        #endregion

    }

    public class KontoPlus : Konto
    {
        private decimal limit;
        private bool jednorazowyWykorzystany = false;

        public override decimal Bilans => bilans + (jednorazowyWykorzystany ? 0m : limit);

        public decimal Limit
        {
            get => limit;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Jednorazowy limit debetowy nie może być ujemny.");
                if (jednorazowyWykorzystany && bilans < 0 && -bilans > value)
                    throw new ArgumentException("Nowy limit jest mniejszy niż obecnie wykorzystany debet.");
                limit = value;
            }
        }
        public KontoPlus(string klient, decimal bilansNaStart = 0, decimal limit = 0) : base(klient, bilansNaStart)
        {
            Limit = limit;
        }

        #region konwersje
        // Konwersja do zwykłego Konto (rezygnacja z jednorazowego debetu)
        public Konto ToKonto()
        {
            var k = new Konto(this.klient, this.bilans);
            if (this.Zablokowane)
                k.BlokujKonto();
            return k;
        }

        // Konwersja do KontoLimit (delegacja) - zachowuje bilans i (domyślnie) obecny limit
        public KontoLimit ToKontoLimit(decimal? limit = null)
        {
            var useLimit = limit ?? this.Limit;
            var kl = new KontoLimit(this.klient, this.bilans, useLimit);
            if (this.Zablokowane)
                kl.BlokujKonto();
            return kl;
        }
        #endregion

        #region wpłata i wypłata
        public override void Wplata(decimal kwota)
        {
            if (kwota <= 0)
                throw new ArgumentException("Kwota wpłaty musi być większa od zera.");

            bilans += kwota;

            if (bilans > 0)
            {
                OdblokujKonto();
                jednorazowyWykorzystany = false;
            }
        }

        public override void Wyplata(decimal kwota)
        {
            if (Zablokowane)
                throw new ArgumentException("Konto jest zablokowane. Nie można dokonać wypłaty.");

            if (kwota <= 0)
                throw new ArgumentException("Kwota wypłaty musi być większa od zera.");

            if (kwota <= bilans)
            {
                base.Wyplata(kwota);
                return;
            }

            decimal potrzebne = kwota - bilans;

            if (!jednorazowyWykorzystany && potrzebne <= limit)
            {
                bilans -= kwota;
                jednorazowyWykorzystany = true;
                BlokujKonto();
                return;
            }

            throw new ArgumentException("Niewystarczające środki na koncie.");
            #endregion
        }
    }

    //KontoLimit to samo co KontoPlus ale za pomocą delegacji, bez dziedziczenia
    public class KontoLimit
    {
        //Bilans to bilans z konta plus limit, a konto.Bilans to tylko bilans z konta bez limitu.
        private Konto konto;
        private decimal limit;
        private bool jednorazowyWykorzystany = false;

        public KontoLimit(string klient, decimal bilansNaStart = 0, decimal limit = 0)
        {
            konto = new Konto(klient, bilansNaStart);
            this.limit = limit;
        }

        #region konwersje
        public Konto ToKonto()
        {
            var k = new Konto(this.konto.Nazwa, this.konto.Bilans);
            if (this.Zablokowane)
                k.BlokujKonto();
            return k;
        }

        public KontoPlus ToKontoPlus(decimal limit)
        {
            var kp = new KontoPlus(this.konto.Nazwa, this.konto.Bilans, limit);
            if (this.Zablokowane)
                kp.BlokujKonto();
            return kp;
        }
        #endregion

        public string Nazwa => konto.Nazwa;
        public decimal Bilans => konto.Bilans + (jednorazowyWykorzystany ? 0m : limit);
        public bool Zablokowane => konto.Zablokowane;

        public decimal Limit
        {
            get => limit;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Jednorazowy limit debetowy nie może być ujemny.");
                if (jednorazowyWykorzystany && konto.Bilans < 0 && -konto.Bilans > value)
                    throw new ArgumentException("Nowy limit jest mniejszy niż obecnie wykorzystany debet.");
                limit = value;
            }
        }

        #region wpłata i wypłata
        public void Wplata(decimal kwota)
        {
            if (kwota <= 0)
                throw new ArgumentException("Kwota wpłaty musi być większa od zera.");

            konto.ZmianaBilansu(kwota);

            if (konto.Bilans > 0)
            {
                OdblokujKonto();
                jednorazowyWykorzystany = false;
            }
        }

        public void Wyplata(decimal kwota)
        {
            if (Zablokowane)
                throw new ArgumentException("Konto jest zablokowane. Nie można dokonać wypłaty.");

            if (kwota <= 0)
                throw new ArgumentException("Kwota wypłaty musi być większa od zera.");

            if (kwota <= konto.Bilans)
            {
                konto.ZmianaBilansu(-kwota);
                return;
            }

            decimal potrzebne = kwota - konto.Bilans;

            if (!jednorazowyWykorzystany && potrzebne <= limit)
            {
                konto.ZmianaBilansu(-kwota);
                jednorazowyWykorzystany = true;
                BlokujKonto();
                return;
            }

            throw new ArgumentException("Niewystarczające środki na koncie.");
        }
        #endregion

        #region blokada i odblokada
        public void BlokujKonto()
        {
            konto.BlokujKonto();
        }

        public void OdblokujKonto()
        {
            konto.OdblokujKonto();
        }
        #endregion

    }
}