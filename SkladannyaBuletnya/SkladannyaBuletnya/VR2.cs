using System;
using System.ComponentModel;
using System.Windows.Forms;
using SkladannyaBuletnya.lib;

namespace SkladannyaBuletnya
{
    public partial class VR2 : Form
    {
        DateTime dT = DateTime.Now;

        public VR2()
        {
            InitializeComponent();

            textDay.Text = Convert.ToString(dT.Day); //Автоматичне виведення системного часу до textBox
            textHours.Text = Convert.ToString(dT.Hour);
            textMinutes.Text = Convert.ToString(dT.Minute);            
        }       

        private void buttonSkladBul_Click(object sender, EventArgs e)
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
                buttonApplyVR2.Enabled = true;
            }            
        }

        private void VR2_Shown(object sender, EventArgs e)
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

        private void textHmc_Validating(object sender, CancelEventArgs e)
        {
            if (textHmc.Text.Length != 0)
            {
                int Hmc = Convert.ToInt32(textHmc.Text);
                if (Hmc < 0 || Hmc > 9999)
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

        private void textDalnZnosuKul_Validating(object sender, CancelEventArgs e)
        {
            if (textDalnZnosuKul.Text.Length != 0)
            {
                int dzk = Convert.ToInt32(textDalnZnosuKul.Text);
                if (dzk < 0 || dzk > 150)
                {
                    errorProvider1.SetError(textDalnZnosuKul, "Введено невірне значення дальності зносу куль \n                                    0-150");
                    e.Cancel = true;
                }
                else
                {
                    errorProvider1.SetError(textDalnZnosuKul, "");
                }
            }
            else
            {
                errorProvider1.SetError(textDalnZnosuKul, "Введіть значення дальності зносу куль");
                e.Cancel = true;
            }
        }

        private void VR2_Validating(object sender, CancelEventArgs e)
        {
            CancelEventArgs ev = new CancelEventArgs();
            textAW0_Validating(textAW0, ev);
            e.Cancel = ev.Cancel;
            if (!Validate())
            {
                buttonSkladBul.Enabled = true;
            }
        }


    }
}
