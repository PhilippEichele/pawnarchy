using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Extra Turn", fileName = "PU_ExtraTurn")]
public class ExtraTurnPowerUp : PowerUp
{
    public override void Apply(GameManager gm, Player owner)
    {
        gm.EndTurn(); // beendet sofort ⇒ derselbe Spieler ist erneut dran
    }
}
