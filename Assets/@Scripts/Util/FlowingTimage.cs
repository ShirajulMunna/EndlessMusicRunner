using UnityEngine;
using UnityEngine.UI;

public class FlowingImage : MonoBehaviour
{
    public float speed = 1.0f;
    private Material material;
    [SerializeField] Image Img_FlowingImage;

    void Start()
    {
        Image image = Img_FlowingImage;
        if (image != null)
        {
            material = image.material;
            if (material == null)
            {
                Debug.LogError("Material is missing!");
            }
        }
        else
        {
            Debug.LogError("Image component is missing!");
        }
    }

    void Update()
    {
        if (material != null)
        {
            material.SetFloat("_Speed", speed);
        }
    }
}
