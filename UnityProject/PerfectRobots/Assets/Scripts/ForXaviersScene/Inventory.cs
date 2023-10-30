using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Item Equipped;
    public GameObject LazerBlasterGO;
    public GameObject AmmoGO;
    public int LazerAmmo;
    public int LazerAmmoMax;
    public Transform ItemPos;
    public Vector3 ItemPosToVec;
    public GameObject player;
    public Camera mainCamera;
    public GameObject currentItem;
    public List<Item> items = new List<Item>();


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
        }
        instance = this;
        player = GameObject.FindWithTag("Player");
    }


    public void Add(GameObject AddedObject, Item item)
    {
        Destroy(AddedObject);
        items.Add(item);
        if (Equipped == null)
        {
            Equipped = item;
            switch (item.itemInt)
            {
                case 0:
                    {
                        ItemPosToVec = ItemPos.position;
                        Quaternion rot = Quaternion.LookRotation(Camera.main.transform.up);
                        currentItem = Instantiate(LazerBlasterGO, ItemPosToVec, rot, Camera.main.transform);
                        break;
                    }
            }
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        Destroy(currentItem);
        switch (item.itemInt)
        {
            case 0:
                {
                    Vector3 AheadOfPlayer = player.transform.position;
                    Quaternion rot = Quaternion.LookRotation(transform.forward);
                    Instantiate(LazerBlasterGO, AheadOfPlayer, rot);
                    break;
                }
        }
        Equipped = null;
    }

    public void CollectAmmo(int ammo)
    {
        LazerAmmo = Mathf.Clamp(LazerAmmo += ammo, 0, LazerAmmoMax);
    }
}
