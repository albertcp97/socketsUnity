using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Aquest update només per a qui li pertany
        if (!IsOwner) return;

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movement += Vector3.up;
        if (Input.GetKey(KeyCode.S))
            movement -= Vector3.up;
        if (Input.GetKey(KeyCode.A))
            movement -= Vector3.right;
        if (Input.GetKey(KeyCode.D))
            movement += Vector3.right;

        //i com podem veure, això no funciona al client perquè estem modificant la transform
        //que en principi només la pot tocar el servidor ja que tenim el component de compartir
        //la transform via network. I recordem que: EL CLIENT NO HA DE TENIR PERMÍS, "que fa trampes"
        transform.position += movement.normalized *1.5f * Time.deltaTime;
    }
}
