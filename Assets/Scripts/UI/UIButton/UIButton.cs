using Coffee.UIEffects;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class UIButton : UIButtonClickEventBase
{
    UIShadow _uiShadow;
    
    public float _duration = 1f;
    Color _targetColor;
    Color _zeroColor;
    private void Start()
    {
        _uiShadow = GetComponent<UIShadow>();
        //初始化目标颜色
        _targetColor = new Color(_uiShadow.effectColor.r, _uiShadow.effectColor.g, _uiShadow.effectColor.b, 1f);
        _zeroColor = new Color(_uiShadow.effectColor.r, _uiShadow.effectColor.g, _uiShadow.effectColor.b, 0f);
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        //数值渐渐变为显示
        DOTween.To(() => _uiShadow.effectColor, x => _uiShadow.effectColor = x, _targetColor, _duration);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        //数值渐渐变为隐藏
        DOTween.To(() => _uiShadow.effectColor, x => _uiShadow.effectColor = x, _zeroColor, _duration);
    }

}
