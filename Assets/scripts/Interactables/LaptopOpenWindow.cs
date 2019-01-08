using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopOpenWindow : Interactive {

    public Transform laptopWindow;              //Bunsen Brenner Fenster
    public GameObject Laborkittel;          //Laborkittel des Spielers
    public GameObject LaborkittelError;     //Error Fenster
    [SerializeField] Transform infoFenster;
    bool infoFensterBool = false;
    public override void Interact()
    {
        //Wenn das Laborkittel bereits angezogen ist, dann öffne das Bunsen Brenner Fenster,
        if (Laborkittel.activeSelf)
        {
            laptopWindow.gameObject.SetActive(true);
            if(infoFensterBool == false)
            {
                infoFenster.gameObject.SetActive(true);
                infoFensterBool = true;
            }
        }
        //Falls nicht, dann öffne den Warnhinweis zum Laborkittel
        else
        {
            LaborkittelError.SetActive(true);
        }
        //CameraFollow.instance.closeupInteraction = true;
    }
}
