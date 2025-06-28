using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Knight Forward Leap",
                 fileName = "PU_KnightForwardLeap")]
public class KnightForwardLeapPowerUp : PowerUp
{
    public override void Apply(GameManager gm, Player owner)
    {
        PowerUpRules.For(owner).knightStraightLeap = true;
    }
}
