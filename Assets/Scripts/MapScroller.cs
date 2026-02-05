using UnityEngine;
using UnityEngine.UI; // Avem nevoie de asta pentru UI
using System.Collections; // Avem nevoie de asta pentru Coroutine (asteptare)

public class MapScroller : MonoBehaviour
{
    public ScrollRect myScrollRect; // Aici vom conecta Scroll View-ul

    void Start()
    {
        // Pornim o mica "cronometrare" ca sa asteptam o fractiune de secunda
        // Motivul: Unity are nevoie de un moment sa calculeze marimea hartilor uriase
        StartCoroutine(SetPositionToBottom());
    }

    IEnumerator SetPositionToBottom()
    {
        // Asteptam sfarsitul frame-ului curent ca UI-ul sa se aseze
        yield return new WaitForEndOfFrame();

        // Fortam pozitia la 0 (JOS DE TOT)
        if (myScrollRect != null)
        {
            myScrollRect.verticalNormalizedPosition = 0f;
        }
    }
}