using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    const string Name = "EffectDamage_{0}";

    public static async void Create(E_MoveData e_MoveData)
    {
        var idx = GetIDX(e_MoveData);
        var name = string.Format(Name, idx);
        var result = await name.CreateOBJ<GameObject>();

        CreatePoint(result);
        Destroy(result.gameObject, 1f);
    }

    /// <summary>
    /// 오브젝트 idx 바꾸기
    /// </summary>
    static int GetIDX(E_MoveData e_MoveData)
    {
        var idx = 1;

        switch (e_MoveData)
        {
            case E_MoveData.Down:
            case E_MoveData.Twin:
            case E_MoveData.Middle:
            default:
                break;
            case E_MoveData.Up:
                idx = 2;
                break;
        }

        return idx;
    }

    /// <summary>
    /// 생성 위치
    /// </summary>
    static void CreatePoint(GameObject obj)
    {
        var pos = GameManager.M_Player.transform.position;
        pos.x = -11;
        obj.transform.position = pos;
    }
}