using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance;
    
    [SerializeField] private Animator animator;
    
    [SerializeField] private float speed = 1f;
    
    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject shadowBridge;
    [SerializeField] private GameObject bridgeImg;
    [SerializeField] private float maxSizeBridge = 6f;

    [SerializeField] private AudioSource buildBridgeSound;
    [SerializeField] private AudioSource gameOverSound;
    
    private bool isBuild = false;
    private bool isMoving = true;
    private bool isCallToBuild = false;
    private int currentPlatform = 0;
    
    private int score = 0;
    private bool isAddScore = false;
    
    private GameObject crntBridge;

    private void Start()
    {
        Instance = this;
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

        while (transform.position.x <= PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.x 
               - transform.localScale.x * 1.5f)
        {
            
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.x,
                    transform.position.y, transform.position.z), speed);
            
            if (transform.position.y - transform.localScale.y*2 < 
                PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.y)
            {
                animator.SetBool("isMooving", false);
                gameOverSound.Play();
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
            
            yield return new WaitForSeconds(0.015f);
        }

        
        if (crntBridge != null && 
            crntBridge.gameObject.transform.localScale.y + crntBridge.gameObject.transform.position.x 
            > transform.position.x + 2 * gameObject.transform.localScale.x)
        {
            currentPlatform++;
            isMoving = true;
            yield break;
        }
        
        
        isAddScore = true;
        isBuild = true;
        animator.SetBool("isMooving", false);
    }

    private IEnumerator BuildBridge()
    {
        buildBridgeSound.Play();
        isBuild = false;
        Vector3 pos = PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position;

        GameObject crntShadowBridge = null;
        if (currentPlatform <= 1)
        {
            crntShadowBridge = Instantiate(shadowBridge,pos, quaternion.identity);
            crntShadowBridge.transform.eulerAngles = new Vector3(0, 0, -90);
        }

        crntBridge = Instantiate(bridge,pos, quaternion.identity);
        
        while (Input.GetMouseButton(0) && crntBridge.gameObject.transform.localScale.y <= maxSizeBridge)
        {
            crntBridge.gameObject.transform.localScale = new Vector3(crntBridge.gameObject.transform.localScale.x,
                crntBridge.gameObject.transform.localScale.y + 0.05f);

            if (crntShadowBridge != null)
            {
                crntShadowBridge.gameObject.transform.localScale = new Vector3(
                    crntShadowBridge.gameObject.transform.localScale.x,
                    crntShadowBridge.gameObject.transform.localScale.y + 0.05f);
            }
            
            
            yield return new WaitForSeconds(0.01f);
        }
        buildBridgeSound.Stop();
        
        int step = 0;
        while (step != -90)
        {
            step -= 1;
            crntBridge.gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, step);
            yield return new WaitForSeconds(0.001f);
        }
        
        currentPlatform++;
        isMoving = true;
    }

    private IEnumerator CallToBuild()
    {
        isCallToBuild = true;
        Vector3 pos = PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position;
        bridgeImg.SetActive(true);
        bridgeImg.transform.position = new Vector3(pos.x, pos.y + 5f, pos.z);

        while (isBuild && !Input.GetMouseButtonDown(0) && !isMoving)
        {
            if (bridgeImg.transform.rotation == Quaternion.Euler(0,0,45))
            {
                bridgeImg.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            
            else
                bridgeImg.transform.rotation = Quaternion.Euler(0,0,45);
            
            
            yield return new WaitForSeconds(0.5f);
        }
        
        bridgeImg.SetActive(false);
        isCallToBuild = false;
    }

    public void Relive()
    {
        currentPlatform -= 1;
        Bounds tmp = PlatformGeneration.Instance.platforms[currentPlatform].GetComponent<Collider2D>().bounds;
        transform.position = new Vector3(tmp.min.x + transform.localScale.x * 2, tmp.max.y + transform.localScale.y*2,
            transform.position.z);
        Destroy(crntBridge);
        isMoving = true;
    }
    
}
