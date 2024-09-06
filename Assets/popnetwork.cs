using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class popnetwork : NetworkBehaviour
{ 
    public GameObject popnetwork_obj;
    public TMPro.TextMeshProUGUI popnetwork_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void popupnetwork(string msg)
    {



        popnetwork_obj.GetComponent<shutme>().shutmebool = false;
        popnetwork_text.text = msg;
        popnetwork_obj.SetActive(true);
        
    }
}
