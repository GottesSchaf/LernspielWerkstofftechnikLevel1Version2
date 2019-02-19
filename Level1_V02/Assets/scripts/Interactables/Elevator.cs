using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : Interactive
{
	public Vector3 destination;
    public GameObject door;

    public override void Interact()
    {
        StartCoroutine(LeavingElevator());
    }
       
    IEnumerator LeavingElevator()
    {
        playerAgent.destination = door.transform.position;
        yield return new WaitWhile(() => playerAgent.velocity == new Vector3(0, 0, 0));
        yield return new WaitWhile(() => playerAgent.velocity != new Vector3(0, 0, 0));
		playerAgent.Warp(destination);
    }
}
