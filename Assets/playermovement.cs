using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class playermovement : NetworkBehaviour
{
    public TMPro.TextMeshProUGUI text;
    private NetworkVariable<int> playerName= new NetworkVariable<int>(0);
    void Start()
    { 
        GameObject networkUi = GameObject.Find("networkui");
        if(networkUi!=null)
        {
            return;
        }
        networkmanagerui n=networkUi.GetComponent<networkmanagerui>();
        if (n != null && IsOwner)
        {
            text.SetText(n.getplayername());
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if (!IsOwner)
        {
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); 
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 mousePos = ray.GetPoint(rayDistance);
            Vector3 lookDirection = mousePos - transform.position;
            lookDirection.y = 0;

            if (lookDirection != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
            }
        }

       
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float moveSpeed = 3f;

        
        Vector3 worldMovement = transform.right * movement.x + transform.forward * movement.z;
        worldMovement.y = 0; 

       
        transform.position += worldMovement * Time.deltaTime * moveSpeed;

    }
}
