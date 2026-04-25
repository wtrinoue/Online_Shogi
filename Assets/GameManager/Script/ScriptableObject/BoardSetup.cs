using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Asset/BoardSetup")]
public class BoardSetup : ScriptableObject
{
    [Header("Set PieceData")]
    public List<PiecePlacement> placements = new List<PiecePlacement>();
}