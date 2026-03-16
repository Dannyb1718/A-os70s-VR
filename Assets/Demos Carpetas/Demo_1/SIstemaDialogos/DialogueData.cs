using UnityEngine;

/// <summary>
/// ScriptableObject that holds all dialogue lines for a single NPC.
/// Create via: Assets > Create > Dialogue > Dialogue Data
/// </summary>
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("NPC Info")]
    public string npcName = "NPC";

    [Header("Dialogue Lines")]
    [TextArea(2, 5)]
    public string[] lines;
}
