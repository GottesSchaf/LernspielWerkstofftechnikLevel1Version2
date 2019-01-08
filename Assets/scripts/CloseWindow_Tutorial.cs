using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWindow_Tutorial : MonoBehaviour {

    public void CloseThisWindow()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
