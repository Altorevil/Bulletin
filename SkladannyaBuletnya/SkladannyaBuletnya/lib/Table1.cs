using System;

namespace SkladannyaBuletnya.lib
{
    /// <summary>
    /// Таблица 1.
    /// </summary>
    public static class Table1 {

        #region Data

        /// <summary>
        /// Температура воздуха.
        /// </summary>
        /// <remarks>В градусах цельсия.</remarks>
        private static readonly Double[] t0 = new Double[11] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 };

        /// <summary>
        /// Поправка к температуре.
        /// </summary>
        /// <remarks>В градусах цельсия.</remarks>
        private static readonly Double[] dTv = new Double[11] { 0.3, 0.5, 0.6, 0.9, 1.3, 1.8, 2.4, 3.3, 4.4, 5.8, 7.4 };

        #endregion

        #region Methods

        /// <summary>
        /// Нахождение в таблице 1 значения dTv за t0.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Когда температура <c>t</c> не найдена в таблице.</exception>
        /// <param name="t">Ключ для поиска в таблице.</param>
        /// <returns>Результат поиска в таблице.</returns>
        public static Double GetDelTv(Double t) {
            if (t < 0) {
                return 0;
            }
            try {
                Double d = TableUtils.Get(t0, t, dTv);
                return d;
            } catch (IndexOutOfRangeException ioe) {
                throw new IndexOutOfRangeException(String.Format("В Таблице 1 для температуры {0} не было найдено соответствующего значения dTv", t), ioe);
            }
        }

        #endregion

    }

}
