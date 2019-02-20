using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TextFileReader : MonoBehaviour
{
    #region Variables

    public enum Level
    {
        Menu,
        Game
    }

    [SerializeField]
    private Level levelMode;

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

    [SerializeField]
    private GameObject book;
    public GameObject Book
    {
        get { return book; }
    }

    private List<Book> books;
    #endregion


    #region Methods

    private void Awake()
    {
        if(levelMode == Level.Game)
        {
            books = new List<Book>();
            foreach (Book b in book.GetComponentsInChildren<Book>())
            {
                books.Add(b);
            }
        }
    }

    // Reads the TextFile on the given Path and puts the String in the right Textbox or Book URL
    public void ReadTextFile(string path)
    {
        if (levelMode == Level.Game)
        {
            string rawText = File.ReadAllText(@path, System.Text.Encoding.Default);

            byte[] decbuff = Convert.FromBase64String(rawText);
            rawText = Encoding.UTF8.GetString(decbuff);

            string[] textFileRegions = rawText.Split('|');

            int i = 0;

            foreach (string s in textFileRegions)
            {
                string[] splitText = s.Split('~');

                foreach (GameObject x in ingameTexts)
                {
                    if (x.name == splitText[0].Trim())
                    {
                        x.GetComponent<Text>().text = Regex.Replace(splitText[1], Environment.NewLine, "");
                        break;
                    }
                }


                foreach (Book b in books)
                {
                    if (!splitText[0].Contains("BuchLink"))
                    {
                        break;
                    }
                    else if (splitText[0].Contains("BuchLink") && i < books.Count)
                    {
                        books[i].URL = Regex.Replace(splitText[1], Environment.NewLine, "");
                        i++;
                        break;
                    }
                }
            }
        }
        else if (levelMode == Level.Menu)
        {
            string rawText = File.ReadAllText(@path, System.Text.Encoding.Default);

            byte[] decbuff = Convert.FromBase64String(rawText);
            rawText = Encoding.UTF8.GetString(decbuff);

            string[] textFileRegions = rawText.Split('|');

            foreach (string s in textFileRegions)
            {
                string[] splitText = s.Split('~');

                foreach (GameObject x in ingameTexts)
                {
                    if (x.name == splitText[0].Trim())
                    {
                        x.GetComponent<Text>().text = Regex.Replace(splitText[1], Environment.NewLine, "");
                        break;
                    }
                }
            }
        }
    }

    public void ReadTextFileBB(string path)
    {
        if(levelMode == Level.Game)
        {
            string rawText = Regex.Replace(File.ReadAllText(@path, System.Text.Encoding.Default), Environment.NewLine, "");

            byte[] decbuff = Convert.FromBase64String(rawText);
            rawText = Encoding.UTF8.GetString(decbuff);

            string[] textFileRegions = rawText.Split('|');

            foreach (string s in textFileRegions)
            {
                string[] splitText = s.Split('~');

                switch (splitText[0].Replace(Environment.NewLine, ""))
                {
                    case "4.Wert_Verwendet":
                        if (Regex.Replace(splitText[1], Environment.NewLine, "").ToLower() == "wahr")
                        {                         
                            bb.graphPunkt4 = true;
                        }
                        else
                        {
                            bb.graphPunkt4 = false;
                        }
                        break;
                    case "5.Wert_Verwendet":
                        if (Regex.Replace(splitText[1], Environment.NewLine, "").ToLower() == "wahr")
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
                                bb.BB1_Zieltemp[z - 1] = (float)Convert.ToDouble(Regex.Replace(x, Environment.NewLine, ""));
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
                                bb.BB2_Zieltemp[zz - 1] = (float)Convert.ToDouble(Regex.Replace(x, Environment.NewLine, ""));
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
                                bb.BB3_Zieltemp[zzz - 1] = (float)Convert.ToDouble(Regex.Replace(x, Environment.NewLine, ""));
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
                                bb.BB4_Zieltemp[zzzz - 1] = (float)Convert.ToDouble(Regex.Replace(x, Environment.NewLine, ""));
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
                                bb.BB1_Zeit[o - 1] = Convert.ToInt32(Regex.Replace(x, Environment.NewLine, ""));
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
                                bb.BB2_Zeit[oo - 1] = Convert.ToInt32(Regex.Replace(x, Environment.NewLine, ""));
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
                                bb.BB3_Zeit[ooo - 1] = Convert.ToInt32(Regex.Replace(x, Environment.NewLine, ""));
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
                                bb.BB4_Zeit[oooo - 1] = Convert.ToInt32(Regex.Replace(x, Environment.NewLine, ""));
                            }
                            oooo++;
                        }
                        break;
                }
            }
               
        }
        else if(levelMode == Level.Menu)
        {

        }
    }

    #endregion

}
