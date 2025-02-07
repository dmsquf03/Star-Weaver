using UnityEngine;

// 부재료(SubMaterial) 데이터
[System.Serializable]
public class SubMaterialObjectData
{
    public int index;                   // 부재료 인덱스
    public Sprite sprite;               // 부재료 이미지
    public string name;                 // 부재료 이름
    public string description;          // 설명 (0:은은한, 1:반짝이는)
}

[CreateAssetMenu(fileName = "SubMaterialData", menuName = "ScriptableObjects/SubMaterialData")]
public class SubMaterialData : ScriptableObject
{
    public SubMaterialObjectData[] subMaterials;
}