using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the World-Space Canvas dialogue panel.
///
/// Hierarchy expected:
///  DialogueCanvas  (Canvas – World Space, has GraphicRaycaster + PhysicsRaycaster on Camera)
///    └─ Panel
///         ├─ NPCNameText   (TextMeshProUGUI)
///         ├─ DialogueText  (TextMeshProUGUI)
///         ├─ ContinueBtn   (Button)
///         └─ ExitBtn       (Button)
/// </summary>
public class DialogueUI : MonoBehaviour
{
    // ─── Inspector ────────────────────────────────────────────────────────────
    [Header("UI References")]
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Button          continueButton;
    public Button          exitButton;

    [Header("Position Settings")]
    [Tooltip("Offset relative to the NPC's position where the panel will appear.")]
    public Vector3 offsetFromNPC = new Vector3(0f, 2.2f, 0f);

    [Tooltip("How quickly the panel slides in (lerp speed).")]
    public float appearSpeed = 8f;

    [Header("Typewriter Effect")]
    public bool  useTypewriter     = true;
    public float typewriterSpeed   = 0.04f; // seconds per character

    [Header("Billboard")]
    [Tooltip("Panel always faces the main camera when true.")]
    public bool faceCamera = true;

    // ─── Private ──────────────────────────────────────────────────────────────
    private Canvas    _canvas;
    private Transform _npcTransform;
    private Vector3   _targetPosition;
    private bool      _visible;
    private Coroutine _typewriterCoroutine;

    // ─── Unity ────────────────────────────────────────────────────────────────
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();

        // Wire buttons (also set in Inspector if preferred)
        continueButton.onClick.AddListener(OnContinueClicked);
        exitButton.onClick.AddListener(OnExitClicked);

        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!_visible) return;

        // Smoothly follow the NPC
        if (_npcTransform != null)
        {
            _targetPosition    = _npcTransform.position + offsetFromNPC;
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * appearSpeed);
        }

        // Billboard: face camera
        if (faceCamera && Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0f, 180f, 0f);   // flip so text reads correctly
        }
    }

    // ─── Public API ───────────────────────────────────────────────────────────

    public void Show(Transform npcTransform)
    {
        _npcTransform      = npcTransform;
        _targetPosition    = npcTransform.position + offsetFromNPC;
        transform.position = _targetPosition;

        _visible = true;
        gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _visible = false;
        gameObject.SetActive(false);

        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);
    }

    public void ShowLine(string npcName, string line)
    {
        npcNameText.text = npcName;

        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);

        if (useTypewriter)
            _typewriterCoroutine = StartCoroutine(TypewriterRoutine(line));
        else
            dialogueText.text = line;

        // Disable Continue while text is printing; re-enable when done
        continueButton.interactable = !useTypewriter;
    }

    // ─── Private ──────────────────────────────────────────────────────────────

    private IEnumerator TypewriterRoutine(string line)
    {
        continueButton.interactable = false;
        dialogueText.text           = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        continueButton.interactable = true;
        _typewriterCoroutine        = null;
    }

    private void OnContinueClicked() => DialogueManager.Instance.NextLine();
    private void OnExitClicked()     => DialogueManager.Instance.EndDialogue();
}
