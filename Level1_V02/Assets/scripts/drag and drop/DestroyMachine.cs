using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMachine : MonoBehaviour {

    [SerializeField] GameObject machineNew;
    [SerializeField] GameObject gameOverScreen, gameWonScreen;

    public void DestroyMe()
    {
        for (int i = 0; i < machineNew.transform.childCount; i++)
        {
            machineNew.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        Invoke("ShowGameOverScreen", 1);
    }

    void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    public void ShowGameWonScreen()
    {
        Debug.Log("public void ShowGameWonScreen()");
        gameWonScreen.SetActive(true);
    }
}
