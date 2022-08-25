using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [SerializeField] private float speed = 1f;
    
    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject bridgeImg;
    [SerializeField] private float maxSizeBridge = 5f;
    
    private bool isBuild = false;
    private bool isMoving = true;
    private bool isCallToBuild = false;
    private int currentPlatform = 0;
    
    private int score = 0;
    private bool isAddScore = false;
    
    private GameObject crntBridge;

    private void Start()
    {
        isMoving = true;
        Debug.Log(isMoving);
    }

    void Update()
    {
        if (isMoving)
        {
            StartCoroutine(MoveToNextPlatform());
        }

        if (isBuild && !Input.GetMouseButtonDown(0) && !isMoving && !isCallToBuild)
        {
            StartCoroutine(CallToBuild());
        }
        
        if (isBuild && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(BuildBridge());
        }
        
    }

    private IEnumerator MoveToNextPlatform()
    {
        isMoving = false;
        animator.SetBool("isMooving", true);

        while (transform.position.x <= PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.x - transform.localScale.x * 1.5f)
        {
            
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.x,
                    transform.position.y, transform.position.z), speed);
            if (transform.position.y < PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.y)
            {
                animator.SetBool("isMooving", false);
                StopAllCoroutines();
            }
            
            if (transform.position.x
                >= PlatformGeneration.Instance.platforms[currentPlatform].GetComponent<Collider2D>().bounds.min.x 
                && isAddScore)
            {
                isAddScore = false;
                score++;
                ScoreView.Instance.UpdateScore(score);
            }
            
            yield return new WaitForSeconds(0.01f);
        }

        
        if (crntBridge != null && 
            crntBridge.gameObject.transform.localScale.y + crntBridge.gameObject.transform.position.x 
            > transform.position.x + 2 * gameObject.transform.localScale.x)
        {
            currentPlatform++;
            isMoving = true;
            yield break;
        }
        
        
        Debug.Log(currentPlatform);
        isAddScore = true;
        isBuild = true;
        animator.SetBool("isMooving", false);
    }

    private IEnumerator BuildBridge()
    {
        isBuild = false;

        Vector3 pos = PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position;
        
        crntBridge = Instantiate(bridge,pos, quaternion.identity);
        while (Input.GetMouseButton(0) && crntBridge.gameObject.transform.localScale.y <= 5)
        {
            crntBridge.gameObject.transform.localScale = new Vector3(crntBridge.gameObject.transform.localScale.x,
                crntBridge.gameObject.transform.localScale.y + 0.05f);

            yield return new WaitForSeconds(0.008f);
        }
        
        crntBridge.gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, -90.0f);
        currentPlatform++;
        isMoving = true;
        
    }

    private IEnumerator CallToBuild()
    {
        isCallToBuild = true;
        Vector3 pos = PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position;
        GameObject tmpBuildImg = Instantiate(bridgeImg, new Vector3(pos.x, pos.y + 5f, pos.z), Quaternion.identity);
        while (isBuild && !Input.GetMouseButtonDown(0) && !isMoving)
        {
            if (tmpBuildImg.transform.eulerAngles == new Vector3(0.0f, 0.0f, -90.0f))
                tmpBuildImg.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            
            else
                tmpBuildImg.transform.eulerAngles =  new Vector3(0.0f, 0.0f, -90.0f);
            
            
            Debug.Log(tmpBuildImg.transform.eulerAngles);
            yield return new WaitForSeconds(0.008f);
        }
    }
    
}
