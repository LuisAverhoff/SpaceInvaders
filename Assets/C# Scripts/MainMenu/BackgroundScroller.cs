using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private Material backgroundMaterial;

    public float scrollSpeed;
    private float offset;

    void Start()
    {
        backgroundMaterial = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        offset += scrollSpeed * Time.deltaTime;
        offset = offset % 1.0f;
        backgroundMaterial.mainTextureOffset = new Vector2(offset, 0);
    }
}
