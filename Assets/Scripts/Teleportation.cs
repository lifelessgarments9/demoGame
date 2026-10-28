using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] CinemachineVirtualCameraBase cam;
    [SerializeField] private GameObject[] enableChunks;
    [SerializeField] private GameObject[] disableChunks;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Teleport(other.gameObject.GetComponent<Rigidbody2D>()));
        }
    }

    IEnumerator Teleport(Rigidbody2D rb)
    {
        Vector3 local = transform.InverseTransformPoint(rb.position); //local - x,y
        Vector3 targetLocal = new Vector3(local.x, local.y, 0f);
        Vector3 targetPos = target.transform.TransformPoint(targetLocal);
        
        rb.position = targetPos;
        rb.linearVelocity = Vector2.zero;
        
        cam.enabled = false;
        yield return null;
        cam.enabled = true;
    }
}
