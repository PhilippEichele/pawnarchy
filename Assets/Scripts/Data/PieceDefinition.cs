using UnityEngine;

[CreateAssetMenu(menuName = "Chess/PieceDefinition")]
public class PieceDefinition : ScriptableObject
{
    public string displayName;
    public Sprite sprite;
}
