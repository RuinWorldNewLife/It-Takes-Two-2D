using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOut : MonoBehaviour
{
    public string Tag;
    private Animator _Anim;

    private void Start()
    {
        StaticData.buttonSevenNum = 0;
        _Anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag))
        {
            StaticData.buttonSevenNum++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag))
        {
            StaticData.buttonSevenNum--;
        }
    }
}
