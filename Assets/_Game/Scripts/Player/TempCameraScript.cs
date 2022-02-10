using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraScript : MonoBehaviour
{
    GameObject followTarget;
    [SerializeField] float cameraHeight = 6f;
    [SerializeField] float cameraDistance = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        followTarget = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(followTarget == null)
        {
            followTarget = GameObject.FindGameObjectWithTag("Player");
        }
        
        transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y + cameraHeight,  followTarget.transform.position.z - cameraDistance);
    }
}
