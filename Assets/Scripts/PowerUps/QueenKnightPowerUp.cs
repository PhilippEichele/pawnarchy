using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Queen Knight Move",
                 fileName = "PU_QueenKnight")]
public class QueenKnightPowerUp : PowerUp
{
    public override void Apply(GameManager gm, Player owner)
    {
        PowerUpRules.For(owner).queenKnightMove = true;
    }
}
