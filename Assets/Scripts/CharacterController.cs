using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static CharacterController Instance;
    
    [SerializeField] private Animator animator;
    
    [SerializeField] private float speed = 1f;
    
    [SerializeField] private GameObject bridge;
    [SerializeField] private float speedBuilding;
    [SerializeField] private GameObject shadowBridge;
    [SerializeField] private float speedFalling;
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
        isMoving = false;
        StartCoroutine(MoveToNextPlatform());
    }

    public void TryToBuildBridge()
    {
        if (isCallToBuild)
            StartCoroutine(BuildBridge());
    }
    
    public void TryToStopBuildBridge()
    {
        isBuild = false;
    }
    
    private IEnumerator MoveToNextPlatform()
    {
        isMoving = true;
        animator.SetBool("isMooving", true);

        float step = speed * Time.fixedDeltaTime;

        while (transform.position.x <= PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.x 
               - transform.localScale.x * 1.5f)
        {
            
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.x,
                    transform.position.y, transform.position.z), step);
            
            if (transform.position.y - transform.localScale.y*2 < 
                PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position.y)
            {
                isMoving = false;
                animator.SetBool("isMooving", false);
                PlaySound();
                yield break;
            }
           
            
            if (transform.position.x
                >= PlatformGeneration.Instance.platforms[currentPlatform].GetComponent<Collider2D>().bounds.min.x 
                && isAddScore)
            {
                isAddScore = false;
                score++;
                ScoreView.Instance.UpdateScore(score);
            }

            yield return null;
        }

        
        if (crntBridge != null && 
            crntBridge.gameObject.transform.localScale.y + crntBridge.gameObject.transform.position.x 
            > transform.position.x + 2 * gameObject.transform.localScale.x)
        {
            currentPlatform++;
            StartCoroutine(MoveToNextPlatform());
            yield break;
        }
        
        
        isAddScore = true;
        animator.SetBool("isMooving", false);
        isMoving = false;
        StartCoroutine(CallToBuild());
    }

    public void PlaySound()
    {
        gameOverSound.Play();
    }

    private IEnumerator BuildBridge()
    {
        buildBridgeSound.Play();
        isBuild = true;
        Vector3 pos = PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position;


        if (currentPlatform <= 1)
        {
            shadowBridge.SetActive(true);
            shadowBridge.transform.localScale = new Vector3(1,1,0);
            shadowBridge.transform.position = pos;

        }

        crntBridge = Instantiate(bridge,pos, quaternion.identity);

        float step = speedBuilding * Time.fixedDeltaTime;
        
        while (isBuild)
        {
            if (crntBridge.gameObject.transform.localScale.y >= maxSizeBridge)
            {
                isBuild = false;
            }
            crntBridge.gameObject.transform.localScale = new Vector3(crntBridge.gameObject.transform.localScale.x,
                crntBridge.gameObject.transform.localScale.y + step);

            if (shadowBridge.activeInHierarchy)
            {
                shadowBridge.gameObject.transform.localScale = new Vector3(
                    shadowBridge.gameObject.transform.localScale.x,
                    shadowBridge.gameObject.transform.localScale.y + step);
            }


            yield return null;
        }
        buildBridgeSound.Stop();
        isBuild = false;


        step = speedFalling * Time.fixedDeltaTime;
        float progress = 0;
        while (progress > -90)
        {
            progress -= step;
            crntBridge.gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, progress);
            yield return null;
        }
        crntBridge.gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, -90);
        
        if (shadowBridge.activeInHierarchy)
        {
            shadowBridge.SetActive(false);
        }

        
        currentPlatform++;
        StartCoroutine(MoveToNextPlatform());
    }

    private IEnumerator CallToBuild()
    {
        isCallToBuild = true;
        Vector3 pos = PlatformGeneration.Instance.buildPoints[currentPlatform].transform.position;
        bridgeImg.SetActive(true);
        bridgeImg.transform.position = new Vector3(pos.x, pos.y + 5f, pos.z);
        
        float progress = 0;
        float targetAngle = 90f;
        while (!isBuild)
        {
            if (progress < targetAngle)
            {
                progress = Mathf.PingPong(Time.time * 120, targetAngle);
                bridgeImg.transform.rotation = Quaternion.Euler(0, 0, progress);
            }

            yield return null;
        }
        
        bridgeImg.SetActive(false);
        isCallToBuild = false;
    }

    public void Relive()
    {
        if (currentPlatform != 0)
        {
            currentPlatform -= 1;
        }
        
        Bounds tmp = PlatformGeneration.Instance.platforms[currentPlatform].GetComponent<Collider2D>().bounds;
        transform.position = new Vector3(tmp.min.x + transform.localScale.x * 2, tmp.max.y + transform.localScale.y*2,
            transform.position.z);
        Destroy(crntBridge);
        StartCoroutine(MoveToNextPlatform());
    }
    
}
