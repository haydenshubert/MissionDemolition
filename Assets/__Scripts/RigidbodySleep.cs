using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]   // Makes sure any object with this script has a rigid body
public class RigidbodySleep : MonoBehaviour
{
    private int sleepCountdown = 4; // counts number of FixedUpdates in a row telling rigid body to sleep
    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();  // GetComponent<>() is expensive so call ones with Awake()
    }

    void FixedUpdate()
    {
        if (sleepCountdown > 0) // called and sleepCountdown is decremented four times then stops
        {
            rigid.Sleep();
            sleepCountdown--;
        }
    }
}
