using Bank;

namespace TestBank
{
    [TestClass]
    public sealed class KontoTests
    {
        #region Kontruktor

        [TestMethod]
        public void Konstruktor_dane_poprawne()
        {
            //arrange
            string klient = "Jan Kowalski";
            decimal bilansNaStart = 1000m;

            //act
            Konto konto = new Konto(klient, bilansNaStart);

            //assert
            Assert.AreEqual(klient, konto.Nazwa);

        }

        [TestMethod]
        public void Konstruktor_imie_niepoprawne()
        {
            //arrange
            string klient = string.Empty;
            decimal bilansNaStart = -1000m;

            //act and assert
            Assert.Throws<ArgumentException>(() => new Konto(klient, bilansNaStart));

        }

        [TestMethod]
        public void Konstruktor_bilans_niepoprawne()
        {
            //arrange
            string klient = "Jan Kowalski";
            decimal bilansNaStart = -1000m;
            //act and assert
            Assert.Throws<ArgumentException>(() => new Konto(klient, bilansNaStart));
        }
        #endregion

        #region Wplata
        [TestMethod]
        public void Wplata_dane_poprawne()
        {
            //arrange
            decimal kwota = 500m;
            decimal oczekiwanyBilans = 1500m;
            decimal bilansNaStart = 1000m;
            var konto = new Konto("Jan Kowalski", bilansNaStart);
            //act
            konto.Wplata(kwota);
            //assert
            Assert.AreEqual(oczekiwanyBilans, konto.Bilans);
        }
        [TestMethod]
        public void Wplata_kwota_niepoprawna()
        {
            //arrange
            decimal kwota = -500m;
            decimal bilansNaStart = 1000m;
            var konto = new Konto("Jan Kowalski", bilansNaStart);
            //act and assert
            Assert.Throws<ArgumentException>(() => konto.Wplata(kwota));
        }
        [TestMethod]
        public void Wplata_konto_zablokowane()
        {
            //arrange
            decimal kwota = 500m;
            decimal bilansNaStart = 1000m;
            var konto = new Konto("Jan Kowalski", bilansNaStart);
            konto.BlokujKonto();
            //act and assert
            Assert.Throws<ArgumentException>(() => konto.Wplata(kwota));
        }
        #endregion

        #region wypłata
        [TestMethod]
        public void Wyplata_dane_poprawne()
        {
            //arrange
            decimal kwota = 500m;
            decimal oczekiwanyBilans = 500m;
            decimal bilansNaStart = 1000m;
            var konto = new Konto("Jan Kowalski", bilansNaStart);
            //act
            konto.Wyplata(kwota);
            //assert
            Assert.AreEqual(oczekiwanyBilans, konto.Bilans);
        }
        [TestMethod]
        public void Wyplata_kwota_niepoprawna()
        {
            //arrange
            decimal kwota = -500m;
            decimal bilansNaStart = 1000m;
            var konto = new Konto("Jan Kowalski", bilansNaStart);
            //act and assert
            Assert.Throws<ArgumentException>(() => konto.Wyplata(kwota));
        }

        [TestMethod]
        public void Wyplata_kwota_większa_niż_bilans()
        {
            //arrange
            decimal kwota = 1500m;
            decimal bilansNaStart = 1000m;
            var konto = new Konto("Jan Kowalski", bilansNaStart);
            //act and assert
            Assert.Throws<ArgumentException>(() => konto.Wyplata(kwota));
        }
        [TestMethod]
        public void Wyplata_konto_zablokowane()
        {
            //arrange
            decimal kwota = 500m;
            decimal bilansNaStart = 1000m;
            var konto = new Konto("Jan Kowalski", bilansNaStart);
            konto.BlokujKonto();
            //act and assert
            Assert.Throws<ArgumentException>(() => konto.Wyplata(kwota));
        }
        #endregion

