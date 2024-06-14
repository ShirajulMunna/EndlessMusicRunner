using UnityEngine;
using UnityEngine.UI;

public class FlowingImage : MonoBehaviour
{
    public float speed = 1.0f;
    private Material material;
    [SerializeField] Image Img_FlowingImage;

    void Start()
    {
        material = Img_FlowingImage.material;
        if (material == null)
        {
            Debug.LogError("Material is missing!");
        }
    }

    void Update()
    {
        if (material != null)
        {
            material.SetFloat("_Speed", speed);
            material.SetFloat("_Time", Time.time);
        }
    }
}
