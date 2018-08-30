﻿namespace PubgServiceLayer.Model
{
    // Represents a Compressed Game Mode Stats entity
    class PubgStats
    {
        //Using float for speed
        float KillDeath { get; set; }

        int AverageDamage { get; set; }

        float WinRatio { get; set; }
    }
}
