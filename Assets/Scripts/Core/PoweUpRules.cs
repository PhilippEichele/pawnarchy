using System.Collections.Generic;

public static class PowerUpRules
{
    // Map von Spieler → Feature-Set
    private static readonly Dictionary<Player, Flags> table = new()
    {
        { Player.White, new Flags() },
        { Player.Black, new Flags() }
    };

    public static Flags For(Player p) => table[p];

    // --------- Feature-Bits ---------
    public class Flags
    {
        public bool pawnsAlwaysDouble;   // Bauer darf jeden Zug 2 Felder vor
        public bool pawnsBackwards;      // Bauer darf 1 Feld rückwärts
        public bool knightStraightLeap;  // Pferd: „2 vor“ springen
        public bool bishopPhaseAllies;   // Läufer phasen durch eigene Figuren
    }
}
