using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    
    public void OnPointerDown(PointerEventData eventData)
    {
        CharacterController.Instance.TryToBuildBridge();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CharacterController.Instance.TryToStopBuildBridge();
    }
}
