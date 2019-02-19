using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        GetComponent<TextFileReader>().ReadTextFileBB(Path.Combine(Application.dataPath, "Bunsenbrenner.txt"));
        GetComponent<TextFileReader>().ReadTextFile(Path.Combine(Application.dataPath, "Textfelder.txt"));
        
    }
}
