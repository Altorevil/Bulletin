using System;

namespace SkladannyaBuletnya.lib
{
    /// <summary>
    /// Таблица 4.
    /// </summary>
    public static class Table4 {

        #region Data

        private static readonly Double[] dtau0mp = new Double[15] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50 };

        private static readonly Double[] Y = new Double[9] { 200, 400, 800, 1200, 1600, 2000, 2400, 3000, 4000 };

        private static readonly Double[][] negativeData = new Double[9][] {
            new Double[15] { 0, -1, -2, -3, -4, -5, -6, -7, -8, -8, -9, -20, -29, -39, -49 },
            new Double[15] { 0, -1, -2, -3, -4, -5, -6, -6, -7, -8, -9, -19, -29, -38, -48 },
            new Double[15] { 0, -1, -2, -3, -4, -5, -6, -6, -7, -7, -8, -18, -28, -37, -46 },
            new Double[15] { 0, -1, -2, -3, -4, -4, -5, -5, -6, -7, -8, -17, -26, -35, -44 },
            new Double[15] { 0, -1, -2, -3, -3, -4, -4, -5, -6, -7, -8, -17, -25, -34, -42 },
            new Double[15]  { 0, -1, -2, -3, -3, -4, -4, -5, -6, -6, -7, -16, -24, -32, -40 },
            new Double[15]  { 0, -1, -2, -2, -3, -4, -4, -5, -5, -6, -7, -15, -23, -31, -38 },
            new Double[15]  { 0, -1, -2, -2, -3, -4, -4, -4, -5, -5, -6, -15, -22, -30, -37 },
            new Double[15]  { 0, -1, -2, -2, -3, -4, -4, -4, -4, -5, -6, -14, -20, -27, -34 }
        };

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">
        /// Если в таблице нет информации для заданых входных данных.
        /// </exception>
        /// <param name="Y"></param>
        /// <param name="delTaump"></param>
        /// <returns></returns>
        public static Double GetDelTauY(Double Y, Double delTaump) {
            if (delTaump >= 0)
            {
                TableUtils.GetIndex(Table4.Y, Y); // Just assert Y height exist.
                return delTaump;
            }
            return TableUtils.StraightGet(Table4.Y, Y, dtau0mp, -delTaump, negativeData);
        }

        #endregion

    }

}

/*
 * Добавить документацию.
 */
