using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Door : Interactive
{
	public Vector3 destination;
    [SerializeField] private GameObject questWindow;
    [SerializeField] GameObject gaszufuhrAus;

    public override void Interact()
    {
        if (this.name != "door_elevatorout")
        {
            if (this.gameObject.name.Contains("hallway_room3") == false)
            {
                playerAgent.Warp(destination);
            }
            else if (BunsenBrenner.hauptGasSchalter || BunsenBrenner.platzGasSchalter)
            {
                gaszufuhrAus.SetActive(true);
            }
            else
            {
                playerAgent.Warp(destination);
            }
            if (this.gameObject.name == "Doors_Tutorial_2" || this.gameObject.name == "Door_Tutorial_Backside")
            {
                questWindow.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Choose a floor first.");
        }
    }
}