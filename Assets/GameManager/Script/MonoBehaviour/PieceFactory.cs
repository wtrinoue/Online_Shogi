using System.Collections.Generic;
using UnityEngine;

public class PieceFactory : MonoBehaviour
{
    [System.Serializable]
    public class PieceRow
    {
        public PieceType type;
        public PieceData senteData;
        public PieceData goteData;
    }

    public PieceRow[] pieceTable;

    private Dictionary<(Team, PieceType), PieceData> pieceDict;

    void Awake()
    {
        pieceDict = new Dictionary<(Team, PieceType), PieceData>();

        foreach (var row in pieceTable)
        {
            pieceDict[(Team.Sente, row.type)] = row.senteData;
            pieceDict[(Team.Gote, row.type)] = row.goteData;
        }
    }

    public Piece GetPiece(Team team, PieceType type, bool isPromoted, bool isHanded)
    {
        if (pieceDict.TryGetValue((team, type), out var data))
        {
            return new Piece(data, isPromoted, isHanded);
        }

        Debug.LogError($"Piece not found: {team}, {type}");
        return null;
    }
}