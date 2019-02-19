using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Interactive : MonoBehaviour
{
    public NavMeshAgent playerAgent;

    public virtual void MoveToInteraction(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;
        playerAgent.destination = this.transform.position;
        StartCoroutine(WaitForAction());
    }

    IEnumerator WaitForAction()
    {
        yield return new WaitWhile(() => playerAgent.velocity == new Vector3(0,0,0));
        yield return new WaitWhile(() => playerAgent.velocity != new Vector3(0, 0, 0));
        Interact();
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting with base class.");
    }
}