using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardFlip : MonoBehaviour
{
	[SerializeField] private bool testing = false;
	[SerializeField] private bool faceUp = true;
	private bool flipping = false;
	private float angle = 0;
	private float desiredAngle = 0;
	[SerializeField] private float flipSpeed = 10f;

	[SerializeField] private GameObject infoCanvas;
	[SerializeField] private Sprite faceImage;
	[SerializeField] private Sprite backImage;
	[SerializeField] private float cameraAngle = 55f;


	[SerializeField] private GameObject frameObject;
	[SerializeField] private GameObject[] imageObjects;
	[SerializeField] private GameObject[] textObjects;
	[SerializeField] private SpriteMask sm;
	[SerializeField] private GameObject[] indicators;

	private SpriteRenderer frameRenderer;
	private SpriteRenderer[] spriteRenderers;
	private SpriteRenderer[] indicatorRenderers;
	private TextMeshPro[] textMeshes;


	private SpriteRenderer sr;
    
	private Vector3 ConvertVector(Vector3 from)
	{
		from.x += cameraAngle;
		if (from.x > 180)
		{
			from.x = 0 - (360 - from.x);
		}
		if(from.y > 180)
		{
			from.y = 0 - (360 - from.y);
		}
		
		return from;
	}

	private void HandleFlip()
	{
		angle = Mathf.Lerp(angle, desiredAngle, flipSpeed * Time.deltaTime);
		if(desiredAngle < 90 && angle < 90 || desiredAngle > 90 && angle > 90)
		{
			sr.sprite = faceUp ? faceImage : backImage;
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

	public bool SetFace(bool _faceUp)
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
		sr.sortingOrder = baseLayer;
		sm.frontSortingOrder = baseLayer + 2;
		sm.backSortingOrder = baseLayer;
		frameRenderer.sortingOrder = baseLayer + 2;
		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			spriteRenderers[i].sortingOrder = baseLayer + 1;
		}
		for (int i = 0; i < textMeshes.Length; i++)
		{
			textMeshes[i].sortingOrder = baseLayer + 1;
		}
		for(int i = 0; i < indicators.Length; i++)
		{
			indicatorRenderers[i].sortingOrder = baseLayer + 3;
		}
		
	}
	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		spriteRenderers = new SpriteRenderer[imageObjects.Length];
		textMeshes = new TextMeshPro[textObjects.Length];
		indicatorRenderers = new SpriteRenderer[indicators.Length];
		frameRenderer = frameObject.GetComponent<SpriteRenderer>();
		for(int i = 0; i < imageObjects.Length; i++)
		{
			spriteRenderers[i] = imageObjects[i].GetComponent<SpriteRenderer>();
		}
		for(int i = 0; i < textObjects.Length; i++)
		{
			textMeshes[i] = textObjects[i].GetComponent<TextMeshPro>();
		}
		for(int i = 0; i < indicators.Length; i++)
		{
			indicatorRenderers[i] = indicators[i].GetComponent<SpriteRenderer>();
		}
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
