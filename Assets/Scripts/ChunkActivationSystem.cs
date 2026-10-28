using UnityEngine;
using System.Collections.Generic;

public class ChunkActivationSystem : MonoBehaviour
{ 
    public static ChunkActivationSystem Instance;
    
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private bool initialized;
    
    private readonly Dictionary<GameObject, GameObject> chunkMap = new();
    
    void Awake()
    {
        Instance = this;
    }

    public void InitializeChunks()
    {
        if (initialized) return;
        initialized = true;

        foreach (GameObject prefab in chunkPrefabs)
        {
            GameObject chunk = Instantiate(
                prefab,
                prefab.transform.position,
                prefab.transform.rotation
            );
            
            chunkMap.Add(prefab, chunk);
            chunk.SetActive(false);
        }
        
    }
    
    public GameObject GetChunk(GameObject prefab)
    {
        if (chunkMap.TryGetValue(prefab, out GameObject chunk))
            return chunk;
        
        Debug.LogWarning($"Чанк для префаба {prefab.name} не найден!");
        return null;
    }
}