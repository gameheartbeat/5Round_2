using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusVariables
{
    public static string[] factionNames = new string[2] { "赤＆紫陣営", "青＆緑陣営" };

    public static string[] opponentReadyStateTexts = new string[2] { "準備中", "準備完了" };
}

public class StatusManager : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, WillFinish
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
    StatusUI statusUI;

    [SerializeField]
    int maxLeftTime = 100;

    [SerializeField]
    List<string> m_instructionTexts = new List<string>();

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    // components
    GameObject controller_GO;

    // normal fields
    [SerializeField]
    [ReadOnly]
    int m_localFactionID;

    [SerializeField]
    [ReadOnly]
    int m_leftTime;

    [SerializeField]
    [ReadOnly]
    int m_turnIndex;

    [SerializeField]
    [ReadOnly]
    int m_attackPoint;

    [SerializeField]
    [ReadOnly]
    int m_gold;

    [SerializeField]
    [ReadOnly]
    bool m_opponentReadyState;

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

    public int localFactionID
    {
        get { return m_localFactionID; }
        set
        {
            m_localFactionID = value;

            if (value < 0)
            {
                statusUI.factionName = string.Empty;
            }
            else
            {
                statusUI.factionName = StatusVariables.factionNames[value];
            }
        }
    }

    public int leftTime
    {
        get { return m_leftTime; }
        set
        {
            m_leftTime = value;

            if (value < 0)
            {
                statusUI.leftTime = string.Empty;
            }
            else
            {
                statusUI.leftTime = value.ToString() + "秒";
            }
        }
    }

    public int turnIndex
    {
        get { return m_turnIndex; }
        set
        {
            m_turnIndex = value;

            if (value < 0)
            {
                statusUI.turnIndex = string.Empty;
            }
            else
            {
                statusUI.turnIndex = value.ToString();
            }
        }
    }

    public int attackPoint
    {
        get { return m_attackPoint; }
        set
        {
            m_attackPoint = value;

            if (value < 0)
            {
                statusUI.attackPoint = string.Empty;
            }
            else
            {
                statusUI.attackPoint = value.ToString();
            }
        }
    }

    public int gold
    {
        get { return m_gold; }
        set
        {
            m_gold = value;

            if (value < 0)
            {
                statusUI.gold = string.Empty;
            }
            else
            {
                statusUI.gold = value.ToString();
            }
        }
    }

    public bool opponentReadyState
    {
        get { return m_opponentReadyState; }
        set
        {
            m_opponentReadyState = value;
            
            statusUI.opponentReadyState = StatusVariables.opponentReadyStateTexts[value ? 1 : 0];
        }
    }

    public string instruction
    {
        set
        {
            statusUI.instruction = value;
        }
    }

    //-------------------------------------------------- private properties
    string dateTime
    {
        get { return DateTime.Now.ToString("HH:mm"); }
    }

    int battery
    {
        get { return (int)(SystemInfo.batteryLevel * 100f); }
    }

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
    public int GetExistGameStatesCount(GameState_En gameState_pr)
    {
        int result = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == gameState_pr)
            {
                result++;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public bool IsExistGameState(GameState_En gameState_pr)
    {
        return GetExistGameStatesCount(gameState_pr) > 0;
    }

    //--------------------------------------------------
    public void RemoveGameStates(GameState_En gameState_pr)
    {
        while (gameStates.Contains(gameState_pr))
        {
            gameStates.Remove(gameState_pr);
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
        gameStates.Add(GameState_En.Nothing);

        SetComponents();

        InitVariables();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_GO = GameObject.FindWithTag("GameController");
    }

    //--------------------------------------------------
    void InitVariables()
    {
        SetDateTimeUI();

        SetBatteryUI();

        leftTime = maxLeftTime;

        turnIndex = 0;

        attackPoint = 0;

        gold = 0;

        SetInstruction();
    }

    #endregion

    //--------------------------------------------------
    public void Play()
    {
        mainGameState = GameState_En.Playing;

        //
        StartDateTimeCounting();

        if (battery >= 0)
        {
            StartBatteryCounting();
        }

        StartLeftTimeCounting();
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Counting status by time
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region StatusCounting

    //--------------------------------------------------
    void StartDateTimeCounting()
    {
        StartCoroutine(CorouStartDateTimeCounting());
    }

    IEnumerator CorouStartDateTimeCounting()
    {
        while (mainGameState == GameState_En.Playing)
        {
            SetDateTimeUI();

            yield return new WaitForSeconds(1f);
        }
    }

    //--------------------------------------------------
    void StartBatteryCounting()
    {
        StartCoroutine(CorouStartBatteryCounting());
    }

    IEnumerator CorouStartBatteryCounting()
    {
        while (mainGameState == GameState_En.Playing)
        {
            SetBatteryUI();

            yield return new WaitForSeconds(1f);
        }
    }

    //--------------------------------------------------
    public void StartLeftTimeCounting()
    {
        StartCoroutine(CorouStartLeftTimeCounting());
    }

    IEnumerator CorouStartLeftTimeCounting()
    {
        while (leftTime > 0)
        {
            leftTime = leftTime;
            yield return new WaitForSeconds(1f);
            leftTime--;
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Set status UI
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region SetStatusUI

    //--------------------------------------------------
    void SetDateTimeUI()
    {
        statusUI.dateTime = dateTime;
    }

    //--------------------------------------------------
    void SetBatteryUI()
    {
        if (battery < 0)
        {
            statusUI.battery = "なし";
        }
        else
        {
            statusUI.battery = battery.ToString() + "%";
        }
    }

    //--------------------------------------------------
    void SetInstruction()
    {
        string instruction_tp = string.Empty;

        for (int i = 0; i < m_instructionTexts.Count; i++)
        {
            instruction_tp += (m_instructionTexts[i]
                + (i == (m_instructionTexts.Count - 1) ? string.Empty : "\n"));
        }

        instruction = instruction_tp;
    }

    #endregion


}
