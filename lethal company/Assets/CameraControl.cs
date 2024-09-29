using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public static CameraControl instance;
    public float speed;
    public Transform target;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
            if (target != null)
            {
                Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
            }
        
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}


