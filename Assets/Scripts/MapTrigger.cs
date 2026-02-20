using UnityEngine;
using System.Collections.Generic;

public class MapTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> oldObjects;
    [SerializeField] List<GameObject> newObjects;
    
    private bool triggered=false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggered)return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            triggered=true;
            ReplaceObjects();
        }
    }

    void ReplaceObjects()
    {
        if (oldObjects.Count != newObjects.Count)
        {
            Debug.Log("old:"+oldObjects.Count+", new:"+newObjects.Count);
        }
        for (int i = 0; i < oldObjects.Count; i++)
        {
            oldObjects[i].SetActive(false);
            newObjects[i].SetActive(true);
        }
    }

}
