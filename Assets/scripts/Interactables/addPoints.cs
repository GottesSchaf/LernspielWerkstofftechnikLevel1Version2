using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addPoints : Interactive
{
	public int number;

    public override void Interact()
	{
        Debug.Log("Adding "+number+" Points");
		playerscript.points = playerscript.points + number;
		Debug.Log("New Points: "+playerscript.points);
	}
}
