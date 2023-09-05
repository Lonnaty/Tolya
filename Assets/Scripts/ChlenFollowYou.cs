using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChlenFollowYou : MonoBehaviour
{
    Transform target; //the enemy's target
    float moveSpeed = 3; //move speed
    float rotationSpeed = 3; //speed of turning
    float range = 1000f; //Range within target will be detected
    float stop = 0;
    Transform myTransform;
    private void Start()
    {
        target = GameObject.Find("Player").transform; //target the player
        myTransform = transform; //cache transform data for easy access/preformance
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            
            //rotate to look at the player
            var distance = Vector3.Distance(myTransform.position, target.position);
            if (distance <= range)
            {
                //look
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
                //move
                if (distance > stop)
                {
                    myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
                }
            }
        }
    }
}
