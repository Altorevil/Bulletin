using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using OfficeOpenXml;
using SkladannyaBuletnya.Properties;
using SkladannyaBuletnya.lib;

namespace SkladannyaBuletnya
{
    

    public partial class Meteonablyzheniy : Form
    {
        DateTime dT = DateTime.Now;

        public Meteonablyzheniy()
        {
            InitializeComponent();
            
            WindowState = FormWindowState.Maximized;
            
            dataGridView1.Rows.Add("Δhri");
            dataGridView1.Rows.Add("Yігр гл.осн.");
            dataGridView1.Rows.Add("δΔV0грі, %V0");
            dataGridView1.Rows.Add("ΔT3, 0C");
            dataGridView1.Rows.Add("Δq, в/зн");
            dataGridView1.Rows.Add("Невідповідність кута підвищення");
            dataGridView1.Rows.Add("На відхилення лінії прицілювання");
            dataGridView1.Rows.Add("Δd");
            dataGridView1.Rows.Add("Δl");
            dataGridView1.Rows.Add("Кути укриття:");
            dataGridView1.Rows.Add("Ліворуч");
            dataGridView1.Rows.Add("Прямо");
            dataGridView1.Rows.Add("Праворуч");
            dataGridView1.Rows[9].ReadOnly = true;

            dataGridView2.Rows.Add("Ліворуч");
            dataGridView2.Rows.Add("Прямо");
            dataGridView2.Rows.Add("Праворуч");
           
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            textDay1.Text = textDay.Text = Convert.ToString(dT.Day); //Автоматичне виведення системного часу до textBox
            textHours1.Text = textHours.Text = Convert.ToString(dT.Hour);
            textMinutes1.Text = textMinutes.Text = Convert.ToString(dT.Minute);

            //for meteoseredny
            name_of_station.Text = Convert.ToString("1104");
            textDate.Text = Convert.ToString("22093");
            text_hmc.Text = Convert.ToString("1800");
            text_delH_delt0.Text = Convert.ToString("64858");

            value_t_dlW_Wy_1.Text = Convert.ToString("593907");
            value_t_dlW_Wy_2.Text = Convert.ToString("603908");
            value_t_dlW_Wy_3.Text = Convert.ToString("614107");
            value_t_dlW_Wy_4.Text = Convert.ToString("634110");
            value_t_dlW_Wy_5.Text = Convert.ToString("644210");
            value_t_dlW_Wy_6.Text = Convert.ToString("654412");
            value_t_dlW_Wy_7.Text = Convert.ToString("664413");
            value_t_dlW_Wy_8.Text = Convert.ToString("674514");
            value_t_dlW_Wy_9.Text = Convert.ToString("674614");
            value_t_dlW_Wy_10.Text = Convert.ToString("674715");
            value_t_dlW_Wy_11.Text = Convert.ToString("674715");

            //  приховання кнопок вкладок tabControl2 
            tabControl2.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl2.Appearance = TabAppearance.Buttons;
            tabControl2.ItemSize = new System.Drawing.Size(0, 1);
            tabControl2.SizeMode = TabSizeMode.Fixed;
            tabControl2.TabStop = false;
        }

        #region Метеонаближений

        private void buttonDMK_Click(object sender, EventArgs e) //Виклик форми "ДМК"
        {
            buttonDMK.BackColor = Color.LightCoral;
            buttonVR2.BackColor = Color.WhiteSmoke;
            buttonMeteoSer.BackColor = Color.WhiteSmoke;

            this.tabControl2.SelectedTab = this.tabPage4;

            tabPage4.Enabled = true;
            tabPage6.Enabled = false; ;
            tabPage7.Enabled = false;         
        }

        private void buttonVR2_Click(object sender, EventArgs e) //Виклик форми "ВР-2"
        {
            buttonVR2.BackColor = Color.LightCoral;
            buttonDMK.BackColor = Color.WhiteSmoke;
            buttonMeteoSer.BackColor = Color.WhiteSmoke;

            this.tabControl2.SelectedTab = this.tabPage6;

            tabPage4.Enabled = false;
            tabPage6.Enabled = true;
            tabPage7.Enabled = false;
        }

        private void buttonMeteoSer_Click(object sender, EventArgs e) //Виклик форми "Метеосередній"
        {
            buttonMeteoSer.BackColor = Color.LightCoral;
            buttonVR2.BackColor = Color.WhiteSmoke;
            buttonDMK.BackColor = Color.WhiteSmoke;

            this.tabControl2.SelectedTab = this.tabPage7;

            tabPage4.Enabled = false;
            tabPage6.Enabled = false;
            tabPage7.Enabled = true;
         }

        // for DMK
        private void DMK_Shown(object sender, EventArgs e)
        {
            textDay.Focus();
        }


        private void buttonSkladBul_Click(object sender, EventArgs e) //Скласти бюлетень для ДМК
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                int day = Convert.ToInt32(textDay.Text);

                int hours = Convert.ToInt32(textHours.Text);

                Double minutes = Convert.ToDouble(textMinutes.Text);
                minutes = Math.Round(minutes / 10);

                int Hmc = Convert.ToInt32(textHmc.Text);

                int H0 = Convert.ToInt32(textH0.Text);

                int t0 = Convert.ToInt32(textT0.Text);

                Double aW0 = Convert.ToDouble(textAW0.Text);
                aW0 = Math.Round(aW0 / 6); //Переведення в радіани

                Double[] delTauY = new Double[9];
                Double delH0 = H0 - 750;
                Double delH0V = delH0;
                if (delH0 < 0)
                {
                    delH0V = 500.0d + Math.Abs(delH0V);
                }

                Double delTv = Table1.GetDelTv(t0);
                Double tau0 = t0 + delTv;
                Double delTau0mp = tau0 - 15.9;
                Double delTau0mpV = delTau0mp;
                if (delTau0mpV < 0)
                {
                    delTau0mpV = 50.0d + Math.Abs(delTau0mpV);
                }

                Double[] awy = new Double[9];
                Double[] wy = new Double[9];

                Table5 tf = new Table5();
                delTauY = tf.DelForOutput(delTau0mp);
                Double[] Y = tf.Y;

                Double W0 = Convert.ToInt32(textW0.Text);
                awy = tf.AwForOut(aW0);
                if (W0 == 0 || W0 == 1)
                {
                    for (int i = 0; i < wy.Length; i++)
                    {
                        wy[i] = 0;
                    }
                }
                else
                {
                    wy = tf.WyForOut(W0);
                }
                textBulletin.Clear();
                textBulletin.Text = ("Метеонаближений - ");
                textBulletin.Text += Convert.ToString(day.ToString("00"));
                textBulletin.Text += (hours.ToString("00"));
                textBulletin.Text += (minutes.ToString("0") + " - ");
                textBulletin.Text += (Hmc.ToString("0000") + " - ");
                textBulletin.Text += (delH0V.ToString("000"));
                textBulletin.Text += (delTau0mpV.ToString("00") + " - ");
                for (int i = 0; i < 9; i++)
                {
                    Y[i] = Y[i] / 100;
                    textBulletin.Text += Convert.ToString("\r\n" + Y[i].ToString("00") + "-" + Math.Round(delTauY[i]).ToString("00") + Math.Round(awy[i]).ToString("00") + Math.Round(wy[i]).ToString("00") + "- ");
                }

