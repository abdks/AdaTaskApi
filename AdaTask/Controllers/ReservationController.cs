using AdaTask;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainReservation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RezervasyonController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] RezervasyonIstegi istek)
        {
            var tren = istek.Tren;
            var toplamYolcular = istek.RezervasyonYapilacakKisiSayisi;
            var farkliVagonlaraYerlestirilebilir = istek.KisilerFarkliVagonlaraYerlestirilebilir;

            var cevap = new RezervasyonCevabi();
            var kalanYolcular = toplamYolcular;

            var uygunKoltuklar = new List<(string VagonAdi, int KoltukSayisi)>();

            foreach (var vagon in tren.Vagonlar)
            {
                double dolulukOrani = (double)vagon.DoluKoltukAdet / vagon.Kapasite;

                if (dolulukOrani <= 0.7)
                {
                    int bosKoltuklar = (int)Math.Ceiling(vagon.Kapasite * 0.7) - vagon.DoluKoltukAdet;

                    if (bosKoltuklar > 0)
                    {
                        uygunKoltuklar.Add((vagon.Ad, bosKoltuklar));
                    }
                }
            }

            if (!farkliVagonlaraYerlestirilebilir || !uygunKoltuklar.Any())
            {
                cevap.RezervasyonYapilabilir = false;
                return Ok(JsonConvert.SerializeObject(cevap));
            }

            foreach (var koltuk in uygunKoltuklar)
            {
                var vagonIcinYolcuSayisi = Math.Min(kalanYolcular, koltuk.KoltukSayisi);
                cevap.YerlesimAyrinti.Add(new VagonRezervasyonu
                {
                    VagonAdi = koltuk.VagonAdi,
                    KisiSayisi = vagonIcinYolcuSayisi
                });

                kalanYolcular -= vagonIcinYolcuSayisi;

                if (kalanYolcular == 0)
                    break;
            }

            if (kalanYolcular > 0)
            {
                cevap.RezervasyonYapilabilir = false;
                cevap.YerlesimAyrinti.Clear(); // Yerleşim detaylarını temizle
                return Ok(JsonConvert.SerializeObject(cevap));
            }

            cevap.RezervasyonYapilabilir = true;

            return Ok(JsonConvert.SerializeObject(cevap));
        }
    }

   

    

   
    
   
}
