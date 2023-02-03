using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Diagnostics;
public class BolaController : NetworkBehaviour
{ private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        gameObject.transform.localPosition = new Vector3(1,3,0);


        rb.velocity = rb.transform.forward * 1;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.transform.forward * 1;
        UnityEngine.Debug.Log("position " + gameObject.transform.localPosition);

        UnityEngine.Debug.Log("position " + gameObject.transform.parent.gameObject.transform.localPosition);
    }
}
