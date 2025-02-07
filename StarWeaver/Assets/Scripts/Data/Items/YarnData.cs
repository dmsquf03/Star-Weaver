using UnityEngine;

// 실(Yarn) 데이터
[System.Serializable]
public class YarnData
{
    public string name;                 // 실 이름
    public int count;                   // 보유 수량 (1-99)
    public Sprite sprite;               // 실 이미지
    public string mainType;             // 실 종류 (기본/푹신/동글/리본)
    public int subType = -1;            // 부재료 종류 (-1: 없음, 0: pearl, 1: sparkle)
    public Color color;                 // RGB 색상값
    
    public bool HasSubMaterial => subType != -1;
}