using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject destroyPoint;
    

    void Update()
    {
        if (transform.position.x < destroyPoint.transform.position.x)
        {
            ObjectPool.Instance.TurnOfObject(gameObject);
        }
    }
}
