using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunsenBrennerClose : MonoBehaviour {

    public GameObject BunsenBrennerWindow;

	public void CloseWindow()
    {
        BunsenBrennerWindow.SetActive(false);
    }
}
