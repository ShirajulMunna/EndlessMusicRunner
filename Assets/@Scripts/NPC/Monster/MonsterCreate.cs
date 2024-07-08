using System.Threading.Tasks;
using UnityEngine;

public class MonsterCreate : MonoBehaviour
{
    const string Name = "Monster_{0}";
    public static async Task<IMonster> Create(C_MonsterTable data, Vector3 cratepos, Transform Tr_Parent)
    {
        try
        {
            var name = string.Format(Name, data.PrefabName);
            var result = await name.CreateOBJ<GameObject>(Tr_Parent);
            var monster = result.GetComponent<IMonster>();
            monster.CreateMonster(data, cratepos);
            return monster;
        }
        catch
        {
            GameLog.Log($"번호:{data.PrefabName}");
            return null;
        }

    }
}