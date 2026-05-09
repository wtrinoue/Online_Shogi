# 条件分岐設計（Condition分離）のまとめ

---

## 1. 結論

条件分岐は `if` を単純に分解するのではなく、  
**「true / false を返す評価器（Condition）」として分離する設計が有効である。**

ただし、分離しすぎると逆に複雑化するため、適切な粒度が重要になる。

---

## 2. 基本方針

従来の設計：  
if (条件A) 処理  
if (条件B) 処理


---

改善後の設計：  
条件 = 評価関数（true / false）  
条件の組み合わせで処理を決定  


---

## 3. Conditionの定義

Conditionとは「分岐」ではなく、

> ゲーム状態を入力として真偽値を返す評価関数

である。

---

## 4. 実装イメージ

```csharp
public interface ICondition
{
    bool Evaluate(GameState state, Move move);
}
```
```csharp
public class IsInPromotionZone : ICondition
{
    public bool Evaluate(GameState state, Move move)
    {
        return move.To.y <= 2 || move.To.y >= 6;
    }
}
```
```csharp
if (inPromotionZone.Evaluate(state, move)
    && isNotHandPiece.Evaluate(state, move)
    && isPromotablePiece.Evaluate(state, move))
{
    Promote(piece);
}
```
