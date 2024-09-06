using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class camera_controller : NetworkBehaviour {
float mousx;
float mousy;
float Rotation=0f;
public float sens=350f;
public float maxang=90f;
public float minang=-90f;
public Transform player;
private NetworkVariable<Vector3> headRotation = new NetworkVariable<Vector3>(new Vector3(),NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
[SerializeField] private GameObject head;

void DestroyAllChildren(GameObject parentObject)
{
	// Loop through all children of the parent object
	foreach (Transform child in parentObject.transform)
	{
		// Destroy each child game object
		Destroy(child.gameObject);
	}
}
	void Start () {
		if (!IsOwner)
		{	interact_system i=GetComponent<interact_system>();
			i.enabled = false;
			nightvision4camera nv=GetComponent<nightvision4camera>();
			nv.enabled = false;
			DestroyAllChildren(gameObject);
			return;
		}
	Cursor.visible=false;
	Cursor.lockState = CursorLockMode.Locked;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (IsLocalPlayer)
		{
			// Update the head rotation on the local player
			Vector3 currentRotation = this.transform.eulerAngles;
			head.transform.eulerAngles = currentRotation;

			// Send the rotation to other players
			headRotation.Value = currentRotation;
		}
		
		
		if (!IsOwner)
		{
			head.transform.eulerAngles = headRotation.Value;
			return;
		}

		
	mousx =Input.GetAxis("Mouse X")*sens;
	mousy =Input.GetAxis("Mouse Y")*sens;
	Rotation -= mousy;
	Rotation= Mathf.Clamp(Rotation,minang,maxang);
	transform.localRotation=Quaternion.Euler(Rotation,0,0);
	player.Rotate(Vector3.up*mousx);
	
	
	
	
	
	
	
        
    
 
	
	
	/*
	if (Input.GetMouseButtonDown(0)){
	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	RaycastHit hit;
	if(Physics.Raycast (ray, out hit))
 {
      
      
          Debug.Log ("This is a Player");
     
      //else {
          Debug.Log ("This isn't a Player");                
      //}
 }}*/
	/*if(Input.GetKeyDown(KeyCode.LeftShift)){
		Camera.main.fieldOfView = 150f;
	}
	if((Input.GetKeyUp(KeyCode.LeftShift))){
		Camera.main.fieldOfView = FieldOfView;
	}*/
	}
}
