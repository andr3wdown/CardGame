using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCard", menuName = "Create Card/new monster card", order = 0)]
[System.Serializable]
public class CardData : ScriptableObject
{
	public string cardName;
	public int cost;
	public int HP;
	public int ATK;
	public string effectText;
	public Sprite cardImage;
	public Vector2 imagePositioning;
	public Vector2 imageScaling;

	public string HPText
	{
		get
		{
			return $"{HP/1000}k";
		}
	}
}
