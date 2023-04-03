using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// 脚本功能：UI的基类管理所有UI 
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class UIBase : MonoBehaviourPunCallbacks, IPointerClickHandler
{
    protected CanvasGroup canvasGroup;
    protected GraphicRaycaster graphicRaycaster;
    protected Coroutine coroutine;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        graphicRaycaster = GetComponent<GraphicRaycaster>();
    }
    private void Start()
    {

        if (canvasGroup != null)
        {
            this.canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }
        if (graphicRaycaster != null)
            graphicRaycaster.enabled = false; //检测关闭
    }

    /// <summary>
    /// 开始的状态
    /// </summary>
    public virtual void DoOnStart()
    {
        if (graphicRaycaster != null)
            graphicRaycaster.enabled = true; //检测开始
        //Debug.Log("进来");
        if (canvasGroup != null)
            canvasGroup.interactable = true; //交互开始
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(UpdateAlphaShow()); //显示
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true; //检测射线
    }

    /// <summary>
    /// 暂停的状态
    /// </summary>
    public virtual void DoOnPause()
    {
        if (graphicRaycaster != null)
            graphicRaycaster.enabled = false; //检测关闭
    }

    /// <summary>
    /// 恢复的状态
    /// </summary>
    public virtual void DoOnResume()
    {
        //事件交互关闭
        if (graphicRaycaster != null)
            graphicRaycaster.enabled = true; //检测开始

    }

    /// <summary>
    /// 退出的状态
    /// </summary>
    public virtual void DoOnExit()
    {
        if (canvasGroup != null)
            canvasGroup.interactable = false; //不去交互
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(UpdateAlphaHide()); //透明
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false; //不去检测射线
        if (graphicRaycaster != null)
            graphicRaycaster.enabled = false; //检测关闭
    }

    //显示
    public virtual IEnumerator UpdateAlphaShow()
    {
        if (canvasGroup != null)
        {
            do
            {
                yield return null;
                canvasGroup.alpha += Time.deltaTime;
            } while (canvasGroup.alpha < 1);

            canvasGroup.interactable = true; //交互开始
            canvasGroup.blocksRaycasts = true; //检测射线
            if (graphicRaycaster != null)
            {

                graphicRaycaster.enabled = true; //检测开始
            }
            
        }
        coroutine = null;
    }

    //隐藏
    public virtual IEnumerator UpdateAlphaHide()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false; //不去交互
            canvasGroup.blocksRaycasts = false; //不去检测射线
            if (graphicRaycaster != null)
                graphicRaycaster.enabled = false; //检测关闭
            do
            {
                yield return null;
                canvasGroup.alpha -= Time.deltaTime;
            } while (canvasGroup.alpha > 0);
        }
        coroutine = null;
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(name + " Game Object Clicked!");
        MusicMgr.Instance.PlaySounds("click");
    }
}
