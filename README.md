## Görev ## 

Ne Yapmalıyım?
Aşağıda ayrıntıları verilen uygulamayı istediğiniz programlama dili ve araçları ile geliştirin. Kodu github veya benzeri bir ortama yükleyip bize URL iletin.
Geliştirdiğiniz API'ı deploy edip bize URL'ini iletebilirsiniz ek olarak. Kolay gelsin :)

Genel Bilgiler
Bir tren rezervasyonu uygulaması için, istenilen rezervasyonunun yapılıp yapılamayacağını ve yapılabiliyorsa hangi vagon için rezervasyon onaylanabileceğini belirleyen bir HTTP API geliştirilecektir.

API, HTTP Post isteklerine yanıt verecektir. 

HTTP API'a tren bilgileri ve kaç kişilik rezervasyon istenildiği gönderilecek, geliştirilecek algoritma rezervasyon yapılıp yapılamayacağı bilgisini dönecektir. 

Gereksinimler
- Bir tren içinde birden fazla vagon bulunabilir

- Her vagonun farklı kişi kapasitesi olabilir

- Online rezervasyonlarda, bir vagonun doluluk kapasitesi %70'i geçmemelidir. Yani vagon kapasitesi 100 ise ve 70 koltuk dolu ise, o vagona rezervasyon yapılamaz.

- Bir rezervasyon isteği içinde birden fazla kişi olabilir.

- Rezervasyon isteği yapılırken, kişilerin farklı vagonlara yerleşip yerleştirilemeyeceği belirtilir. Bazı rezervasyonlarda tüm yolcuların aynı vagonda olması istenilirken, bazılarında farklı vagonlar da kabul edilebilir.

- Rezervasyon yapılabilir durumdaysa, API hangi vagonlara kaçar kişi yerleşeceği bilgisini dönecektir.

API Request ve Response Formatı
Input formatı aşağıdaki gibidir. Rezervasyon istenilen trenin bilgileri, vagon ayrıntıları, kaç kişilik rezervasyon istenildiği ve kişilerin farklı vagonlara yerleştirilip yerleştirilemeyeceği bilgileri input içinde yer alır;

{
    "Tren":
    {
        "Ad":"Başkent Ekspres",
        "Vagonlar":
        [
            {"Ad":"Vagon 1", "Kapasite":100, "DoluKoltukAdet":68},
            {"Ad":"Vagon 2", "Kapasite":90, "DoluKoltukAdet":50},
            {"Ad":"Vagon 3", "Kapasite":80, "DoluKoltukAdet":80}
        ]
    },
    "RezervasyonYapilacakKisiSayisi":3,
    "KisilerFarkliVagonlaraYerlestirilebilir":true
}

Dönüş formatı aşağıdaki gibidir.

{
    "RezervasyonYapilabilir":true,
    "YerlesimAyrinti":[
        {"VagonAdi":"Vagon 1","KisiSayisi":2},
        {"VagonAdi":"Vagon 2","KisiSayisi":1}
    ]
}

Rezervasyon yapılamıyorsa YerlesimAyrinti bos array olacaktır; 

{
    "RezervasyonYapilabilir":true,
    "YerlesimAyrinti":[    ]
}

## Ekran Resimleri ## 

![2](https://github.com/abdks/AdaTaskApi/assets/62968246/e52a267e-2f5c-437a-a5d3-97a88eabb5e8)
![4](https://github.com/abdks/AdaTaskApi/assets/62968246/6a3d50d9-016c-4069-abcf-27470f1e47f4)
![3](https://github.com/abdks/AdaTaskApi/assets/62968246/800903f5-eae5-48e3-be9d-df8e620ae8be)
![5](https://github.com/abdks/AdaTaskApi/assets/62968246/43e378bb-3937-4f40-bbdc-c206ba268871)

