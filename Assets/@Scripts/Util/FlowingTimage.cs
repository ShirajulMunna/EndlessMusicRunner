using UnityEngine;
using UnityEngine.UI;

public class FlowingImage : MonoBehaviour
{
    public float speed = 1.0f;
    private Material material;
    [SerializeField] Image Img_FlowingImage;
    [SerializeField] SpriteRenderer Sp_Spite;

    void Start()
    {
        material = Img_FlowingImage == null ? Sp_Spite.material : Img_FlowingImage.material;
        if (material == null)
        {
            Debug.LogError("Material is missing!");
        }
    }

    void Update()
    {
        var conditioncheck_0 = SpawnManager.instance.GetGameState() == E_GameState.Result && speed >= 0.01f;
        var conditioncheck_1 = GameManager.instance.player.isStopPlayer;


        // ������ �����ٸ� ������ ���� 
        if (conditioncheck_0 || conditioncheck_1)
        {
            speed = 0.0f;
        }
        else if (GameManager.instance.player.CurHp <= 0)
            speed = 0.0f;
        if (material != null)
        {
            material.SetFloat("_Speed", speed);
        }
    }
}
