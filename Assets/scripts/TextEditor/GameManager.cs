using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        GetComponent<TextFileReader>().ReadTextFile(@"C:\Users\skowronek\Desktop\ProjektHannah\Assets\scripts\TextEditor\Text_Editor_ViaMaterialia_Level1.txt");
	}
}
