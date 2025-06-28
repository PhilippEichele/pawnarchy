using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Pawn Backwards",
                 fileName = "PU_PawnBackwards")]
public class PawnBackwardsPowerUp : PowerUp
{
    public override void Apply(GameManager gm, Player owner)
    {
        PowerUpRules.For(owner).pawnsBackwards = true;
    }
}
