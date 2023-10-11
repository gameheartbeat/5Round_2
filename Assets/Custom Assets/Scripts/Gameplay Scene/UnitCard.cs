using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using cakeslice;
using System.Linq;

public class UnitCard : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Finished,
        ZoomInOut
    }

    public enum UnitPositionType_PartyDecision
    {
        Candidate, Van, Rear
    }

    public enum UnitPositionType_SetupStand
    {
        Candidate, Stand
    }

    public enum UnitPositionType_Phases
    {
        Mihari, Battle,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    GameObject costPanel_GO;

    [SerializeField]
    TextMeshProUGUI costText_Cp;

    [SerializeField]
    MeshRenderer frontSideMeshR_Cp, backSideMeshR_Cp;

    [SerializeField]
    Outline hlEffect_Cp;

    [SerializeField]
    MeshRenderer[] selectableMeshR_Cps;

    [SerializeField]
    Color enableColor, disableColor;

    [SerializeField]
    Sprite emptyFrontSide, emptyBackSide;

    [SerializeField]
    LongPressDetector longPressDetector_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public int playerID;

    [ReadOnly]
    public int cardIndex;

    [ReadOnly]
    public UnitCardData unitCardData;

    // party decision section
    [ReadOnly]
    public UnitPositionType_PartyDecision posType_PartyDecision;

    [ReadOnly]
    public int posIndex_PartyDecision;

    // setup stand section
    [ReadOnly]
    public UnitPositionType_SetupStand posType_SetupStand;

    [ReadOnly]
    public int posIndex_SetupStand;

    // phases scene
    [ReadOnly]
    public UnitPositionType_Phases posType_Phases;

    [ReadOnly]
    public int posIndex_Phases;

    //-------------------------------------------------- private fields
    Vector3 originPosBeforeZoom;

    Vector3 originScaleBeforeZoom;

    Quaternion originRotBeforeZoom;

    bool m_placedRight;

    bool m_visible;

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Properties
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public GameState_En mainGameState
    {
        get { return gameStates[0]; }
        set { gameStates[0] = value; }
    }

    public int cost
    {
        set { costText_Cp.text = value.ToString(); }
    }

    public Sprite frontSide
    {
        get { return placedPosture ? unitCardData.frontSide : unitCardData.backSide; }
        set { frontSideMeshR_Cp.material.mainTexture = value.texture; }
    }

    public Sprite backSide
    {
        get { return placedPosture ? unitCardData.backSide : unitCardData.frontSide; }
        set { backSideMeshR_Cp.material.mainTexture = value.texture; }
    }

    public bool activate
    {
        get
        {
            return activeCollider;
        }
        set
        {
            activeCollider = value;

            activeEnableColor = value;
        }
    }

    public bool activeEnableColor
    {
        set
        {
            if (value)
            {
                for (int i = 0; i < selectableMeshR_Cps.Length; i++)
                {
                    selectableMeshR_Cps[i].material.color = enableColor;
                }
            }
            else
            {
                for (int i = 0; i < selectableMeshR_Cps.Length; i++)
                {
                    selectableMeshR_Cps[i].material.color = disableColor;
                }
            }
        }
    }

    public bool activeCollider
    {
        get { return hlEffect_Cp.GetComponent<Collider>().enabled; }
        set { hlEffect_Cp.GetComponent<Collider>().enabled = value; }
    }

    public bool activeCostPanel
    {
        get { return costPanel_GO.activeInHierarchy; }
        set { costPanel_GO.SetActive(value); }
    }

    public bool enableZoom
    {
        get { return longPressDetector_Cp.enableLongPress; }
        set { longPressDetector_Cp.enableLongPress = value; }
    }

    public bool enableLongPress
    {
        get { return longPressDetector_Cp.enableLongPress; }
        set { longPressDetector_Cp.enableLongPress = value; }
    }

    public bool enableClickDetect
    {
        get { return longPressDetector_Cp.enableClickDetect; }
        set { longPressDetector_Cp.enableClickDetect = value; }
    }

    public bool isHighlighted
    {
        get { return hlEffect_Cp.enabled; }
        set { hlEffect_Cp.enabled = value; }
    }

    public bool placedPosture
    {
        get { return m_placedRight; }
        set
        {
            m_placedRight = value;

            if (value)
            {
                frontSide = unitCardData.frontSide;
                backSide = unitCardData.backSide;
            }
            else
            {
                frontSide = unitCardData.backSide;
                backSide = unitCardData.frontSide;
            }
        }
    }

    public bool visible
    {
        get { return m_visible; }
        set
        {
            m_visible = value;

            for (int i = 0; i < selectableMeshR_Cps.Length; i++)
            {
                selectableMeshR_Cps[i].enabled = value;
            }
        }
    }

    //-------------------------------------------------- private properties
    GameObject controller_GO
    {
        get { return GameObject.FindWithTag("GameController"); }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    // Methods
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //------------------------------ Start is called before the first frame update
    void Start()
    {

    }

    //------------------------------ Update is called once per frame
    void Update()
    {
        if (enableZoom)
        {
            if (gameStates.Contains(GameState_En.ZoomInOut))
            {
                if (Input.GetMouseButtonDown(0) &&
                    !LongPressDetector.CheckObjectLineage(LongPressDetector.GetPointedObject(), transform))
                {
                    OnZoomInEvent();
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////
    // Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //------------------------------
    public void Init()
    {
        gameStates.Add(GameState_En.Nothing);

        InitComponents();

        mainGameState = GameState_En.Inited;
    }

    //------------------------------
    void InitComponents()
    {
        isHighlighted = false;

        InitLongPressDetector();
    }

    //------------------------------
    void InitLongPressDetector()
    {
        longPressDetector_Cp.onLongPress.AddListener(OnLongPressed);

        longPressDetector_Cp.onClicked.AddListener(OnClicked);
    }

    #endregion

    //------------------------------
    public void OnClicked()
    {
        if (gameStates.Contains(GameState_En.ZoomInOut))
        {
            return;
        }

        isHighlighted = !isHighlighted;

        controller_GO.SendMessage("OnUnitCardClicked", this);
    }

    //////////////////////////////////////////////////////////////////////
    // Zoom In/Out
    //////////////////////////////////////////////////////////////////////
    #region ZoomInOut

    //------------------------------
    void OnLongPressed()
    {
        OnZoomOutEvent();
    }

    void OnZoomOutEvent()
    {
        gameStates.Add(GameState_En.ZoomInOut);

        //
        originPosBeforeZoom = transform.position;
        originScaleBeforeZoom = transform.localScale;
        originRotBeforeZoom = transform.rotation;

        //
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnCompleteZoomOut);

        Vector3 newScale = new Vector3(3f, 3f, 3f);
        Quaternion adjustRot_tp = Quaternion.Euler(90f, 0f, 0f);

        controller_GO.SendMessage("ZoomInOutStarted");

        ZoomInOut.ZoomOut(transform, Camera.main, 0.2f, newScale, adjustRot_tp, unityEvent, 0.3f);
    }

    void OnCompleteZoomOut()
    {
        
    }

    void OnZoomInEvent()
    {
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnCompleteZoomIn);

        ZoomInOut.ZoomIn(transform, originPosBeforeZoom, originScaleBeforeZoom, originRotBeforeZoom,
            unityEvent, 0.3f);
    }

    void OnCompleteZoomIn()
    {
        controller_GO.SendMessage("ZoomInOutFinished");

        gameStates.Remove(GameState_En.ZoomInOut);
    }

    #endregion

    //------------------------------
    public void SetAllStates(int cardIndex_pr, Sprite frontSide_pr, Sprite backSide_pr, bool activeCostPanel_pr,
        int cost_pr, bool activeCollider_pr, bool activeEnableColor_pr)
    {
        cardIndex = cardIndex_pr;
        frontSide = frontSide_pr;
        backSide = backSide_pr;
        activeCostPanel = activeCostPanel_pr;
        cost = cost_pr;
        activeCollider = activeCollider;
        activeEnableColor = activeEnableColor_pr;
    }

    //------------------------------
    public void SetAsEmptyState()
    {
        SetAllStates(0, emptyFrontSide, emptyBackSide, false, 0, false, false);
    }

    //------------------------------
    public void SetAsVirtualState()
    {
        transform.localScale = Vector3.zero;
    }

    //------------------------------
    public void SetUnitDataFromUnitCardData(UnitCardData unitData_pr)
    {
        SetAllStates(unitData_pr.index, unitData_pr.frontSide, unitData_pr.backSide,
            true, unitData_pr.cost, true, true);
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Phases scene
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region PhasesScene

    //--------------------------------------------------
    public void SetStatus_Phases(int playerID_pr,
        UnitCardData unitCardData_pr, bool placedRight_pr = true, bool visible_pr = true)
    {
        playerID = playerID_pr;
        frontSide = unitCardData_pr.frontSide;
        backSide = unitCardData_pr.backSide;
        cardIndex = unitCardData_pr.index;
        unitCardData = unitCardData_pr;
        placedPosture = placedRight_pr;
        visible = visible_pr;
    }

    #endregion

}
