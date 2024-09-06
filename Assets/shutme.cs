using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shutme : MonoBehaviour
{
    public bool shutmebool = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {if(shutmebool)
        {gameObject.SetActive(false);}
        
    }
}
