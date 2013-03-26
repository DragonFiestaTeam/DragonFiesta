namespace DragonFiesta.Configuration
{
    public sealed class ZoneConfiguration : Configuration
    {
        // Note - why ZoneId not dynamic?
        public ZoneConfiguration(byte pZoneId)
        {
            this.ZoneId = pZoneId;
        }
        public ZoneConfiguration()
        {
        }

        public byte ZoneId;
    }
}
