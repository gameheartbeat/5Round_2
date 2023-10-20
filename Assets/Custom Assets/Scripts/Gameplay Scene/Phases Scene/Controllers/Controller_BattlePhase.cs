using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller_BattlePhase : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, WillFinish,
        InitReplacementFinished,
        PhaseFinished,
        TurnStarted, TurnFinished,
        RoundStarted, RoundFinished,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    public UI_BattlePhase battleUI_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    // components
    Controller_Phases controller_Cp;

    UI_Phases uiManager_Cp;

    StatusManager statusManager_Cp;

    List<Player_Phases> player_Cps = new List<Player_Phases>();

    Player_Phases localPlayer_Cp, otherPlayer_Cp, comPlayer_Cp;

    Transform cam_Tf;

    int localPlayerID;

    // variables
    List<RoundValue> localRoundsData = new List<RoundValue>();

    List<RoundValue> otherRoundsData = new List<RoundValue>();

    int playerActionPriority;

    int dice;

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public GameState_En mainGameState
    {
        get { return gameStates[0]; }
        set { gameStates[0] = value; }
    }

    //-------------------------------------------------- private properties

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //-------------------------------------------------- Start is called before the first frame update
    void Start()
    {

    }

    //-------------------------------------------------- Update is called once per frame
    void Update()
    {

    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Manage gameStates
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region ManageGameStates

    //--------------------------------------------------
    public void AddMainGameState(GameState_En value)
    {
        if (gameStates.Count == 0)
        {
            gameStates.Add(value);
        }
    }

    //--------------------------------------------------
    public void AddGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value_tp in values)
        {
            gameStates.Add(value_tp);
        }
    }

    //--------------------------------------------------
    public bool ExistGameStates(params GameState_En[] values)
    {
        bool result = true;
        foreach (GameState_En value in values)
        {
            if (!gameStates.Contains(value))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public bool ExistAnyGameStates(params GameState_En[] values)
    {
        bool result = false;
        foreach (GameState_En value in values)
        {
            if (gameStates.Contains(value))
            {
                result = true;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public int GetExistGameStatesCount(GameState_En value)
    {
        int result = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == value)
            {
                result++;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public void RemoveGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value in values)
        {
            gameStates.RemoveAll(gameState_tp => gameState_tp == value);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Initialize
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        StartCoroutine(Corou_Init());
    }

    IEnumerator Corou_Init()
    {
        AddMainGameState(GameState_En.Nothing);

        //
        SetComponents();

        //
        InitComponents();

        //
        InitVariables();

        //
        InitReplacement();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.InitReplacementFinished));
        RemoveGameStates(GameState_En.InitReplacementFinished);

        //
        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();

        statusManager_Cp = controller_Cp.statusManager_Cp;

        cam_Tf = controller_Cp.cam_Tf;

        player_Cps = controller_Cp.player_Cps;

        localPlayer_Cp = controller_Cp.localPlayer_Cp;
        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;
        comPlayer_Cp = controller_Cp.comPlayer_Cp;

        localPlayerID = controller_Cp.localPlayerID;
    }

    //--------------------------------------------------
    void InitComponents()
    {

    }

    //--------------------------------------------------
    void InitVariables()
    {
        localRoundsData = localPlayer_Cp.roundsData;

        otherRoundsData = otherPlayer_Cp.roundsData;

        playerActionPriority = -1;

        dice = -1;
    }

    //--------------------------------------------------
    void InitReplacement()
    {
        StartCoroutine(Corou_InitReplacement());
    }

    IEnumerator Corou_InitReplacement()
    {
        //
        statusManager_Cp.statusUI.gameObject.SetActive(false);

        //
        UnityEvent unityEvent = new UnityEvent();

        TargetTweening.TranslateGameObject(cam_Tf, localPlayer_Cp.camBpPoint_Tf, unityEvent);
        yield return new WaitForSeconds(1f);

        //
        for (int i = 0; i < player_Cps.Count; i++)
        {
            TargetTweening.TranslateGameObject(player_Cps[i].playerB_Tf, player_Cps[i].pbBpPoint_Tf, unityEvent);
        }
        yield return new WaitForSeconds(1f);

        //
        for (int i = 0; i < player_Cps.Count; i++)
        {
            TargetTweening.TranslateGameObject(player_Cps[i].miharidai_Tf, player_Cps[i].mdBpPoint_Tf, unityEvent);
        }
        yield return new WaitForSeconds(1f);

        //
        AddGameStates(GameState_En.InitReplacementFinished);
    }

    #endregion

    //-------------------------------------------------- Play phase
    public void PlayPhase()
    {
        StartCoroutine(Corou_PlayPhase());
    }

    IEnumerator Corou_PlayPhase()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayRound(i);

            yield return new WaitUntil(() => ExistGameStates(GameState_En.RoundFinished));
            RemoveGameStates(GameState_En.RoundFinished);
        }
    }

    //-------------------------------------------------- Play round
    void PlayRound(int roundIndex_pr)
    {
        StartCoroutine(CorouPlayRound(roundIndex_pr));
    }

    IEnumerator CorouPlayRound(int roundIndex)
    {
        //
        SetPlayerActionPriority(roundIndex);
        yield return new WaitUntil(() => playerActionPriority != -1);

        if (playerActionPriority == 0)
        {
            localPlayer_Cp.PlayRound(roundIndex);
        }
        else if (playerActionPriority == 1)
        {
            otherPlayer_Cp.PlayRound(roundIndex);
        }

        //
        AddGameStates(GameState_En.RoundFinished);
    }

    //-------------------------------------------------- Set player action priority
    void SetPlayerActionPriority(int roundIndex)
    {
        StartCoroutine(Corou_SetPlayerActionPriority(roundIndex));
    }

    IEnumerator Corou_SetPlayerActionPriority(int roundIndex)
    {
        RoundValue localRoundValue = localRoundsData[roundIndex];
        RoundValue otherRoundValue = otherRoundsData[roundIndex];

        //
        int localPlayerPriority = 0;
        switch (localRoundsData[roundIndex].token.type)
        {
            case TokenType.Shien:
                localPlayerPriority = 3;
                break;
            case TokenType.Move:
                localPlayerPriority = 2;
                break;
            case TokenType.Attack:
                localPlayerPriority = 1;
                break;
            case TokenType.Null:
                if (localRoundValue.spMarkerCount > 0)
                {
                    localPlayerPriority = 4;
                }
                else
                {
                    localPlayerPriority = 0;
                }
                break;
        }

        //
        int otherPlayerPriority = 0;
        switch (otherRoundsData[roundIndex].token.type)
        {
            case TokenType.Shien:
                otherPlayerPriority = 3;
                break;
            case TokenType.Move:
                otherPlayerPriority = 2;
                break;
            case TokenType.Attack:
                otherPlayerPriority = 1;
                break;
            case TokenType.Null:
                if (otherRoundValue.spMarkerCount > 0)
                {
                    otherPlayerPriority = 4;
                }
                else
                {
                    otherPlayerPriority = 0;
                }
                break;
        }

        // evaulate priority using action token
        if (localPlayerPriority > otherPlayerPriority)
        {
            playerActionPriority = 0;
            yield break;
        }
        else if (localPlayerPriority < otherPlayerPriority)
        {
            playerActionPriority = 1;
            yield break;
        }
        else if (localPlayerPriority != 1 && localPlayerPriority != 1)
        {
            playerActionPriority = 0;
            yield break;
        }

        // evaulate priority using agi
        int localUnitAgi = localPlayer_Cp.bUnit_Cps[localRoundValue.originUnitIndex].unitCardData.agi;
        int otherUnitAgi = otherPlayer_Cp.bUnit_Cps[otherRoundValue.originUnitIndex].unitCardData.agi;
        if (localUnitAgi > otherUnitAgi)
        {
            playerActionPriority = 0;
            yield break;
        }
        else if (localUnitAgi < otherUnitAgi)
        {
            playerActionPriority = 1;
            yield break;
        }

        // evaluate priority using dice)
        int localPlayerDice = -1, otherPlayerDice = -1;

        do
        {
            ThrowDice();
            yield return new WaitUntil(() => dice != -1);
            localPlayerDice = dice;
            dice = -1;

            ThrowDice();
            yield return new WaitUntil(() => dice != -1);
            otherPlayerDice = dice;
            dice = -1;
        }
        while (localPlayerDice != otherPlayerDice);

        if (localPlayerDice > otherPlayerDice)
        {
            playerActionPriority = 0;
        }
        else if (localPlayerDice < otherPlayerDice)
        {
            playerActionPriority = 1;
        }
    }

    //-------------------------------------------------- Throw dice
    void ThrowDice()
    {
        StartCoroutine(Corou_ThrowDice());
    }

    IEnumerator Corou_ThrowDice()
    {
        yield return new WaitForSeconds(1f);

        dice = Random.Range(1, 7);
    }

}
