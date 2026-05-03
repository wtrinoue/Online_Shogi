// using Fusion;
// using UnityEngine;

// public class NetworkGameManager : NetworkBehaviour
// {
//     // =========================
//     // 盤（81マス）
//     // =========================
//     [Networked, Capacity(81)]
//     public NetworkArray<NetworkPieceData> Board => default;

//     // =========================
//     // 手番
//     // =========================
//     [Networked]
//     public Team CurrentTurn { get; set; }

//     // =========================
//     // 初期化
//     // =========================
//     public override void Spawned()
//     {
//         if (Object.HasStateAuthority)
//         {
//             InitBoard();
//             CurrentTurn = Team.Sente;
//         }
//     }

//     private void InitBoard()
//     {
//         // GameManagerの初期配置を流用
//         var gm = GameManager.Instance;

//         for (int y = 0; y < 9; y++)
//         {
//             for (int x = 0; x < 9; x++)
//             {
//                 var piece = gm.pieceBoard[x, y];

//                 int index = ToIndex(x, y);

//                 if (piece == null)
//                 {
//                     Board.Set(index, default);
//                     continue;
//                 }

//                 Board.Set(index, new NetworkPieceData
//                 {
//                     Team = piece.data.team,
//                     Type = piece.data.type,
//                     IsPromoted = piece.isPromoted,
//                     IsHanded = piece.isHanded
//                 });
//             }
//         }
//     }

//     // =========================
//     // RPC（入力受付）
//     // =========================
//     [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
//     public void RPC_RequestMove(Vector2Int from, Vector2Int to)
//     {
//         if (!Object.HasStateAuthority) return;

//         int fromIndex = ToIndex(from.x, from.y);
//         int toIndex = ToIndex(to.x, to.y);

//         var piece = Board.Get(fromIndex);

//         if (piece.Type == PieceType.None) return;

//         // 手番チェック
//         if (piece.Team != CurrentTurn) return;

//         // 移動実行
//         ExecuteMove(fromIndex, toIndex);

//         // 手番交代
//         CurrentTurn = (CurrentTurn == Team.Sente)
//             ? Team.Gote
//             : Team.Sente;
//     }

//     // =========================
//     // 移動処理（本体）
//     // =========================
//     private void ExecuteMove(int fromIndex, int toIndex)
//     {
//         var piece = Board.Get(fromIndex);
//         var target = Board.Get(toIndex);

//         // 駒取り（簡易）
//         if (target.Type != PieceType.None)
//         {
//             // 将来：持ち駒処理
//         }

//         Board.Set(toIndex, piece);
//         Board.Set(fromIndex, default);
//     }

//     // =========================
//     // GameManagerへ反映
//     // =========================
//     public override void Render()
//     {
//         ApplyToGameManager();
//     }

//     private void ApplyToGameManager()
//     {
//         var gm = GameManager.Instance;

//         // 盤クリア
//         for (int y = 0; y < 9; y++)
//         {
//             for (int x = 0; x < 9; x++)
//             {
//                 gm.pieceBoard[x, y] = null;
//             }
//         }

//         // Network → GameManager
//         for (int i = 0; i < 81; i++)
//         {
//             var data = Board.Get(i);

//             if (data.Type == PieceType.None)
//                 continue;

//             int x = i % 9;
//             int y = i / 9;

//             PieceData pd = new PieceData
//             {
//                 team = data.Team,
//                 type = data.Type
//             };

//             Piece piece = new Piece(pd, data.IsPromoted);
//             piece.isHanded = data.IsHanded;

//             gm.pieceBoard[x, y] = piece;
//         }
//     }

//     // =========================
//     // utility
//     // =========================
//     private int ToIndex(int x, int y)
//     {
//         return x + y * 9;
//     }
// }