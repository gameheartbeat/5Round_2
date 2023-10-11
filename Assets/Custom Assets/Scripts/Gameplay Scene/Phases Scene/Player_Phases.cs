using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Round_Cs
{
    public int index;

    public Transform roundPanel_Tf;

    public Transform allyVan1_Tf, allyVan2_Tf, enemyVan1_Tf, enemyVan2_Tf;

    public Transform markersGroup_Tf;

    public Transform token_Tf;

    public List<Transform> marker_Tfs = new List<Transform>();
}

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

    public List<Round_Cs> rounds = new List<Round_Cs>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    DataManager_Gameplay dataManager_Cp;

    List<Transform> bUnitPoint_Tfs = new List<Transform>();

    List<Transform> mUnitPoint_Tfs = new List<Transform>();

    List<Transform> round_Tfs = new List<Transform>();

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
        //
        InitUnitCardDatas();

        //
        InitBattleUnits();

        InitMihariUnits();

        InitRounds();
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
            Round_Cs round_Cp_tp = new Round_Cs();
            round_Cp_tp.roundPanel_Tf = round_Tfs[i];
            round_Cp_tp.allyVan1_Tf = round_Tfs[i].GetChild(0);
            round_Cp_tp.allyVan2_Tf = round_Tfs[i].GetChild(1);
            round_Cp_tp.enemyVan1_Tf = round_Tfs[i].GetChild(2);
            round_Cp_tp.enemyVan2_Tf = round_Tfs[i].GetChild(3);
            round_Cp_tp.markersGroup_Tf = round_Tfs[i].GetChild(4);

            rounds.Add(round_Cp_tp);
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

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// OnEvents
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    
    #region OnEvents
    //--------------------------------------------------
    void OnClickRound(int index)
    {
        controller_Cp.strController_Cp.strUI_Cp.OnPbPanel_Round(index);
    }

    #endregion

}
