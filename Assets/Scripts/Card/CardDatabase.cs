using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
	public CardData[] cards;
	static CardDatabase instance;


	public static CardData GetCardByID(int id)
	{
		if (id > instance.cards.Length)
		{
			Debug.LogError("id is out of card database range");
		}
		return instance.cards[id];
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if(instance != this)
		{
			Destroy(gameObject);
		}
	}
}
