using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public void Level1()
    {
        SceneManager.LoadScene("level1", LoadSceneMode.Single);
    }

    public void SpielBeenden()
    {
        Application.Quit();
    }
}
