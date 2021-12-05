# METİN TARAMA VE ANLAMI CÜMLE BULMA
Not: Proje C# dilinde Visual Studio 2022 kullanarak .NET 6.0 ile yapılmıştır.

# AÇIKLAMA

Online ortamda bulunan haber içeriklerinin hızlıca taranabilmesi için otomatik döküman özetlerinin çıkarılması amaçlanmaktadır. Buna göre her bir dökümanı en fazla ifade eden cümleler 
dökümandan ayrıştırılarak dökümanların tamamının okunmasına gerek kalmadan gerekli bilgiye erişilmesi sağlanacaktır.

Çözüm:
* Metin noktalama işaretleri(. hariç) temizlendi.
* Kelimelerin metin içerisindeki frekansı bulundu.
* Kaç çeşit frekans olduğu tespit edildi.
* Bulunan frekans çeşitliliğinin ortalaması alındı ve ortalamadan yüksek olanların frekansları katsayısını arttırmak için 2 ile çarpıldı.
* Cümleler içerdiği kelimelerin frekansına göre ağırlıklandırıldı.
* Ortalama ağırlıktan yüksek olan cümleler tespit edildi.
* Bulunan cümlelerın yine ağırlık ortalaması alındı ve ortalamadan yüksek olanlar tespit edildi.
* Tespit edilen cümleler result.txt'ye yazdırıldı. 


# DERLEME VE ÇALIŞTIRMA

Proje, Console Application tipinde oluşturulmuştur. Visual Studio ile doğrudan çalıştırabilirsiniz.

# SONUÇ

Verilen dökümanı en fazla ifade eden cümleler kelime ağırlıklarına göre tespit edilmiştir.
