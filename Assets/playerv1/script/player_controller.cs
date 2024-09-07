using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class player_controller : NetworkBehaviour {
public float speed=5f;
public int multiplspeed = 2;
[SerializeField]
private GameObject light1;
[SerializeField]
private GameObject light2;
[SerializeField]
private GameObject global;


public float gravity=20f;
public float jump=5f;

public float gamespeed=1f;



public TMPro.TextMeshProUGUI text;
CharacterController controller;
private Transform postionspawn;
public  Vector3 movedir=Vector3.zero;
[SerializeField]
private Animator animator;
[SerializeField]
private List<SkinnedMeshRenderer> skinnedMeshRenderers;
[SerializeField]
private List<SkinnedMeshRenderer> skinnedMeshRenderers2;
[SerializeField] 
private Transform player;
private Vector3 target;
Vector3 pp=Vector3.zero;
bool crouching=false;
[SerializeField] private float crouchSpeed=1;
[SerializeField] private float crouchHeight=0.7f;
[SerializeField] private float normalHeight=2f;
[SerializeField] private Vector3 offset=new Vector3(0f,1f,0f);
[SerializeField]
private CinemachineVirtualCamera camx;
public NetworkVariable<FixedString64Bytes> Playername = new NetworkVariable<FixedString64Bytes>("",NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
private string myname="";
public GameObject deathscene;
[SerializeField]
private GameObject cubecam;
[SerializeField]
popnetwork popnetwork;

public AudioSource footstepAudioSource;  // AudioSource to play the footstep sounds
public AudioClip[] footstepClips;        // Array of footstep sound clips
public AudioClip jumpclip;        // Array of footstep sound clips
public float stepInterval = 0.5f;        // Time interval between steps
public float stepIntervalRuning = 0.5f;        // Time interval between steps
private float stepTimer = 0f;
private Vaulting vaulting;
public void DisableSkinnedMeshRenderersInList()
{
	foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
	{
		if (skinnedMeshRenderer != null)
		{
			skinnedMeshRenderer.enabled = false;
		}
	}
	
}
public void SetFrequencyGain(float newFrequencyGain)
{
	if (camx != null)
	{
		CinemachineBasicMultiChannelPerlin noise = camx.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		if (noise != null)
		{
			noise.m_FrequencyGain= newFrequencyGain;
		}
	}
}
public void DisableSkinnedMeshRenderersInList2(bool b)
{
	foreach (var skinnedMeshRenderer in skinnedMeshRenderers2)
	{
		if (skinnedMeshRenderer != null)
		{
			skinnedMeshRenderer.enabled = b;
		}
	}
}

public override void OnNetworkSpawn()
{
	
	base.OnNetworkSpawn();                        
	GameObject networkUi =  GameObject.Find("networkui");
	if(networkUi==null)
	{
		return;
	}
	else
	{
		Debug.Log("oh no");
	}
	Debug.Log("find it");
	networkmanagerui n=networkUi.GetComponent<networkmanagerui>();
	gameObject.name = n.getplayername();
	showmeServerRpc(gameObject.name);
}
	void Start () {
		transform.position=new Vector3(3.672f,1.646f,0.084f);
		vaulting=gameObject.GetComponent<Vaulting>();
		 if (!IsOwner)
                        {
	                        Destroy(light1);
	                        Destroy(light2);
	                        Destroy(global);
	                        
                            return;
                        }

                        if (IsOwner)
                        {
	                        target=player.localPosition;
	                        controller= GetComponent<CharacterController>();
	                        DisableSkinnedMeshRenderersInList();
	                        
	                        GameObject networkUi =  GameObject.Find("networkui");
	                        if(networkUi==null)
	                        {
		                        return;
	                        }
	                        else
	                        {
		                        Debug.Log("oh no");
	                        }
	                        Debug.Log("find it");
	                        networkmanagerui n=networkUi.GetComponent<networkmanagerui>();
	                        gameObject.name = n.getplayername();
	                        myname = n.getplayername();
	                        Playername.Value=new FixedString64Bytes(myname);
	                        


                        }
	
	
	
	
	
	}
	

	public void death()
	{
		if (IsOwner)
		{
			cubecam.SetActive(false);
			animator.SetBool("death",true);
			Instantiate(deathscene,new Vector3(-1000,-1000,-1000),quaternion.identity);
			
			GetComponent<player_controller>().enabled = false;
			
		}
		
	}	
	void Update () {
		if (IsLocalPlayer)
		{
			if (animator.GetInteger("state") == 1 || animator.GetInteger("state") == 2)
			{
			
		
				if (animator.GetBool("running")  )
				{
					PlayFootstepSounds(stepIntervalRuning);
				}
				else
				{
					PlayFootstepSounds(stepInterval);
				}}
		}
        if (!IsOwner)
                        {
                            return;
                        }

                        if (controller.height == normalHeight && vaulting.getstate()==false)
                        {
	                        DisableSkinnedMeshRenderersInList2(true);
                        }
                        else
                        {
	                        DisableSkinnedMeshRenderersInList2(false);
                        }
                        if (Input.GetKeyDown(KeyCode.LeftControl))
                        {
	                        crouching = true;
                        }

                        if (Input.GetKeyUp(KeyCode.LeftControl))
                        {
	                        crouching = false;
                        }
                        if(crouching == true)
                        {

	                        controller.height = crouchHeight;
	                        player.localPosition = new Vector3(player.localPosition.x,-0.5f,player.localPosition.z);
                        }
                        if (crouching == false)
                        {
	                        player.localPosition = target;
	                        controller.height = normalHeight;
                        }
        
        
        
        
        
        
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
		
	if(Input.GetKeyDown(KeyCode.LeftShift)){
		animator.SetBool("running",true);
		SetFrequencyGain(3);
		speed=speed*multiplspeed;
		Debug.Log("speedup");
	}
	if((Input.GetKeyUp(KeyCode.LeftShift))){
		animator.SetBool("running",false);
		SetFrequencyGain(1);
		speed=speed/multiplspeed;
		Debug.Log("speedlow");
	}	
	
	
		
		
		
	if (controller.isGrounded)
	{
		float thespeedgenral = 1;
		if (crouching)
		{
			thespeedgenral = crouchSpeed;
		}
	movedir	=new Vector3(Input.GetAxis("Horizontal")*speed*gamespeed*thespeedgenral,0f,Input.GetAxis("Vertical")*speed*gamespeed*thespeedgenral);
	if (movedir.x!=0  || movedir.y!=0 || movedir.z!=0)
	{
		if (crouching)
		{	
			animator.SetInteger("state",2);
			
		}
		else
		{	
			animator.SetInteger("state",1);
		}

		
		
		
	}
	else
	{	
		if (crouching)
		{
			
			
			animator.SetInteger("state",3);
			
		}
		else
		{	
			animator.SetInteger("state",0);
		}
		
	}
	movedir=transform.TransformDirection(movedir);
	if(Input.GetButton("Jump"))
	{

		footstepAudioSource.clip = jumpclip;
		footstepAudioSource.Play();
		
		//gun.localScale=new Vector3(0.003f,0.003f,0.003f);
		movedir.y=jump;
	}
	
	/*
	if((Input.GetKeyUp(KeyCode.LeftControl))){
		
		speedv=(speedv2)-(speedv2/3);
		
		transform.localScale=new Vector3(2,2,2);
		gun.localScale=new Vector3(0.003f,0.003f,0.003f);
	}
	if((Input.GetKeyDown(KeyCode.LeftControl))){
		xcvdown=xcdown;
		speedv=speedv2;
		
		transform.localScale=new Vector3(2,1,2);
		gun.localScale=new Vector3(0.003f,0.006f,0.003f);
	}*/
	
	
	
	
	}
	/*
	if(Input.GetButton("Fire1")){
		gravity=0f;
	}
	if(Input.GetButton("Fire1")==false){
		gravity=20f;
	}
	if((Input.GetKeyDown(KeyCode.X))){
		gravity=0f;
		
		
	}
	if((Input.GetKeyUp(KeyCode.LeftControl))){
		
		speedv=(speedv2)-(speedv2/3);
		
		transform.localScale=new Vector3(1,1,1);
		//gun.localScale=new Vector3(0.003f,0.003f,0.003f);
	}
	if((Input.GetKeyDown(KeyCode.LeftControl))){
		xcvdown=xcdown;
		speedv=speedv2;
		
		transform.localScale=new Vector3(1f,0.5f,1f);
		//gun.localScale=new Vector3(0.003f,0.006f,0.003f);
		 }*/
	movedir.y-=gravity*Time.deltaTime;
	controller.Move(movedir*Time.deltaTime);
	}
	void OnTriggerEnter(Collider other){
		}
	/*void OnTriggerEnter(Collider other){
		if (other.gameObject.tag=="wall"){
		gravity=0f;
		bx=1;	
		}
	
	}
	void OnTriggerExit(Collider other){
		gravity=20f;
		bx=0;
	}*/
	void PlayFootstepSounds(float step)
	{
		stepTimer += Time.deltaTime;

		if (stepTimer >= step && controller.isGrounded)
		{
			stepTimer = 0f;
			if (footstepClips.Length > 0)
			{
				int clipIndex = Random.Range(0, footstepClips.Length);
				footstepAudioSource.clip = footstepClips[clipIndex];
				footstepAudioSource.Play();
			}
		}
	}
	
	[ServerRpc(RequireOwnership = false)]
	public void showmeServerRpc(FixedString64Bytes msg)
	{	
		showmeClientRpc(msg);
	}
	
	[ClientRpc]
	public void showmeClientRpc(FixedString64Bytes msg)
	{	
		
		if(popnetwork!=null){
		Debug.Log(msg+" ! "+gameObject.name+" "+Playername.Value.ToString());	
				
		popnetwork.popupnetwork(msg+" Join The Game");}
		
		
	}
}
