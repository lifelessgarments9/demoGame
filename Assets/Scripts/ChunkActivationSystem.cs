using UnityEngine;
using System.Collections.Generic;

public class ChunkActivationSystem : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float activationRadius = 65f;
    [SerializeField] float checkInterval = 5f;

    class ChunkData
    {
        public Transform transform;
        public SpriteRenderer[] renderers;
        public bool visible;
    }

    private readonly List<ChunkData> chunks = new();

    private float sqrRadius;
    private float timer;

    void Start()
    {
        sqrRadius = activationRadius * activationRadius;

        foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunk"))
        {
            ChunkData data = new ChunkData();

            data.transform = chunk.transform;
            data.renderers = chunk.GetComponentsInChildren<SpriteRenderer>(true);
            data.visible = false;
            foreach (var r in data.renderers) r.enabled = false;
            chunks.Add(data);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0;
        Vector3 playerPos = player.position;

        foreach (var chunk in chunks)
        {
            bool visible = (chunk.transform.position - playerPos).sqrMagnitude < sqrRadius;
            if (visible == chunk.visible) continue;
            
            chunk.visible = visible;
            foreach (var r in chunk.renderers) r.enabled = visible;
        }
    }
}