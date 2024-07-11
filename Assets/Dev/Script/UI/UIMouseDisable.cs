using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseDisable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Player player;
    public event System.Action OnPointerEnterEvent;
    public event System.Action OnPointerExitEvent;
    private void OnEnable()
    {
        OnPointerEnterEvent+=player.DisableClick;
        OnPointerExitEvent+=player.EnableClick;
    }
    private void OnDisable()
    {
        OnPointerEnterEvent-=player.DisableClick;
        OnPointerExitEvent-=player.EnableClick;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        OnPointerEnterEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke();
    }

    
}
