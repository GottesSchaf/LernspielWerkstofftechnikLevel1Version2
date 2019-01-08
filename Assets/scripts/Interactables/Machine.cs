using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Interactive
{
    public Transform MachineWindow;
    public override void Interact()
    {
        Debug.Log("Interacting with Machine.");
        MachineWindow.gameObject.SetActive(true);
		CameraFollow.instance.closeupInteraction = true;
    }
}
