using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player_Phases : MonoBehaviour
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
    public bool isCreator, isLocalPlayer, isCom;

    [SerializeField]
    GameObject bUnit_Pf, mUnit_Pf;

    [SerializeField]
    Transform bUnitPointsGroup_Tf;

    [SerializeField]
    Transform mUnitPointsGroup_Tf;

    [SerializeField]
    Transform roundsGroup_Tf;

    [SerializeField]
    public Transform playerBLookPoint_Tf, miharidaiLookPoint_Tf, battleBLookPoint_Tf;

    [SerializeField]
    GameObject spMarker_Pf, goldMarker_Pf;

    [SerializeField]
    GameObject shienToken_Pf, move1Token_Pf, move2Token_Pf, move3Token_Pf, atk1Token_Pf, atk2Token_Pf;

    [SerializeField]
    Transform shienUnitPointsGroup_Tf;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //
    [ReadOnly]
    public int playerID;

    [ReadOnly]
    public List<UnitCardData> bUnitCardDatas = new List<UnitCardData>();

    [ReadOnly]
    public List<UnitCardData> mUnitCardDatas = new List<UnitCardData>();

    //
    [ReadOnly]
    public List<UnitCard> bUnit_Cps = new List<UnitCard>();

    [ReadOnly]
    public List<UnitCard> mUnit_Cps = new List<UnitCard>();

    //
    public List<RoundValue> roundsData = new List<RoundValue>();

    public TokenData tokensData = new TokenData();

    public MarkerData markersData = new MarkerData();

    //
    public BattleInfo battleInfo = new BattleInfo();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    DataManager_Gameplay dataManager_Cp;

    List<Transform> bUnitPoint_Tfs = new List<Transform>();

    List<Transform> mUnitPoint_Tfs = new List<Transform>();

    List<Transform> round_Tfs = new List<Transform>();

    List<Transform> shienUnitPoint_Tfs = new List<Transform>();

    Transform cam_Tf;

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
                result = false;
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
        AddMainGameState(GameState_En.Nothing);

        //
        SetComponents();

        InitVariables();

        //
        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();

        dataManager_Cp = controller_Cp.dataManager_Cp;
    }

    //--------------------------------------------------
    void InitVariables()
    {
        InitUnitCardDatas();

        InitMarkersData();

        InitTokensData();

        InitBattleUnits();

        InitMihariUnits();

        InitRounds();

        InitBattleInfo();

        InitShienUnitPoints();
    }

    //--------------------------------------------------
    void InitUnitCardDatas()
    {
        bUnitCardDatas = dataManager_Cp.playersBattleUnitCardsData[playerID].unitCards;

        mUnitCardDatas = dataManager_Cp.playersStandUnitCardsData[playerID].unitCards;
    }

    //--------------------------------------------------
    void InitBattleUnits()
    {
        //
        for (int i = 0; i < bUnitPointsGroup_Tf.childCount; i++)
        {
            bUnitPoint_Tfs.Add(bUnitPointsGroup_Tf.GetChild(i));
        }

        //
        for (int i = 0; i < bUnitPoint_Tfs.Count; i++)
        {
            UnitCard bUnit_Cp_tp = Instantiate(bUnit_Pf, bUnitPoint_Tfs[i]).GetComponent<UnitCard>();
            bUnit_Cps.Add(bUnit_Cp_tp);
        }

        //
        for (int i = 0; i < bUnit_Cps.Count; i++)
        {
            bUnit_Cps[i].SetStatus_Phases(playerID, bUnitCardDatas[i], false, true);
        }
    }

    //--------------------------------------------------
    void InitMihariUnits()
    {
        //
        for (int i = 0; i < mUnitPointsGroup_Tf.childCount; i++)
        {
            mUnitPoint_Tfs.Add(mUnitPointsGroup_Tf.GetChild(i));
        }

        //
        for (int i = 0; i < mUnitPoint_Tfs.Count; i++)
        {
            UnitCard mUnit_Cp_tp = Instantiate(mUnit_Pf, mUnitPoint_Tfs[i]).GetComponent<UnitCard>();
            mUnit_Cps.Add(mUnit_Cp_tp);
        }

        //
        for (int i = 0; i < mUnit_Cps.Count; i++)
        {
            mUnit_Cps[i].SetStatus_Phases(playerID, mUnitCardDatas[i], true, true);
        }
    }

    //--------------------------------------------------
    void InitRounds()
    {
        //
        for (int i = 0; i < roundsGroup_Tf.childCount; i++)
        {
            round_Tfs.Add(roundsGroup_Tf.GetChild(i));
        }

        //
        for (int i = 0; i < round_Tfs.Count; i++)
        {
            RoundValue rndValue_tp = new RoundValue();

            rndValue_tp.roundPanel_Tf = round_Tfs[i];
            rndValue_tp.allyVan1_Tf = round_Tfs[i].GetChild(0);
            rndValue_tp.allyVan2_Tf = round_Tfs[i].GetChild(1);
            rndValue_tp.enemyVan1_Tf = round_Tfs[i].GetChild(2);
            rndValue_tp.enemyVan2_Tf = round_Tfs[i].GetChild(3);
            rndValue_tp.markersGroup_Tf = round_Tfs[i].GetChild(4);

            rndValue_tp.index = i;

            roundsData.Add(rndValue_tp);
        }

        //
        for (int i = 0; i < round_Tfs.Count; i++)
        {
            LongPressDetector clickDetector_Cp_tp = round_Tfs[i].AddComponent<LongPressDetector>();

            clickDetector_Cp_tp.targetObject_Tf = clickDetector_Cp_tp.transform;
            clickDetector_Cp_tp.enableClickDetect = true;

            int index = i;
            UnityEvent unityEvent = new UnityEvent();
            unityEvent.AddListener(() => OnClickRound(index));
            clickDetector_Cp_tp.onClicked = unityEvent;
        }
    }

    //--------------------------------------------------
    void InitMarkersData()
    {
        MarkerValue usedSpMarkers_tp = new MarkerValue();
        usedSpMarkers_tp.type = MarkerType.SP;
        usedSpMarkers_tp.count = 0;
        markersData.usedSpMarkers = usedSpMarkers_tp;

        MarkerValue totalSpMarkers_tp = new MarkerValue();
        totalSpMarkers_tp.type = MarkerType.SP;
        totalSpMarkers_tp.count = 1;
        markersData.totalSpMarkers = totalSpMarkers_tp;

        MarkerValue apMarkers_tp = new MarkerValue();
        apMarkers_tp.type = MarkerType.AP;
        apMarkers_tp.count = 1;
        markersData.apMarkers = apMarkers_tp;

        MarkerValue turnMarkers_tp = new MarkerValue();
        turnMarkers_tp.type = MarkerType.Turn;
        turnMarkers_tp.count = 1;
        markersData.turnMarkers = turnMarkers_tp;

        MarkerValue usedGoldMarkers_tp = new MarkerValue();
        usedGoldMarkers_tp.type = MarkerType.Gold;
        usedGoldMarkers_tp.count = 0;
        markersData.usedGoldMarkers = usedGoldMarkers_tp;

        MarkerValue totalGoldMarkers_tp = new MarkerValue();
        totalGoldMarkers_tp.type = MarkerType.Gold;
        totalGoldMarkers_tp.count = 1;
        markersData.totalGoldMarkers = totalGoldMarkers_tp;
    }

    //--------------------------------------------------
    void InitTokensData()
    {
        TokenValue usedShienToken_tp = new TokenValue();
        usedShienToken_tp.type = TokenType.Shien;
        usedShienToken_tp.count = 0;
        tokensData.usedShienToken = usedShienToken_tp;

        TokenValue totalShienToken_tp = new TokenValue();
        totalShienToken_tp.type = TokenType.Shien;
        totalShienToken_tp.count = 1;
        tokensData.totalShienToken = totalShienToken_tp;

        TokenValue usedMoveToken_tp = new TokenValue();
        usedMoveToken_tp.type = TokenType.Move;
        usedMoveToken_tp.count = 0;
        tokensData.usedMoveToken = usedMoveToken_tp;

        TokenValue totalMoveToken_tp = new TokenValue();
        totalMoveToken_tp.type = TokenType.Move;
        totalMoveToken_tp.count = 3;
        tokensData.totalMoveToken = totalMoveToken_tp;

        TokenValue usedAtkToken_tp = new TokenValue();
        usedAtkToken_tp.type = TokenType.Attack;
        usedAtkToken_tp.count = 0;
        tokensData.usedAtkToken = usedAtkToken_tp;

        TokenValue totalAtkToken_tp = new TokenValue();
        totalAtkToken_tp.type = TokenType.Attack;
        totalAtkToken_tp.count = 2;
        tokensData.totalAtkToken = totalAtkToken_tp;
    }

    void InitBattleInfo()
    {
        battleInfo.mihariUnitCount = 7;
        battleInfo.discardUnitCount = 0;
    }

    void InitShienUnitPoints()
    {
        for (int i = 0; i < shienUnitPointsGroup_Tf.childCount; i++)
        {
            shienUnitPoint_Tfs.Add(shienUnitPointsGroup_Tf.GetChild(i));
        }
    }

    #endregion

    //-------------------------------------------------- Handle sp markers
    public void IncSpMarker(int roundIndex_pr, int incCount_pr = 1)
    {
        SetSpMarker(roundIndex_pr, incCount_pr);
    }

    public void DecSpMarker(int roundIndex_pr, int decCount_pr = 1)
    {
        SetSpMarker(roundIndex_pr, -1 * decCount_pr);
    }

    void SetSpMarker(int roundIndex_pr, int changeAmount_pr)
    {
        //
        roundsData[roundIndex_pr].spMarkerCount = Mathf.Clamp(
            roundsData[roundIndex_pr].spMarkerCount + changeAmount_pr,
            0, markersData.totalSpMarkers.count);

        SetSpMarkersOnPlayerboard(roundIndex_pr, roundsData[roundIndex_pr].spMarkerCount);

        //
        markersData.usedSpMarkers.count = roundsData[roundIndex_pr].spMarkerCount;
    }

    void SetSpMarkersOnPlayerboard(int roundIndex_pr, int markerCount_pr)
    {
        // destroy markers
        RoundValue roundValue = roundsData[roundIndex_pr];
        for (int i = 0; i < roundValue.marker_Tfs.Count; i++)
        {
            Destroy(roundValue.marker_Tfs[i].gameObject);
        }
        roundValue.marker_Tfs.Clear();

        // instant markers
        for (int i = 0; i < markerCount_pr; i++)
        {
            Transform marker_Tf_tp = Instantiate(spMarker_Pf, roundValue.markersGroup_Tf).transform;
            roundValue.marker_Tfs.Add(marker_Tf_tp);
        }

        // arrange markers
        List<Vector3> markersPos = ArrangeObjects.GetArrangePoints(roundValue.markersGroup_Tf.position,
            markerCount_pr, 3, 0.04f, 0.04f);
        for (int i = 0; i < roundValue.marker_Tfs.Count; i++)
        {
            roundValue.marker_Tfs[i].position = markersPos[i];
        }
    }

    //-------------------------------------------------- Set tokens
    public void SetShienToken(int roundIndex_pr, int shienUnitIndex_pr, int shienTargetUnitIndex_pr)
    {
        RoundValue roundValue = roundsData[roundIndex_pr];
        Transform tokenPoint_Tf_tp = null;

        if (shienTargetUnitIndex_pr == 0)
        {
            tokenPoint_Tf_tp = roundValue.allyVan1_Tf;
        }
        else if (shienTargetUnitIndex_pr == 1)
        {
            tokenPoint_Tf_tp = roundValue.allyVan2_Tf;
        }

        // set token variables
        roundValue.token.type = TokenType.Shien;
        roundValue.token.count = 1;
        roundValue.originUnitIndex = shienUnitIndex_pr;
        roundValue.targetUnitIndex = shienTargetUnitIndex_pr;

        // instant token
        roundValue.token_Tf = Instantiate(shienToken_Pf, tokenPoint_Tf_tp).transform;

        // move mihari unit to shien unit point
        roundValue.shienUnit_Cp = mUnit_Cps[shienUnitIndex_pr];
        UnityEvent unityEvent = new UnityEvent();
        TargetTweening.TranslateGameObject(roundValue.shienUnit_Cp.transform, shienUnitPoint_Tfs[roundIndex_pr],
            unityEvent);
        mUnit_Cps[shienUnitIndex_pr] = null;
    }

    public void SetMoveToken(int roundIndex_pr, int originIndex_pr, int targetIndex_pr)
    {
        RoundValue roundValue = roundsData[roundIndex_pr];
        Transform tokenPoint_Tf_tp = null;

        if (targetIndex_pr == 0)
        {
            tokenPoint_Tf_tp = roundValue.allyVan1_Tf;
        }
        else if (targetIndex_pr == 1)
        {
            tokenPoint_Tf_tp = roundValue.allyVan2_Tf;
        }

        // set token variables
        roundValue.token.type = TokenType.Move;
        roundValue.token.count = 1;
        roundValue.originUnitIndex = originIndex_pr;
        roundValue.targetUnitIndex = targetIndex_pr;

        // instant token
        GameObject token_Pf_tp = null;
        if (originIndex_pr == 0)
        {
            token_Pf_tp = move1Token_Pf;
        }
        else if (originIndex_pr == 1)
        {
            token_Pf_tp = move2Token_Pf;
        }
        else if (originIndex_pr == 2)
        {
            token_Pf_tp = move3Token_Pf;
        }
        roundValue.token_Tf = Instantiate(token_Pf_tp, tokenPoint_Tf_tp).transform;
    }

    public void SetAtkToken(int roundIndex_pr, int originIndex_pr, int targetIndex_pr)
    {
        RoundValue roundValue = roundsData[roundIndex_pr];
        Transform tokenPoint_Tf_tp = null;

        if (targetIndex_pr == 0)
        {
            tokenPoint_Tf_tp = roundValue.enemyVan1_Tf;
        }
        else if (targetIndex_pr == 1)
        {
            tokenPoint_Tf_tp = roundValue.enemyVan2_Tf;
        }

        // set token
        roundValue.token.type = TokenType.Attack;
        roundValue.token.count = 1;
        roundValue.originUnitIndex = originIndex_pr;
        roundValue.targetUnitIndex = targetIndex_pr;

        // instant token
        GameObject token_Pf_tp = null;
        if (originIndex_pr == 0)
        {
            token_Pf_tp = atk1Token_Pf;
        }
        else if (originIndex_pr == 1)
        {
            token_Pf_tp = atk2Token_Pf;
        }
        roundValue.token_Tf = Instantiate(token_Pf_tp, tokenPoint_Tf_tp).transform;
    }

    //-------------------------------------------------- Reset round tokens
    public void ResetRoundToken(int roundIndex_pr, UnityEvent unityEvent)
    {
        RoundValue roundValue = roundsData[roundIndex_pr];

        if (roundValue.token.type == TokenType.Null)
        {
            unityEvent.Invoke();
            return;
        }

        // destroy token
        Destroy(roundValue.token_Tf.gameObject);

        // reset shien token in case of shien
        if (roundValue.token.type == TokenType.Shien)
        {
            int mihariUnitIndex_pr = roundValue.originUnitIndex;

            TargetTweening.TranslateGameObject(roundValue.shienUnit_Cp.transform,
                mUnitPoint_Tfs[mihariUnitIndex_pr], unityEvent);

            mUnit_Cps[mihariUnitIndex_pr] = roundValue.shienUnit_Cp;

            roundValue.shienUnit_Cp = null;
        }

        // reset token variables
        roundValue.token.type = TokenType.Null;
        roundValue.token.count = 0;
        roundValue.originUnitIndex = 0;
        roundValue.targetUnitIndex = 0;
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// OnEvents
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region OnEvents

    //--------------------------------------------------
    void OnClickRound(int index)
    {
        controller_Cp.strController_Cp.On_PbRoundPanel(index);
    }

    #endregion

}
