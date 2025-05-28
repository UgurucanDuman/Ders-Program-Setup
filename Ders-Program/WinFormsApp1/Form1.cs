using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
        }

        List<string> dersKodlari = new List<string>();
        List<string> ogretmenler = new List<string>();
        List<string> siniflar = new List<string>();

        // Global program verisi: dosyadan yüklenen ve yeni eklenen tüm dersler
        List<string> globalProgramData = new List<string>();
        // Yerel görünüm: lstProgram'da gösterilen, isteğe bağlı (örn. silme sonucu) değişiklikler içeren liste
        List<string> localProgramData = new List<string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbGun.Items.AddRange(new string[] { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma" });

            cmbSaat.Items.AddRange(new string[]
            {
                "08:00", "09:00", "10:00", "11:00",
                "13:00", "14:00", "15:00", "16:00"
            });

            // Seçim olaylarını ekliyoruz
            cmbSinif.SelectedIndexChanged += cmbSinif_SelectedIndexChanged;

            // CheckedListBox için ItemCheck olayı
            chkDersler.ItemCheck += chkDersler_ItemCheck;

            // Öğretmen dersleri otomatik atama özelliği için olay abonelikleri
            FormInit_OgretmenDersleri();

            VerileriYukle();
        }

        // Öğretmen dersleri ve otomatik atama için gerekli kurulumlar
        private void FormInit_OgretmenDersleri()
        {

            btnOtomatikAta.Click += btnOtomatikAta_Click;
            // Birden fazla ders seçebilme
        }

        private void btnOgretmenEkle_Click(object sender, EventArgs e)
        {
            string ogretmen = txtOgretmen.Text.Trim();
            if (!string.IsNullOrEmpty(ogretmen) && !ogretmenler.Contains(ogretmen))
            {
                ogretmenler.Add(ogretmen);
                cmbOgretmen.Items.Add(ogretmen);
                txtOgretmen.Clear();
            }
            else
            {
                MessageBox.Show("Öğretmen ismi boş olamaz veya zaten eklenmiş.");
            }
        }

        private void btnDersKoduEkle_Click(object sender, EventArgs e)
        {
            string dersKodu = txtDersKodu.Text.Trim().ToUpper();

            if (!string.IsNullOrEmpty(dersKodu) && !dersKodlari.Contains(dersKodu))
            {
                dersKodlari.Add(dersKodu);
                chkDersler.Items.Add(dersKodu);
                txtDersKodu.Clear();
            }
            else
            {
                MessageBox.Show("Ders kodu boş olamaz veya zaten eklenmiş.");
            }
        }

        private void btnSinifEkle_Click(object sender, EventArgs e)
        {
            string sinif = txtSinif.Text.Trim();
            if (!string.IsNullOrEmpty(sinif) && !siniflar.Contains(sinif))
            {
                siniflar.Add(sinif);
                cmbSinif.Items.Add(sinif);
                txtSinif.Clear();
            }
            else
            {
                MessageBox.Show("Sınıf boş olamaz veya zaten eklenmiş.");
            }
        }

        private void btnDersEkle_Click(object sender, EventArgs e)
        {
            string ogretmen = cmbOgretmen.SelectedItem?.ToString() ?? "";
            string ders = "";

            // CheckedListBox'tan seçili ders alınıyor
            if (chkDersler.SelectedItem != null)
            {
                ders = chkDersler.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Lütfen bir ders seçin.");
                return;
            }

            string sinif = cmbSinif.SelectedItem?.ToString() ?? "";
            string gun = cmbGun.SelectedItem?.ToString() ?? "";
            string saat = cmbSaat.SelectedItem?.ToString() ?? "";
            int sure = rdo1Saat.Checked ? 1 : 2;

            if (string.IsNullOrEmpty(ogretmen) || string.IsNullOrEmpty(ders) ||
                string.IsNullOrEmpty(sinif) || string.IsNullOrEmpty(gun) || string.IsNullOrEmpty(saat))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            DateTime baslangic = DateTime.Parse(saat);
            DateTime bitis = baslangic.AddHours(sure);

            // ÇAKIŞMA KONTROLÜ: lstProgram görünümündeki veriler üzerinden kontrol ediyoruz
            foreach (string item in lstProgram.Items)
            {
                string[] parcalar = item.Split('|');
                string listGun = parcalar[1].Trim();
                string saatAraligi = parcalar[2].Trim();
                string listSinif = parcalar[3].Trim();

                if (listGun == gun && listSinif == sinif)
                {
                    string[] saatler = saatAraligi.Split('-');
                    DateTime listBaslangic = DateTime.Parse(saatler[0]);
                    DateTime listBitis = DateTime.Parse(saatler[1]);

                    bool cakismaVar = !(bitis <= listBaslangic || baslangic >= listBitis);

                    if (cakismaVar)
                    {
                        MessageBox.Show($"⚠ Bu sınıf ({sinif}) için {gun} günü saat {saatAraligi} arasında zaten bir ders var.\nYeni dersin saati: {baslangic:HH:mm}-{bitis:HH:mm}",
                            "Zaman Çakışması", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            string satir = $"{ders} | {gun} | {baslangic:HH:mm}-{bitis:HH:mm} | {sinif} | Öğr. {ogretmen}";

            // Yeni ders global veri ve yerel listeye eklenir.
            globalProgramData.Add(satir);
            localProgramData.Add(satir);

            // CheckedListBox'tan seçili derslere göre filtreleme uyguluyoruz
            UygulaCheckListFiltreleme();

            // Seçili sınıfa ait detaylı listeyi de güncelliyoruz.
            if (cmbSinif.SelectedItem != null)
                GuncelleSinifProgrami(cmbSinif.SelectedItem.ToString());
        }

        private void VerileriKaydet()
        {
            File.WriteAllLines("ogretmenler.txt", ogretmenler);
            File.WriteAllLines("dersler.txt", dersKodlari);
            File.WriteAllLines("siniflar.txt", siniflar);

            // Global veriyi kaydediyoruz; böylece silme sadece yerel görünümü etkiler.
            File.WriteAllLines("program.txt", globalProgramData);
        }

        private void VerileriYukle()
        {
            if (File.Exists("ogretmenler.txt"))
            {
                ogretmenler = File.ReadAllLines("ogretmenler.txt").ToList();
                cmbOgretmen.Items.AddRange(ogretmenler.ToArray());
            }

            if (File.Exists("dersler.txt"))
            {
                dersKodlari = File.ReadAllLines("dersler.txt").ToList();
                chkDersler.Items.AddRange(dersKodlari.ToArray());
            }

            if (File.Exists("siniflar.txt"))
            {
                siniflar = File.ReadAllLines("siniflar.txt").ToList();
                cmbSinif.Items.AddRange(siniflar.ToArray());
            }

            if (File.Exists("program.txt"))
            {
                string[] satirlar = File.ReadAllLines("program.txt");
                globalProgramData = satirlar.ToList();
                // Başlangıçta yerel görünümü global verinin aynısı yapıyoruz.
                localProgramData = new List<string>(globalProgramData);
                lstProgram.Items.AddRange(localProgramData.ToArray());
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            VerileriKaydet();
        }

        // "Ders Sil" butonunda, yalnızca lstProgram (yerel görünüm) üzerinden silme yapıyoruz.
        private void btnDersSil_Click(object sender, EventArgs e)
        {
            if (lstProgram.SelectedItem != null)
            {
                DialogResult sonuc = MessageBox.Show("Seçili dersi silmek istediğinizden emin misiniz?",
                                                       "Silme Onayı",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Warning);
                if (sonuc == DialogResult.Yes)
                {
                    string selectedLesson = lstProgram.SelectedItem.ToString();
                    lstProgram.Items.Remove(selectedLesson);
                    localProgramData.Remove(selectedLesson);

                    // Eğer sınıf seçili ise, detaylı listeyi yeniden güncelleyelim (global veriye göre).
                    if (cmbSinif.SelectedItem != null)
                        GuncelleSinifProgrami(cmbSinif.SelectedItem.ToString());
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir ders seçin.");
            }
        }

        // CheckedListBox'taki bir öğe işaretlendiği veya işareti kaldırıldığında çalışır
        private void chkDersler_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // ItemCheck olayı henüz tamamlanmadığı için Invoke ile UI thread'inde biraz gecikme ile çalıştırıyoruz
            BeginInvoke(new Action(() => { UygulaCheckListFiltreleme(); }));
        }

        // CheckedListBox'tan seçili derslere göre filtreleme uygular
        private void UygulaCheckListFiltreleme()
        {
            List<string> secilenDersler = new List<string>();

            // Tüm işaretlenmiş dersleri topluyoruz
            for (int i = 0; i < chkDersler.Items.Count; i++)
            {
                if (chkDersler.GetItemChecked(i))
                {
                    secilenDersler.Add(chkDersler.Items[i].ToString().Trim());
                }
            }

            // Eğer hiçbir ders seçilmemişse tüm program gösterilir
            if (secilenDersler.Count == 0)
            {
                lstProgram.Items.Clear();
                lstProgram.Items.AddRange(localProgramData.ToArray());
                return;
            }

            // Global veriyi seçilen ders kodlarına göre filtreliyoruz
            var filteredItems = globalProgramData.Where(item =>
            {
                string[] parts = item.Split('|');
                if (parts.Length > 0)
                {
                    string derKodu = parts[0].Trim();
                    return secilenDersler.Contains(derKodu);
                }
                return false;
            }).ToList();

            // lstProgram'ı güncelliyoruz
            lstProgram.Items.Clear();
            foreach (var item in filteredItems)
                lstProgram.Items.Add(item);

            // Detaylı liste de güncelleniyor
            if (cmbSinif.SelectedItem != null)
                GuncelleSinifProgrami(cmbSinif.SelectedItem.ToString());
            else
            {
                lstSinifProgrami.Items.Clear();
                foreach (var item in filteredItems)
                    lstSinifProgrami.Items.Add(item);
            }
        }

        // cmbSinif'de sınıf seçildiğinde, detaylı liste global veriden güncellenir.
        private void cmbSinif_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSinif.SelectedItem != null)
            {
                string secilenSinif = cmbSinif.SelectedItem.ToString();
                GuncelleSinifProgrami(secilenSinif);
            }
        }

        // Global veriden, isteğe göre seçilen derslere göre filtrelenmiş ve
        // seçilen sınıfa ait detaylı programı güncelliyoruz.
        private void GuncelleSinifProgrami(string sinif)
        {
            lstSinifProgrami.Items.Clear();
            IEnumerable<string> data = globalProgramData;

            // Eğer ders kodları filtrelenmişse, önce onları uygulayalım
            List<string> secilenDersler = new List<string>();
            for (int i = 0; i < chkDersler.Items.Count; i++)
            {
                if (chkDersler.GetItemChecked(i))
                {
                    secilenDersler.Add(chkDersler.Items[i].ToString().Trim());
                }
            }

            if (secilenDersler.Count > 0)
            {
                data = data.Where(item =>
                {
                    string[] parts = item.Split('|');
                    if (parts.Length > 0)
                    {
                        string dersKodu = parts[0].Trim();
                        return secilenDersler.Contains(dersKodu);
                    }
                    return false;
                });
            }

            // Sadece seçilen sınıfa ait dersleri filtreleyelim.
            data = data.Where(item =>
            {
                string[] parts = item.Split('|');
                return parts.Length >= 4 && parts[3].Trim() == sinif;
            });

            foreach (var item in data)
                lstSinifProgrami.Items.Add(item);
        }

        // Seçilen detaylı derse tıklanınca, bilgiler MessageBox ile gösterilir.
        private void lstSinifProgrami_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSinifProgrami.SelectedItem != null)
            {
                string selectedItem = lstSinifProgrami.SelectedItem.ToString();
                string[] details = selectedItem.Split('|');

                if (details.Length >= 5)
                {
                    string ders = details[0].Trim();
                    string gun = details[1].Trim();
                    string saatAraligi = details[2].Trim();
                    string sinif = details[3].Trim();
                    string ogretmen = details[4].Trim();

                    string mesaj = $"Ders: {ders}\n" +
                                   $"Gün: {gun}\n" +
                                   $"Saat Aralığı: {saatAraligi}\n" +
                                   $"Sınıf: {sinif}\n" +
                                   $"Öğretmen: {ogretmen}";

                    MessageBox.Show(mesaj, "Ders Detayları", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Seçilen derse ait yeterli detay bilgisi bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen detayları görmek istediğiniz bir ders seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnBosSaatleriGoster_Click(object sender, EventArgs e)
        {
            if (cmbSinif.SelectedItem == null || cmbGun.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir sınıf ve gün seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secilenSinif = cmbSinif.SelectedItem.ToString();
            string secilenGun = cmbGun.SelectedItem.ToString();

            lstBosSaatler.Items.Clear();

            var saatler = cmbSaat.Items.Cast<string>().ToList();
            List<string> bosSaatler = new List<string>();

            foreach (var saatStr in saatler)
            {
                DateTime baslangic = DateTime.Parse(saatStr);
                DateTime bitis = baslangic.AddHours(1);

                bool doluMu = globalProgramData.Any(kayit =>
                {
                    var parts = kayit.Split('|');
                    if (parts.Length < 4) return false;

                    string kayitGun = parts[1].Trim();
                    string kayitSaatAraligi = parts[2].Trim();
                    string kayitSinif = parts[3].Trim();

                    if (kayitGun != secilenGun || kayitSinif != secilenSinif)
                        return false;

                    string[] saatParcalari = kayitSaatAraligi.Split('-');
                    if (saatParcalari.Length != 2) return false;

                    DateTime kayitBaslangic = DateTime.Parse(saatParcalari[0]);
                    DateTime kayitBitis = DateTime.Parse(saatParcalari[1]);

                    return !(bitis <= kayitBaslangic || baslangic >= kayitBitis);
                });

                if (!doluMu)
                {
                    string saatAraligi = $"{baslangic:HH:mm}-{bitis:HH:mm}";
                    bosSaatler.Add(saatAraligi);
                }
            }

            if (bosSaatler.Count > 0)
            {
                lstBosSaatler.Items.Add($"📅 {secilenGun} günü için boş saatler:");
                foreach (var saat in bosSaatler)
                {
                    lstBosSaatler.Items.Add($"- {saat}");
                }
            }
            else
            {
                lstBosSaatler.Items.Add($"⚠ {secilenGun} günü için boş saat bulunamadı.");
            }
        }

        // Tüm dersleri seçme/seçimini kaldırma butonu
        private void btnTumDersleriSec_Click(object sender, EventArgs e)
        {
            bool tumunuSec = true;

            // Eğer hepsi seçiliyse, seçimi kaldır
            if (chkDersler.CheckedItems.Count == chkDersler.Items.Count)
            {
                tumunuSec = false;
            }

            for (int i = 0; i < chkDersler.Items.Count; i++)
            {
                chkDersler.SetItemChecked(i, tumunuSec);
            }

            UygulaCheckListFiltreleme();
        }

        // Kalıcı silme - globalProgramData'dan ve yerel görünümden siler
        private void btnKaliciSil_Click(object sender, EventArgs e)
        {
            if (lstProgram.SelectedItem != null)
            {
                DialogResult sonuc = MessageBox.Show("Seçili dersi kalıcı olarak silmek istediğinizden emin misiniz?" +
                                                   "\nBu işlem geri alınamaz!",
                                                   "Kalıcı Silme Onayı",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Warning);

                if (sonuc == DialogResult.Yes)
                {
                    string selectedLesson = lstProgram.SelectedItem.ToString();

                    // Hem yerel hem de global veriden sil
                    lstProgram.Items.Remove(selectedLesson);
                    localProgramData.Remove(selectedLesson);
                    globalProgramData.Remove(selectedLesson);

                    // Görünümü güncelle
                    UygulaCheckListFiltreleme();

                    // Eğer sınıf seçili ise, detaylı listeyi de güncelle
                    if (cmbSinif.SelectedItem != null)
                        GuncelleSinifProgrami(cmbSinif.SelectedItem.ToString());

                    MessageBox.Show("Ders kalıcı olarak silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Lütfen kalıcı silmek için bir ders seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // "Öğretmen Derslerini Göster" butonu için olay işleyicisi
        private void btnOgretmenDersleriGoster_Click(object sender, EventArgs e)
        {
            if (cmbOgretmen.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir öğretmen seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secilenOgretmen = cmbOgretmen.SelectedItem.ToString();

            // Öğretmenin tüm derslerini göster


            // Tüm ders kodlarını ekleyelim, sonra öğretmen bunlardan seçecek
            foreach (object dersKodu in chkDersler.Items)
            {

            }
        }

        // "Seçili Dersleri Otomatik Ata" butonu için olay işleyicisi
        private void btnOtomatikAta_Click(object sender, EventArgs e)
        {
            if (cmbOgretmen.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir öğretmen seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // İşaretlenen dersleri al
            List<string> atanacakDersler = new List<string>();
            for (int i = 0; i < chkDersler.Items.Count; i++)
            {
                if (chkDersler.GetItemChecked(i))
                {
                    atanacakDersler.Add(chkDersler.Items[i].ToString());
                }
            }

            if (atanacakDersler.Count == 0)
            {
                MessageBox.Show("Lütfen en az bir ders seçin (işaretleyin).", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secilenOgretmen = cmbOgretmen.SelectedItem.ToString();
            var gunler = new[] { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma" };
            var saatler = cmbSaat.Items.Cast<string>().ToList();

            int basariliAtama = 0;
            List<string> eklenenDersler = new List<string>();

            foreach (string ders in atanacakDersler)
            {
                bool atandi = false;

                foreach (string sinif in siniflar)
                {
                    foreach (string gun in gunler)
                    {
                        foreach (string saatStr in saatler)
                        {
                            DateTime baslangic = DateTime.Parse(saatStr);
                            DateTime bitis = baslangic.AddHours(1);

                            bool cakismaVar = globalProgramData.Any(kayit =>
                            {
                                var parts = kayit.Split('|');
                                if (parts.Length < 4)
                                    return false;

                                string kayitGun = parts[1].Trim();
                                string kayitSaatAraligi = parts[2].Trim();
                                string kayitSinif = parts[3].Trim();

                                if (kayitGun != gun || kayitSinif != sinif)
                                    return false;

                                string[] saatParcalari = kayitSaatAraligi.Split('-');
                                if (saatParcalari.Length != 2)
                                    return false;

                                DateTime kayitBas = DateTime.Parse(saatParcalari[0]);
                                DateTime kayitBit = DateTime.Parse(saatParcalari[1]);

                                return !(bitis <= kayitBas || baslangic >= kayitBit);
                            });

                            if (!cakismaVar)
                            {
                                string satir = $"{ders} | {gun} | {baslangic:HH:mm}-{bitis:HH:mm} | {sinif} | Öğr. {secilenOgretmen}";
                                globalProgramData.Add(satir);
                                localProgramData.Add(satir);
                                eklenenDersler.Add(satir);
                                basariliAtama++;
                                atandi = true;
                                break;
                            }
                        }
                        if (atandi) break;
                    }
                    if (atandi) break;
                }
            }

            lstProgram.Items.Clear();
            lstProgram.Items.AddRange(localProgramData.ToArray());
            UygulaCheckListFiltreleme();

            MessageBox.Show($"{basariliAtama} ders başarıyla otomatik olarak atandı!\n\nAtanan Dersler:\n{string.Join("\n", eklenenDersler)}",
                "Otomatik Atama", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // Belirli bir gün ve sınıf için boş saatleri bulan yardımcı metod
        private List<string> GetBosSaatler(string gun, string sinif)
        {
            // Ders aralıkları ComboBox'tan alınır
            var saatler = cmbSaat.Items.Cast<string>().ToList();
            List<string> bosSaatler = new List<string>();

            foreach (var saatStr in saatler)
            {
                DateTime baslangic = DateTime.Parse(saatStr);
                DateTime bitis = baslangic.AddHours(1); // 1 saatlik aralık

                bool doluMu = false;

                foreach (string kayit in globalProgramData)
                {
                    var parts = kayit.Split('|');
                    if (parts.Length < 4)
                        continue;

                    string kayitGun = parts[1].Trim();
                    string kayitSaatAraligi = parts[2].Trim();
                    string kayitSinif = parts[3].Trim();

                    if (kayitGun != gun || kayitSinif != sinif)
                        continue;

                    string[] saatParcalari = kayitSaatAraligi.Split('-');
                    if (saatParcalari.Length != 2)
                        continue;

                    DateTime kayitBaslangic = DateTime.Parse(saatParcalari[0]);
                    DateTime kayitBitis = DateTime.Parse(saatParcalari[1]);

                    // ÇAKIŞMA VAR MI KONTROLÜ
                    if (!(bitis <= kayitBaslangic || baslangic >= kayitBitis))
                    {
                        doluMu = true;
                        break;
                    }
                }

                if (!doluMu)
                {
                    string saatAraligi = $"{baslangic:HH:mm}-{bitis:HH:mm}";
                    bosSaatler.Add(saatAraligi);
                }
            }

            return bosSaatler;
        }



        private void btnExcelCiktiAl_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Dosyası|*.xlsx",
                Title = "Programı Excel'e Aktar",
                FileName = "DersProgrami.xlsx"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                MessageBox.Show("Dosya adı belirtilmemiş!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                foreach (string sinif in siniflar)
                {
                    var worksheet = package.Workbook.Worksheets.Add(sinif);

                    var gunler = new[] { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma" };
                    var saatler = cmbSaat.Items.Cast<string>().ToList();

                    // Sol üst köşeye "SAATLER" yaz
                    worksheet.Cells[1, 1].Value = "SAATLER";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    worksheet.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Günleri başlık satırına yaz (B1'den başlayarak)
                    for (int i = 0; i < gunler.Length; i++)
                    {
                        worksheet.Cells[1, i + 2].Value = gunler[i];
                        worksheet.Cells[1, i + 2].Style.Font.Bold = true;
                        worksheet.Cells[1, i + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, i + 2].Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        worksheet.Cells[1, i + 2].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, i + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, i + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    // Saatleri sol sütuna yaz (A2'den başlayarak)
                    for (int i = 0; i < saatler.Count; i++)
                    {
                        DateTime baslangic = DateTime.Parse(saatler[i]);
                        DateTime bitis = baslangic.AddHours(1);
                        string saatAraligi = $"{baslangic:HH:mm}-{bitis:HH:mm}";

                        worksheet.Cells[i + 2, 1].Value = saatAraligi;
                        worksheet.Cells[i + 2, 1].Style.Font.Bold = true;
                        worksheet.Cells[i + 2, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[i + 2, 1].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                        worksheet.Cells[i + 2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[i + 2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    // Ders programını matris olarak doldur
                    for (int saatIndex = 0; saatIndex < saatler.Count; saatIndex++)
                    {
                        DateTime baslangic = DateTime.Parse(saatler[saatIndex]);
                        DateTime bitis = baslangic.AddHours(1);

                        for (int gunIndex = 0; gunIndex < gunler.Length; gunIndex++)
                        {
                            string gun = gunler[gunIndex];
                            int satirNo = saatIndex + 2; // Saat satırı
                            int sutunNo = gunIndex + 2;  // Gün sütunu

                            // Bu saat ve günde ders var mı kontrol et
                            var mevcutDers = globalProgramData.FirstOrDefault(kayit =>
                            {
                                var parts = kayit.Split('|');
                                if (parts.Length < 5)
                                    return false;

                                string kayitGun = parts[1].Trim();
                                string kayitSaatAraligi = parts[2].Trim();
                                string kayitSinif = parts[3].Trim();

                                return kayitGun == gun && kayitSinif == sinif &&
                                       kayitSaatAraligi.Contains(baslangic.ToString("HH:mm"));
                            });

                            if (mevcutDers != null)
                            {
                                var parts = mevcutDers.Split('|');
                                string dersAdi = parts[0].Trim();
                                string ogretmen = parts[4].Trim().Replace("Öğr. ", "");

                                // Ders adı ve öğretmeni birleştir
                                worksheet.Cells[satirNo, sutunNo].Value = $"{dersAdi}\n({ogretmen})";

                                // Dolu saatleri yeşil renkle boyama
                                worksheet.Cells[satirNo, sutunNo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[satirNo, sutunNo].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                                worksheet.Cells[satirNo, sutunNo].Style.Font.Size = 10;
                                worksheet.Cells[satirNo, sutunNo].Style.WrapText = true;
                            }
                            else
                            {
                                worksheet.Cells[satirNo, sutunNo].Value = "BOŞ";

                                // Boş saatleri sarı renkle boyama
                                worksheet.Cells[satirNo, sutunNo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[satirNo, sutunNo].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                                worksheet.Cells[satirNo, sutunNo].Style.Font.Color.SetColor(Color.Gray);
                                worksheet.Cells[satirNo, sutunNo].Style.Font.Size = 9;
                            }

                            // Hücre formatını ayarla
                            worksheet.Cells[satirNo, sutunNo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[satirNo, sutunNo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[satirNo, sutunNo].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[satirNo, sutunNo].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[satirNo, sutunNo].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[satirNo, sutunNo].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }

                    // Başlık satırı ve sütun kenarlıklarını ayarla
                    using (var range = worksheet.Cells[1, 1, 1, gunler.Length + 1])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    }

                    using (var range = worksheet.Cells[2, 1, saatler.Count + 1, 1])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                    }

                    // Sütun genişliklerini ayarla
                    worksheet.Column(1).Width = 15; // Saatler sütunu
                    for (int i = 2; i <= gunler.Length + 1; i++)
                    {
                        worksheet.Column(i).Width = 20; // Gün sütunları
                    }

                    // Satır yüksekliklerini ayarla
                    worksheet.Row(1).Height = 25; // Başlık satırı
                    for (int i = 2; i <= saatler.Count + 1; i++)
                    {
                        worksheet.Row(i).Height = 40; // Ders satırları
                    }

                    // Açıklama ekle
                    int aciklamaSatir = saatler.Count + 3;
                    worksheet.Cells[aciklamaSatir, 1].Value = "AÇIKLAMA:";
                    worksheet.Cells[aciklamaSatir, 1].Style.Font.Bold = true;
                    worksheet.Cells[aciklamaSatir, 1].Style.Font.Size = 12;

                    worksheet.Cells[aciklamaSatir + 1, 1].Value = "Yeşil: Dolu Saatler";
                    worksheet.Cells[aciklamaSatir + 1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[aciklamaSatir + 1, 1].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);

                    worksheet.Cells[aciklamaSatir + 2, 1].Value = "Sarı: Boş Saatler";
                    worksheet.Cells[aciklamaSatir + 2, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[aciklamaSatir + 2, 1].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                }

                // Özet sayfa ekle
                var ozet = package.Workbook.Worksheets.Add("ÖZET");
                ozet.Cells["A1"].Value = "SINIF";
                ozet.Cells["B1"].Value = "TOPLAM DERS SAATİ";
                ozet.Cells["C1"].Value = "BOŞ SAAT SAYİSİ";
                ozet.Cells["D1"].Value = "DOLULUK ORANI (%)";

                using (var range = ozet.Cells["A1:D1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Orange);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thick;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thick;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thick;
                }

                int ozetSatir = 2;
                foreach (string sinif in siniflar)
                {
                    var sinifDersleri = globalProgramData.Where(line =>
                        line.Split('|').Length >= 5 && line.Split('|')[3].Trim() == sinif).ToList();

                    int toplamSlot = 5 * cmbSaat.Items.Count; // 5 gün x saat sayısı
                    int doluSaat = sinifDersleri.Count;
                    int bosSaat = toplamSlot - doluSaat;
                    double dolulukOrani = toplamSlot > 0 ? (double)doluSaat / toplamSlot * 100 : 0;

                    ozet.Cells[ozetSatir, 1].Value = sinif;
                    ozet.Cells[ozetSatir, 2].Value = doluSaat;
                    ozet.Cells[ozetSatir, 3].Value = bosSaat;
                    ozet.Cells[ozetSatir, 4].Value = $"{Math.Round(dolulukOrani, 1)}%";

                    // Satır kenarlıkları
                    using (var range = ozet.Cells[ozetSatir, 1, ozetSatir, 4])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    ozetSatir++;
                }

                ozet.Cells.AutoFitColumns();

                try
                {
                    File.WriteAllBytes(saveFileDialog.FileName, package.GetAsByteArray());
                    MessageBox.Show($"Excel dosyası başarıyla oluşturuldu!\n\n" +
                                  $"Dosya Konumu: {saveFileDialog.FileName}\n\n" +
                                  $"İçerik:\n" +
                                  $"• Her sınıf için ayrı sayfa (matris görünüm)\n" +
                                  $"• Sol sütunda saatler, üst satırda günler\n" +
                                  $"• Dolu saatler (yeşil) ve boş saatler (sarı)\n" +
                                  $"• Özet sayfa ile istatistikler\n" +
                                  $"• Ders ve öğretmen bilgileri birlikte",
                                  "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya kaydedilirken bir hata oluştu:\n" + ex.Message,
                                  "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
    
