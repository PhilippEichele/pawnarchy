using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private List<PowerUp> pool;

    private IEnumerator Start()
    {
        // Warte bis TurnSystem sicher da ist
        while (TurnSystem.Instance == null)
            yield return null;

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
		Debug.Log($"[PowerUpManager] Signal empfangen: {owner} Batch {batch}");

		if (batch > 3 || pool.Count < 2) return;

		Shuffle(pool);
		var choices = new List<PowerUp> { pool[0], pool[1] };

		if (PowerUpPicker.Instance == null)
		{
			Debug.LogError("[PowerUpManager] ❌ Picker.Instance ist NULL!");
			return;
		}

		PowerUpPicker.Instance.Show(
			choices,
			owner,
			pu => pu.Apply(GameManager.Instance, owner)
		);
	}


    // Fisher-Yates Shuffle
    private static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}

