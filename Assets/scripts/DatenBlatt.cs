using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatenBlatt : Interactive
{
    public Transform ThisPaper;             //Datenblatt Position
    public static Transform instance;       //Position
    public CameraFollow boop;               //Kamera Position
    public GameObject datenblatt;           //Datenblatt Objekt

    void Update()
    {
        instance = this.ThisPaper;
        boop = CameraFollow.instance;

        //if (CameraFollow.instance.closeupInteraction == true)
        //{
        //    ThisPaper.GetComponent<BoxCollider>().enabled = true;
        //}
    }
    //Wenn der Close Button gedrückt wird, wird das Datenblatt "geschlossen"
    public void CloseWindow()
    {
        datenblatt.SetActive(false);
    }
    //Wenn man auf das Datenblatt klickt, öffnet es sich
    public override void Interact()
    {
        datenblatt.SetActive(true);
    }
}
