using UnityEngine;

[CreateAssetMenu(fileName = "BoardConfig", menuName = "Asset/BoardConfig")]
public class BoardConfig : ScriptableObject
{
    public Vector3 boardOffset = new Vector3(0f, 0f, 0f);
    public Vector3 senteHandOffset = new Vector3(0f, 0f, 0f);
    public Vector3 goteHandOffset = new Vector3(0f, 0f, 0f);
    public float cellSize;
}
