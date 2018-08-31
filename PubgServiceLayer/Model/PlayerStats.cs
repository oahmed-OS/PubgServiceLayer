namespace PubgServiceLayer.Model
{
    public class PlayerStats
    {

        public string PlayerName { get; set; }

        public PubgModeStats SoloFPP { get; set; }

        public PubgModeStats DuoFPP { get; set; }

        public PubgModeStats SquadFPP { get; set; }

        public PubgModeStats SoloTPP { get; set; }

        public PubgModeStats DuoTPP { get; set; }

        public PubgModeStats SquadTPP { get; set; }
    }
}
