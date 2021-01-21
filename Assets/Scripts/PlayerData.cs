using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public string[] items = new string[18] { "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty", "Empty" };
    public int[] itemAmounts = new int[18];
    public List<int> questsCompleted = new List<int>();
    public List<int> rewardsPurchased = new List<int>();

    public Vector3 playerPosition;
    public int currentSceneId = 1;

    public string playerName = "";
    public int bossKills = 0;

    public int health = 50;
    public int attackXp = 1;
    public int defenseXp =1;
    public int huntingXp =1;
    public int gatheringXp =1;
    public int craftingXp =1;
    public int cookingXp =1;

    public int cash = 2000;
    public bool cheated = false;

    public string dateTimeModified;
    public string fileHash;
}