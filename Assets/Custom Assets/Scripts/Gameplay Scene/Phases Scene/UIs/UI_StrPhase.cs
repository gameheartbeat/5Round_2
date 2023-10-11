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
    public GameObject aw_p1UnitsPanel_GO, aw_p2UnitsPanel_GO;

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
    public Text bb_mihariText_Cp, bb_discardpileText_Cp, bb_kenText_Cp, bb_maText_Cp, bb_yumiText_Cp,
        bb_fushiText_Cp, bb_ryuText_Cp;

    [SerializeField]
    public Transform p1UnitUIsGroup_Tf, p2UnitUIsGroup_Tf;

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
    public List<UnitUI_Phases> aw_sh_mihariUnit_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<UnitUI_Phases> aw_sh_p1BattleBUnit_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<UnitUI_Phases> aw_sh_p2BattleBUnit_Cps = new List<UnitUI_Phases>();

    // battlebaord
    [ReadOnly]
    public List<UnitUI_Phases> bb_p1UnitUI_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<UnitUI_Phases> bb_p2UnitUI_Cps = new List<UnitUI_Phases>();

    //-------------------------------------------------- private fields

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

        mainGameState = GameState_En.Inited;
    }

    #endregion

}
