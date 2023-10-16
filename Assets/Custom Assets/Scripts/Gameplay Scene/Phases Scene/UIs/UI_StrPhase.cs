using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StrPhase : MonoBehaviour
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
        OnPlayerboardPanel, OnActionWindowPanel, OnMiharidaiPanel, OnBattleboardPanel, OnCardDetailPanel,
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
    public GameObject playerBPanel_GO;

    [SerializeField]
    public GameObject actionWPanel_GO;

    [SerializeField]
    public GameObject miharidaiPanel_GO;

    [SerializeField]
    public GameObject battleBPanel_GO;

    [SerializeField]
    public GameObject cardDetailPanel_GO;

    // playerboard panel
    [SerializeField]
    public Text pb_apText_Cp, pb_spMarkerText_Cp, pb_shienText_Cp, pb_move1Text_Cp, pb_move2Text_Cp,
        pb_move3Text_Cp, pb_atk1Text_Cp, pb_atk2Text_Cp;

    // action window panel
    [SerializeField]
    public GameObject aw_guardPanel_GO, aw_shienPanel_GO, aw_movePanel_GO, aw_atkPanel_GO;

    [SerializeField]
    public GameObject aw_p1bUnitsPanel_GO, aw_p2bUnitsPanel_GO;

    [SerializeField]
    public Text aw_gu_guardDesText_Cp, aw_gu_spMarkerText_Cp;

    [SerializeField]
    public Button aw_gu_incBtn_Cp, aw_gu_decBtn_Cp;

    [SerializeField]
    public Text aw_sh_descText_Cp, aw_sh_unitText_Cp, aw_sh_shienText_Cp;

    [SerializeField]
    public GameObject aw_sh_selectUnitsPanel_GO;

    [SerializeField]
    public GameObject aw_sh_mihariUnitsGroup_GO;

    [SerializeField]
    public GameObject aw_sh_van1Bgd_GO, aw_sh_van2Bgd_GO;

    [SerializeField]
    public Text aw_mo_descText_Cp;

    [SerializeField]
    public GameObject aw_mo_van1Bgd_GO, aw_mo_van2Bgd_GO, aw_mo_rear1Bgd_Cp, aw_mo_rear2Bgd_Cp, aw_mo_rear3Bgd_Cp;

    [SerializeField]
    public RectTransform aw_mo_arrow_RT;

    [SerializeField]
    public Text aw_at_descText_Cp;

    [SerializeField]
    public GameObject aw_at_allyVan1_GO, aw_at_allyVan2_GO, aw_at_enemyVan1_GO, aw_at_enemyVan2_GO;

    [SerializeField]
    public RectTransform aw_at_arrow_RT;

    [SerializeField]
    public Text aw_at_normalAtkText_Cp, aw_at_spc1AtkText_Cp, aw_at_spc2AtkText_Cp;

    // battleboard panel
    [SerializeField]
    public Text bb_p1mihariText_Cp, bb_p1discardpileText_Cp, bb_p1kenText_Cp, bbp1_maText_Cp, bb_p1yumiText_Cp,
        bb_p1fushiText_Cp, bb_p1ryuText_Cp;

    [SerializeField]
    public Text bb_p2mihariText_Cp, bb_p2discardpileText_Cp, bb_p2kenText_Cp, bbp2_maText_Cp, bb_p2yumiText_Cp,
        bb_p2fushiText_Cp, bb_p2ryuText_Cp;

    [SerializeField]
    public Transform bb_p1UnitUIsGroup_Tf, bb_p2UnitUIsGroup_Tf;

    // card details panel
    [SerializeField]
    public Image cd_cardImage_Cp;

    [SerializeField]
    public Text cd_cardNameText_Cp;

    [SerializeField]
    public Text cd_costText_Cp, cd_atrText_Cp, cd_maxHpText_Cp, cd_curHPText_Cp, cd_atkText_Cp, cd_agiText_Cp,
        cd_defText_Cp, cd_accuracyCorrText_Cp, cd_CTCorrText_Cp, cd_normalAtkCorrText_Cp, cd_spcAtkCorrText_Cp,
        cd_dmgCorrText_Cp, cd_indirDmgCorrText_Cp, cd_noiseCorrText_Cp, cd_shienEffectCorrText_Cp,
        cd_diceEffectCorrText_Cp;

    [SerializeField]
    public RectTransform cd_equipItemsContent_RT;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    // action window
    [ReadOnly]
    public List<UnitUI_Phases> aw_sh_mUnit_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<UnitUI_Phases> aw_p1bUnitUI_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<UnitUI_Phases> aw_p2bUnitUI_Cps = new List<UnitUI_Phases>();

    // battlebaord
    [ReadOnly]
    public List<UnitUI_Phases> bb_p1UnitUI_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<UnitUI_Phases> bb_p2UnitUI_Cps = new List<UnitUI_Phases>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    Controller_StrPhase strController_Cp;

    List<Player_Phases> player_Cps = new List<Player_Phases>();

    Player_Phases localPlayer_Cp, otherPlayer_Cp;

    int localPlayerID;

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

        strController_Cp = controller_Cp.strController_Cp;

        player_Cps = controller_Cp.player_Cps;

        localPlayer_Cp = controller_Cp.localPlayer_Cp;

        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;

        localPlayerID = controller_Cp.localPlayerID;
    }

    //--------------------------------------------------
    void InitVariables()
    {
        //
        playerBPanel_GO.SetActive(false);
        actionWPanel_GO.SetActive(false);
        miharidaiPanel_GO.SetActive(false);
        battleBPanel_GO.SetActive(false);
        cardDetailPanel_GO.SetActive(false);

        //
        InitActionWindowPanel();

        InitBattleboardPanel();
    }

    //--------------------------------------------------
    void InitActionWindowPanel()
    {
        //
        aw_guardPanel_GO.SetActive(false);
        aw_shienPanel_GO.SetActive(false);
        aw_movePanel_GO.SetActive(false);
        aw_atkPanel_GO.SetActive(false);

        //
        aw_sh_mUnit_Cps = new List<UnitUI_Phases>(aw_sh_mihariUnitsGroup_GO.GetComponentsInChildren<UnitUI_Phases>());
        for (int i = 0; i < aw_sh_mUnit_Cps.Count; i++)
        {
            int index = i;
            aw_sh_mUnit_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Aw_Sh_ShienUnitBtn(index));
        }

        //
        aw_p1bUnitUI_Cps = new List<UnitUI_Phases>(aw_p1bUnitsPanel_GO.GetComponentsInChildren<UnitUI_Phases>());
        for (int i = 0; i < aw_p1bUnitUI_Cps.Count; i++)
        {
            int index = i;
            aw_p1bUnitUI_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Aw_BUnitUI(0, index));
        }

        aw_p2bUnitUI_Cps = new List<UnitUI_Phases>(aw_p2bUnitsPanel_GO.GetComponentsInChildren<UnitUI_Phases>());
        for (int i = 0; i < aw_p2bUnitUI_Cps.Count; i++)
        {
            int index = i;
            aw_p2bUnitUI_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Aw_BUnitUI(1, index));
        }

        //
        RefreshActionWindowShienUnits();

        RefreshActionWindowBattleUnits();
    }

    //--------------------------------------------------
    void InitBattleboardPanel()
    {
        //
        bb_p1UnitUI_Cps = new List<UnitUI_Phases>(bb_p1UnitUIsGroup_Tf.GetComponentsInChildren<UnitUI_Phases>());
        for (int i = 0; i < bb_p1UnitUI_Cps.Count; i++)
        {
            int index = i;
            bb_p1UnitUI_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Bb_Unit(0, index));
        }

        bb_p2UnitUI_Cps = new List<UnitUI_Phases>(bb_p2UnitUIsGroup_Tf.GetComponentsInChildren<UnitUI_Phases>());
        for (int i = 0; i < bb_p2UnitUI_Cps.Count; i++)
        {
            int index = i;
            bb_p2UnitUI_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Bb_Unit(1, index));
        }

        //
        RefreshBattleboardUnits();
    }

    #endregion

    //--------------------------------------------------
    public void RefreshActionWindowShienUnits()
    {
        for (int i = 0; i < aw_sh_mUnit_Cps.Count; i++)
        {
            aw_sh_mUnit_Cps[i].frontSprite = localPlayer_Cp.mUnit_Cps[i].unitCardData.frontSide;
        }
    }

    //--------------------------------------------------
    public void RefreshActionWindowBattleUnits()
    {
        for (int i = 0; i < aw_p1bUnitUI_Cps.Count; i++)
        {
            if (localPlayerID == 0)
            {
                aw_p1bUnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].unitCardData.frontSide;
            }
            else
            {
                aw_p1bUnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].frontSide;
            }
        }

        for (int i = 0; i < aw_p2bUnitUI_Cps.Count; i++)
        {
            if (localPlayerID == 1)
            {
                aw_p2bUnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].unitCardData.frontSide;
            }
            else
            {
                aw_p2bUnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].frontSide;
            }
        }
    }

    //--------------------------------------------------
    public void RefreshBattleboardUnits()
    {
        for (int i = 0; i < bb_p1UnitUI_Cps.Count; i++)
        {
            if (localPlayerID == 0)
            {
                bb_p1UnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].unitCardData.frontSide;
            }
            else
            {
                bb_p1UnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].frontSide;
            }
        }

        for (int i = 0; i < bb_p2UnitUI_Cps.Count; i++)
        {
            if (localPlayerID == 1)
            {
                bb_p2UnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].unitCardData.frontSide;
            }
            else
            {
                bb_p2UnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].frontSide;
            }
        }
    }

    //--------------------------------------------------
    void SetActivePanel(GameState_En gameState_pr, bool flag)
    {
        switch (gameState_pr)
        {
            case GameState_En.OnPlayerboardPanel:
                playerBPanel_GO.SetActive(flag);
                break;
            case GameState_En.OnActionWindowPanel:
                actionWPanel_GO.SetActive(flag);
                break;
            case GameState_En.OnMiharidaiPanel:
                miharidaiPanel_GO.SetActive(flag);
                break;
            case GameState_En.OnBattleboardPanel:
                battleBPanel_GO.SetActive(flag);
                break;
            case GameState_En.OnCardDetailPanel:
                cardDetailPanel_GO.SetActive(flag);
                break;
        }
    }

    public void MoveToPlayerboard()
    {
        SetActivePanel(mainGameState, false);

        mainGameState = GameState_En.OnPlayerboardPanel;
        SetActivePanel(mainGameState, true);

        //
        strController_Cp.MoveCamToPlayerboard();
    }

    void MoveToActionWindow(int index)
    {
        AddGameStates(GameState_En.OnActionWindowPanel);
        SetActivePanel(GameState_En.OnActionWindowPanel, true);

        //
        DisableActionWindowActionPanels();
        aw_guardPanel_GO.SetActive(true);
    }

    void MoveToMiharidai()
    {
        SetActivePanel(mainGameState, false);

        mainGameState = GameState_En.OnMiharidaiPanel;
        SetActivePanel(mainGameState, true);

        //
        strController_Cp.MoveCamToMiharidai();
    }

    void MoveToBattleboard()
    {
        SetActivePanel(mainGameState, false);

        //
        mainGameState = GameState_En.OnBattleboardPanel;
        SetActivePanel(mainGameState, true);
    }

    void MoveToCardDetail(int playerID_pr, UnitCard unit_Cp_pr)
    {
        //
        if (playerID_pr != localPlayerID && !unit_Cp_pr.placedPosture)
        {
            cd_cardImage_Cp.sprite = unit_Cp_pr.unitCardData.backSide;
        }
        else
        {
            cd_cardImage_Cp.sprite = unit_Cp_pr.unitCardData.frontSide;
        }

        //
        AddGameStates(GameState_En.OnCardDetailPanel);
        SetActivePanel(GameState_En.OnCardDetailPanel, true);
    }

    //--------------------------------------------------
    void DisableActionWindowActionPanels()
    {
        aw_guardPanel_GO.SetActive(false);
        aw_shienPanel_GO.SetActive(false);
        aw_movePanel_GO.SetActive(false);
        aw_atkPanel_GO.SetActive(false);
    }

    //--------------------------------------------------
    public void SetSpMarker(int totalSpCount_pr, int usedSpCount_pr)
    {
        aw_gu_spMarkerText_Cp.text = usedSpCount_pr + "/" + totalSpCount_pr + " 使用";
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// OnEvents
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region OnEvents

    //-------------------------------------------------- playerboard panel
    public void OnPbPanel_Round(int index)
    {
        if (mainGameState != GameState_En.OnPlayerboardPanel)
        {
            return;
        }
        if (ExistAnyGameStates(GameState_En.OnActionWindowPanel, GameState_En.OnCardDetailPanel))
        {
            return;
        }

        MoveToActionWindow(index);
    }

    //-------------------------------------------------- playerboard
    public void On_Pb_ToBattleboard()
    {
        MoveToBattleboard();
    }

    public void On_Pb_ToMiharidai()
    {
        MoveToMiharidai();
    }

    public void On_Pb_ToBattlePhase()
    {
        
    }

    //-------------------------------------------------- action window
    public void On_Aw_Update()
    {
        RemoveGameStates(GameState_En.OnActionWindowPanel);
        SetActivePanel(GameState_En.OnActionWindowPanel, false);
    }

    public void On_Aw_BUnitUI(int playerID_pr, int index_pr)
    {
        //
        UnitCard selectedUnit_Cp = player_Cps[playerID_pr].bUnit_Cps[index_pr];

        //
        MoveToCardDetail(playerID_pr, selectedUnit_Cp);
    }

    public void On_Aw_GuardBtn()
    {
        DisableActionWindowActionPanels();
        aw_guardPanel_GO.SetActive(true);
    }

    public void On_Aw_ShienBtn()
    {
        DisableActionWindowActionPanels();
        aw_shienPanel_GO.SetActive(true);
    }

    public void On_Aw_MoveBtn()
    {
        DisableActionWindowActionPanels();
        aw_movePanel_GO.SetActive(true);
    }

    public void On_Aw_AtkBtn()
    {
        DisableActionWindowActionPanels();
        aw_atkPanel_GO.SetActive(true);
    }

    public void On_Aw_Gu_Inc()
    {
        strController_Cp.On_IncSpMarker();
    }

    public void On_Aw_Gu_Dec()
    {
        strController_Cp.On_DecSpMarker();
    }

    public void On_Aw_Sh_SelectShienUnit()
    {

    }

    public void On_Aw_Sh_Reset()
    {

    }

    public void On_Aw_Sh_Van1()
    {

    }

    public void On_Aw_Sh_Van2()
    {

    }

    public void On_Aw_Sh_ShienUnitBtn(int index)
    {

    }

    public void On_Aw_Mo_Van1()
    {

    }

    public void On_Aw_Mo_Van2()
    {

    }

    public void On_Aw_Mo_Rear1()
    {

    }

    public void On_Aw_Mo_Rear2()
    {

    }

    public void On_Aw_Mo_Rear3()
    {

    }

    public void On_Aw_At_AllyVan1()
    {

    }

    public void On_Aw_At_AllyVan2()
    {

    }

    public void On_Aw_At_EnemyVan1()
    {

    }

    public void On_Aw_At_EnemyVan2()
    {

    }

    //-------------------------------------------------- miharidai
    public void On_Md_ToPlayerboard()
    {
        MoveToPlayerboard();
    }

    public void On_Md_ToBattleboard()
    {
        MoveToBattleboard();
    }

    //-------------------------------------------------- battleboard
    public void On_Bb_Unit(int playerID_pr, int index_pr)
    {
        UnitCard selectedUnit_Cp_tp = player_Cps[playerID_pr].bUnit_Cps[index_pr];

        MoveToCardDetail(playerID_pr, selectedUnit_Cp_tp);
    }

    public void On_Bb_ToMiharidai()
    {
        MoveToMiharidai();
    }

    public void On_Bb_ToPlayerboard()
    {
        MoveToPlayerboard();
    }

    //-------------------------------------------------- card detail
    public void On_Cd_Close()
    {
        RemoveGameStates(GameState_En.OnCardDetailPanel);
        SetActivePanel(GameState_En.OnCardDetailPanel, false);
    }

    #endregion

}
