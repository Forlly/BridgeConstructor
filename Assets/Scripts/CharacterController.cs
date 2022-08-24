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
        Debug.Log("pos");
        Debug.Log(transform.position);
        Debug.Log(currentPlatform);
        Debug.Log(PlatformGeneration.Instance.buildPoints[currentPlatform]);
        Debug.Log(PlatformGeneration.Instance.platforms[currentPlatform].name);
        while (transform.position != PlatformGeneration.Instance.platforms[currentPlatform].transform.position)
        {
            step += Time.deltaTime * speed;
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.x,
                    transform.position.y, transform.position.z), step);
            yield return new WaitForSeconds(0.01f);
        }

        currentPlatform++;
    }
}
