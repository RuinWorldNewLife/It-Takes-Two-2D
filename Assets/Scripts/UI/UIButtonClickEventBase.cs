using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonClickEventBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(name + " Game Object Clicked!");
        MusicMgr.Instance.PlaySounds("click");
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }
}
