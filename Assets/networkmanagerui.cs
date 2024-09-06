using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Unity.Netcode;
using Unity.VisualScripting;

public class networkmanagerui : MonoBehaviour
{
    
    [SerializeField] private string playerName;
    [SerializeField] private TMPro.TextMeshProUGUI playerNameInput;
    [SerializeField] private TMPro.TMP_InputField ipserver;
    [SerializeField] private TMPro.TMP_InputField port;
    [SerializeField] private GameObject background;
    [SerializeField] private Button host;
    [SerializeField] private Button client;
    [SerializeField] private Button server;
    
    [SerializeField] private GameObject Camera;
    // Start is called before the first frame update
    public void setgameobjectenable(GameObject g){
    g.SetActive(true); 
    }
    public void setgameobjectdisable(GameObject g){
        g.SetActive(false); 
    }
    void showall()
    {
        host.gameObject.SetActive(true);
        client.gameObject.SetActive(true);
        background.SetActive(true);
        playerNameInput.gameObject.SetActive(true);
        port.gameObject.SetActive(true);
        ipserver.gameObject.SetActive(true);
    }
    void hideallthem()
    {
        Camera.SetActive(false);
        host.gameObject.SetActive(false);
        client.gameObject.SetActive(false);
        server.gameObject.SetActive(false);
        playerNameInput.gameObject.SetActive(false);
        ipserver.gameObject.SetActive(false);
        port.gameObject.SetActive(false);
        background.SetActive(false);
    }

    public string getplayername()
    {
        return playerNameInput.text;
    }
    void Start()
    { 
        
        server.onClick.AddListener(() =>
        {
            
            NetworkManager.Singleton.StartServer();
            hideallthem();
        });
    
        client.onClick.AddListener(() =>
        {
            try
            {   ushort portnb;
                bool success = ushort.TryParse(port.text, out portnb);
                if (!success)
                {
                    portnb = 7777;
                }
                Debug.Log(portnb+" : "+ipserver.text);
                NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData(ipserver.text, portnb);
                NetworkManager.Singleton.StartClient();
                hideallthem();

            }
            catch
            {
                showall();
            }
            
        });
        
        host.onClick.AddListener(() =>
        {
            startHost();
        });
        
    }

    public void join()
    {
        ushort portnb;
        bool success = ushort.TryParse(port.text, out portnb);
        if (!success)
        {
            portnb = 7777;
        }
        Debug.Log(portnb+" : "+ipserver.text);
        NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>().SetConnectionData(ipserver.text, portnb);
        NetworkManager.Singleton.StartClient();
        hideallthem();
        
    }
    public void startHost()
    {
        NetworkManager.Singleton.StartHost();
        hideallthem();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
