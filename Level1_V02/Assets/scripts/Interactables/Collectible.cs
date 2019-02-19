using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Collectible : MonoBehaviour
{
    public NavMeshAgent playerAgent;

    public virtual void MoveToCollectible(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;
        playerAgent.destination = this.transform.position;
        StartCoroutine(WaitForAction());
    }

    IEnumerator WaitForAction()
    {
        //yield return new WaitWhile(() => playerAgent.velocity == new Vector3(0, 0, 0));    <--- Das auch!
        //yield return new WaitWhile(() => playerAgent.velocity != new Vector3(0, 0, 0));    <--- Das hier ist böse Roman :D
        Collect();
        yield return null;
    }

    public virtual void Collect()
    {
        Debug.Log("Collecting base class.");
    }
}