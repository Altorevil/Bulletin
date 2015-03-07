namespace SkladannyaBuletnya
{
    class DannieCeli
    {
        public int VC; 
        public int Fc6;
        public int Gc6;
        public int Fc16;
        public int Gc16;
        public int FcMore;
        public int GcMore;
        public int Sy;
        public int ZC;
        public int VS;
        public int SO;
        public int Np;
        public int Yk;
        public int ZC1;
        public readonly int ZC2;

        public DannieCeli(int VC, int Fc6, int Gc6, int Fc16, int Gc16, int FcMore, int GcMore, int Sy, int ZC, int VS, int SO, int Np, int Yk, int ZC1, int ZC2)
        {
            this.VC = VC;
            this.Fc6 = Fc6;
            this.Gc6 = Gc6;
            this.Fc16 = Fc16;
            this.Gc16 = Gc16;
            this.FcMore = FcMore;
            this.GcMore = GcMore;
            this.Sy = Sy;
            this.ZC = ZC;
            this.VS = VS;
            this.SO = SO;
            this.Np = Np;
            this.Yk = Yk;
            this.ZC1 = ZC1;
            this.ZC2 = ZC2;
        }

        public DannieCeli(int ZC2)
        {
            ZC2 = this.ZC2;
        }

       
    }
}
