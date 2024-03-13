using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
	public int playerId = 0;
    [SerializeField]private List<CardData> deckCards = new List<CardData>();
	private Queue<CardData> deck = new Queue<CardData>();

	public void ShuffleDeck(bool initial = false)
	{
		if (initial)
		{
			deckCards.Shuffle();
			deck = new Queue<CardData>(deckCards);
			return;
		}

		List<CardData> newDeck = new List<CardData>(deck);
		newDeck.Shuffle();
		deck = new Queue<CardData>(newDeck);	
	}

	public CardData GetCard()
	{
		return deck.Dequeue();
	}



}
