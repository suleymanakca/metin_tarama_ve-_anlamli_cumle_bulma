using System.Data;
using System.Text;
using System.Text.RegularExpressions;

// satirlari aldım
string[] lines = File.ReadAllLines(@"..\..\..\input.txt");

string metin = "";
int x = 0;

// başlıkları cümle gibi düşünmek için boş satırların uerinde nokta koydum. 
for (x = 0;x < lines.Length; x++)
{
    if(lines[x] == "")
    {
        lines[x] = ". ";
    }
    metin += lines[x];  

}

// cümlelerin asıllarını tuttum
DataTable dt_asil_cumleler = new DataTable();
dt_asil_cumleler.Columns.Add("id", typeof(int));
dt_asil_cumleler.Columns.Add("cumle");
string[] asil_cumleler = metin.Split(". ");
DataRow ro_asil_cumler;
for (x = 0; x < asil_cumleler.Length; x++)
{
    ro_asil_cumler = dt_asil_cumleler.NewRow();
    ro_asil_cumler["id"] = x;
    ro_asil_cumler["cumle"] = asil_cumleler[x];
    dt_asil_cumleler.Rows.Add(ro_asil_cumler);
}

// metin içerisinde kelimeleri doğru ayırabilmek için noktalama işareti temizliği yaptım
metin = metin.Replace(',', ' ');
metin = metin.Replace('"', ' ');
metin = metin.Replace('“', ' ');
metin = metin.Replace('\'', ' ');
metin = Regex.Replace(metin, @"\s+", " ");

// cümleleri elde ettim
string[] cumleler = metin.Split(". ");

// cümleleri trimledim
for (x = 0; x < cumleler.Length; x++)
{
    cumleler[x] = cumleler[x].Trim();   
}

// cümle sonunda kalan noktaları temizledim
for (x =0;x < cumleler.Length; x++)
{

    string son_karakter = cumleler[x].Substring(cumleler[x].Length - 1, 1);

    if (cumleler[x].Substring(cumleler[x].Length-1, 1) == ".")
    {
        cumleler[x] = cumleler[x].Substring(0, cumleler[x].Length - 1);
    }
}

metin = "";
// cumleleri boşluk ile birleştirdim, kelimeleri elde edeceğim
for (x = 0; x < cumleler.Length; x++)
{
    if(metin == "")
    {
        metin = cumleler[x];
    }
    else
    {
        metin += " " + cumleler[x];
    }

}

// kellimeleri elde ettim
string[] kelimeler = metin.Split(' ');

// kelimelerin frekansını bulmak için tablo oluşturdum
DataTable DT_kelime = new DataTable();
DT_kelime.Columns.Add("kelime");
DT_kelime.Columns.Add("index",typeof(int));
DT_kelime.Columns.Add("adet",typeof(double));
DT_kelime.Columns.Add("frekans",typeof(double));
DT_kelime.Columns.Add("frekans_kat_sayi",typeof(double));

// kelimelerin sıklığını buldum
int i = 0;
for(x =0;x < kelimeler.Length; x++)
{
    i++;

    DataRow[] ro = DT_kelime.Select("kelime = '" + kelimeler[x] + "'");
    if(ro.Length > 0)
    {
        int index = DT_kelime.Rows.IndexOf(ro[0]);
        DT_kelime.Rows[index]["adet"] =Int16.Parse(DT_kelime.Rows[index]["adet"].ToString()) + 1;
    }
    else
    {
        DataRow dr = DT_kelime.NewRow();
        dr["kelime"] = kelimeler[x];
        dr["index"] = i;
        dr["adet"] = 1;
        DT_kelime.Rows.Add(dr);
    }
}

// frekans çeşitliliğini buldum
var query = from row in DT_kelime.AsEnumerable()
            group row by row.Field<double>("adet") into adet
            orderby adet.Key
            select new
            {
                adet = adet.Key
            };


// ortalama frekansı buldum
double frekans_cesitlilik_toplam = 0;
int frekans_cesitlilik = query.ToList().Count;
double frekans_cesitlilik_ortalama = 0;
foreach (var r in query)
{
    frekans_cesitlilik_toplam += r.adet;

}
frekans_cesitlilik_ortalama = frekans_cesitlilik_toplam / frekans_cesitlilik;

