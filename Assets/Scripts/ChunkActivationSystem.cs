using UnityEngine;
using System.Collections.Generic;

public class ChunkActivationSystem : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float activationRadius = 30f;
    [SerializeField] float checkInterval = 0.8f;

    private readonly List<Transform> chunks = new();
    private float sqrRadius;
    private float timer;

    void Start()
    {
        sqrRadius = activationRadius * activationRadius;

        // ищем все объекты которые являются чанками
        GameObject[] foundchunks = GameObject.FindGameObjectsWithTag("Chunk");
        foreach (var chunk in foundchunks)
        {
            chunks.Add(chunk.transform);
            chunk.SetActive(false);
        }
    }
    

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        Vector3 playerPos = player.position;

        foreach (var chunk in chunks)
        {
            float sqrDist = (chunk.position - playerPos).sqrMagnitude;
            bool active = sqrDist < sqrRadius;
            

            if (chunk.gameObject.activeSelf != active)
                chunk.gameObject.SetActive(active);
        }
        
    }
}