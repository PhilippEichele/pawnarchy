using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Bishop Phase Allies",
                 fileName = "PU_BishopPhase")]
public class BishopPhasePowerUp : PowerUp
{
    public override void Apply(GameManager gm, Player owner)
    {
        PowerUpRules.For(owner).bishopPhaseAllies = true;
    }
}
