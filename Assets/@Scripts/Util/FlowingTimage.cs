using UnityEngine;
using UnityEngine.UI;

public class FlowingImage : MonoBehaviour
{
    public float speed = 1.0f;
    private Material material;
    [SerializeField] Image Img_FlowingImage;
    [SerializeField] SpriteRenderer Sp_Spite;

    bool isStop;

    void Start()
    {
        PlayManager.instance.AddAction(E_Play.Boss, () => isStop = true);
        PlayManager.instance.AddAction(E_Play.Boss, () => material.SetFloat("_Speed", 0));

        material = Img_FlowingImage == null ? Sp_Spite.material : Img_FlowingImage.material;
    }

    void Update()
    {
        if (isStop)
        {
            return;
        }

        if (material != null)
        {
            material.SetFloat("_Speed", speed);
        }
    }
}
