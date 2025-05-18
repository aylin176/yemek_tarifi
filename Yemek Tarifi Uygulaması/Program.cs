using System;
using System.Collections.Generic;
using System.Text;

namespace YemekTarifiUygulamasi
{
    public class Malzeme
    {
        public string MalzemeAdi { get; set; }
        public string Miktar { get; set; }

        public Malzeme(string malzemeAdi, string miktar)
        {
            MalzemeAdi = malzemeAdi;
            Miktar = miktar;
        }

        public override string ToString()
        {
            return $"{MalzemeAdi} ({Miktar})";
        }
    }

    
    public class Tarif
    {
        public int TarifId { get; set; }
        public string TarifAdi { get; set; }
        public List<Malzeme> Malzemeler { get; set; }
        public string TarifIcerigi { get; set; }
        public int ToplamPuan { get; set; }
        public int OySayisi { get; set; }
        public double OrtalamaPuan { get { return OySayisi == 0 ? 0 : (double)ToplamPuan / OySayisi; } }

        public Tarif(int tarifId, string tarifAdi, List<Malzeme> malzemeler, string tarifIcerigi)
        {
            TarifId = tarifId;
            TarifAdi = tarifAdi;
            Malzemeler = malzemeler;
            TarifIcerigi = tarifIcerigi;
            ToplamPuan = 0;
            OySayisi = 0;
        }

       
        public void Degerlendir(int puan)
        {
            ToplamPuan += puan;
            OySayisi++;
        }

        public override string ToString()
        {
            string malzemeListesi = string.Join(", ", Malzemeler);
            return $"Tarif ID: {TarifId}\nAd: {TarifAdi}\nMalzemeler: {malzemeListesi}\nIçerik: {TarifIcerigi}\nOrtalama Puan: {OrtalamaPuan:F2} ({OySayisi} oy)";
        }
    }

  
    public class Kullanici
    {
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }

        public Kullanici(string kullaniciAdi, string sifre)
        {
            KullaniciAdi = kullaniciAdi;
            Sifre = sifre;
        }

