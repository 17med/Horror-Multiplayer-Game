using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
public class Vaulting : NetworkBehaviour
{
    public float vaultHeight = 1.5f;       // Maximum height of vaultable obstacles
    public float vaultDistance = 2.0f;     // Maximum distance to vaultable obstacles
    public float vaultSpeed = 3.0f;        // Speed at which player vaults
    public LayerMask obstacleLayer;        // Layer for vaultable obstacles

    private bool isVaulting = false;       // Is the player currently vaulting?
    private Vector3 vaultTarget;           // The target position to vault to
    private CharacterController characterController;
    public Animator animatorx;
    player_controller player;
    public int state;
    void Start()
    {   
        player=GetComponent<player_controller>();
        characterController = GetComponent<CharacterController>();  // Reference to Character Controller
    }

    public bool getstate()
    {
        return isVaulting;
    }
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        state=animatorx.GetInteger("state");
        if (isVaulting)
        {
            PerformVault();
        }
        else
        {
            DetectVaultableObstacle();
        }
    }

    void DetectVaultableObstacle()
    {
        RaycastHit hit;

        // Cast a ray forward to detect vaultable objects
        if (Physics.Raycast(transform.position, transform.forward, out hit, vaultDistance, obstacleLayer))
        {
            float objectHeight = hit.collider.bounds.size.y;

            // Check if the obstacle is low enough to vault over
            if (objectHeight <= vaultHeight && Input.GetKeyDown(KeyCode.Space))
            {
                // Set the target vault position slightly above the obstacle
                vaultTarget = hit.point + Vector3.up * objectHeight;

                // Begin vaulting
                isVaulting = true;

                // Optionally disable player movement/controls during vault
                player.DisableSkinnedMeshRenderersInList2(false);
                characterController.enabled = false;
                //player.enabled = false;
                animatorx.SetInteger("state",5);
                animatorx.SetTrigger("playtop");
                
            }
        }
    }

    void PerformVault()
    {
        
        transform.position = Vector3.Lerp(transform.position, vaultTarget, Time.deltaTime * vaultSpeed);

        // Check if the player has reached the target vault position
        if (Vector3.Distance(transform.position, vaultTarget) < 0.1f)
        {   
            animatorx.SetInteger("state",0);
            isVaulting = false;
            characterController.enabled = true;
            player.DisableSkinnedMeshRenderersInList2(true);
            // player.enabled = true;// Re-enable player movement after vaulting
        }
    }
}
