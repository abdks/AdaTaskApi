using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainReservation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] ReservationRequest request)
        {
            var tren = request.Tren;
            var totalPassengers = request.RezervasyonYapilacakKisiSayisi;
            var canAccommodate = request.KisilerFarkliVagonlaraYerlestirilebilir;

            var response = new ReservationResponse();
            var remainingPassengers = totalPassengers;

            var availableSeats = new List<(string VagonAdi, int KoltukSayisi)>();

            // Vagonlarda boş koltukları bul
            foreach (var vagon in tren.Vagonlar)
            {
                // Doluluk oranını hesapla
                double occupancyRate = (double)vagon.DoluKoltukAdet / vagon.Kapasite;

                // Doluluk oranının %70'den az veya eşit olduğunu kontrol et
                if (occupancyRate <= 0.7)
                {
                    // Mevcut doluluk oranına göre boş koltuk sayısını hesapla
                    int emptySeats = (int)Math.Ceiling(vagon.Kapasite * 0.7) - vagon.DoluKoltukAdet;

                    // Boş koltuk varsa, bu vagonu availableSeats listesine ekle
                    if (emptySeats > 0)
                    {
                        availableSeats.Add((vagon.Ad, emptySeats));
                    }
                }
            }

            if (canAccommodate && availableSeats.Any())
            {
                // Uygun vagonlara yolcuları yerleştir
                foreach (var seat in availableSeats)
                {
                    var passengersInVagon = Math.Min(remainingPassengers, seat.KoltukSayisi);
response.YerlesimAyrinti.Add(new VagonReservation
{
  VagonAdi = seat.VagonAdi,
  KisiSayisi = passengersInVagon
});

                    remainingPassengers -= passengersInVagon;

                    if (remainingPassengers == 0)
                        break;
                }
            }
            else
            {
                // Farklı vagonlara yerleştirilemiyorsa veya uygun vagon yoksa, varolan vagonlara yerleştir
                foreach (var vagon in tren.Vagonlar)
                {
                    if (remainingPassengers <= 0)
                        break;

                    var emptySeats = (int)Math.Ceiling(vagon.Kapasite * 0.7) - vagon.DoluKoltukAdet;

                    if (emptySeats > 0)
                    {
                        var passengersInVagon = Math.Min(remainingPassengers, emptySeats);

                        response.YerlesimAyrinti.Add(new VagonReservation
                        {
                            VagonAdi = vagon.Ad,
                            KisiSayisi = passengersInVagon
                        });

                        vagon.DoluKoltukAdet += passengersInVagon;
                        remainingPassengers -= passengersInVagon;
                    }
                }
            }

            response.RezervasyonYapilabilir = remainingPassengers == 0;

            return Ok(JsonConvert.SerializeObject(response));
        }
    }

    public class ReservationRequest
    {
        public Tren Tren { get; set; }
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
    }

    public class Tren
    {
        public string Ad { get; set; }
        public List<Vagon> Vagonlar { get; set; }
    }

    public class Vagon
    {
        public string Ad { get; set; }
        public int Kapasite { get; set; }
        public int DoluKoltukAdet { get; set; }
    }
    public class VagonReservation
    {
        public string VagonAdi { get; set; }
        public int KisiSayisi { get; set; }
    }
    public class ReservationResponse
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<VagonReservation> YerlesimAyrinti { get; set; } // "VagonReservation" olarak değiştirildi

        public ReservationResponse()
        {
            YerlesimAyrinti = new List<VagonReservation>();
        }
    }
}