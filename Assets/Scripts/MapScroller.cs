using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class MapScroller : MonoBehaviour
{
    public ScrollRect myScrollRect; 

    void Start()
    {
        StartCoroutine(SetPositionToBottom());
    }

    IEnumerator SetPositionToBottom()
    {
        yield return new WaitForEndOfFrame();
        if (myScrollRect != null)
        {
            myScrollRect.verticalNormalizedPosition = 0f;
        }
    }
}