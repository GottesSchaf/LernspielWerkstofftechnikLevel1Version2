using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookshelf : Interactive
{
    public override void Interact()
    {
        Debug.Log("You interacted with a bookshelf.!!!");
        CameraFollow.instance.closeupInteraction = true;
    }
}
