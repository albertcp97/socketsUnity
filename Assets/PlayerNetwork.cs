using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Diagnostics;
using System;
using Cinemachine;
using UnityEngine.SceneManagement;
public class PlayerNetwork : NetworkBehaviour
{
    private GameObject cam;
    private bool a = true;
    private bool b = true;
    private List<GameObject> list = new List<GameObject>();
    
    
    private NetworkVariable<int> velocity = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<int> turn = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    // Start is called before the first frame update
    private NetworkVariable<Color> color = new NetworkVariable<Color>(Color.blue, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] public GameObject project;
    [SerializeField] public Transform _camTransform;
    [SerializeField] public string name;
    [SerializeField] public ulong id;

    public Collider col = null;
    public Dictionary<string, ulong>  map= new Dictionary<string, ulong>();
    public List<ulong>  map2= new List<ulong>();
    public List<GameObject>  map3= new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        //print("add client " + map.Value.Count);
        if (IsOwner)
        {
            name = "Player" ;
            id = OwnerClientId;
            positionCameraClientRpc();


        }
        try
        {
            //map.Value.Add("Player" + (map.Value.Count + 1), OwnerClientId);
        }catch(Exception e)
        {
            print("error " + e);
        }
        velocity.OnValueChanged += (int previousValue, int newValue) =>
        {
            UnityEngine.Debug.Log(OwnerClientId + " a " + newValue);
        };
        color.OnValueChanged += (Color previousValue, Color newValue) =>
        {
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", newValue);
            UnityEngine.Debug.Log(OwnerClientId + " color " + newValue);
        };
        //https://www.youtube.com/watch?v=2rYjg5N4YZc&list=PLxmtWA2eKdQSf2EXE-tv0lmqmmdDzs0fV&index=12&ab_channel=CarlBoisvertDev
        CinemachineVirtualCamera cvm = _camTransform.gameObject.GetComponent<CinemachineVirtualCamera>();
        
        if (IsOwner)
        {
            if(!(SceneManager.GetActiveScene().name=="lobby"))
                cvm.Priority = 1;
            else
                cvm.Priority = 1;
        }
        else
        {
            cvm.Priority = 0;
        }
    }
    void Awake()
    {
        List<GameObject> o = new List<GameObject>() ;
        foreach (GameObject gameObj in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (gameObj.name.Contains("Player"))
            {
                //Do somet$$anonymous$$ng...
                o.Add(gameObj);
                print(gameObj);
            }
        }

    }
    void Start()
    {
        cam = new GameObject();
        
        // cam.AddComponent<Camera>();
        //cam.transform.SetParent(gameObject.transform);
        //gameObject.transform.position = new Vector3(0, 0, 0);
        //cam.transform.position = new Vector3(0, 0, 0);
        //positionCameraClientRpc();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Palanca")
        {
            col = other;
            b = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Palanca")
        {
            col = null;
            b = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        print("PlayerNetwork " + map2[turn.Value]);
        //print("PlayerNetwork " + map[turn.Value]);
        //Aquest update només per a qui li pertany

        
        if (!IsOwner) return;
        if (map2[turn.Value] != OwnerClientId)
            return;
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movement += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            movement += Vector3.back;
        if (Input.GetKey(KeyCode.A))
            movement -= Vector3.right;
        if (Input.GetKey(KeyCode.D))
            movement += Vector3.right;

        if (Input.GetKey(KeyCode.T))
            velocity.Value += 5;

        if (Input.GetKey(KeyCode.C))
            color.Value = Color.red;

        if (Input.GetKey(KeyCode.F)&&b)
        {
                col.gameObject.GetComponent<pistonController>().changeColor();
                changeTurnClientRpc();
        }

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

    [ClientRpc]
    private void positionCameraClientRpc()
    {
        map.Add(name, id);
        map2.Add(id);
        map3.Add(gameObject);
    }
    [ClientRpc]
    private void changeTurnClientRpc()
    {
            //holiServerRpc();

        if (turn.Value == map.Count)
            turn.Value=0;
        else
            turn.Value += 1;


    }
    [ServerRpc]
    private void holiServerRpc()
    {
        //print("Vamo a matarlo"); changeTurn2ClientRpc();
    }
    [ClientRpc]
    private void changeTurn2ClientRpc()
    {
        map3[turn.Value].SetActive(false);       


    }
}
