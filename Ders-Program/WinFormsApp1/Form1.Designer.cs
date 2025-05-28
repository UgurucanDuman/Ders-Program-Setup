namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtOgretmen = new TextBox();
            label1 = new Label();
            btnOgretmenEkle = new Button();
            txtDersKodu = new TextBox();
            label3 = new Label();
            bindingSource1 = new BindingSource(components);
            btnDersKoduEkle = new Button();
            cmbOgretmen = new ComboBox();
            txtSinif = new TextBox();
            label2 = new Label();
            btnSinifEkle = new Button();
            cmbSinif = new ComboBox();
            label4 = new Label();
            cmbGun = new ComboBox();
            label5 = new Label();
            cmbSaat = new ComboBox();
            label6 = new Label();
            rdo1Saat = new RadioButton();
            rdo2Saat = new RadioButton();
            btnDersEkle = new Button();
            lstProgram = new ListBox();
            btnDersSil = new Button();
            lstSinifProgrami = new ListBox();
            lstBosSaatler = new ListBox();
            btnBosSaatleriGoster = new Button();
            btnOtomatikAta = new Button();
            chkDersler = new CheckedListBox();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            btnExcelCiktiAl = new Button();
            label10 = new Label();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // txtOgretmen
            // 
            txtOgretmen.Location = new Point(125, 28);
            txtOgretmen.Name = "txtOgretmen";
            txtOgretmen.Size = new Size(100, 23);
            txtOgretmen.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(31, 31);
            label1.Name = "label1";
            label1.Size = new Size(88, 15);
            label1.TabIndex = 1;
            label1.Text = "Öğretmen Ekle:";
            // 
            // btnOgretmenEkle
            // 
            btnOgretmenEkle.Location = new Point(121, 72);
            btnOgretmenEkle.Name = "btnOgretmenEkle";
            btnOgretmenEkle.Size = new Size(104, 23);
            btnOgretmenEkle.TabIndex = 2;
            btnOgretmenEkle.Text = "Öğretmen ekle";
            btnOgretmenEkle.UseVisualStyleBackColor = true;
            btnOgretmenEkle.Click += btnOgretmenEkle_Click;
            // 
            // txtDersKodu
            // 
            txtDersKodu.Location = new Point(125, 113);
            txtDersKodu.Name = "txtDersKodu";
            txtDersKodu.Size = new Size(100, 23);
            txtDersKodu.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(55, 116);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 6;
            label3.Text = "Ders Kodu:";
            // 
            // btnDersKoduEkle
            // 
            btnDersKoduEkle.Location = new Point(125, 159);
            btnDersKoduEkle.Name = "btnDersKoduEkle";
            btnDersKoduEkle.Size = new Size(100, 23);
            btnDersKoduEkle.TabIndex = 8;
            btnDersKoduEkle.Text = "Ders Kodu Ekle";
            btnDersKoduEkle.UseVisualStyleBackColor = true;
            btnDersKoduEkle.Click += btnDersKoduEkle_Click;
            // 
            // cmbOgretmen
            // 
            cmbOgretmen.FormattingEnabled = true;
            cmbOgretmen.Location = new Point(231, 29);
            cmbOgretmen.Name = "cmbOgretmen";
            cmbOgretmen.Size = new Size(121, 23);
            cmbOgretmen.TabIndex = 9;
            // 
            // txtSinif
            // 
            txtSinif.Location = new Point(125, 203);
            txtSinif.Name = "txtSinif";
            txtSinif.Size = new Size(100, 23);
            txtSinif.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(59, 206);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 11;
            label2.Text = "Sınıf Ekle :";
            // 
            // btnSinifEkle
            // 
            btnSinifEkle.Location = new Point(125, 247);
            btnSinifEkle.Name = "btnSinifEkle";
            btnSinifEkle.Size = new Size(75, 23);
            btnSinifEkle.TabIndex = 12;
            btnSinifEkle.Text = "Sınıf Ekle";
            btnSinifEkle.UseVisualStyleBackColor = true;
            btnSinifEkle.Click += btnSinifEkle_Click;
            // 
            // cmbSinif
            // 
            cmbSinif.FormattingEnabled = true;
            cmbSinif.Location = new Point(231, 203);
            cmbSinif.Name = "cmbSinif";
            cmbSinif.Size = new Size(121, 23);
            cmbSinif.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(90, 279);
            label4.Name = "label4";
            label4.Size = new Size(32, 15);
            label4.TabIndex = 14;
            label4.Text = "Gün:";
            // 
            // cmbGun
            // 
            cmbGun.FormattingEnabled = true;
            cmbGun.Location = new Point(125, 276);
            cmbGun.Name = "cmbGun";
            cmbGun.Size = new Size(100, 23);
            cmbGun.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(31, 329);
            label5.Name = "label5";
            label5.Size = new Size(88, 15);
            label5.TabIndex = 16;
            label5.Text = "Başlangıc Saati:";
            // 
            // cmbSaat
            // 
            cmbSaat.FormattingEnabled = true;
            cmbSaat.Location = new Point(125, 321);
            cmbSaat.Name = "cmbSaat";
            cmbSaat.Size = new Size(121, 23);
            cmbSaat.TabIndex = 17;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(73, 363);
            label6.Name = "label6";
            label6.Size = new Size(33, 15);
            label6.TabIndex = 18;
            label6.Text = "Süre:";
            // 
            // rdo1Saat
            // 
            rdo1Saat.AutoSize = true;
            rdo1Saat.Location = new Point(125, 363);
            rdo1Saat.Name = "rdo1Saat";
            rdo1Saat.Size = new Size(56, 19);
            rdo1Saat.TabIndex = 19;
            rdo1Saat.TabStop = true;
            rdo1Saat.Text = "1 Saat";
            rdo1Saat.UseVisualStyleBackColor = true;
            // 
            // rdo2Saat
            // 
            rdo2Saat.AutoSize = true;
            rdo2Saat.Location = new Point(190, 363);
            rdo2Saat.Name = "rdo2Saat";
            rdo2Saat.Size = new Size(56, 19);
            rdo2Saat.TabIndex = 20;
            rdo2Saat.TabStop = true;
            rdo2Saat.Text = "2 Saat";
            rdo2Saat.UseVisualStyleBackColor = true;
            // 
            // btnDersEkle
            // 
            btnDersEkle.Location = new Point(73, 410);
            btnDersEkle.Name = "btnDersEkle";
            btnDersEkle.Size = new Size(75, 23);
            btnDersEkle.TabIndex = 21;
            btnDersEkle.Text = "Ders Ekle";
            btnDersEkle.UseVisualStyleBackColor = true;
            btnDersEkle.Click += btnDersEkle_Click;
            // 
            // lstProgram
            // 
            lstProgram.FormattingEnabled = true;
            lstProgram.ItemHeight = 15;
            lstProgram.Location = new Point(375, 44);
            lstProgram.Name = "lstProgram";
            lstProgram.Size = new Size(366, 334);
            lstProgram.TabIndex = 22;
            // 
            // btnDersSil
            // 
            btnDersSil.Location = new Point(154, 410);
            btnDersSil.Name = "btnDersSil";
            btnDersSil.Size = new Size(75, 23);
            btnDersSil.TabIndex = 23;
            btnDersSil.Text = "Dersi Sil ";
            btnDersSil.UseVisualStyleBackColor = true;
            btnDersSil.Click += btnDersSil_Click;
            // 
            // lstSinifProgrami
            // 
            lstSinifProgrami.FormattingEnabled = true;
            lstSinifProgrami.ItemHeight = 15;
            lstSinifProgrami.Location = new Point(747, 44);
            lstSinifProgrami.Name = "lstSinifProgrami";
            lstSinifProgrami.Size = new Size(353, 334);
            lstSinifProgrami.TabIndex = 24;
            // 
            // lstBosSaatler
            // 
            lstBosSaatler.FormattingEnabled = true;
            lstBosSaatler.ItemHeight = 15;
            lstBosSaatler.Location = new Point(1106, 44);
            lstBosSaatler.Name = "lstBosSaatler";
            lstBosSaatler.Size = new Size(295, 334);
            lstBosSaatler.TabIndex = 25;
            // 
            // btnBosSaatleriGoster
            // 
            btnBosSaatleriGoster.Location = new Point(232, 410);
            btnBosSaatleriGoster.Name = "btnBosSaatleriGoster";
            btnBosSaatleriGoster.Size = new Size(108, 23);
            btnBosSaatleriGoster.TabIndex = 26;
            btnBosSaatleriGoster.Text = "BosSaatleriGoster";
            btnBosSaatleriGoster.UseVisualStyleBackColor = true;
            btnBosSaatleriGoster.Click += btnBosSaatleriGoster_Click;
            // 
            // btnOtomatikAta
            // 
            btnOtomatikAta.Location = new Point(346, 410);
            btnOtomatikAta.Name = "btnOtomatikAta";
            btnOtomatikAta.Size = new Size(128, 23);
            btnOtomatikAta.TabIndex = 29;
            btnOtomatikAta.Text = "Otomatik Ders Ata";
            btnOtomatikAta.UseVisualStyleBackColor = true;
            // 
            // chkDersler
            // 
            chkDersler.FormattingEnabled = true;
            chkDersler.Location = new Point(232, 106);
            chkDersler.Name = "chkDersler";
            chkDersler.Size = new Size(120, 58);
            chkDersler.TabIndex = 27;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(375, 26);
            label7.Name = "label7";
            label7.Size = new Size(191, 15);
            label7.TabIndex = 30;
            label7.Text = "Tüm Dersler / Ders Kodu Filtreleme";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(747, 26);
            label8.Name = "label8";
            label8.Size = new Size(142, 15);
            label8.TabIndex = 31;
            label8.Text = "Seçilen Sınıf ve Ders Kodu";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(1106, 26);
            label9.Name = "label9";
            label9.Size = new Size(96, 15);
            label9.TabIndex = 32;
            label9.Text = "Boş Ders Saatleri ";
            // 
            // btnExcelCiktiAl
            // 
            btnExcelCiktiAl.Location = new Point(480, 410);
            btnExcelCiktiAl.Name = "btnExcelCiktiAl";
            btnExcelCiktiAl.Size = new Size(86, 23);
            btnExcelCiktiAl.TabIndex = 33;
            btnExcelCiktiAl.Text = "Excal aktar";
            btnExcelCiktiAl.UseVisualStyleBackColor = true;
            btnExcelCiktiAl.Click += btnExcelCiktiAl_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(626, 476);
            label10.Name = "label10";
            label10.Size = new Size(263, 15);
            label10.TabIndex = 34;
            label10.Text = "MUSA VE UĞURCAN TARAFINDAN YAPILMIŞTIR ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1416, 500);
            Controls.Add(label10);
            Controls.Add(btnExcelCiktiAl);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(btnOtomatikAta);
            Controls.Add(chkDersler);
            Controls.Add(btnBosSaatleriGoster);
            Controls.Add(lstBosSaatler);
            Controls.Add(lstSinifProgrami);
            Controls.Add(btnDersSil);
            Controls.Add(lstProgram);
            Controls.Add(btnDersEkle);
            Controls.Add(rdo2Saat);
            Controls.Add(rdo1Saat);
            Controls.Add(label6);
            Controls.Add(cmbSaat);
            Controls.Add(label5);
            Controls.Add(cmbGun);
            Controls.Add(label4);
            Controls.Add(cmbSinif);
            Controls.Add(btnSinifEkle);
            Controls.Add(label2);
            Controls.Add(txtSinif);
            Controls.Add(cmbOgretmen);
            Controls.Add(btnDersKoduEkle);
            Controls.Add(label3);
            Controls.Add(txtDersKodu);
            Controls.Add(btnOgretmenEkle);
            Controls.Add(label1);
            Controls.Add(txtOgretmen);
            Name = "Form1";
            Text = "Girne Üniversitesi Ders Programı Uygulaması";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private TextBox txtOgretmen;
        private Label label1;
        private Button btnOgretmenEkle;
        private TextBox txtDersKodu;
        private Label label3;
        private BindingSource bindingSource1;
        private Button btnDersKoduEkle;
        private ComboBox cmbOgretmen;
        private TextBox txtSinif;
        private Label label2;
        private Button btnSinifEkle;
        private ComboBox cmbSinif;
        private Label label4;
        private ComboBox cmbGun;
        private Label label5;
        private ComboBox cmbSaat;
        private Label label6;
        private RadioButton rdo1Saat;
        private RadioButton rdo2Saat;
        private Button btnDersEkle;
        private ListBox lstProgram;
        private Button btnDersSil;
        private ListBox lstSinifProgrami;
        private ListBox lstBosSaatler;
        private Button btnBosSaatleriGoster;
        private Button btnOtomatikAta;
        private CheckedListBox chkDersler;
        private Label label7;
        private Label label8;
        private Label label9;
        private Button btnExcelCiktiAl;
        private Label label10;
    }
}
