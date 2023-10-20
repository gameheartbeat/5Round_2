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
    public Button aw_guardBtn_Cp, aw_shienBtn_Cp, aw_moveBtn_Cp, aw_atkBtn_Cp;

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
    public GameObject aw_mo_van1Bgd_GO, aw_mo_van2Bgd_GO, aw_mo_rear1Bgd_GO, aw_mo_rear2Bgd_GO, aw_mo_rear3Bgd_GO;

    [SerializeField]
    public RectTransform aw_mo_arrow_RT;

    [SerializeField]
    public Text aw_at_descText_Cp;

    [SerializeField]
    public GameObject aw_at_allyVanBgd1_GO, aw_at_allyVanBgd2_GO, aw_at_enemyVanBgd1_GO, aw_at_enemyVanBgd2_GO;

    [SerializeField]
    public RectTransform aw_at_allyVan1ArrowPoint_RT, aw_at_allyVan2ArrowPoint_RT, aw_at_enemyVan1ArrowPoint_RT,
        aw_at_enemyVan2ArrorPoint_RT;

    [SerializeField]
    public RectTransform aw_at_arrow_RT;

    [SerializeField]
    public Text aw_at_normalAtkText_Cp, aw_at_spc1AtkText_Cp, aw_at_spc2AtkText_Cp;

    // battleboard panel
    [SerializeField]
    public Text bb_p1mihariText_Cp, bb_p1discardUnitText_Cp, bb_p1kenText_Cp, bbp1_maText_Cp, bb_p1yumiText_Cp,
        bb_p1fushiText_Cp, bb_p1ryuText_Cp;

    [SerializeField]
    public Text bb_p2mihariText_Cp, bb_p2discardUnitText_Cp, bb_p2kenText_Cp, bbp2_maText_Cp, bb_p2yumiText_Cp,
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
        cd_defCorrText_Cp, cd_accuracyCorrText_Cp, cd_CTCorrText_Cp, cd_normalAtkCorrText_Cp, cd_spcAtkCorrText_Cp,
        cd_dmgCorrText_Cp, cd_indirDmgCorrText_Cp, cd_noiseCorrText_Cp, cd_shienEffectCorrText_Cp,
        cd_diceEffectCorrText_Cp;

    [SerializeField]
    public RectTransform cd_equipItemsContent_RT;

    [SerializeField]
    GameObject cd_item_Pf;

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

    [ReadOnly]
    public List<GameObject> cd_equipItem_GOs = new List<GameObject>();

    public float equipItemInterval = 0.1f;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    Controller_StrPhase strController_Cp;

    DataManager_Gameplay dataManager_Cp;

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

        dataManager_Cp = controller_Cp.dataManager_Cp;

        player_Cps = controller_Cp.player_Cps;

        localPlayer_Cp = controller_Cp.localPlayer_Cp;

        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;

        localPlayerID = controller_Cp.localPlayerID;
    }

    //--------------------------------------------------
    void InitVariables()
    {
        //
        DisableAllPanel();

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

        // init guard panel
        RefreshAwGuardPanel();

        //
        aw_sh_mUnit_Cps = new List<UnitUI_Phases>(aw_sh_mihariUnitsGroup_GO.GetComponentsInChildren<UnitUI_Phases>());
        for (int i = 0; i < aw_sh_mUnit_Cps.Count; i++)
        {
            int index = i;
            aw_sh_mUnit_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Aw_Sh_ShienUnitBtn(index));
        }

        RefreshAwShienPanel();

        //
        RefreshAwMovePanel();

        //
        RefreshAwAtkPanel();

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
        RefreshAwMihariUnits();

        RefreshAwBattleUnits();
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
        RefreshBattleInfoPanel();

        RefreshBattleboardUnits();
    }

    #endregion

    //-------------------------------------------------- Disable all UI panel
    public void DisableAllPanel()
    {
        playerBPanel_GO.SetActive(false);
        actionWPanel_GO.SetActive(false);
        miharidaiPanel_GO.SetActive(false);
        battleBPanel_GO.SetActive(false);
        cardDetailPanel_GO.SetActive(false);
    }

    //-------------------------------------------------- Reset action window UI panel
    public void RefreshAwShienPanel()
    {
        RoundValue roundValue = localPlayer_Cp.roundsData[strController_Cp.selectedRoundIndex];

        // check token count
        aw_shienBtn_Cp.interactable = true;
        if (localPlayer_Cp.tokensData.usedShienToken.count == localPlayer_Cp.tokensData.totalShienToken.count)
        {
            if (localPlayer_Cp.roundsData[strController_Cp.selectedRoundIndex].token.type != TokenType.Shien)
            {
                aw_shienBtn_Cp.interactable = false;
            }
        }

        //
        if (roundValue.token.type != TokenType.Shien)
        {
            aw_sh_descText_Cp.text = string.Empty;
            aw_sh_unitText_Cp.text = "ユニット : None";
            aw_sh_shienText_Cp.text = string.Empty;

            aw_sh_van1Bgd_GO.SetActive(false);
            aw_sh_van2Bgd_GO.SetActive(false);
        }
        else
        {
            UnitCardData unitData = roundValue.shienUnit_Cp.unitCardData;
            aw_sh_descText_Cp.text = unitData.shienDesc;
            aw_sh_unitText_Cp.text = "ユニット : " + unitData.name;
            aw_sh_shienText_Cp.text = "しえん : " + unitData.shienName;

            if (roundValue.targetUnitIndex == 0)
            {
                aw_sh_van1Bgd_GO.SetActive(true);
                aw_sh_van2Bgd_GO.SetActive(false);
            }
            else if (roundValue.targetUnitIndex == 1)
            {
                aw_sh_van1Bgd_GO.SetActive(false);
                aw_sh_van2Bgd_GO.SetActive(true);
            }
        }
    }

    public void RefreshAwMovePanel()
    {
        RoundValue roundValue = localPlayer_Cp.roundsData[strController_Cp.selectedRoundIndex];

        // check token count
        if (localPlayer_Cp.tokensData.usedMoveToken.count == localPlayer_Cp.tokensData.totalMoveToken.count)
        {
            aw_moveBtn_Cp.interactable = false;
        }
        else
        {
            aw_moveBtn_Cp.interactable = true;
        }

        //
        if (roundValue.token.type != TokenType.Move)
        {
            aw_mo_descText_Cp.text = string.Empty;

            aw_mo_van1Bgd_GO.SetActive(false);
            aw_mo_van2Bgd_GO.SetActive(false);
            aw_mo_rear1Bgd_GO.SetActive(false);
            aw_mo_rear2Bgd_GO.SetActive(false);
            aw_mo_rear3Bgd_GO.SetActive(false);

            aw_mo_arrow_RT.gameObject.SetActive(false);
        }
        else
        {
            aw_mo_descText_Cp.text = roundValue.targetUnitIndex == 0 ? "赤" : "青" + "が\r\n"
                + "後衛" + (roundValue.originUnitIndex + 1).ToString() + "と入替";

            aw_mo_van1Bgd_GO.SetActive(false);
            aw_mo_van2Bgd_GO.SetActive(false);
            if (roundValue.targetUnitIndex == 0)
            {
                aw_mo_van1Bgd_GO.SetActive(true);
            }
            else if (roundValue.targetUnitIndex == 1)
            {
                aw_mo_van2Bgd_GO.SetActive(true);
            }

            aw_mo_rear1Bgd_GO.SetActive(false);
            aw_mo_rear2Bgd_GO.SetActive(false);
            aw_mo_rear3Bgd_GO.SetActive(false);
            if (roundValue.originUnitIndex == 0)
            {
                aw_mo_rear1Bgd_GO.SetActive(true);
            }
            else if (roundValue.originUnitIndex == 1)
            {
                aw_mo_rear2Bgd_GO.SetActive(true);
            }
            else if (roundValue.originUnitIndex == 2)
            {
                aw_mo_rear3Bgd_GO.SetActive(true);
            }

            aw_mo_arrow_RT.gameObject.SetActive(false); // it should be fixed
        }
    }

    public void RefreshAwAtkPanel()
    {
        RoundValue roundValue = localPlayer_Cp.roundsData[strController_Cp.selectedRoundIndex];

        // check token count
        if (localPlayer_Cp.tokensData.usedAtkToken.count == localPlayer_Cp.tokensData.totalAtkToken.count)
        {
            aw_atkBtn_Cp.interactable = false;
        }
        else
        {
            aw_atkBtn_Cp.interactable = true;
        }

        //
        if (roundValue.token.type != TokenType.Attack)
        {
            aw_at_descText_Cp.text = string.Empty;

            aw_at_allyVanBgd1_GO.SetActive(false);
            aw_at_allyVanBgd2_GO.SetActive(false);
            aw_at_enemyVanBgd1_GO.SetActive(false);
            aw_at_enemyVanBgd2_GO.SetActive(false);
            aw_at_arrow_RT.gameObject.SetActive(false);

            aw_at_normalAtkText_Cp.text = "通常\r\n";
            aw_at_spc1AtkText_Cp.text = "特殊1\r\n";
            aw_at_spc2AtkText_Cp.text = "特殊2\r\n";
        }
        else
        {
            aw_at_descText_Cp.text = (roundValue.originUnitIndex == 0 ? "赤" : "紫") + "が"
                + (roundValue.targetUnitIndex == 0 ? "青" : "緑") + "へ攻撃";

            aw_at_allyVanBgd1_GO.SetActive(false);
            aw_at_allyVanBgd2_GO.SetActive(false);
            switch (roundValue.originUnitIndex)
            {
                case 0:
                    aw_at_allyVanBgd1_GO.SetActive(true);
                    break;
                case 1:
                    aw_at_allyVanBgd2_GO.SetActive(true);
                    break;
            }

            aw_at_enemyVanBgd1_GO.SetActive(false);
            aw_at_enemyVanBgd2_GO.SetActive(false);
            switch (roundValue.targetUnitIndex)
            {
                case 0:
                    aw_at_enemyVanBgd1_GO.SetActive(true);
                    break;
                case 1:
                    aw_at_enemyVanBgd2_GO.SetActive(true);
                    break;
            }

            aw_at_arrow_RT.gameObject.SetActive(false); // it will be fixed
        }
    }

    //-------------------------------------------------- Refresh action window
    public void RefreshAwGuardPanel()
    {
        //
        aw_gu_guardDesText_Cp.text = "ラウンド中\r\n前衛DEF+" + localPlayer_Cp.markersData.usedSpMarkers.count;

        //
        aw_gu_spMarkerText_Cp.text = localPlayer_Cp.markersData.usedSpMarkers.count
            + "/" + localPlayer_Cp.markersData.totalSpMarkers.count + " 使用";

        // set active inc/dec button
        if (localPlayer_Cp.markersData.usedSpMarkers.count == 0)
        {
            aw_gu_decBtn_Cp.interactable = false;
        }
        else
        {
            aw_gu_decBtn_Cp.interactable = true;
        }

        if (localPlayer_Cp.roundsData[strController_Cp.selectedRoundIndex].spMarkerCount == 0)
        {
            aw_gu_decBtn_Cp.interactable = false;
        }

        if (localPlayer_Cp.markersData.usedSpMarkers.count == localPlayer_Cp.markersData.totalSpMarkers.count)
        {
            aw_gu_incBtn_Cp.interactable = false;
        }
        else
        {
            aw_gu_incBtn_Cp.interactable = true;
        }
    }

    public void RefreshAwMihariUnits()
    {
        //
        for (int i = 0; i < aw_sh_mUnit_Cps.Count; i++)
        {
            aw_sh_mUnit_Cps[i].gameObject.SetActive(true);

            if (localPlayer_Cp.mUnit_Cps[i] != null)
            {
                aw_sh_mUnit_Cps[i].frontSprite = localPlayer_Cp.mUnit_Cps[i].unitCardData.frontSide;
            }
        }

        //
        List<RoundValue> roundsData_tp = localPlayer_Cp.roundsData;
        for (int i = 0; i < roundsData_tp.Count; i++)
        {
            if (roundsData_tp[i].token.type == TokenType.Shien)
            {
                aw_sh_mUnit_Cps[roundsData_tp[i].originUnitIndex].gameObject.SetActive(false);
            }
        }
    }

    public void RefreshAwBattleUnits()
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
    public void RefreshBattleInfoPanel()
    {
        //
        bb_p1mihariText_Cp.text = "みはり : " + localPlayer_Cp.battleInfo.mihariUnitCount.ToString();
        bb_p1discardUnitText_Cp.text = "捨て札 : " + localPlayer_Cp.battleInfo.discardUnitCount.ToString();

        //
        bb_p2mihariText_Cp.text = "みはり : " + otherPlayer_Cp.battleInfo.mihariUnitCount.ToString();
        bb_p2discardUnitText_Cp.text = "捨て札 : " + otherPlayer_Cp.battleInfo.discardUnitCount.ToString();
    }

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
    void RefreshCardDetail(int playerID_pr, UnitCard unit_Cp_pr)
    {
        // set unit details
        RefreshUnitDetail(playerID_pr, unit_Cp_pr);

        // set equip item details
        RefreshEquipItems(unit_Cp_pr);
    }

    void RefreshUnitDetail(int playerID_pr, UnitCard unit_Cp_pr)
    {
        UnitCardData unitData = unit_Cp_pr.unitCardData;

        // set unit details
        if (playerID_pr != localPlayerID && !unit_Cp_pr.placedPosture)
        {
            cd_cardImage_Cp.sprite = unitData.backSide;
        }
        else
        {
            cd_cardImage_Cp.sprite = unitData.frontSide;
        }

        cd_cardNameText_Cp.text = unitData.name;
        cd_costText_Cp.text = "コスト : " + unitData.cost;
        cd_atrText_Cp.text = "属性 : " + unit_Cp_pr.atr;
        cd_maxHpText_Cp.text = "最大HP : " + unit_Cp_pr.maxHP;
        cd_curHPText_Cp.text = "現HP : " + unit_Cp_pr.curHP;
        cd_atkText_Cp.text = "ATK : " + unitData.atk + "+" + unit_Cp_pr.atkCorr;
        cd_agiText_Cp.text = "AGI : " + unitData.agi + "+" + unit_Cp_pr.agiCorr;
        cd_defCorrText_Cp.text = "DEF補正 : " + unit_Cp_pr.defCorr;
        cd_accuracyCorrText_Cp.text = "命中補正 : " + unit_Cp_pr.accuracyCorr;
        cd_CTCorrText_Cp.text = "CT値補正 : " + unit_Cp_pr.ctCorr;
        cd_normalAtkCorrText_Cp.text = "通常攻撃補正 : " + unit_Cp_pr.normalAtkCorr;
        cd_spcAtkCorrText_Cp.text = "特殊攻撃補正 : " + unit_Cp_pr.spcAtkCorr;
        cd_dmgCorrText_Cp.text = "ダメ補正 : " + unit_Cp_pr.dmgCorr;
        cd_indirDmgCorrText_Cp.text = "間接ダメ補正 : " + unit_Cp_pr.indirDmgCorr;
        cd_noiseCorrText_Cp.text = "ごえい : " + unit_Cp_pr.noise;
        cd_shienEffectCorrText_Cp.text = "しえん効果補正 : " + unit_Cp_pr.shienEffectCorr;
        cd_diceEffectCorrText_Cp.text = "ダイス 効果補正 : " + unit_Cp_pr.diceEffectCorr;
    }

    void RefreshEquipItems(UnitCard unit_Cp_pr)
    {
        // clear old equip item datas
        for (int i = 0; i < cd_equipItem_GOs.Count; i++)
        {
            Destroy(cd_equipItem_GOs[i]);
        }
        cd_equipItem_GOs.Clear();

        // instant new equip items
        for (int i = 0; i < unit_Cp_pr.equipItems.Count; i++)
        {
            ItemCardData itemData = unit_Cp_pr.equipItems[i];
            GameObject item_GO_tp = Instantiate(cd_item_Pf, cd_equipItemsContent_RT);
            cd_equipItem_GOs.Add(item_GO_tp);
            item_GO_tp.GetComponent<ItemCard>().itemData = itemData;
        }

        // Initialize the initial X position to the left edge of the parent container
        cd_equipItemsContent_RT.sizeDelta = new Vector2(equipItemInterval * (cd_equipItem_GOs.Count + 1)
            + cd_item_Pf.GetComponent<RectTransform>().sizeDelta.x * cd_equipItem_GOs.Count,
            cd_equipItemsContent_RT.sizeDelta.y);

        float currentXPosition = equipItemInterval;
        for (int i = 0; i < cd_equipItem_GOs.Count; i++)
        {
            RectTransform item_RT_tp = cd_equipItem_GOs[i].GetComponent<RectTransform>();

            // Set the position of the item from left to right
            item_RT_tp.anchoredPosition = new Vector2(currentXPosition, item_RT_tp.anchoredPosition.y);

            // Update the currentXPosition for the next item
            currentXPosition += item_RT_tp.sizeDelta.x + equipItemInterval;
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

    public void MoveToActionWindow(int index)
    {
        AddGameStates(GameState_En.OnActionWindowPanel);

        //
        DisableActionWindowActionPanels();
        aw_guardPanel_GO.SetActive(true);

        //
        RefreshAwGuardPanel();
        RefreshAwShienPanel();
        RefreshAwMovePanel();
        RefreshAwAtkPanel();

        //
        SetActivePanel(GameState_En.OnActionWindowPanel, true);
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
        RefreshCardDetail(playerID_pr, unit_Cp_pr);

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
    public void SetAwMoDescription(int moveVanUnitIndex_pr, int moveRearUnitIndex_pr)
    {
        if (moveVanUnitIndex_pr == -1 || moveRearUnitIndex_pr == -1)
        {
            aw_mo_descText_Cp.text = string.Empty;
        }
        else
        {
            aw_mo_descText_Cp.text = "" + moveVanUnitIndex_pr.ToString() + "" + "\r\n"
                + "" + moveRearUnitIndex_pr.ToString() + "";
        }
    }

    //--------------------------------------------------
    public void SetAwAtDescription(int atkAllyIndex_pr, int atkEnemyIndex_pr)
    {
        if (atkAllyIndex_pr == -1 || atkEnemyIndex_pr == -1)
        {
            aw_at_descText_Cp.text = string.Empty;
        }
        else
        {
            aw_at_descText_Cp.text = "" + (atkAllyIndex_pr == 0 ? "" : "") + ""
                + (atkEnemyIndex_pr == 0 ? "" : "");
        }
    }

    //--------------------------------------------------
    public void SetAwAtAtkCondText(bool isEmpty = false, int normalAp = 0, int spc1Ap = 0, int spc1Sp = 0,
        int spc2Ap = 0, int spc2Sp = 0)
    {
        if (isEmpty)
        {
            aw_at_normalAtkText_Cp.text = "通常\r\n";
            aw_at_spc1AtkText_Cp.text = "特殊1\r\n";
            aw_at_spc2AtkText_Cp.text = "特殊2\r\n";
        }
        else
        {
            aw_at_normalAtkText_Cp.text = "通常\r\n" + "AP" + normalAp.ToString() + " SP0";
            aw_at_spc1AtkText_Cp.text = "特殊1\r\n" + "AP" + spc1Ap.ToString() + " SP"
                + spc1Sp.ToString();
            aw_at_spc2AtkText_Cp.text = "特殊2\r\n" + "AP" + spc2Ap.ToString() + " SP"
                + spc2Sp.ToString();
        }
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// OnEvents
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region OnEvents

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
        strController_Cp.On_Pb_ToBattlePhase();
    }

    //-------------------------------------------------- action window
    public void On_Aw_Update()
    {
        TokenType selectedTokenType_tp = TokenType.Null;
        if (aw_shienPanel_GO.activeSelf)
        {
            selectedTokenType_tp = TokenType.Shien;
        }
        if (aw_movePanel_GO.activeSelf)
        {
            selectedTokenType_tp = TokenType.Move;
        }
        if (aw_atkPanel_GO.activeSelf)
        {
            selectedTokenType_tp = TokenType.Attack;
        }

        strController_Cp.On_Aw_Update(selectedTokenType_tp);

        //
        RemoveGameStates(GameState_En.OnActionWindowPanel);
        SetActivePanel(GameState_En.OnActionWindowPanel, false);
    }

    public void On_Aw_BUnitUI(int playerID_pr, int index_pr)
    {
        //
        UnitCard selectedUnit_Cp = player_Cps[playerID_pr].bUnit_Cps[index_pr];

        // check valid to show
        if (playerID_pr != localPlayerID && !selectedUnit_Cp.placedPosture)
        {
            return;
        }

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
        strController_Cp.On_Aw_IncSpMarker();
    }

    public void On_Aw_Gu_Dec()
    {
        strController_Cp.On_Aw_DecSpMarker();
    }

    public void On_Aw_Sh_SelectShienUnit()
    {
        aw_sh_selectUnitsPanel_GO.SetActive(true);
    }

    public void On_Aw_Sh_Reset()
    {
        strController_Cp.On_Aw_ShienUnitReset();
    }

    public void On_Aw_Sh_Van1()
    {
        strController_Cp.On_Aw_ShienTargetVanUnitSelected(0);
    }

    public void On_Aw_Sh_Van2()
    {
        strController_Cp.On_Aw_ShienTargetVanUnitSelected(1);
    }

    public void On_Aw_Sh_ShienUnitBtn(int index)
    {
        aw_sh_selectUnitsPanel_GO.SetActive(false);

        //
        strController_Cp.On_Aw_ShienUnitSelected(index);
    }

    public void On_Aw_Mo_Van1()
    {
        strController_Cp.On_Aw_MoveVanUnitSelected(0);
    }

    public void On_Aw_Mo_Van2()
    {
        strController_Cp.On_Aw_MoveVanUnitSelected(1);
    }

    public void On_Aw_Mo_Rear1()
    {
        strController_Cp.On_Aw_MoveRearUnitSelected(0);
    }

    public void On_Aw_Mo_Rear2()
    {
        strController_Cp.On_Aw_MoveRearUnitSelected(1);
    }

    public void On_Aw_Mo_Rear3()
    {
        strController_Cp.On_Aw_MoveRearUnitSelected(2);
    }

    public void On_Aw_At_AllyVan1()
    {
        strController_Cp.On_Aw_AllyVanUnitSelected(0);
    }

    public void On_Aw_At_AllyVan2()
    {
        strController_Cp.On_Aw_AllyVanUnitSelected(1);
    }

    public void On_Aw_At_EnemyVan1()
    {
        strController_Cp.On_Aw_EnemyVanUnitSelected(0);
    }

    public void On_Aw_At_EnemyVan2()
    {
        strController_Cp.On_Aw_EnemyVanUnitSelected(1);
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

        // check valid to show
        if (playerID_pr != localPlayerID && !selectedUnit_Cp_tp.placedPosture)
        {
            return;
        }

        //
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
