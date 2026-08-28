using UnityEngine;
using System.Collections.Generic;

public class ChunkActivationSystem : MonoBehaviour
{ 
    public static ChunkActivationSystem Instance;
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] Transform player;
    [SerializeField] float activationRadius = 65f;
    [SerializeField] float checkInterval = 5f;
    [SerializeField] private bool initialized;
    
    class ChunkData
    {
        public Transform transform;
        public SpriteRenderer[] renderers;
        public bool visible;
    }

    private readonly List<ChunkData> chunks = new();
    private readonly Dictionary<GameObject, ChunkData> chunkMap = new();

    private float sqrRadius;
    private float timer;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        sqrRadius = activationRadius * activationRadius;
    }

    public void InitializeChunks()
    {
        if (initialized)
            return;
        initialized = true;

        foreach (GameObject prefab in chunkPrefabs)
        {
            GameObject chunk = Instantiate(
                prefab,
                prefab.transform.position,
                prefab.transform.rotation
            );
            ChunkData data = new ChunkData
            {
                transform = chunk.transform,
                renderers = chunk.GetComponentsInChildren<SpriteRenderer>(true),
                visible = false
            };
            foreach (var r in data.renderers)
                r.enabled = false;
            chunks.Add(data);
            chunkMap.Add(prefab, data);
        }
    }
    public void SetChunksVisible(GameObject[] enableChunks, GameObject[] disableChunks)
    {
        foreach (var prefab in enableChunks)
        {
            var chunk = chunkMap[prefab];
            chunk.visible = true;
            foreach (var r in chunk.renderers)
                r.enabled = true;
        }

        foreach (var prefab in disableChunks)
        {
            var chunk = chunkMap[prefab];
            chunk.visible = false;
            foreach (var r in chunk.renderers)
                r.enabled = false;
        }
    }

    void Update()
    {
        if (!initialized)
            return;
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