        #region blokowanie i odblokowywanie konta
        [TestMethod]
        public void BlokujKonto_konto_zablokowane()
        {
            //arrange
            decimal bilansNaStart = 1000m;
            var konto = new Konto("Jan Kowalski", bilansNaStart);
            konto.BlokujKonto();
            //act and assert
            Assert.IsTrue(konto.Zablokowane);
        }
        [TestMethod]
        public void OdblokujKonto_konto_odblokowane()
        {
            //arrange
            decimal bilansNaStart = 1000m;
            var konto = new Konto("Jan Kowalski", bilansNaStart);
            //act and assert
            Assert.IsFalse(konto.Zablokowane); ;
        }
        #endregion

    }

    [TestClass]
    public sealed class KontoPlusTests
    {
        #region konstruktor i wlasciwosci
        [TestMethod]
        public void KontoPlus_konstruktor_i_wlasciwosci()
        {
            // arrange
            var konto = new KontoPlus("Adam", 100m, 20m);

            // assert
            Assert.AreEqual("Adam", konto.Nazwa);
            Assert.AreEqual(20m, konto.Limit);
            Assert.AreEqual(120m, konto.Bilans);
        }

        [TestMethod]
        public void KontoPlus_Limit_mniejszy_niz_zero()
        {
            // arrange
            var konto = new KontoPlus("Katarzyna", 100m, 20m);
            // act and assert
            Assert.Throws<ArgumentException>(() => konto.Limit = -10m);
        }
        #endregion

        #region wplata

        [TestMethod]
        public void KontoPlus_wplata_odblokowuje_i_przywroci_jednorazowy()
        {
            // arrange
            var konto = new KontoPlus("Marta", 100m, 100m);
            konto.Wyplata(180m);

            // act
            konto.Wplata(100m);

            // assert
            Assert.IsFalse(konto.Zablokowane);
            Assert.AreEqual(120m, konto.Bilans);
            
        }
        [TestMethod]
        public void KontoPlus_wplata_kwota_niepoprawna()
        {
            // arrange
            var konto = new KontoPlus("Marta", 100m, 100m);
            // act and assert
            Assert.Throws<ArgumentException>(() => konto.Wplata(-50m));
        }
        #endregion

        #region wyplata
        [TestMethod]
        public void KontoPlus_wyplata_wykorzystanie_limitu_kredytowego()
        {
            // arrange
            var konto = new KontoPlus("Ewa", 100m, 50m);

            // act
            konto.Wyplata(150m);

            // assert
            Assert.AreEqual(-50m, konto.Bilans);
            Assert.IsTrue(konto.Zablokowane);
        }

        [TestMethod]
        public void KontoPlus_wyplata_jednorazowy_debet_blokuje_konto()
        {
            // arrange
            var konto = new KontoPlus("Piotr", 100m, 100m);

            // act
            konto.Wyplata(180m);

            // assert
            Assert.AreEqual(-80m, konto.Bilans);
            Assert.IsTrue(konto.Zablokowane);

            Assert.Throws<ArgumentException>(() => konto.Limit = 50m);
        }

        [TestMethod]
        public void KontoPlus_wyplata_kwota_niewieksza_od_zera()
        {
            // arrange
            var konto = new KontoPlus("Marta", 100m, 100m);
            // act and assert
            Assert.Throws<ArgumentException>(() => konto.Wyplata(-50m));
        }

        [TestMethod]
        public void KontoPlus_wyplata_kwota_wieksza_niz_bilans_i_limit()
        {
            // arrange
            var konto = new KontoPlus("Marta", 100m, 100m);
            // act and assert
            Assert.Throws<ArgumentException>(() => konto.Wyplata(250m));
        }

        [TestMethod]
        public void KontoPlus_wyplata_konto_zablokowane()
        {
            // arrange
            var konto = new KontoPlus("Marta", 100m, 100m);
            konto.BlokujKonto();
            // act and assert
            Assert.Throws<ArgumentException>(() => konto.Wyplata(50m));
        }

        [TestMethod]
        public void KontoPlus_kwota_niewieksza_od_bilansu()
        {
            //arrange
            var konto = new KontoPlus("Marta", 100m, 100m);
            konto.Wyplata(100m);
        }
        #endregion

    }

}
