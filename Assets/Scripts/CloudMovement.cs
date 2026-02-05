using UnityEngine;
using UnityEngine.UI;

public class CloudMovement : MonoBehaviour
{
    [Header("1. Float Movement (Left-Right)")]
    public float floatSpeed = 1.0f;      // How fast it floats
    public float floatDistance = 0.05f;  // How far it moves (0.05 is subtle)

    [Header("2. Jelly Effect (Squash & Stretch)")]
    public float squishSpeed = 2.0f;     // Deformation speed
    public float squishAmount = 0.05f;   // How much it squishes

    [Header("3. Rotation (Wind)")]
    public float rotationSpeed = 0.5f;
    public float rotationAngle = 2.0f;   // Rotation angle in degrees (very small)

    private RectTransform rectTrans;
    private Vector2 startPivot;
    private Vector3 startScale;

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        startPivot = rectTrans.pivot;
        startScale = transform.localScale;
    }

    void Update()
    {
        float time = Time.time;

        // --- A. FLOAT MOVEMENT (via Pivot) ---
        // By modifying the pivot, the image moves visually, but the "box" stays put in the Layout Group
        float newPivotX = startPivot.x + Mathf.Sin(time * floatSpeed) * floatDistance;
        rectTrans.pivot = new Vector2(newPivotX, startPivot.y);

        // --- B. JELLY EFFECT (Squash & Stretch) ---
        // Using Sin for X and Cos for Y to be opposite (when it widens, it shortens)
        float scaleX = startScale.x + Mathf.Sin(time * squishSpeed) * squishAmount;
        float scaleY = startScale.y + Mathf.Cos(time * squishSpeed) * squishAmount;
        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        // --- C. SUBTLE ROTATION ---
        float rotationZ = Mathf.Sin(time * rotationSpeed) * rotationAngle;
        transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
    }
}