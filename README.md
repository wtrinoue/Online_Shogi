# Online_Shogi

## プロジェクト概要

Online_ShogiはUnityで作られた将棋系オンラインボードゲームのプロジェクトです。
ゲームの状態管理にはStateパターンを採用し、入力処理と描画は分離されています。
現在の実装ではローカル対戦を中心に、盤上の駒操作・持ち駒の打ち込み・勝敗判定を扱います。

## ファイル構成と役割

### ルート

- `README.md`
  - このプロジェクトの構成とゲームループを説明します。

### シーン管理

- `Assets/SceneLoader/Script/SceneLoader.cs`
  - `SceneType`に応じてタイトルシーンとローカルゲームシーンを切り替えます。
  - `LoadScene()` で `SceneManager.LoadScene(...)` を呼び出します。

### 初期化と起動

- `Assets/Bootstrap/Script/MonoBehaviour/Bootstrap.cs`
  - ゲーム起動時に `GameManager`、`GameViewer`、`StateMachine` を初期化します。
  - `Start()` で `Init()` を呼び出し、ゲーム状態の開始準備を整えます。

### ゲーム状態管理

- `Assets/StateMachine/Script/MonoBehaviour/StateMachine.cs`
  - 現在の `State` を保持し、クリックイベントを現在状態に委譲します。
  - `Init()` で `GameContext` を生成し、最初に `TimerTextState` を経由して `IdleState` を開始します。
  - `ChangeState(State state)` で現在状態の `Exit()` を呼び、次状態の `Enter()` を呼び出します。

### ルールと盤面管理

- `Assets/GameManager/Script/MonoBehaviour/GameManager.cs`
  - 盤上と持ち駒のデータを保持します。
  - 盤面の駒移動、成り判定、持ち駒の追加、セル状態の変更などを管理します。
  - `Init()` で `InitializePiece()` と `InitializeCell()` により盤面状態を初期化します。

### 描画とビュー

- `Assets/GameViewer/Script/GameViewer.cs`
  - `GameManager` のデータを読み取り、駒・セル・持ち駒の表示を生成・更新します。
  - `BuildAll()` / `BuildBoard()` / `BuildSenteHand()` / `BuildGoteHand()` などの描画メソッドを提供します。

### 入力処理

- `Assets/InputSystemActions.cs`
  - Unity Input Systemの生成コードです。
  - `StateMachine` の `OnClick` を呼び出すためのクリックイベントを提供します。

### 状態クラス

- `Assets/StateMachine/Script/State/IdleState.cs`
  - 何も選択していない待機状態。
  - 盤面駒、先手持ち駒、後手持ち駒の選択を受け付けます。
- `Assets/StateMachine/Script/State/SelectBoardState.cs`
  - 盤上の駒を選択した後の状態。
  - 移動先の選択や別の駒の再選択を処理します。
- `Assets/StateMachine/Script/State/SelectSenteState.cs`
  - 先手の持ち駒を選択した状態。
  - 打つ位置の選択や盤上駒への切り替えを処理します。
- `Assets/StateMachine/Script/State/SelectGoteState.cs`
  - 後手の持ち駒を選択した状態。
  - 打つ位置の選択や盤上駒への切り替えを処理します。
- `Assets/StateMachine/Script/State/JudgeState.cs`
  - 手番が終わった後に勝敗判定を行う状態。
  - 終了条件なら `EndState`、続行なら `ModeState` へ遷移します。
- `Assets/StateMachine/Script/State/ModeState.cs`
  - ゲームモードに応じて次の状態を決定します。
  - ローカルモードなら `TimerTextState` を経て `IdleState` に戻ります。
  - ネットワークモードなら `NetworkWaitState` へ遷移します。
- `Assets/StateMachine/Script/State/NetworkWaitState.cs`
  - ネットワーク対戦時に相手の手を待つ状態です。
- `Assets/StateMachine/Script/State/NetworkJudgeState.cs`
  - ネットワーク対戦で勝敗判定を行い、続行なら次ターン表示へ遷移します。
- `Assets/StateMachine/Script/State/TimerTextState.cs`
  - 文字メッセージを表示し、指定時間後に次の状態へ遷移します。
- `Assets/StateMachine/Script/State/EndState.cs`
  - 勝者表示を行う終了状態です。
- `Assets/StateMachine/Script/State/TextState.cs`
  - クリックで次状態に遷移する汎用メッセージ状態です（現状では未使用）。
- `Assets/StateMachine/Script/State/SenteEntryState.cs`
  - 先手ターン開始時に `TimerTextState` へ遷移する実装があります（現状では未使用）。
- `Assets/StateMachine/Script/State/GoteEntryState.cs`
  - ネットワーク対戦の接続待ち開始状態です（現状では未使用）。

### 共通コンテキスト

- `Assets/StateMachine/Script/Class/Module/GameContext.cs`
  - `State` 間で共有されるモジュールをまとめます。
  - `MachineModule`、`ManagerModule`、`ViewerModule`、`TurnModule`、`TextModule`、`ResultModule`、`JudgeModule`、`ModeModule` を保持します。

