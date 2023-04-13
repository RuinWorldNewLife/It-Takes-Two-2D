using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorArea : MonoBehaviour
{
    private Animator redAnim;
    private Animator darkAnim;
    private void Start()
    {
        redAnim = transform.GetChild(0).GetComponent<Animator>();
        darkAnim = transform.GetChild(1).GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerDark"))
        {
            StaticData.buttonFourNum++;
            redAnim.SetBool("IsOpen", true);
            darkAnim.SetBool("IsOpen", true);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerDark"))
        {
            StaticData.buttonFourNum--;
            if (StaticData.buttonFourNum <= 0)
            {
                redAnim.SetBool("IsOpen", false);
                darkAnim.SetBool("IsOpen", false);
            }
        }
    }
}
