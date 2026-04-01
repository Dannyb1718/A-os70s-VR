using UnityEngine;

[DisallowMultipleComponent]
public class InteractionConstraints : MonoBehaviour
{
    [Header("Rotación")]
    public bool permitirRotacion = true;
    public bool rotarX = true;
    public bool rotarY = true;
    public bool rotarZ = false;

    [Header("Zoom")]
    public bool permitirZoom = true;

    [Header("Movimiento (posición)")]
    public bool permitirMovimiento = true;
    public Vector3 minPosition = new Vector3(-1f, 0f, -1f);
    public Vector3 maxPosition = new Vector3(1f, 1f, 1f);

    [Header("Rotación inicial")]
    public Vector3 rotacionInicialE;
    public bool usarRotacionE = false; // para activar/desactivar si aplica al presionar E
}