using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private List<PowerUp> pool;
    private readonly Dictionary<Player, HashSet<PowerUp>> owned = new()
    {
        { Player.White, new HashSet<PowerUp>() },
        { Player.Black, new HashSet<PowerUp>() }
    };

    private IEnumerator Start()
    {
        while (TurnSystem.Instance == null) yield return null;

        pool = Resources.LoadAll<PowerUp>("PowerUps").ToList();
        TurnSystem.Instance.OnFiveMovesPerPlayer += HandleFiveMoves;
    }

    private void OnDestroy()
    {
        if (TurnSystem.Instance != null)
            TurnSystem.Instance.OnFiveMovesPerPlayer -= HandleFiveMoves;
    }

    private void HandleFiveMoves(Player owner, int batch)
    {
        if (batch > 3) return;
        var notOwned = pool.Where(pu => !owned[owner].Contains(pu)).ToList();
        if (notOwned.Count < 2) return;

        Shuffle(notOwned);
        var choices = new List<PowerUp> { notOwned[0], notOwned[1] };

        PowerUpPicker.Instance.Show(
            choices,
            owner,
            pu =>
            {
                pu.Apply(GameManager.Instance, owner);
                owned[owner].Add(pu);
            });
    }

    private static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}

