using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormWindow : Interactive
{
    public Transform ThisForm;              //Position des Fensters
    public static Transform instance;       //Position
    public CameraFollow boop;               //Kamera Position
    public GameObject formWindow;           //Fenster für die Form
    public GameObject deckel;               //Deckel für die Form

    void Update()
    {
        instance = this.ThisForm;
        boop = CameraFollow.instance;

        //if (CameraFollow.instance.closeupInteraction == true)
        //{
        //    ThisForm.GetComponent<BoxCollider>().enabled = true;
        //}
    }
    //Wenn der Close Button gedrückt wird, wird die Form "geschlossen"
    public void CloseWindow()
    {
        
        formWindow.SetActive(false);
    }
    //Wenn man auf die Form klickt, öffnet sie sich
    public override void Interact()
    {
        formWindow.SetActive(true);
    }

    public void FormSchliessen()
    {
        deckel.SetActive(true);
    }

    public void FormOeffnen()
    {
        deckel.SetActive(false);
    }
}
