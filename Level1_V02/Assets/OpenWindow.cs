using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWindow : Interactive {

    [SerializeField] GameObject tiegel_Zettel_Window;


    public override void Interact()
    {
        tiegel_Zettel_Window.gameObject.SetActive(true);
    }
}
