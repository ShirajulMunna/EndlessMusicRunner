using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    const string Name = "EffectDamage_{0}";

    public static async void Create(E_MoveData e_MoveData)
    {
        var idx = GetIDX(e_MoveData);
        var name = string.Format(Name, idx);
        var result = await name.CreateOBJ<GameObject>();

        var pos = IMovePoint.GetMovePoint(e_MoveData);
        pos.x = -11;

        result.transform.position = pos;
        GameLog.Log($"생성위치: {pos}");

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
}