        public override string ToString()
        {
            return $"Kullanıcı Adı: {KullaniciAdi}";
        }
    }

    
    public class YemekTarifiSistemi
    {
        public List<Tarif> Tarifler { get; set; }
        public List<Kullanici> Kullanicilar { get; set; }
        private int nextTarifId = 1;

        public YemekTarifiSistemi()
        {
            Tarifler = new List<Tarif>();
            Kullanicilar = new List<Kullanici>();
        }

      
        public void TarifEkle()
        {
            Console.Write("Tarif Adı giriniz: ");
            string ad = Console.ReadLine();
            Console.Write("Tarif içeriğini giriniz: ");
            string icerik = Console.ReadLine();

            Console.Write("Kaç malzeme eklemek istiyorsunuz? ");
            if (!int.TryParse(Console.ReadLine(), out int malzemeAdet))
            {
                Console.WriteLine("Geçersiz sayı.");
                return;
            }

            List<Malzeme> malzemeList = new List<Malzeme>();
            for (int i = 0; i < malzemeAdet; i++)
            {
                Console.Write($"Malzeme {i + 1} adı: ");
                string malzemeAdi = Console.ReadLine();
                Console.Write($"Malzeme {i + 1} miktarı: ");
                string miktar = Console.ReadLine();
                malzemeList.Add(new Malzeme(malzemeAdi, miktar));
            }
            Tarif yeniTarif = new Tarif(nextTarifId++, ad, malzemeList, icerik);
            Tarifler.Add(yeniTarif);

            Console.WriteLine("Tarif başarıyla eklendi:");
            Console.WriteLine(yeniTarif);
        }

      
        public void TarifAra()
        {
            Console.Write("Arama kelimesi giriniz: ");
            string kelime = Console.ReadLine();
            bool bulundu = false;
            foreach (var t in Tarifler)
            {
                if (t.TarifAdi.IndexOf(kelime, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.WriteLine(t);
                    Console.WriteLine(new String('-', 25));
                    bulundu = true;
                }
            }
            if (!bulundu)
            {
                Console.WriteLine("Aradığınız kelime ile eşleşen tarif bulunamadı.");
            }
        }

       
        public void TarifDegerlendir()
        {
            Console.Write("Değerlendirmek istediğiniz tarifin ID'sini giriniz: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Geçersiz ID.");
                return;
            }
            Tarif tarif = Tarifler.Find(t => t.TarifId == id);
            if (tarif == null)
            {
                Console.WriteLine("Tarif bulunamadı.");
                return;
            }
            Console.Write("Bu tarife vermek istediğiniz puanı giriniz (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out int puan) || puan < 1 || puan > 5)
            {
                Console.WriteLine("Geçersiz puan. Lütfen 1 ile 5 arasında bir sayı giriniz.");
                return;
            }
            tarif.Degerlendir(puan);
            Console.WriteLine("Tarif güncellendi. Güncel değerlendirme:");
            Console.WriteLine(tarif);
        }

       
        public void TarifleriListele()
        {
            if (Tarifler.Count == 0)
            {
                Console.WriteLine("Eklenmiş tarif bulunmamaktadır.");
                return;
            }
            foreach (var t in Tarifler)
            {
                Console.WriteLine(t);
                Console.WriteLine(new String('-', 25));
            }
        }
    }

   
    class Program
    {
        static void Main(string[] args)
        {
         
            PerformLogin();

            
            YemekTarifiSistemi sistem = new YemekTarifiSistemi();

            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Yemek Tarifi Uygulaması ---");
                Console.WriteLine("1. Tarif Ekle");
                Console.WriteLine("2. Tarif Ara");
                Console.WriteLine("3. Tarif Değerlendir");
                Console.WriteLine("4. Tarifleri Listele");
                Console.WriteLine("5. Çıkış");
                Console.Write("Seçiminiz: ");
                string secim = Console.ReadLine();
                Console.WriteLine();

                switch (secim)
                {
                    case "1":
                        sistem.TarifEkle();
                        break;
                    case "2":
                        sistem.TarifAra();
                        break;
                    case "3":
                        sistem.TarifDegerlendir();
                        break;
                    case "4":
                        sistem.TarifleriListele();
                        break;
                    case "5":
                        Console.WriteLine("Sistemden çıkılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyiniz.");
                        break;
                }

                Console.WriteLine("\nDevam etmek için bir tuşa basınız...");
                Console.ReadKey();
            }
        }

       
        static void PerformLogin()
        {
            int maxAttempts = 3;
            int attempt = 0;
            bool isAuthenticated = false;

            while (attempt < maxAttempts && !isAuthenticated)
            {
                Console.Clear();

                Console.Write("Kullanıcı Adı: ");
                string username = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Kullanıcı adı boş olamaz. Tekrar deneyiniz.\n");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Parola: ");
                string password = ReadPassword();
                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Parola boş olamaz. Tekrar deneyiniz.\n");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

               
                if (username.Equals("aylin", StringComparison.OrdinalIgnoreCase) && password == "bozkurt")
                {
                    isAuthenticated = true;
                    Console.WriteLine("\nGiriş başarılı. Ana menüye yönlendiriliyorsunuz...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nGiriş bilgileri hatalı. Lütfen tekrar deneyiniz.\n");
                    attempt++;
                    if (attempt < maxAttempts)
                    {
                        Console.WriteLine($"Kalan deneme hakkınız: {maxAttempts - attempt}\n");
                        Console.WriteLine("Devam etmek için bir tuşa basınız...");
                        Console.ReadKey();
                    }
                }
            }

            if (!isAuthenticated)
            {
                Console.WriteLine("Çok fazla hatalı giriş denemesi. Program sonlandırılıyor.");
                Environment.Exit(0);
            }
        }

      
        static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password.Remove(password.Length - 1, 1);
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    Console.Write("*");
                    password.Append(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password.ToString();
        }
    }
}