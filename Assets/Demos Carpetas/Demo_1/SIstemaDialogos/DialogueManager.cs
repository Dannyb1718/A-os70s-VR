using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton que controla el flujo completo del diálogo con opciones de respuesta.
///
/// Estados internos:
///   OpeningLines  →  mostrando las líneas de apertura del NPC
///   WaitingChoice →  esperando que el jugador elija una opción
///   ResponseLines →  mostrando las líneas de respuesta del NPC tras la elección
/// </summary>
public class DialogueManager : MonoBehaviour
{
    // ── Singleton ──────────────────────────────────────────────────────────────
    public static DialogueManager Instance { get; private set; }

    // ── Inspector ──────────────────────────────────────────────────────────────
    [Header("Referencias")]
    [Tooltip("Arrastra aquí el DialogueCanvas (World Space).")]
    public DialogueUI dialogueUI;

    [Header("Ajustes")]
    [Tooltip("Segundos de espera antes de abrir el panel.")]
    public float openDelay = 0.15f;

    [Header("Eventos (opcionales)")]
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    // ── Estado interno ─────────────────────────────────────────────────────────
    private enum Phase { Idle, OpeningLines, WaitingChoice, ResponseLines }

    private Phase         _phase        = Phase.Idle;
    private DialogueData  _data;
    private int           _lineIndex;          // índice dentro de la secuencia activa
    private string[]      _activeLines;        // apunta a openingLines o responseLines

    // ── Unity ──────────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── API pública ────────────────────────────────────────────────────────────

    /// <summary>Abre el diálogo con el NPC dado. Llamado por NPCDialogueTrigger.</summary>
    public void StartDialogue(DialogueData data, Transform npcTransform)
    {
        if (_phase != Phase.Idle) return;

        _data      = data;
        _lineIndex = 0;

        StartCoroutine(OpenWithDelay(npcTransform));
        onDialogueStart?.Invoke();
    }

    /// <summary>
    /// Avanza a la siguiente línea de la secuencia activa.
    /// Si es la última, pasa al siguiente estado (choices o cierre).
    /// Llamado por el botón "Continuar".
    /// </summary>
    public void NextLine()
    {
        _lineIndex++;

        if (_lineIndex < _activeLines.Length)
        {
            // Todavía hay líneas en esta secuencia
            dialogueUI.ShowLine(_data.npcName, _activeLines[_lineIndex]);
            return;
        }

        // Fin de la secuencia activa
        switch (_phase)
        {
            case Phase.OpeningLines:
                TryShowChoices();
                break;

            case Phase.ResponseLines:
                EndDialogue();
                break;
        }
    }

    /// <summary>
    /// Llamado cuando el jugador pulsa uno de los botones de opción.
    /// index: 0 = primera opción, 1 = segunda opción.
    /// </summary>
    public void OnChoiceSelected(int index)
    {
        if (_phase != Phase.WaitingChoice) return;
        if (_data.choices == null || index >= _data.choices.Length) { EndDialogue(); return; }

        DialogueChoice chosen = _data.choices[index];

        if (chosen.isExitChoice || chosen.responseLines == null || chosen.responseLines.Length == 0)
        {
            EndDialogue();
            return;
        }

        // Mostrar las líneas de respuesta del NPC
        _phase       = Phase.ResponseLines;
        _activeLines = chosen.responseLines;
        _lineIndex   = 0;

        dialogueUI.HideChoices();
        dialogueUI.ShowContinueButton(true);
        dialogueUI.ShowLine(_data.npcName, _activeLines[0]);
    }

    /// <summary>Cierra el panel inmediatamente. Llamado por botón Salir o exit choice.</summary>
    public void EndDialogue()
    {
        if (_phase == Phase.Idle) return;

        _phase = Phase.Idle;
        dialogueUI.Hide();
        onDialogueEnd?.Invoke();
    }

    public bool IsOpen => _phase != Phase.Idle;

    // ── Privados ───────────────────────────────────────────────────────────────

    private IEnumerator OpenWithDelay(Transform npcTransform)
    {
        _phase = Phase.OpeningLines;

        yield return new WaitForSeconds(openDelay);

        _activeLines = _data.openingLines;
        _lineIndex   = 0;

        dialogueUI.Show(npcTransform);
        dialogueUI.ShowContinueButton(true);
        dialogueUI.HideChoices();
        dialogueUI.ShowLine(_data.npcName, _activeLines[0]);
    }

    private void TryShowChoices()
    {
        if (_data.choices == null || _data.choices.Length == 0)
        {
            // Sin opciones: cierra directamente
            EndDialogue();
            return;
        }

        _phase = Phase.WaitingChoice;
        dialogueUI.ShowContinueButton(false);
        dialogueUI.ShowChoices(_data.choices);
    }
}
