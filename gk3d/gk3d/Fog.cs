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
            FogStart = 800;
            FogEnd = 2000;
            FogPower = 0.5f;
        }
    }
}
