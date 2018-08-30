namespace PubgServiceLayer.Model
{
    public class PlayerStats
    {

        string PlayerName { get; set; }

        PubgStats SoloFPP { get; set; }

        PubgStats DuoFPP { get; set; }

        PubgStats SquadFPP { get; set; }

        PubgStats SoloTPP { get; set; }

        PubgStats DuoTPP { get; set; }

        PubgStats SquadTPP { get; set; }
    }
}
