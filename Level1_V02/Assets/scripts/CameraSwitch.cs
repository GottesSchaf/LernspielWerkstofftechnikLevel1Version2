using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour {

    public Camera Playercamera;
    public Camera Tutorialcamera;
    public GameObject Inventory;
    public GameObject Photo;
    public GameObject Speed1;
    public GameObject Speed2;
    public GameObject Button; 

    //switching between two different cameras

    public void ShowTutorialView()
    {
        Playercamera.enabled = false;
        Tutorialcamera.enabled = true;
    }

    public void ShowPlayerView()
    {
        Tutorialcamera.enabled = false;
        Playercamera.enabled = true;
        Button.SetActive(false);
        // enabling parts of the canvas
        Inventory.SetActive(true);
        Photo.SetActive(true);
        Speed1.SetActive(true);
        Speed2.SetActive(true);
    }


}
