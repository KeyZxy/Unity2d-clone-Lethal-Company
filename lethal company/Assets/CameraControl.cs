using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Transform target;
    [SerializeField] Vector3 offset;
    public float smoothSpeed = 0.125f;//相机缓动速度
    float cameraHalfWidth, cameraHalfHeight;//存储相机视野范围的一半
    float topLimit, bottomLimit, leftLimit, rightLimit;//摄像机活动的范围
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        cameraHalfHeight = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Screen.width / Screen.height;

        refreshBoundary(GameObject.Find("CameraBoundary/1").GetComponent<BoxCollider2D>());
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        desiredPosition = new Vector3(
        Mathf.Clamp(desiredPosition.x, leftLimit, rightLimit),
        Mathf.Clamp(desiredPosition.y, bottomLimit, topLimit),
        desiredPosition.z

        );

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
    public void refreshBoundary(BoxCollider2D boundary)
    {
        leftLimit = boundary.transform.position.x - boundary.size.x / 2 + boundary.offset.x;
        rightLimit = boundary.transform.position.x + boundary.size.x / 2 + boundary.offset.x;
        bottomLimit = boundary.transform.position.y - boundary.size.y / 2 + boundary.offset.y;
        topLimit = boundary.transform.position.y + boundary.size.y / 2 + boundary.offset.y;
    }

}


