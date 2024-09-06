using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LampOnOFF : NetworkBehaviour
{
    public NetworkVariable<bool> isON=new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    public GameObject[] Lamps;
    public Renderer[] Renderers;
    public Material[] Materials;
    // Start is called before the first frame update
    void Start()
    {
        isON.OnValueChanged += OnMyIntValueChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Activate();
        
        
    }
    void Activate()
    {
        foreach (var Lamp in Lamps)
        {
            Lamp.SetActive(isON.Value);
            
        }

        foreach (var Renderer in Renderers)
        {
            if (isON.Value)
            {
                Renderer.material = Materials[0];
            }
            else
            {
                Renderer.material = Materials[1];
            }
        }
        
    }
    private void OnMyIntValueChanged(bool oldValue, bool newValue)
    {
        Activate();
        Debug.Log($"Value changed from {oldValue} to {newValue}");
    }
    public void ChangeState()
    {
        
       
           Debug.Log("Change state");
           RequestChangeStateServerRpc();
        
    }
    [ServerRpc(RequireOwnership = false)]
    private void RequestChangeStateServerRpc(ServerRpcParams rpcParams = default)
    {
        // Toggle the NetworkVariable value
        isON.Value = !isON.Value;
    }
}
