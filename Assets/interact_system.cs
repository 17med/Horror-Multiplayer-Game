using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class interact_system : MonoBehaviour
{   [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    Camera cam;

    [SerializeField] private Image img;
    
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite interact;
    [SerializeField] private TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { Ray ray =cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, 1f, layerMask))
        {
            
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.gameObject.tag == "Button")
            {
                img.sprite = interact;
                Text.text = hitObject.gameObject.name;
                if (hitObject.GetComponent<LampOnOFF>() != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hitObject.GetComponent<LampOnOFF>().ChangeState();
                    }
                    
                }
            }
            else
            {
                img.sprite = normal;
            }
        }
        else
        {
            Text.text = "";
            img.sprite = normal;
        }
        
    }
}
