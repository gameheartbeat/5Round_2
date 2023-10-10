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

    public string shien;

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