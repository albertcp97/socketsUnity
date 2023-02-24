using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class NetworkmanagerUI : MonoBehaviour
{
    [SerializeField] private Button serverB;
    [SerializeField] private Button hostB;
    [SerializeField] private Button clientB;
    [SerializeField] public TMPro.TMP_InputField input;
    [SerializeField] public TMPro.TMP_Text placeholder;
    void Awake()
    {

        serverB.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });

        clientB.onClick.AddListener(() =>
        {
            try
            {

                if (input.text.Length > 0)
                {
                    int a = int.Parse(input.text);
                    print(a);
                    NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = (ushort)int.Parse(input.text);
                    print(a);
                    NetworkManager.Singleton.StartClient();
                    NetworkManager.Singleton.SceneManager.LoadScene("lobby", UnityEngine.SceneManagement.LoadSceneMode.Single);

                }
            }
            catch (System.Exception e)
            {
                print(e.Message);
                placeholder.text = "Put an another port to conect";
                input.text = "";
            }
        });

        hostB.onClick.AddListener(() =>
        {
            try
            {

                if (input.text.Length > 0)
                {
                    int a = int.Parse(input.text);
                    print(a);
                    NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = (ushort)int.Parse(input.text);
                    print(a);
                    NetworkManager.Singleton.StartHost();
                    NetworkManager.Singleton.SceneManager.LoadScene("lobby",UnityEngine.SceneManagement.LoadSceneMode.Single);

                }
            }
            catch(System.Exception e)
            {print(e.Message);
                placeholder.text = "Put an another port to conect";
                input.text = "";
            }
        });
    }
}
