using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColor : Interactive
{
	public Material colorA;
	public Material colorB;

	public override void Interact()
	{
        if (GetComponent<Renderer>().material.name.Contains(colorA.name))
        {
            GetComponent<Renderer>().material = colorB;
            Debug.Log("The color changed from "+colorA.name+" to "+colorB.name+".");
        }
        else
        {
            GetComponent<Renderer>().material = colorA;
            Debug.Log("The color changed from " + colorB.name + " to " + colorA.name + ".");
        }
	}
}
