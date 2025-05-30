using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region Stat

    [Serializable]
    public class Stat
    {
        // public이나 [SerializeField]를 붙여야 어떤 데이타를 가져와야 하는지 읽을 수 있음
        // json 문자열을 해당 타입으로 파싱함( "level" 값의 문자열을 파싱했을 때 int 타입이 아니면 오류가 남)
        public int level;
        public int maxHp;
        public int attack;
        public int defense;
        public int totalExp;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();
        public Dictionary<int, Stat> MakeDic()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (var stat in stats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;
        }
    }

    #endregion
}

