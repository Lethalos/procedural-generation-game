using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LookAtCamera : MonoBehaviour
{
    
    private void LateUpdate()
    {
       
        transform.LookAt(Camera.main.transform.position, Vector3.forward);
        
    }

}
