using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Self references")]
    [SerializeField] protected Image background;

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    UIManager.Instance.EnterPanelUnderCursor(this);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    UIManager.Instance.ExitPanelUnderCursor(this);
    //}
}
