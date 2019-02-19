using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchColorMode : MonoBehaviour {

    [SerializeField] Dropdown drop;
    [SerializeField] SimulateColorBlindness simColor;

    public void UpdateColorMode()
    {
        simColor.UpdateColorMode(drop.value);
    }
}
