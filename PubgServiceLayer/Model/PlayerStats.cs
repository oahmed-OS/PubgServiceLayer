namespace PubgServiceLayer.Model
{
    public class PlayerStats
    {

        string PlayerName { get; set; }

        PubgModeStats SoloFPP { get; set; }

        PubgModeStats DuoFPP { get; set; }

        PubgModeStats SquadFPP { get; set; }

        PubgModeStats SoloTPP { get; set; }

        PubgModeStats DuoTPP { get; set; }

        PubgModeStats SquadTPP { get; set; }
    }
}
