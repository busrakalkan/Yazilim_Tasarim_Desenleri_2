using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        //state kullanımı
        #region stateDeseni
        public interface HesapDurumu
        {
            int ucret();
            string aciklama();
        }
        public class TamBilet : HesapDurumu
        {
            public int ucret()
            {
                return 30;
            }
            public string aciklama()
            {
                return "Tam";
            }
        }
        public class OgrenciBilet : HesapDurumu
        {
            public int ucret()
            {
                return 25;
            }
            public string aciklama()
            {
                return "Öğrenci";
            }
        }
        public class Hesap
        {

            private HesapDurumu hesapDurumu;

            public Hesap()
            {
                hesapDurumu = new TamBilet();
            }

            public int ödeme()
            {
                return hesapDurumu.ucret();
            }
            public string aciklama()
            {
                return "bilet seçimi:" + hesapDurumu.aciklama();

            }

            public void durumDeğiştir(HesapDurumu hesapDurumu)
            {
                this.hesapDurumu = hesapDurumu;
            }
        } 
        #endregion

        //template kullanımı

        enum BiletTipi
        {
            Promosyon, Ekonomi
        }
        abstract class Alisveris
        {
            protected BiletTipi biletTipi;
            void Bitir() => MessageBox.Show($"{biletTipi}  tipi bilet seçildi");
            abstract public void Sefer();
            public void TemplateMethod()
            {
                Sefer();
                Bitir();
            }
        }
        class PromosyonBilet : Alisveris
        {
            public override void Sefer()
            {
                biletTipi = BiletTipi.Promosyon;
                
            }
        }
        class EcoBilet : Alisveris
        {
            public override void Sefer()
            {
                biletTipi = BiletTipi.Ekonomi;
            }
        }

       //command kullanımı
        public class Kayıt
        {
            public int TelNum { get; set; }
            public string AdSoyad { get; set; }
            
        }
        public class ReceiverKayıt
        {
            private Kayıt _EntityKayıt;

            public ReceiverKayıt(Kayıt entityKayıt)
            {
                this._EntityKayıt = entityKayıt;
            }

            public void Ekle()
            {
                MessageBox.Show($"{_EntityKayıt.AdSoyad}  İsimli Kullanıcı Eklendi.");
            }

            public void Sil()
            {
                MessageBox.Show($"Telefon Numarası:{_EntityKayıt.TelNum} Olan Müşteri Silindi.");
            }
        }
        public abstract class CommandKayıt
        {
            protected ReceiverKayıt _receiverKayıt;
            public CommandKayıt(ReceiverKayıt receiverKayıt)
            {
                this._receiverKayıt = receiverKayıt;
            }

            public abstract void Execute();
        }
        public class ConcreteCommandKayıtEkle : CommandKayıt
        {
            public ConcreteCommandKayıtEkle(ReceiverKayıt receiverKayıt)
                : base(receiverKayıt)
            {

            }

            public override void Execute()
            {
                _receiverKayıt.Ekle();
            }
        }
        public class ConcreteCommandKayıtSil : CommandKayıt
        {
            public ConcreteCommandKayıtSil(ReceiverKayıt receiverKayıt)
                : base(receiverKayıt)
            {

            }

            public override void Execute()
            {
                _receiverKayıt.Sil();
            }
        }
        public class InvokerKayıt
        {
            public List<CommandKayıt> CommandKayıtList { get; set; }

            public InvokerKayıt()
            {
                CommandKayıtList = new List<CommandKayıt>();
            }

            public void ExecuteAll()
            {
                if (CommandKayıtList.Count == 0)
                    return;

                foreach (CommandKayıt item in CommandKayıtList)
                {
                    item.Execute();
                }
            }
        }

        //adaptör kullanımı
        public interface DenizOtobüsü
        {
            void feribot();
            string gemitipi();
        }
        public interface ArabalıFeribot
        {
            void arabalıferibot();
            string gemitipi();
        }
        public class DO1145 : DenizOtobüsü
        {
            public void feribot()
            {
                MessageBox.Show("deniz otobüsü seçildi");
            }

            public string gemitipi()
            {
                return "Deniz Otobüsü";
            }

        }
        public class AF745 : ArabalıFeribot
        {
            public void arabalıferibot()
            {
                MessageBox.Show("arabalı feribot seçildi");
            }

            public string gemitipi()
            {
                return "Arabalı Feribot";
            }

        }
        public class AfAdapter : DenizOtobüsü
        {
            ArabalıFeribot turkey;


            public AfAdapter(ArabalıFeribot turkey)
            {
                this.turkey = turkey;
            }

            public void feribot()
            {
                turkey.arabalıferibot();
            }

            public string gemitipi()
            {
                return turkey.gemitipi();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 2000;
            timer1.Enabled = true;
            pictureBox1.Image = ımageList1.Images[0];
        }

 
        

        private void button1_Click_1(object sender, EventArgs e)
        {
            Hesap hesap = new Hesap();
            int ıd = Convert.ToInt32(textBox2.Text.ToString());
            string c = textBox1.Text;

            Kayıt Kisi = new Kayıt { TelNum = ıd, AdSoyad = c };
            ReceiverKayıt rk1 = new ReceiverKayıt(Kisi);
            CommandKayıt ckAdd = new ConcreteCommandKayıtEkle(rk1);
            InvokerKayıt ik = new InvokerKayıt();
            ik.CommandKayıtList.Add(ckAdd);
            ik.ExecuteAll();

            AF745 arbl = new AF745();
            DenizOtobüsü AfAdapter = new AfAdapter(arbl);
            textBox3.Text = arbl.gemitipi();
            DO1145 do11 = new DO1145();
            textBox4.Text = do11.gemitipi();
            

            if (radioButton1.Checked)
            {
                int a,b;
                a = hesap.ödeme()-1;
                b = a + 2;
                radioButton3.Text = a.ToString();
                radioButton5.Text = a.ToString();

                radioButton4.Text = b.ToString();
                radioButton6.Text = b.ToString();

            }

            if (radioButton2.Checked)
            {
                hesap.durumDeğiştir(new OgrenciBilet());
                int a, b;
                a = hesap.ödeme() - 1;
                b = a + 2;
                radioButton3.Text = a.ToString();
                radioButton5.Text = a.ToString();

                radioButton4.Text = b.ToString();
                radioButton6.Text = b.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           if(radioButton3.Checked)
            {
                MessageBox.Show($"{groupBox2.Text} sefer saatli {textBox3.Text}"+ "\n" +$"{label1.Text} tipi " + "\n" + $"{radioButton3.Text} tl ücretli bilet kaydedildi");
            }
            if (radioButton4.Checked)
            {
                MessageBox.Show($"{groupBox2.Text} sefer saatli {textBox3.Text}" + "\n" + $"{label2.Text} tipi " + "\n" + $"{radioButton4.Text} tl ücretli bilet kaydedildi");
            }
            if (radioButton5.Checked)
            {
                MessageBox.Show($"{groupBox3.Text} sefer saatli {textBox4.Text}" + "\n" + $"{label2.Text} tipi " + "\n" + $"{radioButton5.Text} tl ücretli bilet kaydedildi");
            }
            if (radioButton6.Checked)
            {
                MessageBox.Show($"{groupBox3.Text} sefer saatli {textBox4.Text}" + "\n" + $"{label2.Text} tipi " + "\n" + $"{radioButton6.Text} tl ücretli bilet kaydedildi");
            }

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Alisveris a1 = new PromosyonBilet();
            a1.TemplateMethod();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Alisveris a1 = new PromosyonBilet();
            a1.TemplateMethod();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Alisveris a1 = new EcoBilet();
            a1.TemplateMethod();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Alisveris a1 = new EcoBilet();
            a1.TemplateMethod();
        }

        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;
            if (i == 5)
                i = 0;
            pictureBox1.Image = ımageList1.Images[i];
        }
    }
    
}