using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackHome : MonoBehaviour
{

    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BackHome"))
        {
            anim.SetBool("stopRun", true);
        }

    }



}
