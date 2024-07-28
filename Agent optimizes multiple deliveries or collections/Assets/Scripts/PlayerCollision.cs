using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ignore"))
        {
            float a = GetComponent<ACOTester>().speed;
            GetComponent<ACOTester>().speed = 0f;
            yield return new WaitForSeconds(0.9f);
            GetComponent<ACOTester>().speed = a;
        }
    }


}