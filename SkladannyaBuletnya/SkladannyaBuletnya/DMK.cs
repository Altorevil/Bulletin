using System;
using System.ComponentModel;
using System.Windows.Forms;
using SkladannyaBuletnya.lib;

namespace SkladannyaBuletnya
{
    public partial class Dmk : Form
    {
        readonly DateTime _dT = DateTime.Now;
        
        public Dmk()
        {
            InitializeComponent();

            textDay.Text = Convert.ToString(_dT.Day); //Автоматичне виведення системного часу до textBox
            textHours.Text = Convert.ToString(_dT.Hour);
            textMinutes.Text = Convert.ToString(_dT.Minute);
            
        }       

        private void buttonSkladBul_Click(object sender, EventArgs e) 
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                int day = Convert.ToInt32(textDay.Text);

                int hours = Convert.ToInt32(textHours.Text);

                Double minutes = Convert.ToDouble(textMinutes.Text);
                minutes = Math.Round(minutes / 10);

                int hmc = Convert.ToInt32(textHmc.Text);

                int h0 = Convert.ToInt32(textH0.Text);

                int t0 = Convert.ToInt32(textT0.Text);

                Double aW0 = Convert.ToDouble(textAW0.Text);
                aW0 = Math.Round(aW0 / 6); //Переведення в радіани

                Double delH0 = h0 - 750;
                Double delH0V = delH0;
                if (delH0 < 0)
                {
                    delH0V = 500.0d + Math.Abs(delH0V);
                }

                Double delTv = Table1.GetDelTv(t0);
                Double tau0 = t0 + delTv;
                Double delTau0Mp = tau0 - 15.9;
                Double delTau0MpV = delTau0Mp;
                if (delTau0MpV < 0)
                {
                    delTau0MpV = 50.0d + Math.Abs(delTau0MpV);
                }

                Double[] awy = new Double[9];
                Double[] wy = new Double[9];

                Table5 tf = new Table5();
                var delTauY = tf.DelForOutput(delTau0Mp);
                Double[] y = tf.Y;

                Double w0 = Convert.ToInt32(textW0.Text);
                awy = tf.AwForOut(aW0);
                if (w0 == 0 || w0 == 1)
                {
                    for (int i = 0; i < wy.Length; i++)
                    {
                        wy[i] = 0;
                    }
                }
                else
                {
                    wy = tf.WyForOut(w0);
                }
                textBulletin.Clear();
                textBulletin.Text = (@"Метеонаближений - ");
                textBulletin.Text += Convert.ToString(day.ToString("00"));
                textBulletin.Text += (hours.ToString("00"));
                textBulletin.Text += (minutes.ToString("0") + @" - ");
                textBulletin.Text += (hmc.ToString("0000") + @" - ");
                textBulletin.Text += (delH0V.ToString("000"));
                textBulletin.Text += (delTau0MpV.ToString("00") + @" - ");
                for (int i = 0; i < 9; i++)
                {
                    y[i] = y[i] / 100;
                    textBulletin.Text += Convert.ToString("\r\n" + y[i].ToString("00") + "-" + Math.Round(delTauY[i]).ToString("00") + Math.Round(awy[i]).ToString("00") + Math.Round(wy[i]).ToString("00") + "- ");
                }
                buttonApplyDMK.Enabled = true;
            }
        }

        private void DMK_Shown(object sender, EventArgs e)
        {
            textDay.Focus();
        }        

        private void textDay_Validating(object sender, CancelEventArgs e)
        {
            if (textDay.Text.Length != 0)
            {
                int day = Convert.ToInt32(textDay.Text);
                if (day < 1 || day > 31)
                {
                    errorProvider1.SetError(textDay, "Введено невірне число місяця  \n                                    1-31");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(textDay, "");
                }
            }
            else
            {
                errorProvider1.SetError(textDay, "Введіть число місяця");
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
                    errorProvider1.SetError(textHours, "Введено невірну кількість годин  \n                                    0-23");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(textHours, "");
                }
            }
            else
            {
                errorProvider1.SetError(textHours, "Введіть кількість годин");
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
                    errorProvider1.SetError(textMinutes, "Введено невірну кількість хвилин  \n                                    0-59");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(textMinutes, "");
                }
            }
            else
            {
                errorProvider1.SetError(textMinutes, "Введіть кількість хвилин");
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
                    errorProvider1.SetError(textHmc, "Введено невірне значення висоти метеопосту \n                                    0-9999");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(textHmc, "");
                }
            }
            else
            {
                errorProvider1.SetError(textHmc, "Введіть значення висоти метеопосту");
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
                    errorProvider1.SetError(textH0, "Введено невірне значення атмосферного тиску \n                                    500-800");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(textH0, "");
                }
            }
            else
            {
                errorProvider1.SetError(textH0, "Введіть значення атмосферного тиску");
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
                    errorProvider1.SetError(textT0, "Введено невірне значення температури повітря \n                                    -50 - +50");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(textT0, "");
                }
            }
            else
            {
                errorProvider1.SetError(textT0, "Введіть значення температури повітря");
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
                    errorProvider1.SetError(textAW0, "Введено невірне значення напрямку середнього вітру \n                                    0-360");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(textAW0, "");
                }
            }
            else
            {
                errorProvider1.SetError(textAW0, "Введіть значення напрямку середнього вітру");
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
                    errorProvider1.SetError(textW0, "Введено невірне значення швидкості середнього вітру \n                                    0-15");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(textW0, "");
                }
            }
            else
            {
                errorProvider1.SetError(textW0, "Введіть значення швидкості середнього вітру");
                e.Cancel = true;
            }
        }        
            
    }
}
