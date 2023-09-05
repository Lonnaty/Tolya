using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateShluxa : MonoBehaviour
{
    public GameObject objectForActivate;
    public bool isActivated = false;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated)
        {
            if (objectForActivate.name == "chlen")
            {
                GameObject.Find("ShluxaDver").transform.Find("DoorZone").gameObject.GetComponent<DoorController>().canOpenDver = true;
                GameObject.Find("Player").GetComponent<AudioSource>().Stop();
                PlayerPrefs.SetInt("XuyIsActivated", 1);
                PlayerPrefs.Save();
            }
            isActivated = true;
            objectForActivate.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
