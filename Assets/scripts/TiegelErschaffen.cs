using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiegelErschaffen : Collectible
{
    GameObject tempGO;
    public GameObject[] tiegelObj;
    public Slot slot1, slot2, slot3, slot4;

    private void Start()
    {
        Collect();
    }
    public override void Collect()
    {
        for (int i = 0; i < tiegelObj.Length; i++)
        {
            int rnd = Random.Range(0, tiegelObj.Length);
            tempGO = tiegelObj[rnd];
            tiegelObj[rnd] = tiegelObj[i];
            tiegelObj[i] = tempGO;
        }
        for (int i = 0; i < tiegelObj.Length; i++)
        {
            if (slot1.transform.childCount == 0)
            {
                GameObject temp = Instantiate(tiegelObj[i], new Vector3(0, 0, 0), Quaternion.identity);
                temp.transform.SetParent(slot1.transform);
            }
            else if (slot2.transform.childCount == 0)
            {
                GameObject temp = Instantiate(tiegelObj[i], new Vector3(0, 0, 0), Quaternion.identity);
                temp.transform.SetParent(slot2.transform);
            }
            else if (slot3.transform.childCount == 0)
            {
                GameObject temp = Instantiate(tiegelObj[i], new Vector3(0, 0, 0), Quaternion.identity);
                temp.transform.SetParent(slot3.transform);
            }
            else if (slot4.transform.childCount == 0)
            {
                GameObject temp = Instantiate(tiegelObj[i], new Vector3(0, 0, 0), Quaternion.identity);
                temp.transform.SetParent(slot4.transform);
            }
        }
    }
}
