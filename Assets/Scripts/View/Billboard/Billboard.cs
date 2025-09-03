using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool lockY = true;
    [SerializeField] private bool doRandomFlip = true;
    [SerializeField] private bool realTimeSortingOrderUpdate = false;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            DoRandomFlip();
            UpdateSortingLayer();
        }
    }

    private void LateUpdate()
    {
        if(realTimeSortingOrderUpdate) UpdateSortingLayer();
    }

    private void DoRandomFlip()
    {
        if (spriteRenderer == null || !doRandomFlip) return;

        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.flipX = Random.value < 0.5f;
    }

    private void UpdateSortingLayer()
    {
        if(spriteRenderer == null) return;

        spriteRenderer.sortingOrder = Mathf.FloorToInt(transform.position.z * 100) * -1;
    }
}
