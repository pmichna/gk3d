namespace gk3d
{
    class Fog
    {
        public bool IsFogEnabled { get; set; }
        public int FogStart { get; set; }
        public int FogEnd { get; set; }
        public float FogPower { get; set; }

        public Fog()
        {
            IsFogEnabled = false;
            FogStart = 5;
            FogEnd = 100;
            FogPower = 0.5f;
        }
    }
}
