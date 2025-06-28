using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Pawn Always Double",
                 fileName = "PU_PawnAlwaysDouble")]
public class PawnDoubleStepPowerUp : PowerUp
{
    public override void Apply(GameManager gm, Player owner)
    {
        PowerUpRules.For(owner).pawnsAlwaysDouble = true;
    }
}
