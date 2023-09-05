using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : MonoBehaviour
{
    [SerializeField] GameObject point;
    [SerializeField] float speed = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, point.transform.position) > 0.1f)
            transform.position = Vector3.MoveTowards(transform.position, point.transform.position, speed * Time.deltaTime);
    }
}
