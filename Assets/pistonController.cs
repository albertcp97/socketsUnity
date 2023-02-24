using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class pistonController : NetworkBehaviour
{   
    private NetworkVariable<Color> color = new NetworkVariable<Color>(Color.yellow, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> activated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        color.OnValueChanged += (Color previousValue, Color newValue) =>
        {
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", newValue);
            UnityEngine.Debug.Log(OwnerClientId + " color " + newValue);
            print(activated.Value);
           
        };
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeColor()
    {
        
        if (!activated.Value==true)
        {
            color.Value = Color.red;
            activated.Value = true;
        }

    }
}
