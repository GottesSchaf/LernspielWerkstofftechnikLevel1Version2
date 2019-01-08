using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlock : Collectible
{
    public GameObject itemToCreate;
    public Slot Slot1;
    public Slot Slot2;
    public Slot Slot3;
    public Slot Slot4;
    public Slot Slot5;
    public Slot Slot6;

    public override void Collect()
    {
        //check every item slot if it is empty
        if(Slot1.transform.childCount == 0)
        {
            //create item in inventory
            GameObject temp = Instantiate(itemToCreate, new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(gameObject);
            temp.transform.SetParent(Slot1.transform);
        }
        else if (Slot2.transform.childCount == 0)
        {
            //create item in inventory
            GameObject temp = Instantiate(itemToCreate, new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(gameObject);
            temp.transform.SetParent(Slot2.transform);
        }
        else if (Slot3.transform.childCount == 0)
        {
            //create item in inventory
            GameObject temp = Instantiate(itemToCreate, new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(gameObject);
            temp.transform.SetParent(Slot3.transform);
        }
        else if (Slot4.transform.childCount == 0)
        {
            //create item in inventory
            GameObject temp = Instantiate(itemToCreate, new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(gameObject);
            temp.transform.SetParent(Slot4.transform);
        }
        else if (Slot5.transform.childCount == 0)
        {
            //create item in inventory
            GameObject temp = Instantiate(itemToCreate, new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(gameObject);
            temp.transform.SetParent(Slot5.transform);
        }
        else if (Slot6.transform.childCount == 0)
        {
            //create item in inventory
            GameObject temp = Instantiate(itemToCreate, new Vector3(0, 0, 0), Quaternion.identity);
            Destroy(gameObject);
            temp.transform.SetParent(Slot6.transform);
        }
        

        
    }
}