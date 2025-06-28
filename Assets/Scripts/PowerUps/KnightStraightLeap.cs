using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Knight Straight Leap",
                 fileName = "PU_KnightStraightLeap")]
public class KnightForwardLeapPowerUp : PowerUp
{
    public override void Apply(GameManager gm, Player owner)
    {
        PowerUpRules.For(owner).knightStraightLeap = true;
    }
}
