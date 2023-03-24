using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoHelper : Singleton<MonoHelper>
{
    Coroutine coroutine = null;
    Coroutine coroutine2 = null;
    /// <summary>
    /// ÿ��һ��ʱ��ִ��һ��Э��
    /// </summary>
    public void InvokeReapeat(Action method, float interval, Func<bool> endCondition)
    {
        coroutine2 = StartCoroutine(RepeatCRT(method, interval, endCondition));
    }

    public void InvokeReapeatStop()
    {
        StopCoroutine(coroutine2);
    }
    IEnumerator RepeatCRT(Action method, float interval, Func<bool> endCondition)
    {
        while (true)
        {
            if (interval <= 0)
            {
                yield return 0;
            }
            else
            {
                yield return new WaitForSeconds(interval);
            }
            method();
            if (endCondition())
            {
                yield break;
            }
        }
    }
    /// <summary>
    /// һ��ʱ���ִ��method����
    /// </summary>
    /// <param name="method"></param>
    /// <param name="waittingTime"></param>
    public void WaitSomeTimeInvoke(Action method, float waittingTime, Func<bool> endCondition)
    {
        if (!endCondition())
        {
            coroutine = StartCoroutine(WaitSomeTimeCRT(method, waittingTime));
        }
        else if (endCondition() && coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
    IEnumerator WaitSomeTimeCRT(Action method, float waittingTime)
    {

        yield return new WaitForSeconds(waittingTime);
        method();
        yield break;
    }
    /// <summary>
    /// �ٶȲ�ֵ�ݼ�����
    /// </summary>
    /// <param name="time"></param>
    /// <param name="velocity"></param>
    /// <returns></returns>
    public float TimeLerpDown(float time,float velocity)
    {
        
        if(time < 0.05f)
        {
            time = 0f;
        }
        else
        {
            time = Mathf.Lerp(time,0, velocity * Time.deltaTime);
        }
        return time;
    }
}