                // buttonApplyDMK.Enabled = true;
            }
        }

        

        private void button33_Click(object sender, EventArgs e) //Скласти бюлетень для ВР-2
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {

                int day = Convert.ToInt32(textDay1.Text);

                int hours = Convert.ToInt32(textHours1.Text);

                Double minutes = Convert.ToDouble(textMinutes1.Text);
                minutes = Math.Round(minutes / 10);

                int Hmc = Convert.ToInt32(textHmc1.Text);

                int H0 = Convert.ToInt32(textH0_1.Text);

                int t0 = Convert.ToInt32(textT0_1.Text);

                Double aW0 = Convert.ToDouble(textAW0_1.Text);
                aW0 = Math.Round(aW0 / 6); //Переведення в радіани

                Double[] delTauY = new Double[9];
                Double delH0 = H0 - 750;
                Double delH0V = delH0;
                if (delH0 < 0)
                {
                    delH0V = 500.0d + Math.Abs(delH0V);
                }

                Double delTv = Table1.GetDelTv(t0);
                Double tau0 = t0 + delTv;
                Double delTau0mp = tau0 - 15.9;
                Double delTau0mpV = delTau0mp;
                if (delTau0mpV < 0)
                {
                    delTau0mpV = 50.0d + Math.Abs(delTau0mpV);
                }

                Double[] awy = new Double[9];
                Double[] wy = new Double[9];

                Table5 tf = new Table5();
                TableVR2 tvr2 = new TableVR2();

                delTauY = tf.DelForOutput(delTau0mp);
                Double[] Y = tf.Y;

                Double dzk = Convert.ToInt32(textDalnZnosuKul.Text);
                awy = tvr2.AwForOut(aW0);
                if (dzk >= 0 && dzk < 40)
                {
                    for (int i = 0; i < wy.Length; i++)
                    {
                        wy[i] = 0;
                    }
                }
                else
                {
                    wy = tvr2.WyForOut(dzk);
                }

                //Виведення бюлетня
                textBulletin.Clear();
                textBulletin.Text = ("Метеонаближений - ");
                textBulletin.Text += Convert.ToString(day.ToString("00"));
                textBulletin.Text += (hours.ToString("00"));
                textBulletin.Text += (minutes.ToString("0") + " - ");
                textBulletin.Text += (Hmc.ToString("0000") + " - ");
                textBulletin.Text += (delH0V.ToString("000"));
                textBulletin.Text += (delTau0mpV.ToString("00") + " - ");
                for (int i = 0; i < 9; i++)
                {
                    Y[i] = Y[i] / 100;
                    textBulletin.Text += Convert.ToString("\r\n" + Y[i].ToString("00") + "-" + Math.Round(delTauY[i]).ToString("00") + Math.Round(awy[i]).ToString("00") + Math.Round(wy[i]).ToString("00") + "- ");
                }
                //buttonApplyVR2.Enabled = true;
            }            
        }

        //for Meteoseredniy

        public void buttonApply2_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                int hb = 2440;
                int mumber_of_stantion = Convert.ToInt32(name_of_station.Text);
                int date = Convert.ToInt32(textDate.Text);
                int Hmc = Convert.ToInt32(text_hmc.Text);
                int del_h_t = Convert.ToInt32(text_delH_delt0.Text);

                int tem = del_h_t % 100;
                del_h_t = del_h_t / 100;
                int del_h = del_h_t % 1000;

                textBulletin.Text = Convert.ToString(del_h_t) + "\n\r--";

                int[] high = new int[11] { 2, 4, 8, 12, 16, 20, 24, 30, 40, 50, 60 };

                int[] del_t = new int[11] { Convert.ToInt32(value_t_dlW_Wy_1.Text), Convert.ToInt32(value_t_dlW_Wy_2.Text), Convert.ToInt32(value_t_dlW_Wy_3.Text), Convert.ToInt32(value_t_dlW_Wy_4.Text), Convert.ToInt32(value_t_dlW_Wy_5.Text), Convert.ToInt32(value_t_dlW_Wy_6.Text), Convert.ToInt32(value_t_dlW_Wy_7.Text), Convert.ToInt32(value_t_dlW_Wy_8.Text), Convert.ToInt32(value_t_dlW_Wy_9.Text), Convert.ToInt32(value_t_dlW_Wy_10.Text), Convert.ToInt32(value_t_dlW_Wy_11.Text) };

                int[] delt0 = new int[11];
                
                for (int i = 0; i < 11; i++)
                {
                    delt0[i] = del_t[i] % 10000;
                    del_t[i] = del_t[i] / 10000;
                    // del_h[i] = del_t[i] % 1000;
                    textBulletin.Text += Convert.ToString(del_t[i]) + "\n\r--";
                }
                
                int del_Yst, q;
                if (Math.Abs(2 * (Hmc - hb)) > 200)
                {
                    del_Yst = 2 * (Hmc - hb);
                    q = del_Yst % 100; ;
                    if (del_Yst >= 0)
                    {
                        if (q > 50)
                        {
                            q = 100 - q;
                            del_Yst = del_Yst + q;
                        }
                        else
                        { del_Yst = del_Yst - q; }
                    }
                    else
                    {
                        if (q >= -50) { del_Yst = del_Yst - q; }
                        else
                        {
                            q = -100 - q;
                            del_Yst = del_Yst + q;
                        }
                    }

                    // textBulletin.Text += Convert.ToString(del_Yst);
                    for (int i = 0; i < 11; i++)
                    {
                        high[i] = high[i] * 100 + del_Yst;
                        //  textBulletin.Text += Convert.ToString(high[i]);
                    }
                }

                int tem1;

                if (tem > 50)
                {
                    tem1 = -(tem - 50);
                }
                else
                {
                    tem1 = tem;
                }

                int delat_t = (int)(Math.Round(0.006 * (Hmc - hb)));

                if (tem <= 50)
                {
                    if ((tem + delat_t) >= 0)
                    {
                        tem = tem + delat_t;
                    }
                    else { tem = Math.Abs(tem + delat_t) + 50; }
                }
                else
                {
                    if (delat_t <= 0)
                    {
                        tem = tem + Math.Abs(delat_t);
                    }
                    else
                    {
                        if ((tem - delat_t) < 51) { tem = 50 - (tem - delat_t); }
                        else { tem = tem + Math.Abs(delat_t); }
                    }
                }

                for (int i = 0; i < 11; i++)
                {
                    if (del_t[i] <= 50)
                    {
                        if ((delat_t + del_t[i]) >= 0)
                        {
                            del_t[i] = del_t[i] + delat_t;
                        }
                        else { del_t[i] = Math.Abs(del_t[i] + delat_t) + 50; }
                    }
                    else
                    {
                        if (delat_t <= 0)
                        {
                            del_t[i] = del_t[i] + Math.Abs(delat_t);
                        }
                        else
                        {
                            if ((del_t[i] - delat_t) < 51) { del_t[i] = 50 - (del_t[i] - delat_t); }
                            else { del_t[i] = del_t[i] + Math.Abs(delat_t); }
                        }
                    }
                }

                double del_H;
                double B;


                if (del_h > 500)
                {
                    del_h = -(del_h - 500);
                }
                textBulletin.Text += Convert.ToString(tem1) + "\n--<>---";
               
                Meteoseredniy_iterpol interpolation = new Meteoseredniy_iterpol();
                
                interpolation.input(del_h, tem1, del_h, out B);
                if (B == -1) { MessageBox.Show("Файл із значенням барометричних ступенів не існує!   результат не правильний"); }

                textBulletin.Text += Convert.ToString(B) + "\n<->";
               
                if (Math.Abs(Hmc - hb) > 500)
                {
                    del_H = (del_h + (Hmc - hb) / (2 * B));
                }

                interpolation.input(del_h, tem1, del_h, out del_H);

                del_h = del_h + (int)(Math.Round((Hmc - hb) / B));
                if (del_h < 0)
                {
                    del_h = -(del_h - 500);
                }
                /*for (int l = 0; l < 11; l++)
                {
                    high[l] = high[l] * 100;
                }
                */

                if (tem > 99) { tem = 99; }

                textBulletin.Clear();
                textBulletin.Text += ("Метео - ");
                textBulletin.Text += Convert.ToString(mumber_of_stantion.ToString("00")) + " - ";
                //textBulletin.Text += (date.ToString("00"));
                textBulletin.Text += (date.ToString("0") + " - ");
                textBulletin.Text += (Hmc.ToString("0000") + " - ");
                textBulletin.Text += (del_h.ToString("000"));
                textBulletin.Text += (tem.ToString("00") + " - ");
                for (int i = 0; i < 11; i++)
                {
                    high[i] = high[i] / 100;
                    if (high[i] > 0)
                    {
                        textBulletin.Text += Convert.ToString("\r\n" + high[i].ToString("00") + "-" + del_t[i].ToString("00") + delt0[i].ToString("00") + "- ");
                    }
                }
                // buttonApplyDMeteoseredniy.Enabled = true;
            }
        }

        private void textDay_Validating(object sender, CancelEventArgs e)
        {
            if (textDay.Text.Length != 0)
            {
                int day = Convert.ToInt32(textDay.Text);
                if (day < 1 || day > 31)
                {
                    errorMain.SetError(textDay, "Введено невірне число місяця  \n                                    1-31");
                    e.Cancel = true;
                }
                else
                {
                    errorMain.SetError(textDay, "");
                }
            }
            else
            {
                errorMain.SetError(textDay, "Введіть число місяця");
                e.Cancel = true;
            }
        }

        private void textHours_Validating(object sender, CancelEventArgs e)
        {
            if (textHours.Text.Length != 0)
            {
                int hours = Convert.ToInt32(textHours.Text);
                if (hours < 0 || hours > 23)
                {
                    errorMain.SetError(textHours, "Введено невірну кількість годин  \n                                    0-23");
                    e.Cancel = true;
                }
                else
                {
                    errorMain.SetError(textHours, "");
                }
            }
            else
            {
                errorMain.SetError(textHours, "Введіть кількість годин");
                e.Cancel = true;
            }
        }

        private void textMinutes_Validating(object sender, CancelEventArgs e)
        {
            if (textMinutes.Text.Length != 0)
            {
                int minutes = Convert.ToInt32(textMinutes.Text);
                if (minutes < 0 || minutes > 59)
                {
                    errorMain.SetError(textMinutes, "Введено невірну кількість хвилин  \n                                    0-59");
                    e.Cancel = true;
                }
                else
                {
                    errorMain.SetError(textMinutes, "");
                }
            }
            else
            {
                errorMain.SetError(textMinutes, "Введіть кількість хвилин");
                e.Cancel = true;
            }
        }

        private void textHmc_Validating_1(object sender, CancelEventArgs e)
        {
            if (textHmc.Text.Length != 0)
            {
                int hmc = Convert.ToInt32(textHmc.Text);
                if (hmc < 0 || hmc > 9999)
                {
                    errorMain.SetError(textHmc, "Введено невірне значення висоти метеопосту \n                                    0-9999");
                    e.Cancel = true;
                }
                else
                {
                    errorMain.SetError(textHmc, "");
                }
            }
            else
            {
                errorMain.SetError(textHmc, "Введіть значення висоти метеопосту");
                e.Cancel = true;
            }
        }

        private void textH0_Validating(object sender, CancelEventArgs e)
        {
            if (textH0.Text.Length != 0)
            {
                int h0 = Convert.ToInt32(textH0.Text);
                if (h0 < 500 || h0 > 800)
                {
                    errorMain.SetError(textH0, "Введено невірне значення атмосферного тиску \n                                    500-800");
                    e.Cancel = true;
                }
                else
                {
                    errorMain.SetError(textH0, "");
                }
            }
            else
            {
                errorMain.SetError(textH0, "Введіть значення атмосферного тиску");
                e.Cancel = true;
            }
        }

        private void textT0_Validating(object sender, CancelEventArgs e)
        {
            if (textT0.Text.Length != 0)
            {
                int t0 = Convert.ToInt32(textT0.Text);
                if (t0 < -50 || t0 > 50)
                {
                    errorMain.SetError(textT0, "Введено невірне значення температури повітря \n                                    -50 - +50");
                    e.Cancel = true;
                }
                else
                {
                    errorMain.SetError(textT0, "");
                }
            }
            else
            {
                errorMain.SetError(textT0, "Введіть значення температури повітря");
                e.Cancel = true;
            }
        }

        private void textAW0_Validating(object sender, CancelEventArgs e)
        {
            if (textAW0.Text.Length != 0)
            {
                int aw0 = Convert.ToInt32(textAW0.Text);
                if (aw0 < 0 || aw0 > 360)
                {
                    errorMain.SetError(textAW0, "Введено невірне значення напрямку середнього вітру \n                                    0-360");
                    e.Cancel = true;
                }
                else
                {
                    errorMain.SetError(textAW0, "");
                }
            }
            else
            {
                errorMain.SetError(textAW0, "Введіть значення напрямку середнього вітру");
                e.Cancel = true;
            }
        }

        private void textW0_Validating(object sender, CancelEventArgs e)
        {
            if (textW0.Text.Length != 0)
            {
                int w0 = Convert.ToInt32(textW0.Text);
                if (w0 < 0 || w0 > 15)
                {
                    errorMain.SetError(textW0, "Введено невірне значення швидкості середнього вітру \n                                    0-15");
                    e.Cancel = true;
                }
                else
                {
                    errorMain.SetError(textW0, "");
                }
            }
            else
            {
                errorMain.SetError(textW0, "Введіть значення швидкості середнього вітру");
                e.Cancel = true;
            }
        }

        private void textDalnZnosuKul_Validating(object sender, CancelEventArgs e)
        {
            if (textDalnZnosuKul.Text.Length != 0)
            {
                int dzk = Convert.ToInt32(textDalnZnosuKul.Text);
                if (dzk < 0 || dzk > 150)
                {
                    this.errorMain.SetError(textDalnZnosuKul, "Введено невірне значення дальності зносу куль \n                                    0-150");
                    e.Cancel = true;
                }
                else
                {
                    this.errorMain.SetError(textDalnZnosuKul, "");
                }
            }
            else
            {
                this.errorMain.SetError(textDalnZnosuKul, "Введіть значення дальності зносу куль");
                e.Cancel = true;
            }
        }

        
        //for file
        int i;
        string path;
         private void zastosuvaty_Click(object sender, EventArgs e)
         {   
             i = 0;
             do
             {
                 i++;
                 path = @"..\\..\\..\\meteo\\Метео_" + i + ".txt";
             } while (File.Exists(path));
             StreamWriter wr = new StreamWriter(path);
             wr.WriteLine(textBulletin.Text);
             wr.Close();
         }
          
        #endregion

        #region Бойовий порядок

        private void textPozivnyi_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectionStart = 0;
            tb.SelectionLength = tb.Text.Length;
        }

        private void textKG_Validating(object sender, CancelEventArgs e) //Введення кількості батарей і відображення їх у таблиці
        {
            if (textKG.Text == "")
            {
                textKG.Text = "0";
            }
            int kol = Int32.Parse(textKG.Text);
            int i = 1;
            while (i <= dataGridView1.Columns.Count - 1)
            {
                dataGridView1.Columns.RemoveAt(i);
            }

            i = 1;
            if (kol > 8)
            {
                errorMain.SetError(textKG, "Введено невірну кількість гармат в батареї  \n 1-8");
                e.Cancel = true;
            }
            else
            {
                errorMain.SetError(textKG, "");
                while (i <= kol)
                {
                    dataGridView1.Columns.Add(null, Convert.ToString(i) + " гр");
                    dataGridView1.Rows[9].Cells[i].Style.BackColor = Color.Black;
                    for (int j = 10; j < 13; j++)
                    {
                        dataGridView1.Rows[j].Cells[i].Value = "0";
                    }
                    i++;
                }
            }
        }

        private void comboInd1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemEditor(1);
        }

        private void comboInd2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemEditor(2);
        }

        private void comboInd3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemEditor(3);
        }

        private void comboInd4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemEditor(4);
        }

        private void comboInd5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemEditor(5);
        }

        private void ItemEditor(int comboNomer) 
        {
            ComboBox pidr = comboPidr1;
            ComboBox ind = comboInd1;
            ComboBox zar = comboZar1;
            switch (comboNomer)
            {
                case 1:                    
                    pidr = comboPidr1;
                    ind = comboInd1;
                    zar = comboZar1;
                    break;
                case 2:
                    pidr = comboPidr2;
                    ind = comboInd2;
                    zar = comboZar2;
                    break;
                case 3:
                    pidr = comboPidr3;
                    ind = comboInd3;
                    zar = comboZar3;
                    break;
                case 4:
                    pidr = comboPidr4;
                    ind = comboInd4;
                    zar = comboZar4;
                    break;
                case 5:
                    pidr = comboPidr5;
                    ind = comboInd5;
                    zar = comboZar5;
                    break;               
            }

            if (ind.SelectedIndex == 1 || ind.SelectedIndex == 2 || ind.SelectedIndex == 3)
            {
                if (!pidr.Items.Contains("РГМ-2"))
                {
                    pidr.Items.Add("РГМ-2");
                }
                if (!pidr.Items.Contains("В-90"))
                {
                    pidr.Items.Add("В-90");
                }
                if (!zar.Items.Contains("Повний змінний"))
                {
                    zar.Items.Add("Повний змінний");
                }
                if (!zar.Items.Contains("Зменшений змінний"))
                {
                    zar.Items.Add("Зменшений змінний");
                }
                pidr.Items.Remove("ДТМ-75");
                pidr.Items.Remove("Т-7");
                pidr.Items.Remove("Т-90");
                pidr.Items.Remove("В-429");
            }
            else if (ind.SelectedIndex == 4)
            {
                if (!pidr.Items.Contains("ДТМ-75"))
                {
                    pidr.Items.Add("ДТМ-75");
                }
                if (!zar.Items.Contains("Повний змінний"))
                {
                    zar.Items.Add("Повний змінний");
                }
                if (!zar.Items.Contains("Зменшений змінний"))
                {
                    zar.Items.Add("Зменшений змінний");
                }
                pidr.Items.Remove("РГМ-2");
                pidr.Items.Remove("В-90");
                pidr.Items.Remove("Т-7");
                pidr.Items.Remove("Т-90");
                pidr.Items.Remove("В-429");
            }
            else if (ind.SelectedIndex == 5)
            {
                if (!pidr.Items.Contains("Т-7"))
                {
                    pidr.Items.Add("Т-7");
                }
                if (!zar.Items.Contains("Повний змінний"))
                {
                    zar.Items.Add("Повний змінний");
                }
                if (!zar.Items.Contains("Зменшений змінний"))
                {
                    zar.Items.Add("Зменшений змінний");
                }
                pidr.Items.Remove("РГМ-2");
                pidr.Items.Remove("В-90");
                pidr.Items.Remove("ДТМ-75");
                pidr.Items.Remove("Т-90");
                pidr.Items.Remove("В-429");
            }
            else if (ind.SelectedIndex == 6)
            {
                if (!pidr.Items.Contains("РГМ-2"))
                {
                    pidr.Items.Add("РГМ-2");
                }
                if (!pidr.Items.Contains("В-90"))
                {
                    pidr.Items.Add("В-90");
                }                
                if (!zar.Items.Contains("Зменшений змінний"))
                {
                    zar.Items.Add("Зменшений змінний");
                }
                zar.Items.Remove("Повний змінний");
                pidr.Items.Remove("ДТМ-75");
                pidr.Items.Remove("Т-7");
                pidr.Items.Remove("Т-90");
                pidr.Items.Remove("В-429");
            }
            else if (ind.SelectedIndex == 7)
            {
                if (!pidr.Items.Contains("Т-90"))
                {
                    pidr.Items.Add("Т-90");
                }
                if (!zar.Items.Contains("Повний змінний"))
                {
                    zar.Items.Add("Повний змінний");
                }
                if (!zar.Items.Contains("Зменшений змінний"))
                {
                    zar.Items.Add("Зменшений змінний");
                }
                pidr.Items.Remove("РГМ-2");
                pidr.Items.Remove("В-90");
                pidr.Items.Remove("ДТМ-75");
                pidr.Items.Remove("Т-7");
                pidr.Items.Remove("В-429");
            }
            else if (ind.SelectedIndex == 8)
            {
                if (!pidr.Items.Contains("В-429"))
                {
                    pidr.Items.Add("В-429");
                }
                if (!zar.Items.Contains("Повний змінний"))
                {
                    zar.Items.Add("Повний змінний");
                }
                zar.Items.Remove("Зменшений змінний");
                pidr.Items.Remove("РГМ-2");
                pidr.Items.Remove("В-90");
                pidr.Items.Remove("ДТМ-75");
                pidr.Items.Remove("Т-7");
                pidr.Items.Remove("Т-90");
            }
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += tb_KeyPress;
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e) //Метод обмеження введення символів до таблиці
        {
            if (!Char.IsNumber(e.KeyChar))
            {
                if ((e.KeyChar != (char)Keys.Back) || (e.KeyChar != (char)Keys.Delete))
                {
                    e.Handled = true;
                }
            }
        }

        private void dataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int rowNum = dataGridView1.CurrentRow.Index;
                if (rowNum == 10 || rowNum == 11 || rowNum == 12)
                {
                    int[] angles = new int[Convert.ToInt32(textKG.Text)];
                    for (int i = 0; i < angles.Length; i++)
                    {
                        angles[i] = Convert.ToInt32(dataGridView1.Rows[rowNum].Cells[i + 1].Value);
                    }
                    int maxAngle = angles.Max();
                    dataGridView2.Rows[rowNum - 10].Cells[1].Value = maxAngle;
                
                }
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int rowNum = e.RowIndex, cellNum = e.ColumnIndex;
            Double [, ,] xValues = new Double[7, 12, 12]; //Для зберігання інтерпольованих значень по дальності
            
            if ((cellNum == 2 && rowNum >= 0) || (cellNum == 1 && rowNum >= 0))
            {
                int ky = Convert.ToInt32(dataGridView2.Rows[rowNum].Cells[1].Value);
                int dg = Convert.ToInt32(dataGridView2.Rows[rowNum].Cells[2].Value);
                double delHg = (double)(ky * dg) / 955;
                Math.Round(delHg, 1);
                textBox14.Text += delHg;
                textBox14.Text += "  ";

                using (ExcelPackage p = new ExcelPackage())
                {
                    using (FileStream stream = new FileStream("D:\\Study\\Воєнна кафедра\\Складання бюлетеня\\SkladannyaBuletnya\\SkladannyaBuletnya\\lib\\TC 2C3_Pmin.xlsx", FileMode.Open))
                        p.Load(stream);

                    string[] sheet = { "3.1.0", "3.1.1", "3.1.2", "3.1.3", "3.1.4", "3.1.5", "3.1.6" };

                    for (int k = 0; k <= 6; k++)
                    {
                        ExcelWorksheet ws = p.Workbook.Worksheets[sheet[k]];

                        for (int i = 2; i < 13; i++)
                        {
                            string textI = ws.Cells[i, 1].Value.ToString(); //Зчитати текст з Excel документа
                            int d1 = Convert.ToInt32(textI); //Значення відстані з файлу
                        
                            if ((dg - d1) <= 100)
                            {
                                textBox8.Text += Environment.NewLine;
                                for (int j = 2; j < 14; j++)
                                {
                                    Double x1 = Convert.ToDouble(ws.Cells[i, j - 1].Value); //Попереднє значення по-вертикалі
                                    Double x2 = Convert.ToDouble(ws.Cells[i + 1, j - 1].Value); //Наступне значення по-вертикалі
                                    Double x = ((dg - d1) * (x2 - x1)) / 100 + x1;
                                    xValues[k, 0, j - 2] = Convert.ToInt32(ws.Cells[1, j - 1].Value);
                                    xValues[k, 1, j - 2] = x;
                                    textBox8.Text += xValues[k, 0, j-2];
                                    textBox8.Text += "  ";
                                } //Цикл j
                                textBox8.Text += Environment.NewLine;
                                for (int j = 2; j < 14; j++)
                                {
                                    textBox8.Text += xValues[k, 1, j - 2];
                                    textBox8.Text += "  ";
                                }
                                break;
                            }
                        } //Цикл i
                        for (int i = 1; i < 12; i++)
                        {
                             if (i <= 11)
                             {
                                    for (int j = 1; j < 12; j++)
                                    {
                                        if ((delHg - xValues[k, 0, j]) <= 10)
                                        {
                                        Double y1 = xValues[k, 1, j]; //Попереднє значення по-горизонталі
                                        Double y2 = xValues[k, 1, j+1]; //Наступне значення по-горизонталі
                                        Double y = ((delHg - xValues[k, 0, j]) * (y2 - y1)) / 10 + y1;
                                        dataGridView2.Rows[rowNum].Cells[k + 3].Value = Math.Round(y, 1);

                                        textBox15.Text += y;
                                        textBox15.Text += "  ";
                                        textBox12.Text += y1;
                                        textBox12.Text += "  ";
                                        textBox13.Text += y2;
                                        textBox13.Text += "  ";
                                        break;
                                    }
                                }
                                    break;
                            }
                        }
                    }
                }
            }
        }
                 
         
        

        #endregion

        #region Позивні

        private void textPozivnyi1_TextChanged(object sender, EventArgs e)
        {
            buttonZa1.Text = textPozivnyi1.Text;
            buttonZa11.Text = textPozivnyi1.Text;
        }

        private void textPozivnyi2_TextChanged(object sender, EventArgs e)
        {
            buttonZa2.Text = textPozivnyi2.Text;
            buttonZa12.Text = textPozivnyi2.Text;
        }

        private void textPozivnyi3_TextChanged(object sender, EventArgs e)
        {
            buttonZa3.Text = textPozivnyi3.Text;
            buttonZa13.Text = textPozivnyi3.Text;
        }
        #endregion

        #region Команда старшого начальника
        #region Buttons
        private void PanelVisibility1(Panel panel) //Виведення і приховування панелей у команді старшого начальника
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            
            panel.Visible = true;
        }

        private void buttonDetector() //Перевіряє комбінації натиснутих кнопок
        {
            if (buttonXC1.BackColor == Color.LightCoral)
            {
                if (buttonOC1.BackColor == Color.LightCoral || buttonOC2.BackColor == Color.LightCoral)
                {
                    if (buttonKC1.BackColor == Color.LightCoral)
                    {
                        PanelVisibility1(panel4);
                        PanelVisibility2(panel14);
                    }
                    else if (buttonKC2.BackColor == Color.LightCoral)
                    {
                        PanelVisibility1(panel5);
                        PanelVisibility2(panel16);
                    }
                    
                }
                else if (buttonOC3.BackColor == Color.LightCoral)
                {
                    if (buttonKC1.BackColor == Color.LightCoral)
                    {
                        PanelVisibility1(panel3);
                        PanelVisibility2(panel12);
                    }
                    else if (buttonKC2.BackColor == Color.LightCoral)
                    {
                        PanelVisibility1(panel2);
                        PanelVisibility2(panel13);
                    }
                }
            }
            else if (buttonXC2.BackColor == Color.LightCoral || buttonXC3.BackColor == Color.LightCoral)
            {
                if (buttonOC1.BackColor == Color.LightCoral || buttonOC2.BackColor == Color.LightCoral)
                {
                    if (buttonKC1.BackColor == Color.LightCoral)
                    {
                        PanelVisibility1(panel6);
                        PanelVisibility2(panel18);
                    }
                    else if (buttonKC2.BackColor == Color.LightCoral)
                    {
                        PanelVisibility1(panel7);
                        PanelVisibility2(panel15);
                    }
                    else if (buttonKC3.BackColor == Color.LightCoral)
                    {
                        PanelVisibility1(panel8);
                        PanelVisibility2(panel17);
                    }
                }
            }
        }

        private void buttonXC1_Click(object sender, EventArgs e)
        {
            if (buttonKC3.BackColor == Color.LightCoral)
            {
                MessageBox.Show(@"Виберіть прямокутні або полярні координати!");
            }
            else
            {
                HarakterCili hc = new HarakterCili();
                hc.ShowDialog();
                buttonXC1.BackColor = Color.LightCoral;
                buttonXC2.BackColor = Color.AliceBlue;
                buttonXC3.BackColor = Color.AliceBlue;

                buttonXC11.BackColor = Color.LightCoral;
                buttonXC12.BackColor = Color.AliceBlue;
                buttonXC13.BackColor = Color.AliceBlue;
                hc.LabelPsiUvChange(ref labelPSiUV);
                label166.Text = labelPSiUV.Text;
                buttonDetector();
            }
        }

        private void buttonXC2_Click(object sender, EventArgs e)
        {
            if (buttonOC3.BackColor == Color.LightCoral)
            {
                MessageBox.Show(@"Виберіть іншу ознаку спостережності цілі");
            }
            else
            {
                buttonXC1.BackColor = Color.AliceBlue;
                buttonXC2.BackColor = Color.LightCoral;
                buttonXC3.BackColor = Color.AliceBlue;

                buttonXC11.BackColor = Color.AliceBlue;
                buttonXC12.BackColor = Color.LightCoral;
                buttonXC13.BackColor = Color.AliceBlue;
                labelPSiUV.Text = "";
                label166.Text = "";
                buttonDetector();
            }
        }

        private void buttonXC3_Click(object sender, EventArgs e)
        {
            if (buttonOC3.BackColor == Color.LightCoral)
            {
                MessageBox.Show(@"Виберіть іншу ознаку спостережності цілі");
            }
            else
            {
                buttonXC1.BackColor = Color.AliceBlue;
                buttonXC2.BackColor = Color.AliceBlue;
                buttonXC3.BackColor = Color.LightCoral;

                buttonXC11.BackColor = Color.AliceBlue;
                buttonXC12.BackColor = Color.AliceBlue;
                buttonXC13.BackColor = Color.LightCoral;
                labelPSiUV.Text = "";
                label166.Text = "";
                buttonDetector();
            }
        }

        private void buttonZa1_Click(object sender, EventArgs e)
        {
            buttonZa1.BackColor = Color.LightCoral;
            buttonZa2.BackColor = Color.AliceBlue;
            buttonZa3.BackColor = Color.AliceBlue;

            buttonZa11.BackColor = Color.LightCoral;
            buttonZa12.BackColor = Color.AliceBlue;
            buttonZa13.BackColor = Color.AliceBlue;
            
        }

        private void buttonZa2_Click(object sender, EventArgs e)
        {
            buttonZa1.BackColor = Color.AliceBlue;
            buttonZa2.BackColor = Color.LightCoral;
            buttonZa3.BackColor = Color.AliceBlue;

            buttonZa11.BackColor = Color.AliceBlue;
            buttonZa12.BackColor = Color.LightCoral;
            buttonZa13.BackColor = Color.AliceBlue;
        }

        private void buttonZa3_Click(object sender, EventArgs e)
        {
            buttonZa1.BackColor = Color.AliceBlue;
            buttonZa2.BackColor = Color.AliceBlue;
            buttonZa3.BackColor = Color.LightCoral;

            buttonZa11.BackColor = Color.AliceBlue;
            buttonZa12.BackColor = Color.AliceBlue;
            buttonZa13.BackColor = Color.LightCoral;
        }

        private void buttonPK1_Click(object sender, EventArgs e)
        {
            buttonPK1.BackColor = Color.LightCoral;
            buttonPK2.BackColor = Color.AliceBlue;

            buttonPK11.BackColor = Color.LightCoral;
            buttonPK12.BackColor = Color.AliceBlue;
        }

        private void buttonPK2_Click(object sender, EventArgs e)
        {
            buttonPK2.BackColor = Color.LightCoral;
            buttonPK1.BackColor = Color.AliceBlue;

            buttonPK12.BackColor = Color.LightCoral;
            buttonPK11.BackColor = Color.AliceBlue;
        }

        private void buttonOC1_Click(object sender, EventArgs e)
        {
            buttonOC1.BackColor = Color.LightCoral;
            buttonOC2.BackColor = Color.AliceBlue;
            buttonOC3.BackColor = Color.AliceBlue;

            buttonOC11.BackColor = Color.LightCoral;
            buttonOC12.BackColor = Color.AliceBlue;
            buttonOC13.BackColor = Color.AliceBlue;
            buttonDetector();
        }

        private void buttonOC2_Click(object sender, EventArgs e)
        {
            buttonOC1.BackColor = Color.AliceBlue;
            buttonOC2.BackColor = Color.LightCoral;
            buttonOC3.BackColor = Color.AliceBlue;

            buttonOC11.BackColor = Color.AliceBlue;
            buttonOC12.BackColor = Color.LightCoral;
            buttonOC13.BackColor = Color.AliceBlue;
            buttonDetector();
        }

        private void buttonOC3_Click(object sender, EventArgs e)
        {
            if (buttonXC1.BackColor == Color.LightCoral)
            {
                buttonOC1.BackColor = Color.AliceBlue;
                buttonOC2.BackColor = Color.AliceBlue;
                buttonOC3.BackColor = Color.LightCoral;

                buttonOC11.BackColor = Color.AliceBlue;
                buttonOC12.BackColor = Color.AliceBlue;
                buttonOC13.BackColor = Color.LightCoral;
                buttonDetector();
            }
            else
            {
                MessageBox.Show(@"Виберіть характер цілі з ПСіУВ!");
            }
        }

        private void buttonZC1_Click(object sender, EventArgs e)
        {
            buttonZC1.BackColor = Color.LightCoral;
            buttonZC2.BackColor = Color.AliceBlue;

            buttonZC11.BackColor = Color.LightCoral;
            buttonZC12.BackColor = Color.AliceBlue;
        }

        private void buttonZC2_Click(object sender, EventArgs e)
        {
            buttonZC1.BackColor = Color.AliceBlue;
            buttonZC2.BackColor = Color.LightCoral;

            buttonZC11.BackColor = Color.AliceBlue;
            buttonZC12.BackColor = Color.LightCoral;
        }

        private void buttonKC1_Click(object sender, EventArgs e)
        { 
            buttonKC1.BackColor = Color.LightCoral;
            buttonKC2.BackColor = Color.AliceBlue;
            buttonKC3.BackColor = Color.AliceBlue;

            buttonKC11.BackColor = Color.LightCoral;
            buttonKC12.BackColor = Color.AliceBlue;
            buttonKC13.BackColor = Color.AliceBlue;
            buttonDetector();
        }

        private void buttonKC2_Click(object sender, EventArgs e)
        {
            buttonKC1.BackColor = Color.AliceBlue;
            buttonKC2.BackColor = Color.LightCoral;
            buttonKC3.BackColor = Color.AliceBlue;

            buttonKC11.BackColor = Color.AliceBlue;
            buttonKC12.BackColor = Color.LightCoral;
            buttonKC13.BackColor = Color.AliceBlue;
            buttonDetector();
        }

        private void buttonKC3_Click(object sender, EventArgs e)
        {
           if (buttonXC2.BackColor == Color.LightCoral || buttonXC3.BackColor == Color.LightCoral)
            {
            buttonKC1.BackColor = Color.AliceBlue;
            buttonKC2.BackColor = Color.AliceBlue;
            buttonKC3.BackColor = Color.LightCoral;

            buttonKC11.BackColor = Color.AliceBlue;
            buttonKC12.BackColor = Color.AliceBlue;
            buttonKC13.BackColor = Color.LightCoral;
            buttonDetector();
            }
           else
            {
            MessageBox.Show(@"Виберіть характер цілі НЗВ або РЗВ!");
            }
        }
        
        private void buttonSO1_Click(object sender, EventArgs e)
        {
            buttonSO1.BackColor = Color.LightCoral;
            buttonSO2.BackColor = Color.AliceBlue;
            buttonSO3.BackColor = Color.AliceBlue;

            buttonSO11.BackColor = Color.LightCoral;
            buttonSO12.BackColor = Color.AliceBlue;
            buttonSO13.BackColor = Color.AliceBlue;
        }

        private void buttonSO2_Click(object sender, EventArgs e)
        {
            buttonSO1.BackColor = Color.AliceBlue;
            buttonSO2.BackColor = Color.LightCoral;
            buttonSO3.BackColor = Color.AliceBlue;

            buttonSO11.BackColor = Color.AliceBlue;
            buttonSO12.BackColor = Color.LightCoral;
            buttonSO13.BackColor = Color.AliceBlue;
        }

        private void buttonSO3_Click(object sender, EventArgs e)
        {
            buttonSO1.BackColor = Color.AliceBlue;
            buttonSO2.BackColor = Color.AliceBlue;
            buttonSO3.BackColor = Color.LightCoral;

            buttonSO11.BackColor = Color.AliceBlue;
            buttonSO12.BackColor = Color.AliceBlue;
            buttonSO13.BackColor = Color.LightCoral;
        }

        private void buttonYk1_Click(object sender, EventArgs e)
        {
            buttonYk1.BackColor = Color.LightCoral;
            buttonYk2.BackColor = Color.AliceBlue;

            buttonYk11.BackColor = Color.LightCoral;
            buttonYk12.BackColor = Color.AliceBlue;
        }

        private void buttonYk2_Click(object sender, EventArgs e)
        {
            buttonYk1.BackColor = Color.AliceBlue;
            buttonYk2.BackColor = Color.LightCoral;

            buttonYk11.BackColor = Color.AliceBlue;
            buttonYk12.BackColor = Color.LightCoral;
        }

        private void buttonTV1_Click(object sender, EventArgs e)
        {
            buttonTV1.BackColor = Color.LightCoral;
            buttonTV2.BackColor = Color.AliceBlue;
            buttonTV3.BackColor = Color.AliceBlue;
            buttonTV4.BackColor = Color.AliceBlue;

            buttonTV11.BackColor = Color.LightCoral;
            buttonTV12.BackColor = Color.AliceBlue;
            buttonTV13.BackColor = Color.AliceBlue;
            buttonTV14.BackColor = Color.AliceBlue;
        }

        private void buttonTV2_Click(object sender, EventArgs e)
        {
            buttonTV1.BackColor = Color.AliceBlue;
            buttonTV2.BackColor = Color.LightCoral;
            buttonTV3.BackColor = Color.AliceBlue;
            buttonTV4.BackColor = Color.AliceBlue;

            buttonTV11.BackColor = Color.AliceBlue;
            buttonTV12.BackColor = Color.LightCoral;
            buttonTV13.BackColor = Color.AliceBlue;
            buttonTV14.BackColor = Color.AliceBlue;

            panelTryvalist.Visible = true;
            panelMetodVogon.Visible = false;
        }

        private void buttonTV3_Click(object sender, EventArgs e)
        {
            buttonTV1.BackColor = Color.AliceBlue;
            buttonTV2.BackColor = Color.AliceBlue;
            buttonTV3.BackColor = Color.LightCoral;
            buttonTV4.BackColor = Color.AliceBlue;

            buttonTV11.BackColor = Color.AliceBlue;
            buttonTV12.BackColor = Color.AliceBlue;
            buttonTV13.BackColor = Color.LightCoral;
            buttonTV14.BackColor = Color.AliceBlue;

            panelMetodVogon.Visible = true;
            panelTryvalist.Visible = false;
        }

        private void buttonTV4_Click(object sender, EventArgs e)
        {
            buttonTV1.BackColor = Color.AliceBlue;
            buttonTV2.BackColor = Color.AliceBlue;
            buttonTV3.BackColor = Color.AliceBlue;
            buttonTV4.BackColor = Color.LightCoral;

            buttonTV11.BackColor = Color.AliceBlue;
            buttonTV12.BackColor = Color.AliceBlue;
            buttonTV13.BackColor = Color.AliceBlue;
            buttonTV14.BackColor = Color.LightCoral;

            panelTryvalist.Visible = true;
            panelMetodVogon.Visible = false;
        }

        private void buttonVS1_Click(object sender, EventArgs e)
        {
            buttonVS1.BackColor = Color.LightCoral;
            buttonVS2.BackColor = Color.AliceBlue;
            buttonVS3.BackColor = Color.AliceBlue;
            buttonVS4.BackColor = Color.AliceBlue;

            buttonVS11.BackColor = Color.LightCoral;
            buttonVS12.BackColor = Color.AliceBlue;
            buttonVS13.BackColor = Color.AliceBlue;
            buttonVS14.BackColor = Color.AliceBlue;
            contextNaCil.Show();
        }

        private void buttonVS2_Click(object sender, EventArgs e)
        {
            buttonVS1.BackColor = Color.AliceBlue;
            buttonVS2.BackColor = Color.LightCoral;
            buttonVS3.BackColor = Color.AliceBlue;
            buttonVS4.BackColor = Color.AliceBlue;

            buttonVS11.BackColor = Color.AliceBlue;
            buttonVS12.BackColor = Color.LightCoral;
            buttonVS13.BackColor = Color.AliceBlue;
            buttonVS14.BackColor = Color.AliceBlue;
            panelNaBatareu.Visible = true;
        }

        private void buttonVS3_Click(object sender, EventArgs e)
        {
            buttonVS1.BackColor = Color.AliceBlue;
            buttonVS2.BackColor = Color.AliceBlue;
            buttonVS3.BackColor = Color.LightCoral;
            buttonVS4.BackColor = Color.AliceBlue;

            buttonVS11.BackColor = Color.AliceBlue;
            buttonVS12.BackColor = Color.AliceBlue;
            buttonVS13.BackColor = Color.LightCoral;
            buttonVS14.BackColor = Color.AliceBlue;

            panelNaBatareu.Visible = false;

            GarmataUstanovka gu = new GarmataUstanovka();
            gu.ShowDialog();
        }

        private void buttonVS4_Click(object sender, EventArgs e)
        {
            buttonVS1.BackColor = Color.AliceBlue;
            buttonVS2.BackColor = Color.AliceBlue;
            buttonVS3.BackColor = Color.AliceBlue;
            buttonVS4.BackColor = Color.LightCoral;

            buttonVS11.BackColor = Color.AliceBlue;
            buttonVS12.BackColor = Color.AliceBlue;
            buttonVS13.BackColor = Color.AliceBlue;
            buttonVS14.BackColor = Color.LightCoral;
            panelNaBatareu.Visible = false;

            VytrataNaGarmatu vng = new VytrataNaGarmatu();
            vng.ShowDialog();
        }

        private void buttonVV1_Click(object sender, EventArgs e)
        {
            buttonVV1.BackColor = Color.LightCoral;
            buttonVV2.BackColor = Color.AliceBlue;
            buttonVV3.BackColor = Color.AliceBlue;
            button1.BackColor = Color.AliceBlue;
            buttonVV6.BackColor = Color.AliceBlue;

            buttonVV11.BackColor = Color.LightCoral;
            buttonVV12.BackColor = Color.AliceBlue;
            buttonVV13.BackColor = Color.AliceBlue;
            buttonVV14.BackColor = Color.AliceBlue;
            buttonVV16.BackColor = Color.AliceBlue;
        }

        private void buttonVV2_Click(object sender, EventArgs e)
        {
            buttonVV1.BackColor = Color.AliceBlue;
            buttonVV2.BackColor = Color.LightCoral;
            buttonVV3.BackColor = Color.AliceBlue;
            button1.BackColor = Color.AliceBlue;
            buttonVV6.BackColor = Color.AliceBlue;

            buttonVV11.BackColor = Color.AliceBlue;
            buttonVV12.BackColor = Color.LightCoral;
            buttonVV13.BackColor = Color.AliceBlue;
            buttonVV14.BackColor = Color.AliceBlue;
            buttonVV16.BackColor = Color.AliceBlue;
        }

        private void buttonVV3_Click(object sender, EventArgs e)
        {
            buttonVV1.BackColor = Color.AliceBlue;
            buttonVV2.BackColor = Color.AliceBlue;
            buttonVV3.BackColor = Color.LightCoral;
            button1.BackColor = Color.AliceBlue;
            buttonVV6.BackColor = Color.AliceBlue;

            buttonVV11.BackColor = Color.AliceBlue;
            buttonVV12.BackColor = Color.AliceBlue;
            buttonVV13.BackColor = Color.LightCoral;
            buttonVV14.BackColor = Color.AliceBlue;
            buttonVV16.BackColor = Color.AliceBlue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonVV1.BackColor = Color.AliceBlue;
            buttonVV2.BackColor = Color.AliceBlue;
            buttonVV3.BackColor = Color.AliceBlue;
            button1.BackColor = Color.LightCoral;
            buttonVV6.BackColor = Color.AliceBlue;

            buttonVV11.BackColor = Color.AliceBlue;
            buttonVV12.BackColor = Color.AliceBlue;
            buttonVV13.BackColor = Color.AliceBlue;
            buttonVV14.BackColor = Color.LightCoral;
            buttonVV16.BackColor = Color.AliceBlue;

            Gotovnist gotovnist = new Gotovnist();
            gotovnist.ShowDialog();
        }

        private void buttonVV6_Click(object sender, EventArgs e)
        {
            buttonVV1.BackColor = Color.AliceBlue;
            buttonVV2.BackColor = Color.AliceBlue;
            buttonVV3.BackColor = Color.AliceBlue;
            button1.BackColor = Color.AliceBlue;
            buttonVV6.BackColor = Color.LightCoral;

            buttonVV11.BackColor = Color.AliceBlue;
            buttonVV12.BackColor = Color.AliceBlue;
            buttonVV13.BackColor = Color.AliceBlue;
            buttonVV14.BackColor = Color.AliceBlue;
            buttonVV16.BackColor = Color.LightCoral;
        }
        #endregion

        #region textBoxes
        private void textNoCili_TextChanged(object sender, EventArgs e) //NC
        {
            textNoCili1.Text = textNoCili.Text;
        }

        private void numericFrontCili_TextChanged(object sender, EventArgs e) //Fc
        {
            numericFrontCili1.Text = numericFrontCili.Text;
        }

        private void numericTextBox8_TextChanged(object sender, EventArgs e) //Iv
        {
            numericIntervalViyala1.Text = numericTextBox8.Text;
        }

        private void numericTextBox9_TextChanged(object sender, EventArgs e) //Gc
        {
            numericTextBox65.Text = numericTextBox9.Text;
        }

        private void numericTextBox10_TextChanged(object sender, EventArgs e) //Sp
        {
            numericTextBox4.Text = numericTextBox10.Text;
        }

        private void numericSnaryadiv_TextChanged(object sender, EventArgs e)
        {
            numericTextBox68.Text = numericSnaryadiv.Text;
        }

        private void numericTextBox13_TextChanged(object sender, EventArgs e) //Снаряди біглим
        {
            numericTextBox69.Text = numericTextBox13.Text;
        }

        private void numericTextBox12_TextChanged(object sender, EventArgs e) //Секунд постріл
        {
            numericTextBox70.Text = numericTextBox12.Text;
        }

        private void numericTextBox11_TextChanged(object sender, EventArgs e) //Тривалість
        {
            numericTextBox71.Text = numericTextBox11.Text;
        }

        private void textNaBatareu_TextChanged(object sender, EventArgs e) //VS7
        {
            textBox17.Text = textNaBatareu.Text;
        }
        #endregion

        #region Panels
        #region Ціль прямокутні
        private void numericKCXc_TextChanged(object sender, EventArgs e) //Xc
        {
            numericTextBox87.Text = numericKCXc.Text;
        }

        private void numericKCYc_TextChanged(object sender, EventArgs e) //Yc
        {
            numericTextBox88.Text = numericKCYc.Text;
        }

        private void numericKChc_TextChanged(object sender, EventArgs e) //hc
        {
            numericTextBox86.Text = numericKChc.Text;
        }
        #endregion

        #region  Ціль полярні
        private void numeric5KCAk_TextChanged(object sender, EventArgs e)
        {
            numericTextBox98.Text = numeric5KCAk.Text;
        }

        private void numeric5KCDk_TextChanged(object sender, EventArgs e)
        {
            numericTextBox99.Text = numeric5KCDk.Text;
        }

        private void numeric5KCMk_TextChanged(object sender, EventArgs e)
        {
            numericTextBox97.Text = numeric5KCMk.Text;
        }
        #endregion

        #region Рухома прямокутні
        private void numericTextBox64_TextChanged(object sender, EventArgs e) //X1
        {
            numericTextBox78.Text = numericTextBox64.Text;
        }

        private void numericTextBox63_TextChanged(object sender, EventArgs e) //Y1
        {
            numericTextBox77.Text = numericTextBox63.Text;
        }

        private void numericTextBox62_TextChanged(object sender, EventArgs e) //h1
        {
            numericTextBox76.Text = numericTextBox62.Text;
        }

        private void numericTextBox61_TextChanged(object sender, EventArgs e) //X2
        {
            numericTextBox75.Text = numericTextBox61.Text;
        }

        private void numericTextBox60_TextChanged(object sender, EventArgs e) //Y2
        {
            numericTextBox74.Text = numericTextBox60.Text;
        }

        private void numericTextBox59_TextChanged(object sender, EventArgs e) //h2
        {
            numericTextBox73.Text = numericTextBox59.Text;
        }

        private void numericTextBox58_TextChanged(object sender, EventArgs e) //Темп засічки
        {
            numericTextBox72.Text = numericTextBox58.Text;
        }
        #endregion

        #region Рухома полярні
        private void numericTextBox33_TextChanged(object sender, EventArgs e) //alpha1
        {
            numericTextBox85.Text = numericTextBox33.Text;
        }

        private void numericTextBox47_TextChanged(object sender, EventArgs e) //D1
        {
            numericTextBox84.Text = numericTextBox47.Text;
        }

        private void numericTextBox48_TextChanged(object sender, EventArgs e) //M1
        {
            numericTextBox83.Text = numericTextBox48.Text;
        }

        private void numericTextBox56_TextChanged(object sender, EventArgs e) //alpha2
        {
            numericTextBox82.Text = numericTextBox56.Text;
        }

        private void numericTextBox55_TextChanged(object sender, EventArgs e) //D2
        {
            numericTextBox81.Text = numericTextBox55.Text;
        }

        private void numericTextBox54_TextChanged(object sender, EventArgs e) //M2
        {
            numericTextBox80.Text = numericTextBox54.Text;
        }

        private void numericTextBox57_TextChanged(object sender, EventArgs e) //Темп засічки
        {
            numericTextBox79.Text = numericTextBox57.Text;
        }
        #endregion

        #region НЗВ прямокутні
        private void numericXp_TextChanged(object sender, EventArgs e) //Xp
        {
            numericTextBox111.Text = numericXp.Text;
        }

        private void numericYp_TextChanged(object sender, EventArgs e) //Yp
        {
            numericTextBox110.Text = numericYp.Text;
        }

        private void numericKC6X1_TextChanged(object sender, EventArgs e) //Xl
        {
            numericTextBox109.Text = numericKC6X1.Text;
        }

        private void numericKC6Y1_TextChanged(object sender, EventArgs e) //Yl
        {
            numericTextBox108.Text = numericKC6Y1.Text;
        }

        private void numericKC6hc_TextChanged(object sender, EventArgs e) //hc
        {
            numericTextBox107.Text = numericKC6hc.Text;
        }
        #endregion

        #region НЗВ полярні
        private void numericKC7Ap_TextChanged(object sender, EventArgs e) //Ap
        {
            numericTextBox96.Text = numericKC7Ap.Text;
        }

        private void numericKC7Dp_TextChanged(object sender, EventArgs e) //Dp
        {
            numericTextBox95.Text = numericKC7Dp.Text;
        }

        private void numericKC7Mp_TextChanged(object sender, EventArgs e) //Mp
        {
            numericTextBox94.Text = numericKC7Mp.Text;
        }

        private void numericKC7Al_TextChanged(object sender, EventArgs e) //Al
        {
            numericTextBox93.Text = numericKC7Al.Text;
        }

        private void numericKC7Dl_TextChanged(object sender, EventArgs e) //Dl
        {
            numericTextBox92.Text = numericKC7Dl.Text;
        }

        private void numericKC7Ml_TextChanged(object sender, EventArgs e) //Ml
        {
            numericTextBox91.Text = numericKC7Ml.Text;
        }

        private void numericKC7Kr_TextChanged(object sender, EventArgs e) //Kr
        {
            numericTextBox90.Text = numericKC7Kr.Text;
        }

        private void numericKC7Ir_TextChanged(object sender, EventArgs e) //Ir
        {
            numericTextBox89.Text = numericKC7Ir.Text;
        }
        #endregion

        #region НЗВ центр
        private void numericKC8Xc_TextChanged(object sender, EventArgs e) //Xc
        {
            numericTextBox105.Text = numericKC8Xc.Text;
        }

        private void numericKC8Yc_TextChanged(object sender, EventArgs e) //Yc
        {
            numericTextBox106.Text = numericKC8Yc.Text;
        }

        private void numericKC8hc_TextChanged(object sender, EventArgs e) //hc
        {
            numericTextBox104.Text = numericKC8hc.Text;
        }

        private void numericKC8Ar_TextChanged(object sender, EventArgs e) //Ar
        {
            numericTextBox103.Text = numericKC8Ar.Text;
        }

        private void numericKC8Fc_TextChanged(object sender, EventArgs e) //Fc
        {
            numericTextBox102.Text = numericKC8Fc.Text;
        }

        private void numericKC8Kr_TextChanged(object sender, EventArgs e) //Kr
        {
            numericTextBox101.Text = numericKC8Kr.Text;
        }

        private void numericKC8Ir_TextChanged(object sender, EventArgs e) //Ir
        {
            numericTextBox100.Text = numericKC8Ir.Text;
        }
        #endregion
        #endregion

        private void buttonZastKomStarNach_Click(object sender, EventArgs e)
        {
        }
        #endregion

        private void Meteonablyzheniy_Load(object sender, EventArgs e) //Натиснення кнопок за-замовчуванням
        {
            buttonZa1_Click(buttonZa1, null);
            buttonPK1_Click(buttonPK1, null);
            buttonOC1_Click(buttonOC1, null);
            buttonZC1_Click(buttonZC1, null);
            buttonKC1_Click(buttonKC1, null);
            buttonSO1_Click(buttonSO1, null);
            buttonYk1_Click(buttonYk1, null);
            buttonTV1_Click(buttonTV1, null);
            buttonVV1_Click(buttonVV1, null);
        }

        #region ToolStripMenu

        private void штукToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelNaBatareu.Visible = true;
        }

        private void нормаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textNaBatareu.Text = @"Норма";
        }

        private void нормиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textNaBatareu.Text = @"1/2 норми";
        }

        private void нормиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textNaBatareu.Text = @"1/3 норми";
        }

        private void нормиToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            textNaBatareu.Text = @"1/4 норми";
        }

        private void нормиToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            textNaBatareu.Text = @"1/5 норми";
        }

        private void нормиToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            textNaBatareu.Text = @"1/6 норми";
        }

        #endregion

        private void button13_Click(object sender, EventArgs e)
        {
            using (ExcelPackage p = new ExcelPackage())
            {
                using (FileStream stream = new FileStream("D:\\Study\\Воєнна кафедра\\Складання бюлетеня\\SkladannyaBuletnya\\SkladannyaBuletnya\\TC\\ТС 2С3-Т-90.xlsx", FileMode.Open))
                {
                    p.Load(stream);
                    int nz = Convert.ToInt32(textBox7.Text);
                    int dt = Convert.ToInt32(textBox9.Text); //Значення відстані, введене з клавіатури
                    string sheet = "";
                    switch (nz)   //Вибір аркуша Excel
                    {
                        case 0:
                            sheet = "ПОВНИЙ-наст";
                            break;
                        case 1:
                            sheet = "ПЕРВЫЙ-наст";
                            break;
                        case 2:
                            sheet = "ВТОРОЙ-наст";
                            break;
                        case 3:
                            sheet = "ТРЕТИЙ-наст";
                            break;
                        case 4:
                            sheet = "ЧЕТВЕРТЫЙ-наст";
                            break;
                        case 5:
                            sheet = "ПЯТЫЙ-наст";
                            break;
                        case 6:
                            sheet = "ШЕСТОЙ-наст";
                            break;
                    }

                    ExcelWorksheet ws = p.Workbook.Worksheets[sheet];
                    const int rowIndex = 11;

                    for (int i = 0; i < 43; i++)
                    {
                        string text = ws.Cells[rowIndex+i, 1].Value.ToString(); //Зчитати текст з Excel документа
                        int d1 = Convert.ToInt32(text); //Значення відстані з файлу
                        
                        if ((dt - d1) <= 200)
                        {

                            for (int j = 2; j <= 21; j++)
                            {
                                Double x1 = Convert.ToDouble(ws.Cells[rowIndex + i, j].Value.ToString()); //Попереднє значення 
                                Double x2 = Convert.ToDouble(ws.Cells[rowIndex + i + 1, j].Value.ToString()); //Наступне значення 
                                Double x = ((dt - d1) * (x2 - x1)) / 200 + x1;
                                if (j == 10 || j == 15)
                                {
                                    x = Math.Round(x, 1);
                                }
                                else
                                {
                                    x = Math.Round(x, 0);
                                }
                                textBox8.Text += Convert.ToString(x);
                                textBox8.Text += @"  ";
                            }
                            break;
                            
                        }
                    }
                    stream.Close();
                }
            }
        }

        

        private void button15_Click(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"D:\Study\Воєнна кафедра\Складання бюлетеня\SkladannyaBuletnya\SkladannyaBuletnya\xml\DanieCeli.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            XmlElement datalistElem = xDoc.CreateElement("datalist");
            XmlAttribute nameAttr = xDoc.CreateAttribute("name");
            XmlElement valueElem = xDoc.CreateElement("value");
            XmlText nameText = xDoc.CreateTextNode("VS");
            XmlText valueText = xDoc.CreateTextNode("42");

            nameAttr.AppendChild(nameText);
            valueElem.AppendChild(valueText);
            datalistElem.Attributes.Append(nameAttr);
            datalistElem.AppendChild(valueElem);
            if (xRoot != null) xRoot.AppendChild(datalistElem);
            xDoc.Save("D:/Study/Воєнна кафедра/Складання бюлетеня/SkladannyaBuletnya/SkladannyaBuletnya/xml/DanieCeli.xml");
            /*foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                    {
                        textBox8.Text += attr.Value;
                        textBox8.Text += "  ";
                    }
                    //textBox8.Text = Environment.NewLine;
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        if (childnode.Name == "value")
                        {
                            if (childnode.InnerText != null)
                            {
                                textBox10.Text += childnode.InnerText;
                                textBox10.Text += "  ";
                            }
                        }
                    }
                }
            }*/
            //textBox8.Text = Convert.ToString(DC.ZC2);
        }

        #region Команда командира батареї

        #region
        private void PanelVisibility2(Panel panel) //Виведення і приховування необхідних панелей у команді командира батареї
        {
            panel12.Visible = false;
            panel13.Visible = false;
            panel14.Visible = false;
            panel15.Visible = false;
            panel16.Visible = false;
            panel17.Visible = false;
            panel18.Visible = false;

            panel.Visible = true;
        }

        private void SnaryadButtonPaint(Button button)
        {
            buttonOF540.BackColor = Color.WhiteSmoke;
            buttonOF250G.BackColor = Color.WhiteSmoke;
            buttonOF25.BackColor = Color.WhiteSmoke;
            buttonZSH2.BackColor = Color.WhiteSmoke;
            buttonZS1.BackColor = Color.WhiteSmoke;
            buttonOF540V.BackColor = Color.WhiteSmoke;
            buttonC61.BackColor = Color.WhiteSmoke;
            buttonOF22.BackColor = Color.WhiteSmoke;

            button.BackColor = Color.LightCoral;
        }

        private void buttonOF540_Click(object sender, EventArgs e)
        {
            SnaryadButtonPaint(buttonOF540);
            PidryvnykButtonInvisible();
            buttonRGM2.Visible = true;
            buttonV90.Visible = true;
        }

        private void buttonOF250G_Click(object sender, EventArgs e)
        {
            SnaryadButtonPaint(buttonOF250G);
            PidryvnykButtonInvisible();
            buttonRGM2.Visible = true;
            buttonV90.Visible = true;
        }

        private void buttonOF25_Click(object sender, EventArgs e)
        {
            SnaryadButtonPaint(buttonOF25);
            PidryvnykButtonInvisible();
            buttonRGM2.Visible = true;
            buttonV90.Visible = true;
        }

        private void buttonZSH2_Click(object sender, EventArgs e)
        {
            SnaryadButtonPaint(buttonZSH2);
            PidryvnykButtonInvisible();
            buttonDTM75.Visible = true;
        }

        private void buttonZS1_Click(object sender, EventArgs e)
        {
            SnaryadButtonPaint(buttonZS1);
            PidryvnykButtonInvisible();
            buttonT7.Visible = true;
        }

        private void buttonOF540V_Click(object sender, EventArgs e)
        {
            SnaryadButtonPaint(buttonOF540V);
            PidryvnykButtonInvisible();
            buttonRGM2.Visible = true;
            buttonV90.Visible = true;
        }

        private void buttonC61_Click(object sender, EventArgs e)
        {
            SnaryadButtonPaint(buttonC61);
            PidryvnykButtonInvisible();
            buttonT90.Visible = true;
        }

        private void buttonOF22_Click(object sender, EventArgs e)
        {
            SnaryadButtonPaint(buttonOF22);
            PidryvnykButtonInvisible();
            buttonV429.Visible = true;
        }

        private void PidryvnykButtonInvisible()
        {
            buttonRGM2.Visible = false;
            buttonV90.Visible = false;
            buttonDTM75.Visible = false;
            buttonT7.Visible = false;
            buttonT90.Visible = false;
            buttonV429.Visible = false;
        }

        private void PidryvnykButtonPaint(Button button)
        {
            buttonRGM2.BackColor = Color.WhiteSmoke;
            buttonV90.BackColor = Color.WhiteSmoke;
            buttonDTM75.BackColor = Color.WhiteSmoke;
            buttonT7.BackColor = Color.WhiteSmoke;
            buttonT90.BackColor = Color.WhiteSmoke;
            buttonV429.BackColor = Color.WhiteSmoke;

            
            
            button.BackColor = Color.LightCoral;
            button.Visible = true;
        }


        private void buttonRGM2_Click(object sender, EventArgs e)
        {
            PidryvnykButtonPaint(buttonRGM2);
        }

        private void buttonV90_Click(object sender, EventArgs e)
        {
            PidryvnykButtonPaint(buttonV90);
        }

        private void buttonDTM75_Click(object sender, EventArgs e)
        {
            PidryvnykButtonPaint(buttonDTM75);
        }

        private void buttonT7_Click(object sender, EventArgs e)
        {
            PidryvnykButtonPaint(buttonT7);
        }

        private void buttonT90_Click(object sender, EventArgs e)
        {
            PidryvnykButtonPaint(buttonT90);
        }

        private void buttonV429_Click(object sender, EventArgs e)
        {
            PidryvnykButtonPaint(buttonV429);
        }
        #endregion

        private void ButtonDetector1() //Перевіряє комбінації натиснутих кнопок для команди командира батареї
        {
            if (buttonXC11.BackColor == Color.LightCoral)
            {
                if (buttonOC11.BackColor == Color.LightCoral || buttonOC12.BackColor == Color.LightCoral)
                {
                    if (buttonKC11.BackColor == Color.LightCoral)
                    {
                        PanelVisibility2(panel14);
                    }
                    else if (buttonKC12.BackColor == Color.LightCoral)
                    {
                        PanelVisibility2(panel16);
                    }
                }
                else if (buttonOC13.BackColor == Color.LightCoral)
                {
                    if (buttonKC11.BackColor == Color.LightCoral)
                    {
                        PanelVisibility2(panel12);
                    }
                    else if (buttonKC12.BackColor == Color.LightCoral)
                    {
                        PanelVisibility2(panel13);
                    }
                }
            }
            else if (buttonXC12.BackColor == Color.LightCoral || buttonXC13.BackColor == Color.LightCoral)
            {
                if (buttonOC11.BackColor == Color.LightCoral || buttonOC12.BackColor == Color.LightCoral)
                {
                    if (buttonKC11.BackColor == Color.LightCoral)
                    {
                        PanelVisibility2(panel18);
                    }
                    else if (buttonKC12.BackColor == Color.LightCoral)
                    {
                        PanelVisibility2(panel15);
                    }
                    else if (buttonKC13.BackColor == Color.LightCoral)
                    {
                        PanelVisibility2(panel17);
                    }
                }
            }
        }

        private void buttonZa11_Click(object sender, EventArgs e)
        {
            buttonZa11.BackColor = Color.LightCoral;
            buttonZa12.BackColor = Color.AliceBlue;
            buttonZa13.BackColor = Color.AliceBlue;
        }

        private void buttonZa12_Click(object sender, EventArgs e)
        {
            buttonZa11.BackColor = Color.AliceBlue;
            buttonZa12.BackColor = Color.LightCoral;
            buttonZa13.BackColor = Color.AliceBlue;
        }

        private void buttonZa13_Click(object sender, EventArgs e)
        {
            buttonZa11.BackColor = Color.AliceBlue;
            buttonZa12.BackColor = Color.AliceBlue;
            buttonZa13.BackColor = Color.LightCoral;
        }

        private void buttonPK11_Click(object sender, EventArgs e)
        {
            buttonPK11.BackColor = Color.LightCoral;
            buttonPK12.BackColor = Color.AliceBlue;
        }

        private void buttonPK12_Click(object sender, EventArgs e)
        {
            buttonPK12.BackColor = Color.LightCoral;
            buttonPK11.BackColor = Color.AliceBlue;
        }

        private void buttonXC11_Click(object sender, EventArgs e)
        {
            if (buttonKC13.BackColor == Color.LightCoral)
            {
                MessageBox.Show(@"Виберіть прямокутні або полярні координати!");
            }
            else
            {
                HarakterCili hc = new HarakterCili();
                hc.ShowDialog();
                buttonXC11.BackColor = Color.LightCoral;
                buttonXC12.BackColor = Color.AliceBlue;
                buttonXC13.BackColor = Color.AliceBlue;
                hc.LabelPsiUvChange(ref label166);
                ButtonDetector1();
            }
        }

        private void buttonXC12_Click(object sender, EventArgs e)
        {
            if (buttonOC13.BackColor == Color.LightCoral)
            {
                MessageBox.Show(@"Виберіть іншу ознаку спостережності цілі");
            }
            else
            {
                buttonXC11.BackColor = Color.AliceBlue;
                buttonXC12.BackColor = Color.LightCoral;
                buttonXC13.BackColor = Color.AliceBlue;
                label166.Text = "";
                ButtonDetector1();
            }
        }

        private void buttonXC13_Click(object sender, EventArgs e)
        {
            if (buttonOC13.BackColor == Color.LightCoral)
            {
                MessageBox.Show(@"Виберіть іншу ознаку спостережності цілі");
            }
            else
            {
                buttonXC11.BackColor = Color.AliceBlue;
                buttonXC12.BackColor = Color.AliceBlue;
                buttonXC13.BackColor = Color.LightCoral;
                label166.Text = "";
                ButtonDetector1();
            }
        }

        private void buttonOC11_Click(object sender, EventArgs e)
        {
            buttonOC11.BackColor = Color.LightCoral;
            buttonOC12.BackColor = Color.AliceBlue;
            buttonOC13.BackColor = Color.AliceBlue;
            ButtonDetector1();
        }

        private void buttonOC12_Click(object sender, EventArgs e)
        {
            buttonOC11.BackColor = Color.AliceBlue;
            buttonOC12.BackColor = Color.LightCoral;
            buttonOC13.BackColor = Color.AliceBlue;
            ButtonDetector1();
        }

        private void buttonOC13_Click(object sender, EventArgs e)
        {
            buttonOC11.BackColor = Color.AliceBlue;
            buttonOC12.BackColor = Color.AliceBlue;
            buttonOC13.BackColor = Color.LightCoral;
            ButtonDetector1();
        }

        private void buttonZC11_Click(object sender, EventArgs e)
        {
            buttonZC11.BackColor = Color.LightCoral;
            buttonZC12.BackColor = Color.AliceBlue;
        }

        private void buttonZC12_Click(object sender, EventArgs e)
        {
            buttonZC11.BackColor = Color.AliceBlue;
            buttonZC12.BackColor = Color.LightCoral;
        }

        private void buttonKC11_Click(object sender, EventArgs e)
        {
            buttonKC11.BackColor = Color.LightCoral;
            buttonKC12.BackColor = Color.AliceBlue;
            buttonKC13.BackColor = Color.AliceBlue;
            ButtonDetector1();
        }

        private void buttonKC12_Click(object sender, EventArgs e)
        {
            buttonKC11.BackColor = Color.AliceBlue;
            buttonKC12.BackColor = Color.LightCoral;
            buttonKC13.BackColor = Color.AliceBlue;
            ButtonDetector1();
        }

        private void buttonKC13_Click(object sender, EventArgs e)
        {
            if (buttonXC12.BackColor == Color.LightCoral || buttonXC13.BackColor == Color.LightCoral)
            {
                buttonKC11.BackColor = Color.AliceBlue;
                buttonKC12.BackColor = Color.AliceBlue;
                buttonKC13.BackColor = Color.LightCoral;
                ButtonDetector1();
            }
            else
            {
                MessageBox.Show(@"Виберіть характер цілі НЗВ або РЗВ!");
            }
        }

        private void buttonSO11_Click(object sender, EventArgs e)
        {
            buttonSO11.BackColor = Color.LightCoral;
            buttonSO12.BackColor = Color.AliceBlue;
            buttonSO13.BackColor = Color.AliceBlue;
        }

        private void buttonSO12_Click(object sender, EventArgs e)
        {
            buttonSO11.BackColor = Color.AliceBlue;
            buttonSO12.BackColor = Color.LightCoral;
            buttonSO13.BackColor = Color.AliceBlue;
        }

        private void buttonSO13_Click(object sender, EventArgs e)
        {
            buttonSO11.BackColor = Color.AliceBlue;
            buttonSO12.BackColor = Color.AliceBlue;
            buttonSO13.BackColor = Color.LightCoral;
        }

        private void buttonTV11_Click(object sender, EventArgs e)
        {
            buttonTV11.BackColor = Color.LightCoral;
            buttonTV12.BackColor = Color.AliceBlue;
            buttonTV13.BackColor = Color.AliceBlue;
            buttonTV14.BackColor = Color.AliceBlue;
        }

        private void buttonTV12_Click(object sender, EventArgs e)
        {
            buttonTV11.BackColor = Color.AliceBlue;
            buttonTV12.BackColor = Color.LightCoral;
            buttonTV13.BackColor = Color.AliceBlue;
            buttonTV14.BackColor = Color.AliceBlue;

            panelTryvalist.Visible = true;
            panelMetodVogon.Visible = false;
        }

        private void buttonTV13_Click(object sender, EventArgs e)
        {
            buttonTV11.BackColor = Color.AliceBlue;
            buttonTV12.BackColor = Color.AliceBlue;
            buttonTV13.BackColor = Color.LightCoral;
            buttonTV14.BackColor = Color.AliceBlue;

            panelMetodVogon.Visible = true;
            panelTryvalist.Visible = false;
        }

        private void buttonTV14_Click(object sender, EventArgs e)
        {
            buttonTV11.BackColor = Color.AliceBlue;
            buttonTV12.BackColor = Color.AliceBlue;
            buttonTV13.BackColor = Color.AliceBlue;
            buttonTV14.BackColor = Color.LightCoral;

            panelTryvalist.Visible = true;
            panelMetodVogon.Visible = false;
        }

        private void buttonVS11_Click(object sender, EventArgs e)
        {
            buttonVS11.BackColor = Color.LightCoral;
            buttonVS12.BackColor = Color.AliceBlue;
            buttonVS13.BackColor = Color.AliceBlue;
            buttonVS14.BackColor = Color.AliceBlue;
            contextNaCil.Show();
        }

        private void buttonVS12_Click(object sender, EventArgs e)
        {
            buttonVS11.BackColor = Color.AliceBlue;
            buttonVS12.BackColor = Color.LightCoral;
            buttonVS13.BackColor = Color.AliceBlue;
            buttonVS14.BackColor = Color.AliceBlue;
            panelNaBatareu.Visible = true;
        }

        private void buttonVS13_Click(object sender, EventArgs e)
        {
            buttonVS11.BackColor = Color.AliceBlue;
            buttonVS12.BackColor = Color.AliceBlue;
            buttonVS13.BackColor = Color.LightCoral;
            buttonVS14.BackColor = Color.AliceBlue;

            panelNaBatareu.Visible = false;

            GarmataUstanovka gu = new GarmataUstanovka();
            gu.ShowDialog();
        }

        private void buttonVS14_Click(object sender, EventArgs e)
        {
            buttonVS11.BackColor = Color.AliceBlue;
            buttonVS12.BackColor = Color.AliceBlue;
            buttonVS13.BackColor = Color.AliceBlue;
            buttonVS14.BackColor = Color.LightCoral;
            panelNaBatareu.Visible = false;

            VytrataNaGarmatu vng = new VytrataNaGarmatu();
            vng.ShowDialog();
        }

        private void buttonVV11_Click(object sender, EventArgs e)
        {
            buttonVV11.BackColor = Color.LightCoral;
            buttonVV12.BackColor = Color.AliceBlue;
            buttonVV13.BackColor = Color.AliceBlue;
            buttonVV14.BackColor = Color.AliceBlue;
            buttonVV16.BackColor = Color.AliceBlue;
        }

        private void buttonVV12_Click(object sender, EventArgs e)
        {
            buttonVV11.BackColor = Color.AliceBlue;
            buttonVV12.BackColor = Color.LightCoral;
            buttonVV13.BackColor = Color.AliceBlue;
            buttonVV14.BackColor = Color.AliceBlue;
            buttonVV16.BackColor = Color.AliceBlue;
        }

        private void buttonVV13_Click(object sender, EventArgs e)
        {
            buttonVV11.BackColor = Color.AliceBlue;
            buttonVV12.BackColor = Color.AliceBlue;
            buttonVV13.BackColor = Color.LightCoral;
            buttonVV14.BackColor = Color.AliceBlue;
            buttonVV16.BackColor = Color.AliceBlue;
        }

        private void buttonVV14_Click(object sender, EventArgs e)
        {
            buttonVV11.BackColor = Color.AliceBlue;
            buttonVV12.BackColor = Color.AliceBlue;
            buttonVV13.BackColor = Color.AliceBlue;
            buttonVV14.BackColor = Color.LightCoral;
            buttonVV16.BackColor = Color.AliceBlue;

            Gotovnist gotovnist = new Gotovnist();
            gotovnist.ShowDialog();
        }

        private void buttonVV16_Click(object sender, EventArgs e)
        {
            buttonVV11.BackColor = Color.AliceBlue;
            buttonVV12.BackColor = Color.AliceBlue;
            buttonVV13.BackColor = Color.AliceBlue;
            buttonVV14.BackColor = Color.AliceBlue;
            buttonVV16.BackColor = Color.LightCoral;
        }

        private void buttonYk11_Click(object sender, EventArgs e)
        {
            buttonYk11.BackColor = Color.LightCoral;
            buttonYk12.BackColor = Color.AliceBlue;
        }

        private void buttonYk12_Click(object sender, EventArgs e)
        {
            buttonYk11.BackColor = Color.AliceBlue;
            buttonYk12.BackColor = Color.LightCoral;
        }
        #endregion



        private void tabPage3_Validating(object sender, CancelEventArgs e)
        {
            /*DialogResult result2 = MessageBox.Show(Resources.Meteonablyzheniy_tabPage5_Validating_Підтвердити_зміни_, @"Команда старшого начальника",
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result2 == DialogResult.Cancel)
            {
                    e.Cancel = true;
            }*/
        }

        private void textKolBP5_Validating(object sender, CancelEventArgs e)
        {
            /*DialogResult result2 = MessageBox.Show(Resources.Meteonablyzheniy_tabPage5_Validating_Підтвердити_зміни_, @"Бойовий порядок",
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result2 == DialogResult.Cancel)
            {
                e.Cancel = true;
            }*/
        }

        private void tabPage5_Validating(object sender, CancelEventArgs e)
        {
            /*DialogResult result2 = MessageBox.Show(Resources.Meteonablyzheniy_tabPage5_Validating_Підтвердити_зміни_, @"Команда командира батареї",
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result2 == DialogResult.Cancel)
            {
                e.Cancel = true;
            }*/
        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }
        

        

    }      
    }

    

