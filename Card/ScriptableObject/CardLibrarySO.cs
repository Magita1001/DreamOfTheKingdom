using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardLibrary", menuName = "Card/CardLibrarySO")] 
public class CardLibrarySO : ScriptableObject
{
    public List<CardLibrarySOEntry> cardLibraryList;

}

[Serializable]
public struct CardLibrarySOEntry
{
    public CardDataSO cardData;
    public int amount;
}
