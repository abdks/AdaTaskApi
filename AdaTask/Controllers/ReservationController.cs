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

            foreach (var vagon in tren.Vagonlar)
            {
                double occupancyRate = (double)vagon.DoluKoltukAdet / vagon.Kapasite;

                if (occupancyRate <= 0.7)
                {
                    int emptySeats = (int)Math.Ceiling(vagon.Kapasite * 0.7) - vagon.DoluKoltukAdet;

                    if (emptySeats > 0)
                    {
                        availableSeats.Add((vagon.Ad, emptySeats));
                    }
                }
            }

            if (!canAccommodate)
            {
                response.RezervasyonYapilabilir = false;
                return Ok(JsonConvert.SerializeObject(response));
            }

            if (availableSeats.Any())
            {
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
        public List<VagonReservation> YerlesimAyrinti { get; set; }

        public ReservationResponse()
        {
            YerlesimAyrinti = new List<VagonReservation>();
        }
    }
}
