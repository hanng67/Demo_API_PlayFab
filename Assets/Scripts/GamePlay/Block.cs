using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Block : MonoBehaviour
{
    [SerializeField] private Image blockImage;
    [SerializeField] private TextMeshProUGUI valueText;

    public int value;
    public Vector2Int coordinates;
    public Node node;

    public bool isMoving = false;
    public bool isLocked = false;

    public void SetBlockType(BlockType blockType)
    {
        value = blockType.value;
        blockImage.color = blockType.color;
        valueText.text = value.ToString();
    }

    public void Spawn(Node node)
    {
        if (this.node != null)
        {
            this.node.occupiedBlock = null;
        }

        this.node = node;
        this.coordinates = node.coordinates;
        node.occupiedBlock = this;

        transform.position = node.transform.position;
    }

    public void MoveTo(Node node)
    {
        if (this.node != null)
        {
            this.node.occupiedBlock = null;
        }

        this.node = node;
        this.coordinates = node.coordinates;
        node.occupiedBlock = this;

        StartCoroutine(AnimateCoroutine(node.transform.position));
    }

    public bool CanMerge(Block block)
    {
        return !isLocked && !block.isLocked && value == block.value;
    }

    public void MergeTo(Node node)
    {
        if (this.node != null)
        {
            this.node.occupiedBlock = null;
        }

        this.node = null;
        node.occupiedBlock.isLocked = true;

        StartCoroutine(AnimateCoroutine(node.transform.position, true));
    }

    private IEnumerator AnimateCoroutine(Vector3 targetPosition, bool isMerge = false)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float time = 0.2f;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / time);
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;

        if (isMerge)
        {
            Destroy(gameObject);
        }
    }
}
