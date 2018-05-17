using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{

    public float gravity = -10;

    public void Attract(Transform body)
    {

        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        Rigidbody rb = body.GetComponent<Rigidbody>();
        float dist = Vector3.Distance(transform.position, body.position);
        rb.AddForce((gravityUp * gravity) / (Mathf.Pow(dist, 2)));
    }
}
