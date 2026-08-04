using UnityEngine;
public class DestroyAfterAnimation : MonoBehaviour
{
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}