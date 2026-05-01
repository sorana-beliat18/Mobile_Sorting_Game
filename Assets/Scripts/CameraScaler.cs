using UnityEngine;
public class CameraScaler : MonoBehaviour
{
    public float targetWidth = 6f;
    void Update()
    {
        Camera.main.orthographicSize = targetWidth / (2f * Camera.main.aspect);
    }
}