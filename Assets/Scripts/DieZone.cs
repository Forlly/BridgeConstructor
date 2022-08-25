using UnityEngine;

public class DieZone : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            menuPanel.SetActive(true);
            Menu.Instance.GameOver();
        }
    }
}
