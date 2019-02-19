using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    private bool hitTarget;
    public bool HitTarget
    {
        get { return hitTarget; }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Boop");
        if (other.tag == "Target")
        {
            hitTarget = true;
        }
    }
}
