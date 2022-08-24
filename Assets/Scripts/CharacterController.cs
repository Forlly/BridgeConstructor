using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    private bool isBuild = false;
    private int currentPlatform = 0;
   
    void Update()
    {
        if (!isBuild)
        {
            StartCoroutine(MoveToNextPlatform());
        }
    }

    private IEnumerator MoveToNextPlatform()
    {
        isBuild = true;
        float step = 0;

        while (transform.position.x <= PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.x)
        {
            step += Time.deltaTime * speed;
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.x,
                    transform.position.y, transform.position.z), step);
            yield return new WaitForSeconds(0.01f);
        }

        currentPlatform++;
        Debug.Log(currentPlatform);
       // StartCoroutine(MoveToNextPlatform());
    }
}
