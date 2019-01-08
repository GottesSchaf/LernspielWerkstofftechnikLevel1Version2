using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSwitchScript : MonoBehaviour {

    [SerializeField] List<GameObject> switchList = new List<GameObject>();
    [SerializeField] List<Material> materials = new List<Material>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] PlatzGasSchalterScript platzGas;
    [SerializeField] HauptGasSchalterScript hauptGas;

    private void Start()
    {
        PlatzGasSchalterScript.platzGasOn = materials[6];
        PlatzGasSchalterScript.platzGasOff = materials[7];
        HauptGasSchalterScript.fuseOn = materials[8];
        HauptGasSchalterScript.fuseOff = materials[9];
        switchList[0].GetComponent<Renderer>().material = materials[10];
        switchList[1].GetComponent<Renderer>().material = materials[11];
        switchList[2].GetComponent<Image>().sprite = sprites[3];
        switchList[3].GetComponent<Image>().sprite = sprites[4];
        switchList[4].GetComponent<Image>().sprite = sprites[5];
    }
    public void SwitchToColorBlindness()
    {
        PlatzGasSchalterScript.platzGasOn = materials[0];
        PlatzGasSchalterScript.platzGasOff = materials[1];
        HauptGasSchalterScript.fuseOn = materials[2];
        HauptGasSchalterScript.fuseOff = materials[3];
        switchList[0].GetComponent<Renderer>().material = materials[4];
        switchList[1].GetComponent<Renderer>().material = materials[5];
        switchList[2].GetComponent<Image>().sprite = sprites[0];
        switchList[3].GetComponent<Image>().sprite = sprites[1];
        switchList[4].GetComponent<Image>().sprite = sprites[2];
        platzGas.UpdateMaterial();
        hauptGas.UpdateMaterial();
    }

    public void SwitchToNormalColor()
    {
        PlatzGasSchalterScript.platzGasOn = materials[6];
        PlatzGasSchalterScript.platzGasOff = materials[7];
        HauptGasSchalterScript.fuseOn = materials[8];
        HauptGasSchalterScript.fuseOff = materials[9];
        switchList[0].GetComponent<Renderer>().material = materials[10];
        switchList[1].GetComponent<Renderer>().material = materials[11];
        switchList[2].GetComponent<Image>().sprite = sprites[3];
        switchList[3].GetComponent<Image>().sprite = sprites[4];
        switchList[4].GetComponent<Image>().sprite = sprites[5];
        platzGas.UpdateMaterial();
        hauptGas.UpdateMaterial();
    }
}
