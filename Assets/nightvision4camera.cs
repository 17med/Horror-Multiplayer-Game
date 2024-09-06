using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

using UnityEngine.UI;
public class nightvision4camera : MonoBehaviour
{float FieldOfView=60;
    private float scroll;
    [SerializeField]
    private NightVision n;
    [SerializeField]
    private CinemachineVirtualCamera camx;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject cam;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {scroll = Input.mouseScrollDelta.y;
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
    }
}
