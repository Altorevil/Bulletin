using System;

namespace SkladannyaBuletnya.lib
{
    /// <summary>
    /// Таблица 5 - приближённый бюллетень.
    /// </summary>
    public class Table5 {

        #region Fields

        private readonly Double[] delAwy = new Double[9] { 1, 2, 3, 3, 4, 4, 4, 5, 5 };

        public readonly Double[] Y = new Double[9] { 200, 400, 800, 1200, 1600, 2000, 2400, 3000, 4000 };

        public readonly Double[] delTauY = new Double[9];
        public readonly Double[] delTauYV = new Double[9];

        public readonly Double[] awy = new Double[9];

        public readonly Double[] Wy = new Double[9];

        #endregion

        #region Methods

        private Table5(String bulletin, out Double delH) {
            #region Validate
            if (bulletin == null) {
                throw new ArgumentNullException("Аргументы не могут быть нулевыми.");
            }
            #endregion

            try {
                String[] tokens = bulletin.Split(new[] { ' ', '-', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                #region Validate
                if (tokens.Length != 4 + 2 * 9) {
                    throw new FormatException("В бюллетне слишком мало, либо слишком много параметров.");
                }
                if (tokens[0] != "1107") {
                    throw new FormatException("Неизвестный тип бюллетня.");
                }
                #endregion

                delH = Math.Floor(Double.Parse(tokens[3]) / 100.0);
                if (delH >= 500 - Dashboard.Tolerance) {
                    delH = 500 - delH;
                }
                for (Int32 tmp, i = 0; i < 9; ++i) {
                    if (!Double.TryParse(tokens[4 + i * 2], out Y[i]) ||
                        !Int32.TryParse(tokens[4 + i * 2 + 1], out tmp)) {
                        throw new FormatException("Не удалось считать аргументы из бюллетня.");
                    }
                    delTauY[i] = tmp / 10000;
                    awy[i] = (tmp / 100) % 100;
                    Wy[i] = tmp % 100;

                    Y[i] *= 100;
                    if (delTauY[i] >= 50 - Dashboard.Tolerance) {
                        delTauY[i] = 50 - delTauY[i];
                    }
                }
            } catch (FormatException fex) {
                throw new FormatException(String.Format("Бюллетень \"{0}\" задан в неправильном формате.", bulletin), fex);
            }
        }

        public Table5()
        {
        }

        public Double[] DelForOutput (Double delTau0mp) 
        {
            Double[] delTauYV = new Double[9];
            for (Int32 high = 0; high < 9; ++high)
            {
                delTauY[high] = Table4.GetDelTauY(Y[high], delTau0mp);
                delTauYV[high] = Table4.GetDelTauY(Y[high], delTau0mp);
                Table4.GetDelTauY(Y[high], delTau0mp);
                if (delTauY[high] < 0)
                {
                    delTauY[high] = 50.0d + Math.Abs(delTauY[high]);
                    delTauYV[high] = 50.0d + Math.Abs(delTauY[high]);
                }
            }       
                return delTauYV;          
        }

        public Double[] AwForOut (Double aw0) {
        Double[] awyV = new Double[9];
        for (Int32 high = 0; high < 9; ++high)
            {
               awy[high] = aw0 + delAwy[high];
               awyV[high] = aw0 + delAwy[high];
            }
        return awyV;
        }

        public Double[] WyForOut(Double W0)
        {
            Double[] WyV = new Double[9];
            for (Int32 high = 0; high < 9; ++high)
            {
                Wy[high] = Table3.GetWy(Y[high], W0);
                WyV[high] = Table3.GetWy(Y[high], W0);
            }
            return WyV;
        }

        public Double GetDelTauY(Double Ym) {
            return TableUtils.Get(Y, Ym, delTauY);
        }

        public Double GetAwy(Double Ym) {
            return TableUtils.Get(Y, Ym, awy);
        }

        public Double GetWy(Double Ym) {
            return TableUtils.Get(Y, Ym, Wy);
        }

        #endregion

        #region Factory

        /// <summary>
        /// Таблица 5 за данными бюллетня «Метеосредний».
        /// </summary>
        /// <remarks>
        /// Данные бюлетня должны быть правильно форматированы
        /// и разделены пробелами, символами '-' и переносами строк.
        /// </remarks>
        /// <param name="bulletin">Бюллетень «Метеосредний».</param>
        /// <param name="delH">Значение ∆H из бюллетня.</param>
        /// <example>
        ///  1107-20105-0180-51010-
        ///              02-652112-
        ///              04-632214-
        ///              08-602315-
        ///              12-582316-
        ///              16-571808-
        ///              20-561809-
        ///              24-561909-
        ///              30-541909-
        ///              40-532010-
        /// </example>
        /// <exception cref="FormatException">
        /// Если бюллетень <c>bulletin</c> форматирован неправильно.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Если <c>bulletin</c> нулевой.
        /// </exception>        
        public static Table5 FromMeteoseredniy(String bulletin, out Double delH) {
            return new Table5(bulletin, out delH);
        }

        #endregion
		
    }

}