using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    [Header("Meta")]
    public string displayName = "Neues Power-Up";
    [TextArea(2,4)] public string description;
    public Sprite icon;

    public abstract void Apply(GameManager gm, Player owner);
}

