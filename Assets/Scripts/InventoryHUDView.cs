using UnityEngine;
using UnityEngine.UI;

public class InventoryHUDView : MonoBehaviour
{
    [SerializeField] private Image[] slots = new Image[3];

    private void Start()
    {
        // Empieza todas las casillas invisibles
        foreach (var slot in slots)
        {
            if (slot != null)
            {
                slot.sprite = null;
                var c = slot.color;
                c.a = 0f;
                slot.color = c;
            }
        }

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnItemCollected += FillSlot;
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnItemCollected -= FillSlot;
    }

    private void FillSlot(int slotIndex, Sprite sprite)
    {
        if (slotIndex >= slots.Length || slots[slotIndex] == null) return;

        slots[slotIndex].sprite = sprite;
        var c = slots[slotIndex].color;
        c.a = 1f;
        slots[slotIndex].color = c;
    }
}