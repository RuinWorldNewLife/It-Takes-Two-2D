using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 月亮跟随玩家移动的代码
/// </summary>
public class MoonMove : MonoBehaviour
{
    private Transform _cam;
    private Vector3 _camStartPos;
    private Vector3 _myStartPos;
    private void Start()
    {
        _cam = GameObject.Find("Camera").transform.Find("Main Camera");
        _camStartPos = _cam.position;
        _myStartPos = transform.position;
    }

    private void FixedUpdate()
    {
        MoonMoveMothod();
    }
    private void MoonMoveMothod()
    {
        Vector3 _camDiffer = _cam.position - _camStartPos;
        //Vector3 _myDiffer = _camDiffer;
        transform.position = new Vector3(_camDiffer.x * 0.95f + _myStartPos.x, _camDiffer.y + _myStartPos.y, _camDiffer.z + _myStartPos.z);
    }
}
