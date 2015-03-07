using System;

namespace SkladannyaBuletnya.lib
{
    class TableVR2
    {
        private static readonly Double[] Y = new Double[9] { 200, 400, 800, 1200, 1600, 2000, 2400, 3000, 4000 };

        private static readonly Double[][] data = new Double[9][] {
            new Double[12] { 3, 4, 5, 6, 7, 7, 8, 9, 10, 11, 12, 12 },
            new Double[12] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
            new Double[12] { 4, 5, 6, 7, 8, 9, 10, 11, 13, 14, 15, 16 },
            new Double[12] { 4, 5, 7, 8, 8, 9, 11, 12, 13, 15, 15, 16 },
            new Double[12] { 4, 6, 7, 8, 9, 10, 11, 13, 14, 15, 17, 17 },
            new Double[12] { 4, 6, 7, 8, 9, 10, 11, 13, 14, 16, 17, 18 },
            new Double[12] { 4, 6, 8, 9, 9, 10, 12, 14, 15, 16, 18, 19 },
            new Double[12] { 5, 6, 8, 9, 10, 11, 12, 14, 15, 17, 18, 19 },
            new Double[12] { 5, 6, 8, 9, 10, 11, 12, 14, 16, 18, 19, 20 }
        };

        private static readonly Double[] Dg = new Double[12] { 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150 };

        private static readonly Double[] alpha = new Double[9] { 0, 1, 2, 2, 3, 3, 3, 4, 4 };

        public readonly Double[] awy = new Double[9];
        public readonly Double[] Wy = new Double[9];

        public Double[] AwForOut(Double aw0)
        {
            Double[] awyV = new Double[9];
            for (Int32 high = 0; high < 9; ++high)
            {
                awy[high] = aw0 + alpha[high];
                awyV[high] = aw0 + alpha[high];
            }
            return awyV;
        }

        public Double[] WyForOut(Double Dg)
        {
            Double[] WyV = new Double[9];
            for (Int32 high = 0; high < 9; ++high)
            {
                Wy[high] = GetWy(Y[high], Dg);
                WyV[high] = GetWy(Y[high], Dg);
            }
            return WyV;
        }

        public static Double GetWy(Double Y, Double Dg)
        {
            return TableUtils.StraightGet(TableVR2.Y, Y, TableVR2.Dg, Dg, data);
        }
    }
}
