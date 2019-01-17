using System;
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

    [SerializeField]
    private BunsenBrenner bb;
    public BunsenBrenner BB
    {
        get { return bb; }
    }

    public string bbtext;

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

    public void ReadTextFileBB(string path)
    {
        string rawText = Regex.Replace(File.ReadAllText(@path, System.Text.Encoding.Default), @"\t|\n|\r", "");
        
        string[] textFileRegions = rawText.Split('|');

        foreach (string s in textFileRegions)
        {
            string[] splitText = s.Split('~');
            bbtext += "\n" + splitText[1];
            switch (splitText[0])
            {
                case "4.Wert_Verwendet":
                    if (Regex.Replace(splitText[1], @"\t|\n|\r", "").ToLower() == "wahr")
                    {
                        bb.graphPunkt4 = true;
                    }
                    else
                    {
                        bb.graphPunkt4 = false;
                    }
                    break;
                case "5.Wert_Verwendet":
                    if (Regex.Replace(splitText[1], @"\t|\n|\r", "").ToLower() == "wahr")
                    {
                        bb.graphPunkt5 = true;
                    }
                    else
                    {
                        bb.graphPunkt5 = false;
                    }
                    break;
                case "BB1_Zieltemperatur":
                    int z = 0;

                    foreach (string x in splitText[1].Split(new string[] { "Element :" }, StringSplitOptions.None))
                    {
                        if (z > 0)
                        {
                            bb.BB1_Zieltemp[z - 1] = (float)Convert.ToDouble(Regex.Replace(x, @"\t|\n|\r| ", ""));
                        }
                        z++;
                    }
                    break;
                case "BB2_Zieltemperatur":
                    int zz = 0;
                    foreach (string x in splitText[1].Split(new string[] { "Element :" }, StringSplitOptions.None))
                    {
                        if (zz > 0)
                        {
                            bb.BB2_Zieltemp[zz - 1] = (float)Convert.ToDouble(Regex.Replace(x, @"\t|\n|\r| ", ""));
                        }
                        zz++;
                    }
                    break;
                case "BB3_Zieltemperatur":
                    int zzz = 0;
                    foreach (string x in splitText[1].Split(new string[] { "Element :" }, StringSplitOptions.None))
                    {
                        if (zzz > 0)
                        {
                            bb.BB3_Zieltemp[zzz - 1] = (float)Convert.ToDouble(Regex.Replace(x, @"\t|\n|\r| ", ""));
                        }
                        zzz++;
                    }
                    break;
                case "BB4_Zieltemperatur":
                    int zzzz = 0;
                    foreach (string x in splitText[1].Split(new string[] { "Element :" }, StringSplitOptions.None))
                    {
                        if (zzzz > 0)
                        {
                            bb.BB4_Zieltemp[zzzz - 1] = (float)Convert.ToDouble(Regex.Replace(x, @"\t|\n|\r| ", ""));
                        }
                        zzzz++;
                    }
                    break;
                case "BB1_Zeit":
                    int o = 0;
                    foreach (string x in splitText[1].Split(new string[] { "Element :" }, StringSplitOptions.None))
                    {

                        if (o > 0)
                        {
                            bb.BB1_Zeit[o - 1] = Convert.ToInt32(Regex.Replace(x, @"\t|\n|\r| ", ""));
                        }
                        o++;
                    }
                    break;
                case "BB2_Zeit":
                    int oo = 0;
                    foreach (string x in splitText[1].Split(new string[] { "Element :" }, StringSplitOptions.None))
                    {
                        if (oo > 0)
                        {
                            bb.BB2_Zeit[oo - 1] = Convert.ToInt32(Regex.Replace(x, @"\t|\n|\r| ", ""));
                        }
                        oo++;
                    }
                    break;
                case "BB3_Zeit":
                    int ooo = 0;
                    foreach (string x in splitText[1].Split(new string[] { "Element :" }, StringSplitOptions.None))
                    {
                        if (ooo > 0)
                        {
                            bb.BB3_Zeit[ooo - 1] = Convert.ToInt32(Regex.Replace(x, @"\t|\n|\r| ", ""));
                        }
                        ooo++;
                    }
                    break;
                case "BB4_Zeit":
                    int oooo = 0;
                    foreach (string x in splitText[1].Split(new string[] { "Element :" }, StringSplitOptions.None))
                    {
                        if (oooo > 0)
                        {
                            bb.BB4_Zeit[oooo -1 ] = Convert.ToInt32(Regex.Replace(x, @"\t|\n|\r| ", ""));
                        }
                        oooo++;
                    }
                    break;
            }        
        }
    }

    #endregion

}
