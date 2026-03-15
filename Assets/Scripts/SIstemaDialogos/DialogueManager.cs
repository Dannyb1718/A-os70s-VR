using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton that controls dialogue flow.
/// Attach to a persistent GameObject in your scene (e.g. "DialogueManager").
/// </summary>
public class DialogueManager : MonoBehaviour
{
    // ─── Singleton ────────────────────────────────────────────────────────────
    public static DialogueManager Instance { get; private set; }

    // ─── Inspector ────────────────────────────────────────────────────────────
    [Header("References")]
    [Tooltip("The WorldSpace dialogue panel prefab (or scene object).")]
    public DialogueUI dialogueUI;

    [Header("Settings")]
    [Tooltip("Seconds to wait before the panel appears (optional cinematic feel).")]
    public float openDelay = 0.1f;

    // ─── Events (optional hooks for audio, animations, etc.) ─────────────────
    [Header("Events")]
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    // ─── Private state ────────────────────────────────────────────────────────
    private DialogueData   _currentData;
    private int            _lineIndex;
    private bool           _isOpen;

    // ─── Unity ────────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ─── Public API ───────────────────────────────────────────────────────────

    /// <summary>
    /// Called by NPCDialogueTrigger when the player enters range.
    /// </summary>
    public void StartDialogue(DialogueData data, Transform npcTransform)
    {
        if (_isOpen) return;

        _currentData = data;
        _lineIndex   = 0;
        _isOpen      = true;

        StartCoroutine(OpenWithDelay(npcTransform));
        onDialogueStart?.Invoke();
    }

    /// <summary>
    /// Advances to the next line, or closes if finished.
    /// Wired to the "Continue" button on DialogueUI.
    /// </summary>
    public void NextLine()
    {
        _lineIndex++;

        if (_lineIndex >= _currentData.lines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueUI.ShowLine(_currentData.npcName, _currentData.lines[_lineIndex]);
    }

    /// <summary>
    /// Closes the dialogue immediately.
    /// Wired to the "Exit" button on DialogueUI.
    /// </summary>
    public void EndDialogue()
    {
        if (!_isOpen) return;

        _isOpen = false;
        dialogueUI.Hide();
        onDialogueEnd?.Invoke();
    }

    public bool IsOpen => _isOpen;

    // ─── Private ──────────────────────────────────────────────────────────────
    private IEnumerator OpenWithDelay(Transform npcTransform)
    {
        yield return new WaitForSeconds(openDelay);

        dialogueUI.Show(npcTransform);
        dialogueUI.ShowLine(_currentData.npcName, _currentData.lines[_lineIndex]);
    }
}
