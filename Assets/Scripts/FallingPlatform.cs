using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float fallDelay = 1f;
    [SerializeField] float activeDelay = 1f;
    private Rigidbody rb;
    private Material material;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "Player")
        {
            StartCoroutine(Fall());
        }
    }

    private void Active()
    {
        gameObject.SetActive(true);
    }

    public IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        gameObject.SetActive(false);
        Invoke(nameof(Active), activeDelay);
    }
}
