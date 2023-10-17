using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage_Gameplay
{

    public UnitCardsData unitCardsData = new UnitCardsData();

    public TakaraCardsData takaraCards = new TakaraCardsData();

    public ItemCardsData itemCards = new ItemCardsData();

}

public class UnitCardsData
{
    public List<UnitCardData> unitCards = new List<UnitCardData>();

    public int playerID = -1;
}

public class UnitCardData
{
    public int index;
    public string name;

    public Sprite frontSide;
    public Sprite backSide;

    public int cost;
    public string attribute;
    public int hp;
    public int atk;
    public int agi;

    public int normalAP;
    public string normalAtk;

    public int special1AP;
    public int special1SP;
    public string specialAtk1;

    public int special2AP;
    public int special2SP;
    public string specialAtk2;

    public string uniqueAbility;

    public string shienName;
    public string shienDesc;
}

public class TakaraCardsData
{
    public List<TakaraCardData> takaraCards = new List<TakaraCardData>();
}

public class TakaraCardData
{

    public int index;
    public string name;

    public Sprite frontSide;
    public Sprite backSide;

    public int sheetsNum;
    public int gold;
    public string effect;

}

public class ItemCardsData
{
    public ItemCardData itemCards = new ItemCardData();
}

public class ItemCardData
{

    public int index;
    public string name;

    public Sprite frontSide;
    public Sprite backSide;

    public string effect;

}

public enum TokenType
{
    Null, Shien, Shien2, Move, Attack, Attack2
}

public class TokenValue
{
    public TokenType type;

    public int count;
}

public class TokenData
{
    public TokenValue usedShienToken = new TokenValue();
    public TokenValue totalShienToken = new TokenValue();
    public TokenValue usedMoveToken = new TokenValue();
    public TokenValue totalMoveToken = new TokenValue();
    public TokenValue usedAtkToken = new TokenValue();
    public TokenValue totalAtkToken = new TokenValue();
}

public enum MarkerType
{
    Null, SP, AP, Gold, Turn
}

public class MarkerValue
{
    public MarkerType type;

    public int count;
}

public class MarkerData
{
    public MarkerValue usedSpMarkers = new MarkerValue();
    public MarkerValue totalSpMarkers = new MarkerValue();
    public MarkerValue usedGoldMarkers = new MarkerValue();
    public MarkerValue totalGoldMarkers = new MarkerValue();
    public MarkerValue apMarkers = new MarkerValue();
    public MarkerValue turnMarkers = new MarkerValue();
}

public class RoundValue
{
    public int index;

    public Transform roundPanel_Tf;

    public Transform allyVan1_Tf, allyVan2_Tf, enemyVan1_Tf, enemyVan2_Tf;

    public Transform markersGroup_Tf;

    public Transform token_Tf;

    public TokenValue token;

    public List<Transform> marker_Tfs = new List<Transform>();

    public int spMarkerCount;
}

