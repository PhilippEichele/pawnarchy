using System.Collections.Generic;

public static class PowerUpRules
{
    private static readonly Dictionary<Player, Flags> table = new()
    {
        { Player.White, new Flags() },
        { Player.Black, new Flags() }
    };

    public static Flags For(Player p) => table[p];

    public class Flags
    {
        public bool pawnsAlwaysDouble;
        public bool pawnsBackwards;
        public bool knightStraightLeap;
        public bool bishopPhaseAllies;
		public bool queenKnightMove;
    }
}
