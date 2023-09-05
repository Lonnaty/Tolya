using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDanceFloor : MonoBehaviour
{
    [SerializeField] GameObject[] danceFloors;
    bool activated = false;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            activated = true;
            foreach (var floor in danceFloors)
            {
                floor.GetComponent<SetTransparency>().isFadingUp = true;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
