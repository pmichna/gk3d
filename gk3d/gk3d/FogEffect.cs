namespace gk3d
{
    class FogEffect
    {
        public bool IsFogEnabled { get; set; }
        public int FogStart { get; set; }
        public int FogEnd { get; set; }
        public float FogPower { get; set; }

        public FogEffect()
        {
            IsFogEnabled = false;
            FogStart = 5;
            FogEnd = 400;
            FogPower = 0.8f;
        }
    }
}
