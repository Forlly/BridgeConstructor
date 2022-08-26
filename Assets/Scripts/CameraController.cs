using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    private Vector3 lastPos;
    private Vector3 distanceToMove;

    private void Start()
    {

        lastPos = characterController.transform.position;
    }

    private void Update()
    {
        distanceToMove = characterController.transform.position - lastPos;
        transform.position = new Vector3(transform.position.x + distanceToMove.x, transform.position.y,
            transform.position.z);
        lastPos = characterController.transform.position;
    }
}
