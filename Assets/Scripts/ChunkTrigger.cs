using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    [SerializeField] private GameObject chunkPrefab; 
    private GameObject chunkVisual; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (chunkVisual == null)
        {
            chunkVisual = ChunkActivationSystem.Instance.GetChunk(chunkPrefab);
        }
        
        if (chunkVisual != null)
            chunkVisual.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (chunkVisual != null)
            chunkVisual.SetActive(false);
    }
}