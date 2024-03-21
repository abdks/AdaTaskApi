using TrainReservation.Controllers;

namespace AdaTask
{
    public class RezervasyonCevabi
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<VagonRezervasyonu> YerlesimAyrinti { get; set; }

        public RezervasyonCevabi()
        {
            YerlesimAyrinti = new List<VagonRezervasyonu>();
        }
    }
}
