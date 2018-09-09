namespace PubgServiceLayer.Model
{
    // Represents a Compressed Game Mode Stats entity
    public class PubgModeStats
    {
        //Using float for speed
        public double KillDeath { get; set; }

        public int AverageDamage { get; set; }

        public double WinRatio { get; set; }
    }
}
