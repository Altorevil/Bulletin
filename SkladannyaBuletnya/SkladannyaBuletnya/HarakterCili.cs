using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;

namespace SkladannyaBuletnya
{
    public partial class HarakterCili : Form
    {      

        public HarakterCili()
        {
            InitializeComponent();            
        }

        public int Vc;
        public int Fc6;
        public int Gc6;
        public int Fc16;
        public int Gc16;
        public int FcMore;
        public int GcMore;
        public int Sy;
        public int Zc;
        public int Vs;
        public int So;
        public int Np;
        public int Yk;
        public int Zc1;
        public int Zc2;

        String _harCil = " ";

        private void label_MouseEnter(object sender, EventArgs e)
        {
            Label l = (Label)sender;                   
            l.ForeColor = Color.Maroon;
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            Label l = (Label)sender;            
            l.ForeColor = Color.Black;
        }        

        private void label_Click(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            _harCil = l.Text;
            DataReader(l);
            Close();
        }

        public void LabelPsiUvChange(ref Label label) //Виведення тексту обраного характеру цілі до основної форми
        {            
            label.Text = _harCil;            
        }

        public void DataReader(Label label)
        {
            int vc, fc6, gc6, fc16, gc16, fcMore, gcMore, sy, zc, vs, so, np, yk, zc1, zc2;
            using (ExcelPackage p = new ExcelPackage())
            {
                using (FileStream stream = new FileStream(@"D:\Study\Воєнна кафедра\Складання бюлетеня\SkladannyaBuletnya\SkladannyaBuletnya\lib\Витрата снарядів.xlsx", FileMode.Open))
                    p.Load(stream);
                ExcelWorksheet ws = p.Workbook.Worksheets["Лист1"];
                int[] data = new int[15];
                int counter = 0;
                for (int i = 11; i <= 35; i++)
                {
                    if (label.TabIndex == Convert.ToInt32(ws.Cells[i, 1].Value))
                    {
                        for (int j = 4; j <= 18; j++)
                        {
                            string text = ws.Cells[i, j].Value.ToString();
                            data[counter] = Convert.ToInt32(text);
                            counter++;
                        } //Цикл j
                    }
                } //Цикл i
                vc = data[0];
                fc6 = data[1];
                gc6 = data[2];
                fc16 = data[3];
                gc16 = data[4];
                fcMore = data[5];
                gcMore = data[6];
                sy = data[7];
                zc = data[8];
                vs = data[9];
                so = data[10];
                np = data[11];
                yk = data[12];
                zc1 = data[13];
                zc2 = data[14];
                new DannieCeli(vc, fc6, gc6, fc16, gc16, fcMore, gcMore, sy, zc, vs, so, np, yk, zc1, zc2);
                textBox1.Text = Convert.ToString(zc2);
            }
        }
    }
}
