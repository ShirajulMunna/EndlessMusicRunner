using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class Data : MonoBehaviour
    {
        #region ���������� ���̺� (LevelDesigin Table)
        static Dictionary<int, List<C_LevelDesign>> _LevelDesign;
        public static Dictionary<int, List<C_LevelDesign>> LevelDesigin
        {
            get
            {
                if (_LevelDesign == null)
                {
                    var datas = ReadData_Sync<LevelDesign>("/LevelTable");
                    _LevelDesign = datas.Init();

                }
                return _LevelDesign;
            }
        }
        #endregion

        #region MonsterTable
        static Dictionary<int, C_MonsterTable> _MonsterTable;
        public static Dictionary<int, C_MonsterTable> MonsterTable
        {
            get
            {
                if(_MonsterTable == null)
                {
                    var datas = ReadData_Sync<MonsterTable>("/MonsterTable");
                    _MonsterTable = datas.Init();
                }
                return _MonsterTable;
            }
        }
        #endregion



        public static T ReadData_Sync<T>(string filename)
        {
            var data = DataSet.ReadData_Sync<T>(filename);
            return data;
        }


        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                var t = MonsterTable.ContainsKey(0);
                Debug.Log(t);
            }
        }
    }

}
