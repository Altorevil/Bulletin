using System;

namespace SkladannyaBuletnya.lib
{
    /// <summary>
    /// Таблица 3.
    /// </summary>
    public static class Table3 {

        #region Data

        private static readonly Double[] Y = new Double[9] { 200, 400, 800, 1200, 1600, 2000, 2400, 3000, 4000 };

        private static readonly Double[] W0 = new Double[14] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        private static readonly Double[][] data = new Double[9][] {
            new Double[14] { 3, 4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 22 },
            new Double[14] { 4, 5, 7, 10, 11, 12, 14, 17, 18, 20, 22, 23, 25, 27 },
            new Double[14] { 4, 5, 8, 10, 11, 13, 15, 18, 19, 21, 23, 25, 27, 28 },
            new Double[14] { 4, 5, 8, 11, 12, 13, 16, 19, 20, 22, 24, 26, 28, 30 },
            new Double[14] { 4, 6, 8, 11, 13, 14, 17, 20, 21, 23, 25, 27, 29, 32 },
            new Double[14] { 4, 6, 9, 11, 13, 14, 17, 20, 21, 24, 26, 28, 30, 32 },
            new Double[14] { 4, 6, 9, 12, 14, 15, 18, 21, 22, 25, 27, 29, 32, 34 },
            new Double[14] { 5, 6, 9, 12, 14, 15, 18, 21, 23, 25, 28, 30, 32, 36 },
            new Double[14] { 5, 6, 10, 12, 14, 16, 19, 22, 24, 26, 29, 32, 34, 36 }
        };

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">
        /// Если в таблице нет информации для заданых входных данных.
        /// </exception>
        /// <param name="Y">Высота для которой берётся значение.</param>
        /// <param name="W0">Скорость ветра.</param>
        /// <returns></returns>
        public static Double GetWy(Double Y, Double W0) {
            return TableUtils.StraightGet(Table3.Y, Y, Table3.W0, W0, data);
        }

        #endregion

    }

}

/*
 * Дополнить документацию.
 */