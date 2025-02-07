using UnityEngine;

[System.Serializable]
public class GemObjectData
{
  public int index;                   // 젬 인덱스
  public Sprite sprite;               // 젬 이미지
  public string name;                 // 젬 이름
}

[CreateAssetMenu(fileName = "GemData", menuName = "ScriptableObjects/GemData")]
public class GemData : ScriptableObject
{
    public GemObjectData[] gems;
}