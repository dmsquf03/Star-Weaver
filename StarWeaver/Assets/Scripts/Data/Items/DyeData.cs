using UnityEngine;

// 염색약(Dye) 데이터

[System.Serializable]
public class DyeObjectData
{
    public int index;                   // 염색약 인덱스
    public Sprite sprite;               // 염색약 이미지
    public string name;                 // 염색약 이름
    public Color color;                 // RGB 색상값
}

[CreateAssetMenu(fileName = "DyeData", menuName = "ScriptableObjects/DyeData")]
public class DyeData : ScriptableObject
{
    public DyeObjectData[] dyes;
}
