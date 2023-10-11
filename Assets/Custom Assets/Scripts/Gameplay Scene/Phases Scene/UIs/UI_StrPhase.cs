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

        strController_Cp = controller_Cp.strController_Cp;

        player_Cps = controller_Cp.player_Cps;

        localPlayer_Cp = controller_Cp.localPlayer_Cp;

        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;
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
        UnitUI_Phases[] aw_sh_mUnitUI_Cps_tp = aw_sh_mihariUnitsGroup_GO.GetComponentsInChildren<UnitUI_Phases>();
        for (int i = 0; i < aw_sh_mUnitUI_Cps_tp.Length; i++)
        {
            aw_sh_mUnit_Cps.Add(aw_sh_mUnitUI_Cps_tp[i]);
        }

        //
        UnitUI_Phases[] aw_p1bUnitUI_Cps_tp = aw_p1bUnitsPanel_GO.GetComponentsInChildren<UnitUI_Phases>();
        for (int i = 0; i < aw_p1bUnitUI_Cps_tp.Length; i++)
        {
            aw_p1bUnitUI_Cps.Add(aw_p1bUnitUI_Cps_tp[i]);
        }

        UnitUI_Phases[] aw_p2bUnitUI_Cps_tp = aw_p2bUnitsPanel_GO.GetComponentsInChildren<UnitUI_Phases>();
        for (int i = 0; i < aw_p2bUnitUI_Cps_tp.Length; i++)
        {
            aw_p2bUnitUI_Cps.Add(aw_p2bUnitUI_Cps_tp[i]);
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
        bb_p2UnitUI_Cps = new List<UnitUI_Phases>(bb_p2UnitUIsGroup_Tf.GetComponentsInChildren<UnitUI_Phases>());

        //
        RefreshBattleboardUnits();
    }

    #endregion

    //--------------------------------------------------
    public void RefreshActionWindowShienUnits()
    {
        for (int i = 0; i < aw_sh_mUnit_Cps.Count; i++)
        {
            aw_sh_mUnit_Cps[i].frontSprite = localPlayer_Cp.mUnit_Cps[i].frontSide;
        }
    }

    //--------------------------------------------------
    public void RefreshActionWindowBattleUnits()
    {
        for (int i = 0; i < aw_p1bUnitUI_Cps.Count; i++)
        {
            aw_p1bUnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].frontSide;
        }

        for (int i = 0; i < aw_p2bUnitUI_Cps.Count; i++)
        {
            aw_p2bUnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].frontSide;
        }
    }

    //--------------------------------------------------
    public void RefreshBattleboardUnits()
    {
        for (int i = 0; i < bb_p1UnitUI_Cps.Count; i++)
        {
            bb_p1UnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].frontSide;
        }

        for (int i = 0; i < aw_p2bUnitUI_Cps.Count; i++)
        {
            bb_p1UnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].frontSide;
        }
    }

}
