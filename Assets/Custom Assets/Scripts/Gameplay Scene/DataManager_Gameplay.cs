using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager_Gameplay : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, WillFinish,
        LoadDBFinished, LoadUnitCardsDBFinished, LoadTakaraCardsDBFinished, LoadItemCardsDBFinished
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- static fields
    public static int maxPartyUnitsCount = 12;

    public static int maxBattleUnitsCount = 5;

    public static int maxVanUnitsCount = 2;

    public static int maxRearUnitsCount = 3;

    public static int maxStandUnitsCount = 7;

    public static List<int> cardLayers = new List<int>() { 6, 7, 8 };

    //-------------------------------------------------- serialize fields
    [SerializeField]
    string unitCardsDBRelativePath = "Data/GameplayData/UnitCards.txt",
        takaraCardsDBRelativePath = "Data/GameplayData/TakaraCards.txt",
        itemCardsDBRelativePath = "Data/GameplayData/ItemCards.txt";

    [SerializeField]
    string unitCardsRSRelativePath = "Sprites/UnitCards",
        takaraCardsRSRelativePath = "Sprites/TakaraCards",
        itemCardsRSRelativePath = "Sprites/ItemCards";

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    public DataStorage_Gameplay dataStorage = new DataStorage_Gameplay();

    [ReadOnly]
    public List<UnitCardsData> playersUnitCardsData = new List<UnitCardsData>();

    [ReadOnly]
    public List<UnitCardsData> playersBattleUnitCardsData = new List<UnitCardsData>();

    [ReadOnly]
    public List<UnitCardsData> playersVanUnitCardsData = new List<UnitCardsData>();

    [ReadOnly]
    public List<UnitCardsData> playersRearUnitCardsData = new List<UnitCardsData>();

    [ReadOnly]
    public List<UnitCardsData> playersStandUnitCardsData = new List<UnitCardsData>();

    //-------------------------------------------------- private fields
    string unitCardsDBPath, takaraCardsDBPath, itemCardsDBPath;

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

    //-------------------------------------------------- private properties

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    // Methods
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
    // ManageGameState
    //////////////////////////////////////////////////////////////////////
    #region ManageGameStates

    //--------------------------------------------------
    public int GetExistGameStatesCount(GameState_En gameState_pr)
    {
        int stateCount = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == gameState_pr)
            {
                stateCount++;
            }
        }

        return stateCount;
    }

    //--------------------------------------------------
    public bool IsExistGameState(GameState_En gameState_pr)
    {
        return GetExistGameStatesCount(gameState_pr) > 0;
    }

    //--------------------------------------------------
    public bool ContainsGameStates(params GameState_En[] gameStates_pr)
    {
        bool result = true;

        for (int i = 0; i < gameStates_pr.Length; i++)
        {
            if (!gameStates.Contains(gameStates_pr[i]))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    void RemoveGameStates(params GameState_En[] gameStates_pr)
    {
        for (int i = 0; i < gameStates_pr.Length; i++)
        {
            gameStates.Remove(gameStates_pr[i]);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        StartCoroutine(CorouInit());
    }

    IEnumerator CorouInit()
    {
        gameStates.Add(GameState_En.Nothing);

        InitDBPaths();

        InitVariables();

        mainGameState = GameState_En.Inited;

        yield return null;
    }

    //--------------------------------------------------
    void InitDBPaths()
    {
        //string appDataPath = Directory.GetParent(Application.dataPath).FullName;
        string appDataPath = Application.dataPath;

        unitCardsDBPath = Path.Combine(appDataPath, unitCardsDBRelativePath);
        takaraCardsDBPath = Path.Combine(appDataPath, takaraCardsDBRelativePath);
        itemCardsDBPath = Path.Combine(appDataPath, itemCardsDBRelativePath);
    }

    //--------------------------------------------------
    void InitVariables()
    {
        StartCoroutine(CorouInitVariables());
    }

    IEnumerator CorouInitVariables()
    {
        //
        playersUnitCardsData.Clear();
        playersBattleUnitCardsData.Clear();
        playersVanUnitCardsData.Clear();
        playersRearUnitCardsData.Clear();
        playersStandUnitCardsData.Clear();

        //
        for (int i = 0; i < 2; i++)
        {
            playersUnitCardsData.Add(new UnitCardsData());

            playersBattleUnitCardsData.Add(new UnitCardsData());

            playersVanUnitCardsData.Add(new UnitCardsData());

            playersRearUnitCardsData.Add(new UnitCardsData());

            playersStandUnitCardsData.Add(new UnitCardsData());
        }

        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Load gameplay data
    //////////////////////////////////////////////////////////////////////
    #region LoadGameplayData

    //--------------------------------------------------
    public void LoadGameplayData()
    {
        StartCoroutine(CorouLoadGameplayData());
    }

    IEnumerator CorouLoadGameplayData()
    {
        LoadUnitCardsData();
        LoadTakaraCardsData();
        LoadItemCardsData();

        yield return new WaitUntil(() => ContainsGameStates(GameState_En.LoadUnitCardsDBFinished,
            GameState_En.LoadTakaraCardsDBFinished, GameState_En.LoadItemCardsDBFinished));

        RemoveGameStates(GameState_En.LoadUnitCardsDBFinished, GameState_En.LoadTakaraCardsDBFinished,
            GameState_En.LoadItemCardsDBFinished);

        mainGameState = GameState_En.LoadDBFinished;
    }

    //--------------------------------------------------
    void LoadUnitCardsData()
    {
        StartCoroutine(CorouLoadUnitCardsData());
    }

    IEnumerator CorouLoadUnitCardsData()
    {
        UnitCardsData unitCardsData = new UnitCardsData();

        //
        string[] dbLines = File.ReadAllLines(unitCardsDBPath);

        //
        for(int i = 0; i < dbLines.Length; i++)
        {
            UnitCardData unitCardData = new UnitCardData();

            //
            dbLines[i] = dbLines[i].Trim();

            string[] dbSections = dbLines[i].Split(';');

            if(dbSections.Length == 0)
            {
                Debug.LogWarning("Invalid UnitCard data has been detected at " + i + " line");
                continue;
            }

            bool dbPartsError = false;

            for (int j = 0; j < dbSections.Length; j++)
            {
                dbSections[j] = dbSections[j].Trim();

                string[] dbParts = dbSections[j].Split(':');
                
                if(dbParts.Length != 2)
                {
                    Debug.LogWarning("Invalid UnitCard data has been detected at " + i + " line "
                        + j + " section");
                    continue;
                }

                for(int k = 0; k < dbParts.Length; k++)
                {
                    dbParts[k] = dbParts[k].Trim();
                    if (string.IsNullOrEmpty(dbParts[k]))
                    {
                        Debug.LogWarning("Invalid UnitCard data has been detected at " + i + " line "
                                + j + " section " + k + " part");
                    }
                }

                //
                switch (dbParts[0])
                {
                    case "index":
                        if (!int.TryParse(dbParts[1], out unitCardData.index))
                        {
                            dbPartsError = true;
                        }
                        break;
                    case "name":
                        unitCardData.name = dbParts[1];
                        break;
                    case "frontSide":
                        if (!LoadSprite(Path.Combine(unitCardsRSRelativePath, dbParts[1]),
                            out unitCardData.frontSide))
                        {
                            dbPartsError = true;
                        }
                        break;
                    case "backSide":
                        if (!LoadSprite(Path.Combine(unitCardsRSRelativePath, dbParts[1]),
                            out unitCardData.backSide))
                        {
                            dbPartsError = true;
                        }
                        break;
                    case "cost":
                        if (!int.TryParse(dbParts[1], out unitCardData.cost))
                        {
                            dbPartsError = true;
                        }
                        break;
                    // *
                    default:
                        //*
                        break;
                }

                if (dbPartsError)
                {
                    Debug.LogWarning("Invalid UnitCard data has been detected at dbParts");
                    break;
                }
            }

            if (!dbPartsError)
            {
                unitCardsData.unitCards.Add(unitCardData);
            }
            else
            {
                dbPartsError = false;
            }
        }

        dataStorage.unitCardsData = unitCardsData;

        gameStates.Add(GameState_En.LoadUnitCardsDBFinished);

        yield return null;
    }

    //--------------------------------------------------
    void LoadTakaraCardsData()
    {
        StartCoroutine(CorouLoadTakaraCardsData());
    }

    IEnumerator CorouLoadTakaraCardsData()
    {
        // *

        gameStates.Add(GameState_En.LoadTakaraCardsDBFinished);

        yield return null;
    }

    //--------------------------------------------------
    void LoadItemCardsData()
    {
        StartCoroutine(CorouLoadItemCardsData());
    }

    IEnumerator CorouLoadItemCardsData()
    {
        // *

        gameStates.Add(GameState_En.LoadItemCardsDBFinished);

        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Generate random UnitCardsData
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region GenerateRandomUnitCardsData

    //--------------------------------------------------
    public void GenerateRandomUnitCardsData_PartyDecision()
    {
        for (int i = 0; i < 2; i++)
        {
            GenerateRandomPlayerUnitCardsData(i);
        }
    }

    //--------------------------------------------------
    public void GenerateRandomUnitCardsData_SetupStand()
    {
        for (int i = 0; i < 2; i++)
        {
            GenerateRandomPlayerUnitCardsData(i);
            GenerateRandomPlayerBattleUnitCardsData(i);
            GenerateRandomPlayerStandUnitCardsData(i);
        }
    }

    //--------------------------------------------------
    void GenerateRandomPlayerUnitCardsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxPartyUnitsCount;
        int otherPlayerID_pr = playerID_pr == 0 ? 1 : 0;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();
        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            int randIndex = Random.Range(0, dataStorage.unitCardsData.unitCards.Count);
            UnitCardData unitCardData_tp = dataStorage.unitCardsData.unitCards[randIndex];

            if (!unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !playersUnitCardsData[otherPlayerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        playersUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    void GenerateRandomPlayerVanUnitCardsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxVanUnitsCount;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();
        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            int randIndex = Random.Range(0, playersUnitCardsData[playerID_pr].unitCards.Count);
            UnitCardData unitCardData_tp = playersUnitCardsData[playerID_pr].unitCards[randIndex];

            if (!unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !playersRearUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !playersBattleUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !playersStandUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        playersVanUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    void GenerateRandomPlayerRearUnitCardsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxRearUnitsCount;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();
        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            int randIndex = Random.Range(0, playersUnitCardsData[playerID_pr].unitCards.Count);
            UnitCardData unitCardData_tp = playersUnitCardsData[playerID_pr].unitCards[randIndex];

            if (!playersVanUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !unitCardsData_tp.unitCards.Contains(unitCardData_tp)
                && !playersBattleUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !playersStandUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        playersRearUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    //--------------------------------------------------
    void GenerateRandomPlayerBattleUnitCardsData(int playerID_pr)
    {
        GenerateRandomPlayerVanUnitCardsData(playerID_pr);
        GenerateRandomPlayerRearUnitCardsData(playerID_pr);

        playersBattleUnitCardsData[playerID_pr].unitCards.AddRange(
            playersVanUnitCardsData[playerID_pr].unitCards);
        playersBattleUnitCardsData[playerID_pr].unitCards.AddRange(
            playersRearUnitCardsData[playerID_pr].unitCards);
    }

    //--------------------------------------------------
    void GenerateRandomPlayerStandUnitCardsData(int playerID_pr)
    {
        int unitsCount_pr = DataManager_Gameplay.maxStandUnitsCount;

        //
        UnitCardsData unitCardsData_tp = new UnitCardsData();
        while (unitCardsData_tp.unitCards.Count < unitsCount_pr)
        {
            int randIndex = Random.Range(0, playersUnitCardsData[playerID_pr].unitCards.Count);
            UnitCardData unitCardData_tp = playersUnitCardsData[playerID_pr].unitCards[randIndex];

            if (!playersVanUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !playersRearUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !playersBattleUnitCardsData[playerID_pr].unitCards.Contains(unitCardData_tp)
                && !unitCardsData_tp.unitCards.Contains(unitCardData_tp))
            {
                unitCardsData_tp.unitCards.Add(unitCardData_tp);
            }
        }
        unitCardsData_tp.playerID = playerID_pr;

        //
        playersStandUnitCardsData[playerID_pr] = unitCardsData_tp;
    }

    #endregion

    //--------------------------------------------------
    public UnitCardData GetUnitCardDataFromCardIndex(int cardIndex_pr)
    {
        List<UnitCardData> unitCardData_Cps_pr = dataStorage.unitCardsData.unitCards;

        return DataManager_Gameplay.GetUnitCardDataFromCardIndex(unitCardData_Cps_pr, cardIndex_pr);
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Static methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region StaticMethods

    //------------------------------
    public static bool IsCard(GameObject target_GO_pr)
    {
        bool result = false;

        List<int> cardLayers = new List<int>() { 6, 7, 8 };

        for (int i = 0; i < cardLayers.Count; i++)
        {
            if (target_GO_pr.layer == cardLayers[i])
            {
                result = true;
                break;
            }
        }

        return result;
    }

    //------------------------------
    public static bool LoadSprite(string path, out Sprite sprite_pr)
    {
        bool result = true;

        sprite_pr = Resources.Load<Sprite>(path);

        if(sprite_pr == null)
        {
            result = false;
        }

        return result;
    }

    //------------------------------
    public static UnitCardData GetUnitCardDataFromCardIndex(List<UnitCardData> unitCardData_Cps_pr,
        int cardIndex_pr)
    {
        UnitCardData result = new UnitCardData();

        for (int i = 0; i < unitCardData_Cps_pr.Count; i++)
        {
            if (unitCardData_Cps_pr[i].index == cardIndex_pr)
            {
                result = unitCardData_Cps_pr[i];
                break;
            }
        }

        return result;
    }

    //------------------------------
    public static UnitCard GetUnitFromCardIndex(List<UnitCard> unit_Cps_pr, int cardIndex_pr)
    {
        UnitCard result = null;

        for (int i = 0; i < unit_Cps_pr.Count; i++)
        {
            if (unit_Cps_pr[i].cardIndex == cardIndex_pr)
            {
                result = unit_Cps_pr[i];
                break;
            }
        }

        return result;
    }

    #endregion

}
