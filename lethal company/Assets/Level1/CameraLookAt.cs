using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{

    public Transform target;  // Ҫ�����Ŀ��  
    public Vector3 offset;    // �����Ŀ��֮���ƫ��  
    public float smoothSpeed = 0.125f; // ƽ��������ٶ�  

    private void LateUpdate()
    {
        if (target != null)
        {
            // ����Ŀ��λ�ü���ƫ��  
            Vector3 desiredPosition = target.position + offset;

            // ƽ���ƶ������Ŀ��λ��  
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // ȷ�����û����ת��������2D��ͼ��  
            transform.rotation = Quaternion.identity;
        }
    }
}