// frekans ortalamadan büyükse katsayısını küçüklerin 2 katı yaptım
for (x =0;x < DT_kelime.Rows.Count; x++)
{
    if(Double.Parse(DT_kelime.Rows[x]["adet"].ToString()) > frekans_cesitlilik_ortalama){
        DT_kelime.Rows[x]["frekans"] = (Double.Parse(DT_kelime.Rows[x]["adet"].ToString()) / DT_kelime.Rows.Count) * 2;
    }
    else
    {
        DT_kelime.Rows[x]["frekans"] = (Double.Parse(DT_kelime.Rows[x]["adet"].ToString()) / DT_kelime.Rows.Count);
    }
}

DT_kelime.DefaultView.Sort = "frekans DESC";

DataTable DT_cumle = new DataTable();
DT_cumle.Columns.Add("id", typeof(int));
DT_cumle.Columns.Add("cumle");
DT_cumle.Columns.Add("kelime_sayisi", typeof(int));
DT_cumle.Columns.Add("toplam_frekans", typeof(double));
DT_cumle.Columns.Add("ortalama_frekans", typeof(double));

// cümlelerin ağırlıklarını buldum
double toplam_kelime_sayisi = 0;
for(x =0; x < cumleler.Length; x++)
{

    string cumle = cumleler[x];
    string[] cumle_kelime = cumle.Split(' ');

    double toplam_freakans = 0;
    double ortalama_frekans = 0;
    double kelime_sayisi = cumle_kelime.Length;
    toplam_kelime_sayisi += kelime_sayisi;

    for(i = 0;i < cumle_kelime.Length; i++) {

        DataRow[] ro = DT_kelime.Select("kelime = '" + cumle_kelime[i] + "'");
        toplam_freakans += ro[0].Field<double>("frekans");
       
    }

    ortalama_frekans = toplam_freakans / kelime_sayisi;

    DataRow dr = DT_cumle.NewRow();
    dr["id"] = x;
    dr["cumle"] = cumle;
    dr["kelime_sayisi"] = kelime_sayisi;
    dr["toplam_frekans"] = toplam_freakans;
    dr["ortalama_frekans"] = ortalama_frekans;
    DT_cumle.Rows.Add(dr);  

}

// ortalama cümle frekansını buldum
double ortalama_cumle_frekans = (double)DT_cumle.Compute("AVG([ortalama_frekans])", "");
// cümle frekansı ortalamadan yüksek olanları buldum, alanı daralttım
double ortalama_kelime_sayisi = toplam_kelime_sayisi / cumleler.Length;
var results = from myRow in DT_cumle.AsEnumerable()
              where myRow.Field<double>("ortalama_frekans") > ortalama_cumle_frekans
              select myRow;


DataTable dt_muhtemel_cumleler = new DataTable();
dt_muhtemel_cumleler = results.CopyToDataTable();

// yukarda elde ettiğim cümlelerin ortalama frekansını buldum
ortalama_cumle_frekans = (double)dt_muhtemel_cumleler.Compute("AVG([ortalama_frekans])", "");
// cümle frekansı ortalama frekanstan büyük olanları buldum
var results2 = from myRow in dt_muhtemel_cumleler.AsEnumerable()
              where myRow.Field<double>("ortalama_frekans") > ortalama_cumle_frekans
              select myRow;

dt_muhtemel_cumleler = results2.CopyToDataTable();
dt_muhtemel_cumleler.DefaultView.Sort = "id ASC";
dt_muhtemel_cumleler.AcceptChanges();

// cümlelerin orjinallerini almak için id'lerini aldım
string ids = "";
for(x =0;x < dt_muhtemel_cumleler.Rows.Count; x++)
{
    if(ids == "")
    {
        ids += dt_muhtemel_cumleler.Rows[x]["id"];
    }
    else
    {
        ids += "," + dt_muhtemel_cumleler.Rows[x]["id"];
    }
}

// en anlamlı cümlelerin id'lerine bakarak orjinallerini aldım ve result.txt'ye yazdırdım
string ozet = "";
DataRow[] ozet_cumleler = dt_asil_cumleler.Select("id IN(" + ids + ")");
StringBuilder sb = new StringBuilder();

for (x =0;x < ozet_cumleler.Length; x++)
{
    sb.AppendLine(ozet_cumleler[x].Field<string>("cumle"));
}

using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"..\..\..\result.txt"))
{
    file.WriteLine(sb.ToString());
}

Console.WriteLine("Metni özetleyen en anlamlı cümleler result.txt dosyasına kaydedilmiştir. Çıkış yapmak için Enter'a basınız.");
Console.ReadLine();