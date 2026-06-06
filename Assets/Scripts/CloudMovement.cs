using UnityEngine;
using UnityEngine.UI;

public class CloudMovement : MonoBehaviour
{
    [Header("1. Float Movement (Left-Right)")]
    public float floatSpeed = 1.0f;     
    public float floatDistance = 0.05f; 

    [Header("2. Jelly Effect (Squash & Stretch)")]
    public float squishSpeed = 2.0f;     
    public float squishAmount = 0.05f;   

    [Header("3. Rotation (Wind)")]
    public float rotationSpeed = 0.5f;
    public float rotationAngle = 2.0f;   

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

        //Float Movement
        float newPivotX = startPivot.x + Mathf.Sin(time * floatSpeed) * floatDistance;
        rectTrans.pivot = new Vector2(newPivotX, startPivot.y);

        //Jelly Effect
        float scaleX = startScale.x + Mathf.Sin(time * squishSpeed) * squishAmount;
        float scaleY = startScale.y + Mathf.Cos(time * squishSpeed) * squishAmount;
        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        //Rotation 
        float rotationZ = Mathf.Sin(time * rotationSpeed) * rotationAngle;
        transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
    }
}