## ゲームループとState遷移

このプロジェクトでは、ゲームループは `StateMachine` の状態遷移とイベント委譲によって実現されています。
クリック入力は `StateMachine.OnClick(Vector2 pos)` に渡され、現在の `State` が処理を行います。
状態遷移は `currentState.Exit()` → `currentState = nextState` → `currentState.Enter()` で実行されます。

1. `Bootstrap.Start()` で `StateMachine.Init()` が呼ばれる
2. `StateMachine` は `GameContext` を生成し、先手ターンを設定
3. 最初に `TimerTextState` が開始され、メッセージ表示後に `IdleState` へ遷移
4. `IdleState` で駒選択を待ち、ユーザーのクリックに応じて `SelectBoardState` / `SelectSenteState` / `SelectGoteState` へ移動
5. 選択状態で駒移動・打ち込みが完了したら `JudgeState` に遷移し、勝敗判定を実行
6. 勝者が決まれば `EndState` へ、続行なら `ModeState` へ移行
7. `ModeState` でモードを判定し、ローカルなら再び `TimerTextState` から `IdleState` へ戻り、ネットワークなら `NetworkWaitState` へ移行

### 状態遷移図

```mermaid
stateDiagram-v2
    [*] --> TimerTextState: 初期化
    TimerTextState --> IdleState: タイマー終了
    IdleState --> SelectBoardState: 盤上駒選択
    IdleState --> SelectSenteState: 先手持ち駒選択
    IdleState --> SelectGoteState: 後手持ち駒選択
    SelectBoardState --> JudgeState: 盤上移動完了
    SelectSenteState --> JudgeState: 先手打ち完了
    SelectGoteState --> JudgeState: 後手打ち完了
    SelectBoardState --> SelectSenteState: 先手持ち駒へ切替
    SelectBoardState --> SelectGoteState: 後手持ち駒へ切替
    SelectSenteState --> SelectBoardState: 盤上駒へ切替
    SelectGoteState --> SelectBoardState: 盤上駒へ切替
    JudgeState --> EndState: 終了判定成立
    JudgeState --> ModeState: 続行
    ModeState --> TimerTextState: ローカルモード
    ModeState --> NetworkWaitState: ネットワークモード
    NetworkWaitState --> NetworkJudgeState: 相手待ち完了
    NetworkJudgeState --> EndState: 終了判定成立
    NetworkJudgeState --> TimerTextState: 続行
```

## 重要な設計ポイント

- `StateMachine` は主に「現在の状態」と「クリックイベントの受け渡し」を担当します。
- `GameManager` はルールに関わるデータ操作を担当し、状態遷移そのものは行いません。
- `GameViewer` は描画を担当し、ロジックを持ちません。
- `TimerTextState` により、ターン開始時の一時停止表示が実現されています。
- `JudgeState` で勝敗を判定し、`EndState` もしくは再開ルートへ分岐します。

## 開発の歩み（Git履歴からの工程）

1. プロジェクト初期段階で基本的な将棋操作の制御を追加し、駒の選択や移動の基礎を作りました。
2. ゲームループと状態管理の必要性を整理し、`StateMachine` による遷移の骨格を構築しました。
3. `State` の抽象化を進め、`IState` から `State` クラスへのリファクタリングで状態実装を統一しました。
4. 盤面クリックや持ち駒選択を扱う `SelectState` を導入し、選択／移動フローを明確化しました。
5. `GameContext` を導入して、`GameManager`／`GameViewer`／`TextManager` などの共有依存を状態間で安全に渡せるようにしました。
6. 結果表示機能を追加し、`TextManager` と `EndState` で勝利メッセージ表示が行えるようにしました。
7. `JudgeState` を追加し、手番終了後の勝敗判定と進行分岐の仕組みを実装しました。
8. ターン開始時の表示として `TimerTextState` を導入し、次状態への遷移を時間制御できるようにしました。
9. `ModeModule` を追加し、ローカルとネットワークの切り替えを `ModeState` で判定する構成にしました。
10. `IGameManager` インターフェースを導入し、ゲーム管理の実装を抽象化してテストや拡張をしやすくしました。
11. `Photon Fusion` とネットワークオブジェクトを追加して、オンライン対戦の基盤を整備しました。
12. `NetworkGameManager` と `NetworkWaitState` / `NetworkJudgeState` を追加し、ネットワーク対戦時の状態遷移フローを実装しました。
13. `Singleton` パターンを廃止して依存注入型に再設計し、`GameManager`／`GameViewer`／`TextManager` の責務を明確化しました。
14. ビルド設定に `LocalGameScene` を追加し、ローカルゲームシーンを含む実行環境を整えました。
15. ネットワーク状態群と `README` を更新し、現在の設計と遷移図を反映しました。

## 今後の拡張候補

- `ModeState` のネットワークモード実装を進める
- `State` の共通処理を整理して `IdleState` / `Select*State` の重複を減らす
- `GameManager` の責務分離を進めて、盤面・手番・持ち駒の管理をより明確にする
- タイトルシーンやゲームセット画面を追加する
