using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Diagnostics;
using System;

public class PlayerNetwork : NetworkBehaviour
{
    private bool a = true;
    private List<GameObject> list = new List<GameObject>();
    private NetworkVariable<int> velocity = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    private NetworkVariable<Color> color = new NetworkVariable<Color>(Color.blue, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] public GameObject project;
    public override void OnNetworkSpawn()
    {
        velocity.OnValueChanged += (int previousValue, int newValue) =>
        {
            UnityEngine.Debug.Log(OwnerClientId + " a " + newValue);
        };
        color.OnValueChanged += (Color previousValue, Color newValue) =>
        {
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", newValue);
            UnityEngine.Debug.Log(OwnerClientId + " color " + newValue);
        };
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

        if (Input.GetKey(KeyCode.T))
            velocity.Value += 5;

        if (Input.GetKey(KeyCode.C))
            color.Value = Color.red;


        if (Input.GetKey(KeyCode.LeftShift)&&a)
        {
            ShootServerRpc();
            a=false;
        }

        //i com podem veure, això no funciona al client perquè estem modificant la transform
        //que en principi només la pot tocar el servidor ja que tenim el component de compartir
        //la transform via network. I recordem que: EL CLIENT NO HA DE TENIR PERMÍS, "que fa trampes"
        transform.position += movement.normalized * velocity.Value * Time.deltaTime;
    }
    void Call()
    {
        UnityEngine.Debug.Log(OwnerClientId + " a " + velocity.Value);

    }
    [ServerRpc]
    private void ShootServerRpc()
    {   

        GameObject bullet = Instantiate(project, transform.position, Quaternion.identity) as GameObject;
        list.Add(bullet);
        bullet.GetComponent<NetworkObject>().Spawn();

        bullet.transform.SetParent(this.gameObject.transform);
    }
}
