using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            door.GetComponent<DoorHandler>().inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {
            door.GetComponent<DoorHandler>().inTrigger = false;
        }
    }
}
