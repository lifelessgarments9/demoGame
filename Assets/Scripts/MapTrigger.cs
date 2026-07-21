using UnityEngine;
using System.Collections.Generic;

public class MapTrigger : MonoBehaviour
{
    [SerializeField] GameObject newObjects;
    
    private bool triggered=false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggered)return;
        if (other.gameObject.CompareTag("Player"))
        {
            triggered=true;
            newObjects.SetActive(true);
        }
    }
}
