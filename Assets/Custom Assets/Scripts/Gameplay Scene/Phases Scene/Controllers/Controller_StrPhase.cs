using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller_StrPhase : MonoBehaviour
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
        PhaseStarted, PhaseFinished,
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
    public UI_StrPhase strUI_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public int selectedRoundIndex;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    List<Player_Phases> player_Cps = new List<Player_Phases>();

    Player_Phases localPlayer_Cp, otherPlayer_Cp;

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

        InitComponents();

        //
        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();

        player_Cps = controller_Cp.player_Cps;

        localPlayer_Cp = controller_Cp.localPlayer_Cp;

        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;

        cam_Tf = controller_Cp.cam_Tf;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        strUI_Cp.Init();
    }

    #endregion

    //--------------------------------------------------
    public void PlayPhase()
    {
        StartCoroutine(Corou_PlayPhase());
    }

    IEnumerator Corou_PlayPhase()
    {
        mainGameState = GameState_En.PhaseStarted;

        //
        strUI_Cp.MoveToPlayerboard();

        //
        //mainGameState = GameState_En.PhaseFinished;

        yield return null;
    }

    //-------------------------------------------------- move camera
    public void MoveCamToPlayerboard()
    {
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_MovecamToMiharidai);
        TargetTweening.TranslateGameObject(cam_Tf, localPlayer_Cp.playerBLookPoint_Tf, unityEvent);
    }

    void OnComplete_MovecamToPlayerboard()
    {

    }

    public void MoveCamToMiharidai()
    {
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_MovecamToMiharidai);
        TargetTweening.TranslateGameObject(cam_Tf, localPlayer_Cp.miharidaiLookPoint_Tf, unityEvent);
    }
    void OnComplete_MovecamToMiharidai()
    {

    }

    //-------------------------------------------------- Handle sp markers on playerboard
    public void On_IncSpMarker()
    {
        localPlayer_Cp.IncSpMarker(selectedRoundIndex);

        strUI_Cp.SetSpMarker(localPlayer_Cp.markersData.usedSpMarkers.count,
            localPlayer_Cp.markersData.totalSpMarkers.count);
    }

    public void On_DecSpMarker()
    {
        localPlayer_Cp.DecSpMarker(selectedRoundIndex);

        strUI_Cp.SetSpMarker(localPlayer_Cp.markersData.usedSpMarkers.count,
            localPlayer_Cp.markersData.totalSpMarkers.count);
    }

    //-------------------------------------------------- 
    public void OnPbPanel_Round(int index)
    {
        //
        if (strUI_Cp.mainGameState != UI_StrPhase.GameState_En.OnPlayerboardPanel)
        {
            return;
        }
        if (strUI_Cp.ExistAnyGameStates(UI_StrPhase.GameState_En.OnActionWindowPanel,
            UI_StrPhase.GameState_En.OnCardDetailPanel))
        {
            return;
        }
        strUI_Cp.OnPbPanel_Round(index);

        //
        selectedRoundIndex = index;
    }

}
