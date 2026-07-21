using UnityEngine;
public class DestroyAfterAnimation : MonoBehaviour
{
    public void DisableSelf()
    {
        Destroy(gameObject);
    }
}