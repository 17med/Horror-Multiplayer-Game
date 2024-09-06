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
float FieldOfView=60;
private float scroll;
[SerializeField]
private NightVision n;
[SerializeField]
private CinemachineVirtualCamera camx;
[SerializeField] private Slider slider;
[SerializeField] private GameObject cam;
[SerializeField] private GameObject head;


	void Start () {
		if (!IsOwner)
		{	interact_system i=GetComponent<interact_system>();
			i.enabled = false;
			Destroy(gameObject);
			return;
		}
	Cursor.visible=false;
	Cursor.lockState = CursorLockMode.Locked;
		
	}
	
	// Update is called once per frame
	void Update () {
		head.transform.eulerAngles=transform.eulerAngles;
		if (!IsOwner)
		{
			return;
		}

		
	mousx =Input.GetAxis("Mouse X")*sens;
	mousy =Input.GetAxis("Mouse Y")*sens;
	Rotation -= mousy;
	Rotation= Mathf.Clamp(Rotation,minang,maxang);
	transform.localRotation=Quaternion.Euler(Rotation,0,0);
	player.Rotate(Vector3.up*mousx);
	scroll = Input.mouseScrollDelta.y;
	
	
	if (n.getstate())
	{
		float v=camx.m_Lens.FieldOfView-scroll*5;
		if (v >= 20 && v <= 60)
		{
			
			camx.m_Lens.FieldOfView=v;
		}
		if (v > 60)
		{
			
			camx.m_Lens.FieldOfView = 60f;
		}

		if (v < 20)
		{	
			camx.m_Lens.FieldOfView = 20f;
		}
		
		slider.value = (1-(camx.m_Lens.FieldOfView/60))*2;
		
	}
	else
	{
		camx.m_Lens.FieldOfView = 60;
	}
	
	
	
        
    
 
	
	
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
