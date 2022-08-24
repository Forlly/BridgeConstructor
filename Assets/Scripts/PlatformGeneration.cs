using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformGeneration : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject destroyPoint;

    [SerializeField] private float minDistanceBetweenPlayforms;
    [SerializeField] private float maxDistanceBetweenPlayforms;
    private float distanceBetweenPlayforms;
    
    [SerializeField] private float minPlatformWidth;
    [SerializeField] private float maxPlatformWidth;
    private float platformWidth;

    private GameObject newPlatform;
    public List<GameObject> platforms;
    public List<GameObject> buildPoints;

    public static PlatformGeneration Instance;

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (transform.position.x < spawnPoint.position.x)
        {
            distanceBetweenPlayforms = Random.Range(minDistanceBetweenPlayforms, maxDistanceBetweenPlayforms);
            platformWidth = Random.Range(minPlatformWidth, maxPlatformWidth);

            transform.position = new Vector3(transform.position.x + platformWidth + distanceBetweenPlayforms,
                transform.position.y, transform.position.z);
            
            newPlatform = ObjectPool.Instance.GetPooledObject();
            newPlatform.transform.position = transform.position;
            newPlatform.gameObject.transform.localScale =
                new Vector3(platformWidth, newPlatform.gameObject.transform.localScale.y);
            newPlatform.GetComponent<PlatformDestroyer>().destroyPoint = destroyPoint;
            platforms.Add(newPlatform);
            buildPoints.Add(newPlatform.GetComponentInChildren<BuildPoint>().gameObject);

        }
    }
}
