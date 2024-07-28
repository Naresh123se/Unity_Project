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
            float a = GetComponent<PathfindingTester>().speed;
            GetComponent<PathfindingTester>().speed = 0f;
            yield return new WaitForSeconds(0.4f);
            GetComponent<PathfindingTester>().speed = a;
        }
    }


}