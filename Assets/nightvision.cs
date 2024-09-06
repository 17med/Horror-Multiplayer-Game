using System;
using Unity.Netcode;
using UnityEngine;

public class NightVision : NetworkBehaviour
{
    public GameObject sun;
    public GameObject things;
    public GameObject innight;
    public GameObject glass;
    
    // A NetworkVariable that allows the owner to change the value, and everyone else to read it
    private NetworkVariable<bool> isGlassOpen = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private bool isActive = false;

    public bool getstate()
    {
        return isGlassOpen.Value;
    }
    void Start()
    {
        glass.SetActive(isGlassOpen.Value);
        if (IsOwner)
        {
            innight.SetActive(true);   
            sun.SetActive(isGlassOpen.Value);
            things.SetActive(isGlassOpen.Value);
            innight.SetActive(!isGlassOpen.Value);
        }
        
        // Subscribe to the value change event so that glasses' state can be updated on all clients
        isGlassOpen.OnValueChanged += OnGlassOpenChanged;
    }

    void Update()
    {
        if (IsLocalPlayer && Input.GetKeyDown(KeyCode.F))
        {
            // Toggle the glass state (only allowed by the owner)
            isGlassOpen.Value = !isGlassOpen.Value;
        }
    }

    private void OnGlassOpenChanged(bool oldValue, bool newValue)
    {
        // Update the glass visibility based on the new value
        glass.SetActive(newValue);

        if (IsOwner)
        {
            isActive = newValue;
            sun.SetActive(isActive);
            things.SetActive(isActive);
            innight.SetActive(!isActive);  
            glass.SetActive(false);
        }
    }

    // This method is called on the client when they connect to the server
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        // Ensure the new player sees the correct state
        glass.SetActive(isGlassOpen.Value);
        if (IsOwner)
        {
            sun.SetActive(isGlassOpen.Value);
            things.SetActive(isGlassOpen.Value);
            innight.SetActive(!isGlassOpen.Value);
        }
    }

    // Unsubscribe from the event when the object is destroyed
    private void OnDestroy()
    {
        isGlassOpen.OnValueChanged -= OnGlassOpenChanged;
    }
}
