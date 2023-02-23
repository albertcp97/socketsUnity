using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraController : MonoBehaviour
{
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] cm = GameObject.FindGameObjectsWithTag("Camera");
        foreach (GameObject o in cm)
        {
            Debug.Log(o+" "+ gameObject.transform.GetChild(1).gameObject);
            if (o == gameObject.transform.GetChild(1).gameObject)
                Debug.Log("Hola " + o);
            o.SetActive(false); 
        }
        cam.SetActive(true);
        //gameObject.transform.GetChild(1).gameObject.SetActive(true);
       


    }

    
}
