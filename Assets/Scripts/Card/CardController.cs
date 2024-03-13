using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardController : MonoBehaviour
{

	[Header("GeneralOptions")]
	private CardData data = null;
	private bool initialized = false;

	[Header("FlippingOptions")]
	[SerializeField] private bool testing = false;
	[SerializeField] private bool faceUp = true;
	private bool flipping = false;
	private float angle = 0;
	private float desiredAngle = 0;
	[SerializeField] private float flipSpeed = 10f;

	[SerializeField] private GameObject infoCanvas;
	[SerializeField] private Sprite faceImage;
	[SerializeField] private Sprite backImage;



	[Header("RenderOptions")]
	[SerializeField] private TextMeshPro[] textElements; // 0: name, 1: cost, 2: hp, 3: atk, 4: effect
	[SerializeField] private SpriteRenderer[] art; // 0: art, 1, speed bar background
	[SerializeField] private SpriteRenderer[] energyIndicators;
	[SerializeField] private SpriteRenderer[] shardIndicators;
	[SerializeField] private SpriteRenderer[] speedIndicators;
	[SerializeField] private SpriteRenderer frameRenderer;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private SpriteMask spriteMask;

	private void HandleFlip()
	{
		angle = Mathf.Lerp(angle, desiredAngle, flipSpeed * Time.deltaTime);
		if(desiredAngle < 90 && angle < 90 || desiredAngle > 90 && angle > 90)
		{
			spriteRenderer.sprite = faceUp ? faceImage : backImage;
			infoCanvas.SetActive(faceUp);
		}
		if(Approx(angle, desiredAngle))
		{
			angle = desiredAngle;
			flipping = false;
		}
		transform.localRotation = Quaternion.Euler(0, angle, 0);
	}

	private bool Approx(float val, float target, float range = 1f)
	{
		return Mathf.Abs(target - val) < range;
	}

	public bool SetFace(bool _faceUp, bool instant = false)
	{
		if (flipping)
		{
			return false;
		}
		faceUp = _faceUp;
		desiredAngle = faceUp ? 0 : 180;
		flipping = true;
		return true;
	}
	public void SetLayerOrder(int baseLayer)
	{
		spriteRenderer.sortingOrder = baseLayer;
		spriteMask.frontSortingOrder = baseLayer + 2;
		spriteMask.backSortingOrder = baseLayer;
		frameRenderer.sortingOrder = baseLayer + 2;
		foreach(SpriteRenderer sr in art)
		{
			sr.sortingOrder = baseLayer + 1;
		}
		foreach (TextMeshPro tmp in textElements)
		{
			tmp.sortingOrder = baseLayer + 1;
		}
		foreach (SpriteRenderer sr in energyIndicators)
		{
			sr.sortingOrder = baseLayer + 3;
		}
		foreach (SpriteRenderer sr in shardIndicators)
		{
			sr.sortingOrder = baseLayer + 3;
		}
		foreach (SpriteRenderer sr in speedIndicators)
		{
			sr.sortingOrder = baseLayer + 3;
		}
	}
	public void Init(CardData _data)
	{
		data = _data;

		textElements[0].text = data.cardName;
		textElements[1].text = data.cost.ToString();
		textElements[2].text = data.HPText;
		textElements[3].text = data.ATK.ToString();
		textElements[4].text = data.effectText;

		art[0].sprite = data.cardImage;
		art[0].transform.localScale = new Vector3(data.imageScaling.x, data.imageScaling.y, 1);
		art[0].transform.localPosition = new Vector3(data.imagePositioning.x, data.imagePositioning.y, 0);


		for(int i = 0; i < energyIndicators.Length; i++)
		{
			if(data.energy <= i)
			{
				energyIndicators[i].gameObject.SetActive(false);
			}
			
		}
		for(int i = 0; i < shardIndicators.Length; i++)
		{
			if(data.shards <= i)
			{
				shardIndicators[i].gameObject.SetActive(false);
			}
			
		}
		for(int i = 0; i < speedIndicators.Length; i++)
		{
			if(i != data.speed)
			{
				speedIndicators[i].gameObject.SetActive(false);
			}
			
		}
		

		initialized = true;
	}
	private void Start()
	{
		Init(CardDatabase.GetCardByID(Random.Range(0, 4)));
	}
	private void Update()
    {
		if(flipping && angle != desiredAngle)
		{
			HandleFlip();
		}
    }
	private void OnMouseDown()
	{
		if (testing)
		{
			SetFace(!faceUp);
		}
		
	}

}
