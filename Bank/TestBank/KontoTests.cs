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

    }
}
