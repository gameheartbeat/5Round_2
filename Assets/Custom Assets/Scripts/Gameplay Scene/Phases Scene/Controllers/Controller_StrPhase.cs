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

    [ReadOnly]
    public TokenType selectedTokenType = new TokenType();

    [ReadOnly]
    public int shienUnitIndex, shienTargetVanUnitIndex;

    [ReadOnly]
    public int moveVanUnitIndex, moveRearUnitIndex;

    [ReadOnly]
    public int atkAllyVanUnitIndex, atkEnemyVanUnitIndex;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    DataManager_Gameplay dataManager_Cp;

    List<Player_Phases> player_Cps = new List<Player_Phases>();

    Player_Phases localPlayer_Cp, otherPlayer_Cp;

    Transform cam_Tf;

    UnitCard shienUnit_Cp;

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

        InitVariables();

        //
        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();

        dataManager_Cp = controller_Cp.dataManager_Cp;

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

    //--------------------------------------------------
    void InitVariables()
    {
        //
        selectedRoundIndex = -1;

        //
        shienUnitIndex = -1;
        shienTargetVanUnitIndex = -1;

        //
        moveVanUnitIndex = -1;
        moveRearUnitIndex = -1;

        //
        atkAllyVanUnitIndex = -1;
        atkEnemyVanUnitIndex = -1;
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
        yield return null;
    }

    //-------------------------------------------------- move camera
    public void MoveCamToPlayerboard()
    {
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_MovecamToPlayerboard);
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

    //-------------------------------------------------- On Playerboard RoundPanel
    public void On_PbRoundPanel(int index)
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

        selectedRoundIndex = index;

        // reset values
        selectedTokenType = TokenType.Null;

        shienUnitIndex = -1;
        shienTargetVanUnitIndex = -1;

        moveVanUnitIndex = -1;
        moveRearUnitIndex = -1;

        atkAllyVanUnitIndex = -1;
        atkEnemyVanUnitIndex = -1;

        // refresh values
        RoundValue roundValue = localPlayer_Cp.roundsData[selectedRoundIndex];
        switch (roundValue.token.type)
        {
            case TokenType.Shien:
                shienUnitIndex = roundValue.originUnitIndex;
                shienTargetVanUnitIndex = roundValue.targetUnitIndex;
                break;
            case TokenType.Move:
                moveRearUnitIndex = roundValue.originUnitIndex;
                moveVanUnitIndex = roundValue.targetUnitIndex;
                break;
            case TokenType.Attack:
                atkAllyVanUnitIndex = roundValue.originUnitIndex;
                atkEnemyVanUnitIndex = roundValue.targetUnitIndex;
                break;
        }

        //
        strUI_Cp.MoveToActionWindow(index);
    }

    //-------------------------------------------------- On ActionWindow panel
    void UpdateActionWindow(TokenType tokenType_pr)
    {
        selectedTokenType = tokenType_pr;

        // reset action window
        switch (selectedTokenType)
        {
            case TokenType.Shien:
                UpdateShienToken();
                break;
            case TokenType.Move:
                UpdateMoveToken();
                break;
            case TokenType.Attack:
                UpdateAtkToken();
                break;
        }
    }

    void UpdateShienToken()
    {
        if (shienUnitIndex == -1 || shienTargetVanUnitIndex == -1)
        {
            return;
        }

        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_ResetToken);
        localPlayer_Cp.ResetRoundToken(selectedRoundIndex, unityEvent);
    }

    void OnComplete_ResetToken()
    {
        localPlayer_Cp.SetShienToken(selectedRoundIndex, shienUnitIndex, shienTargetVanUnitIndex);

        //
        strUI_Cp.RefreshAwMihariUnits();
    }

    void UpdateMoveToken()
    {
        if (moveVanUnitIndex == -1 || moveRearUnitIndex == -1)
        {
            return;
        }

        UnityEvent unityEvent = new UnityEvent();
        localPlayer_Cp.ResetRoundToken(selectedRoundIndex, unityEvent);

        localPlayer_Cp.SetMoveToken(selectedRoundIndex, moveRearUnitIndex, moveVanUnitIndex);
    }

    void UpdateAtkToken()
    {
        if (atkAllyVanUnitIndex == -1 || atkEnemyVanUnitIndex == -1)
        {
            return;
        }

        UnityEvent unityEvent = new UnityEvent();
        localPlayer_Cp.ResetRoundToken(selectedRoundIndex, unityEvent);

        localPlayer_Cp.SetAtkToken(selectedRoundIndex, atkAllyVanUnitIndex, atkEnemyVanUnitIndex);
    }

    void SetActionWindowAtkArrow()
    {
        if (atkAllyVanUnitIndex == -1 || atkEnemyVanUnitIndex == -1)
        {
            strUI_Cp.aw_at_arrow_RT.gameObject.SetActive(false);
            return;
        }

        //strUI_Cp.aw_at_arrow_RT.gameObject.SetActive(true);

        //RectTransform fromArrowPoint_RT_tp = atkAllyVanUnitIndex == 0 ? strUI_Cp.aw_at_allyVan1ArrowPoint_RT
        //    : strUI_Cp.aw_at_allyVan2ArrowPoint_RT;
        //RectTransform toArrowPoint_RT_tp = atkEnemyVanUnitIndex == 0 ? strUI_Cp.aw_at_enemyVan1ArrowPoint_RT
        //    : strUI_Cp.aw_at_enemyVan2ArrorPoint_RT;
        //RectTransform arrow_RT_tp = strUI_Cp.aw_at_arrow_RT;

        //// Calculate the direction from 'from' to 'to' points
        //Vector2 dir = toArrowPoint_RT_tp.anchoredPosition - fromArrowPoint_RT_tp.anchoredPosition;

        //// Set the position of the arrow in the middle of the 'from' and 'to' points
        //arrow_RT_tp.anchoredPosition = (fromArrowPoint_RT_tp.anchoredPosition + toArrowPoint_RT_tp.anchoredPosition) / 2f;

        //// Calculate the rotation angle based on the direction
        //float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //// Set the rotation of the arrow
        //arrow_RT_tp.rotation = Quaternion.Euler(0f, 0f, rotAngle);
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Events from UI
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region EventsFromUI

    //-------------------------------------------------- On Playerboard
    public void On_Pb_ToBattlePhase()
    {
        mainGameState = GameState_En.PhaseFinished;
    }

    //-------------------------------------------------- On ActionWindow
    public void On_Aw_IncSpMarker()
    {
        localPlayer_Cp.IncSpMarker(selectedRoundIndex);

        strUI_Cp.RefreshAwGuardPanel();
    }

    public void On_Aw_DecSpMarker()
    {
        localPlayer_Cp.DecSpMarker(selectedRoundIndex);

        strUI_Cp.RefreshAwGuardPanel();
    }

    public void On_Aw_ShienUnitSelected(int index)
    {
        shienUnitIndex = index;

        shienUnit_Cp = localPlayer_Cp.mUnit_Cps[index];        
        UnitCardData unitData = dataManager_Cp.GetUnitCardDataFromCardIndex(shienUnit_Cp.cardIndex);

        //
        strUI_Cp.aw_sh_unitText_Cp.text = "ユニット : " + unitData.name;
        strUI_Cp.aw_sh_shienText_Cp.text = "しえん : " + unitData.shienName;
        strUI_Cp.aw_sh_descText_Cp.text = unitData.shienDesc;
    }

    public void On_Aw_ShienUnitReset()
    {
        shienUnitIndex = -1;

        shienUnit_Cp = null;

        //
        strUI_Cp.aw_sh_unitText_Cp.text = "ユニット : " + "None";
        strUI_Cp.aw_sh_shienText_Cp.text = string.Empty;
        strUI_Cp.aw_sh_descText_Cp.text = string.Empty;
    }

    public void On_Aw_ShienTargetVanUnitSelected(int index)
    {
        if (shienTargetVanUnitIndex == index)
        {
            shienTargetVanUnitIndex = -1;
        }
        else
        {
            shienTargetVanUnitIndex = index;
        }

        //
        if (shienTargetVanUnitIndex == 0)
        {
            strUI_Cp.aw_sh_van1Bgd_GO.SetActive(true);
            strUI_Cp.aw_sh_van2Bgd_GO.SetActive(false);
        }
        else if (shienTargetVanUnitIndex == 1)
        {
            strUI_Cp.aw_sh_van1Bgd_GO.SetActive(false);
            strUI_Cp.aw_sh_van2Bgd_GO.SetActive(true);
        }
        else
        {
            strUI_Cp.aw_sh_van1Bgd_GO.SetActive(false);
            strUI_Cp.aw_sh_van2Bgd_GO.SetActive(false);
        }
    }

    public void On_Aw_MoveVanUnitSelected(int index)
    {
        if (moveVanUnitIndex == index)
        {
            moveVanUnitIndex = -1;
        }
        else
        {
            moveVanUnitIndex = index;
        }

        //
        strUI_Cp.aw_mo_van1Bgd_GO.SetActive(false);
        strUI_Cp.aw_mo_van2Bgd_GO.SetActive(false);

        if (moveVanUnitIndex == 0)
        {
            strUI_Cp.aw_mo_van1Bgd_GO.SetActive(true);
        }
        else if (moveVanUnitIndex == 1)
        {
            strUI_Cp.aw_mo_van2Bgd_GO.SetActive(true);
        }

        //
        strUI_Cp.SetAwMoDescription(moveVanUnitIndex, moveRearUnitIndex);
    }

    public void On_Aw_MoveRearUnitSelected(int index)
    {
        if (moveRearUnitIndex == index)
        {
            moveRearUnitIndex = -1;
        }
        else
        {
            moveRearUnitIndex = index;
        }

        //
        strUI_Cp.aw_mo_rear1Bgd_GO.SetActive(false);
        strUI_Cp.aw_mo_rear2Bgd_GO.SetActive(false);
        strUI_Cp.aw_mo_rear3Bgd_GO.SetActive(false);

        if (moveRearUnitIndex == 0)
        {
            strUI_Cp.aw_mo_rear1Bgd_GO.SetActive(true);
        }
        else if (moveRearUnitIndex == 1)
        {
            strUI_Cp.aw_mo_rear2Bgd_GO.SetActive(true);
        }
        else if (moveRearUnitIndex == 2)
        {
            strUI_Cp.aw_mo_rear3Bgd_GO.SetActive(true);
        }

        //
        strUI_Cp.SetAwMoDescription(moveVanUnitIndex, moveRearUnitIndex);
    }

    public void On_Aw_AllyVanUnitSelected(int index)
    {
        if (atkAllyVanUnitIndex == index)
        {
            atkAllyVanUnitIndex = -1;
        }
        else
        {
            atkAllyVanUnitIndex = index;
        }

        //
        strUI_Cp.aw_at_allyVanBgd1_GO.SetActive(false);
        strUI_Cp.aw_at_allyVanBgd2_GO.SetActive(false);

        if (atkAllyVanUnitIndex == 0)
        {
            strUI_Cp.aw_at_allyVanBgd1_GO.SetActive(true);
        }
        else if (atkAllyVanUnitIndex == 1)
        {
            strUI_Cp.aw_at_allyVanBgd2_GO.SetActive(true);
        }

        //
        strUI_Cp.SetAwAtDescription(atkAllyVanUnitIndex, atkEnemyVanUnitIndex);

        //
        if (atkAllyVanUnitIndex != -1)
        {
            int selectedAllyVanUnitCardIndex_tp = localPlayer_Cp.bUnit_Cps[atkAllyVanUnitIndex].cardIndex;
            UnitCardData unitData = dataManager_Cp.GetUnitCardDataFromCardIndex(selectedAllyVanUnitCardIndex_tp);

            strUI_Cp.SetAwAtAtkCondText(false, unitData.normalAP, unitData.special1AP, unitData.special1SP,
                unitData.special2AP, unitData.special2SP);
        }
        else
        {
            strUI_Cp.SetAwAtAtkCondText(true);
        }

        // place arrow image
        SetActionWindowAtkArrow();
    }

    public void On_Aw_EnemyVanUnitSelected(int index)
    {
        if (atkEnemyVanUnitIndex == index)
        {
            atkEnemyVanUnitIndex = -1;
        }
        else
        {
            atkEnemyVanUnitIndex = index;
        }

        //
        strUI_Cp.aw_at_enemyVanBgd1_GO.SetActive(false);
        strUI_Cp.aw_at_enemyVanBgd2_GO.SetActive(false);

        if (atkEnemyVanUnitIndex == 0)
        {
            strUI_Cp.aw_at_enemyVanBgd1_GO.SetActive(true);
        }
        else if (atkEnemyVanUnitIndex == 1)
        {
            strUI_Cp.aw_at_enemyVanBgd2_GO.SetActive(true);
        }

        //
        strUI_Cp.SetAwAtDescription(atkAllyVanUnitIndex, atkEnemyVanUnitIndex);

        // place arrow image
        SetActionWindowAtkArrow();
    }

    public void On_Aw_Update(TokenType tokenType_pr)
    {
        UpdateActionWindow(tokenType_pr);
    }

    #endregion
}
