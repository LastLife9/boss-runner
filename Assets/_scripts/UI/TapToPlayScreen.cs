using UnityEngine;
using UnityEngine.EventSystems;

public class TapToPlayScreen : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.GameStart();
    }
}
