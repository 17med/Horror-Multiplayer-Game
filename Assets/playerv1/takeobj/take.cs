using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class take : MonoBehaviour
{Rigidbody rb;
public Transform bbx;
private bool hz=false;
    // Start is called before the first frame update
    void Start()
    {rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {if(hz){
        if (Input.GetKey(KeyCode.Mouse1))
        {   gameObject.transform.SetParent(bbx, false);
            gameObject.transform.SetParent(null);
            gameObject.transform.localScale=new Vector3(1f,1f,1f);
            rb.constraints = RigidbodyConstraints.None;
            
            

        }
    }    
    }
    void OnMouseDown(){
        rb.constraints = RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ |RigidbodyConstraints.FreezePositionX |RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    transform.position=bbx.position;
    gameObject.transform.localScale=new Vector3(0.1f,0.1f,0.1f);
    gameObject.transform.SetParent(bbx, true);
    hz=true;
    
    
    }
    
}
