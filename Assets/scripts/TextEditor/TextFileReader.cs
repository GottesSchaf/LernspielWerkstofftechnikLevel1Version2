using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TextFileReader : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private GameObject[] ingameTexts;
    public GameObject[] IngameTexts
    {
        get { return ingameTexts; }
    }

    #endregion

    #region Methods

    // Reads the TextFile on the given Path and puts the String in the right Textbox
    public void ReadTextFile(string path)
    {
        string rawText = File.ReadAllText(@path, System.Text.Encoding.Default);

        string[] textFileRegions = rawText.Split('|');

        foreach(string s in textFileRegions)
        {
            string[] splitText = s.Split('~');

            foreach(GameObject x in ingameTexts)
            {
                if (x.name == splitText[0].Trim())
                {
                    x.GetComponent<Text>().text = Regex.Replace(splitText[1], @"\t|\n|\r", "");
                    break;
                }
            }
        }
    }

    #endregion

}
