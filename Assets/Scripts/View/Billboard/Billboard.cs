using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool lockY = true;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //LookToCamera();
            DoRandomFlip();
        }
    }

    private void LateUpdate()
    {
        UpdateSortingLayer();
    }

    private void LookToCamera()
    {
        if (mainCamera == null) return;

        Vector3 direction = mainCamera.transform.position;

        if (lockY)
        {
            direction.x = transform.position.x;
        }

        transform.LookAt(direction);
    }

    private void DoRandomFlip()
    {
        if (spriteRenderer == null) return;

        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.flipX = Random.value < 0.5f;
    }

    private void UpdateSortingLayer()
    {
        if(spriteRenderer == null) return;

        spriteRenderer.sortingOrder = Mathf.FloorToInt(transform.position.z * 100) * -1;
    }
}
