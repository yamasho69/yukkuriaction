//  ListExtension.cs
//  http://kan-kikuchi.hatenablog.com/entry/ListExtension
//
//  Created by kan.kikuchi on 2016.04.29.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Listの拡張クラス
/// </summary>
public static class ListExtension {

    //=================================================================================
    //重複
    //=================================================================================

    /// <summary>
    /// 重複しないように追加
    /// </summary>
    public static void AddToNotDuplicate<T>(this List<T> list, T t) {
        if (list.Contains(t)) {
            return;
        }
        list.Add(t);
    }

    /// <summary>
    /// 重複を無くす
    /// </summary>
    public static void RemoveDuplicate<T>(this List<T> list) {
        List<T> newList = new List<T>();

        foreach (T item in list) {
            newList.AddToNotDuplicate(item);
        }

        list = newList;
    }

    //=================================================================================
    //並び変え
    //=================================================================================

    /// <summary>
    /// ランダムに並び替え
    /// </summary>
    public static List<T> Shuffle<T>(this List<T> list) {

        for (int i = 0; i < list.Count; i++) {
            T temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list;
    }

    //=================================================================================
    //取得
    //=================================================================================

    /// <summary>
    /// 指定したNoのものを取得し、リストから消す
    /// </summary>
    public static T GetAndRemove<T>(this List<T> list, int targetNo) {
        if (list.Count <= targetNo || targetNo < 0) {
            Debug.LogError("リストの範囲を超えています！(ListCount : " + list.Count + ", No : " + targetNo + ")");
        }

        T target = list[targetNo];
        list.Remove(target);
        return target;
    }

    //=================================================================================
    //先入先出
    //=================================================================================

    /// <summary>
    /// 先頭から取り出し、リストから消す
    /// </summary>
    public static T Pop<T>(this List<T> list) {
        return list.GetAndRemove(list.Count - 1);
    }

    //=================================================================================
    //後入先出
    //=================================================================================

    /// <summary>
    /// 最後尾から取り出し、リストから消す
    /// </summary>
    public static T Dequeue<T>(this List<T> list) {
        return list.GetAndRemove(0);
    }

    //=================================================================================
    //ランダム取得
    //=================================================================================

    /// <summary>
    /// ランダムに取得する
    /// </summary>
    public static T GetAtRandom<T>(this List<T> list) {
        if (list.Count == 0) {
            Debug.LogError("リストが空です！");
        }

        return list[Random.Range(0, list.Count)];
    }

    /// <summary>
    /// ランダムに取得し、リストから消す
    /// </summary>
    public static T GetAndRemoveAtRandom<T>(this List<T> list) {
        T target = list.GetAtRandom();
        list.Remove(target);
        return target;
    }

}