using UnityEngine;

/// <summary>
/// Attach this component to every NPC that should have dialogue.
///
/// Requirements:
///  - A Collider (set as Trigger) on the NPC or a child object defines the interaction radius.
///  - The Player GameObject must be tagged "Player".
/// </summary>
public class NPCDialogueTrigger : MonoBehaviour
{
    // ─── Inspector ────────────────────────────────────────────────────────────
    [Header("Dialogue Content")]
    [Tooltip("ScriptableObject with this NPC's lines.")]
    public DialogueData dialogueData;

    [Header("Trigger Settings")]
    [Tooltip("Auto-creates a sphere trigger if no Collider is found on this object.")]
    public float autoTriggerRadius = 2.5f;

    [Header("Visual Hint (optional)")]
    [Tooltip("GameObject shown above NPC when player is in range (e.g. '!' icon).")]
    public GameObject interactionHint;

    // ─── Private ──────────────────────────────────────────────────────────────
    private bool _playerInRange;

    // ─── Unity ────────────────────────────────────────────────────────────────
    private void Awake()
    {
        // Auto-add sphere collider if none exists
        if (GetComponent<Collider>() == null)
        {
            var sc            = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger      = true;
            sc.radius         = autoTriggerRadius;
        }

        if (interactionHint != null)
            interactionHint.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = true;

        if (interactionHint != null)
            interactionHint.SetActive(true);

        // Auto-open dialogue as soon as the player enters range.
        // If you prefer a manual "press F to talk" style, move the
        // StartDialogue call to Update() with an input check instead.
        if (dialogueData != null && !DialogueManager.Instance.IsOpen)
            DialogueManager.Instance.StartDialogue(dialogueData, transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = false;

        if (interactionHint != null)
            interactionHint.SetActive(false);

        // Close dialogue if player walks away
        if (DialogueManager.Instance.IsOpen)
            DialogueManager.Instance.EndDialogue();
    }
}
