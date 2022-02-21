using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//配列やリストの中からランダムに1つ返す関数
//参考https://qiita.com/TD12734/items/2da96d3215d88bbd93a2
public class Helper : MonoBehaviour {
    internal static T GetRandom<T>(params T[] Params) {
        return Params[Random.Range(0, Params.Length)];
    }

    internal static T GetRandom<T>(List<T> Params) {
        return Params[Random.Range(0, Params.Count)];
    }
}
