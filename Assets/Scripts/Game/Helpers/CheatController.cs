using UnityEngine;
using UnityEngine.EventSystems;

public class CheatController : MonoBehaviour, IPointerDownHandler
{
    private const int CLOCK_COUNT_MAX = 5;

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.clickCount == CLOCK_COUNT_MAX)
        {
            Card.IsCheat = !Card.IsCheat;
            SoundSingleton.Instance.PlaySelect();
            Message.Banner("Cheat: " + (Card.IsCheat ? "ON" : "OFF"), MessageType.FatalError, 2f);
        }
    }
}
