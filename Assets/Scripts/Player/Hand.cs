using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	[SerializeField] private int startSortingLayer = 20;
	[SerializeField] private int steps = 50;
	[SerializeField] private Vector3 handStart;
	[SerializeField] private Vector3 handControlPoint;
	[SerializeField] private Vector3 handEnd;
	private List<Transform> handCards = new List<Transform>();

	private float currentDistance = 0.1f;
	private float maxDistance = 0.1f;
	private float cardSize = 1f;
	private float curveLenght = -1f;
	private float cardMoveSpeed = 5f;
	

	Vector3 GetStart
	{
		get
		{
			return transform.position + handStart;
		}
	}
	Vector3 GetControl
	{
		get
		{
			return transform.position + handControlPoint;
		}
	}
	Vector3 GetEnd
	{
		get
		{
			return transform.position + handEnd;
		}
	}


    void Start()
    {
		curveLenght = CalculateBezierLenght();
		Debug.Log(curveLenght);
		if (transform.childCount <= 0)
		{
			return;
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform card = transform.GetChild(i);
			card.GetComponent<CardController>().SetLayerOrder(startSortingLayer + ((transform.childCount - i) * 4));
			handCards.Add(card);

		}
	}

    void Update()
    {
		AlignHandCards();
    }

	private void AlignHandCards()
	{
		SetCurrentDistance();
		float handSize = HandSize();
		
		float middlepoint = curveLenght / 2f;
		float startingPoint = middlepoint - (handSize / 2);
		for(int i = 0; i < handCards.Count; i++)
		{
			float pos = startingPoint + (i * cardSize) + (i == 0 ? 0f : i*currentDistance);
			float relativePos = pos / curveLenght;
			handCards[i].position = Vector3.Lerp(handCards[i].position, BezierCurve(GetStart, GetControl, GetEnd, relativePos), cardMoveSpeed * Time.deltaTime);
		}
	}
	
	private float HandSize()
	{
		float handLenght = (cardSize * (handCards.Count - 1)) + (currentDistance * (handCards.Count - 1));
		return handLenght;
	}

	float CalculateBezierLenght(int subdivisions = 1000)
	{
		float lenght = 0f;
		for(int i = 0; i <= subdivisions; i++)
		{
			if(i == 0)
			{
				continue;
			}
			float segmentLenght = Vector3.Distance(BezierCurve(GetStart, GetControl, GetEnd, (i-1)/(float)subdivisions), BezierCurve(GetStart, GetControl, GetEnd, i / (float)subdivisions));
			lenght += segmentLenght;
		}
		return lenght;
	}

	Vector3 BezierCurve(Vector3 start, Vector3 controlPoint, Vector3 end, float ratio)
	{
		return Vector3.Lerp(Vector3.Lerp(start, controlPoint, ratio), Vector3.Lerp(controlPoint, end, ratio), ratio);
	}
	void SetCurrentDistance()
	{
		if(handCards.Count <= 6)
		{
			currentDistance = maxDistance;
		}
		if(handCards.Count > 6)
		{
			currentDistance = maxDistance - 0.1f;
		}
		if(handCards.Count > 8)
		{
			currentDistance = maxDistance - 0.2f;
		}
		if (handCards.Count > 10)
		{
			currentDistance = maxDistance - 0.3f;
		}
		if (handCards.Count > 12)
		{
			currentDistance = maxDistance - 0.4f;
		}

	}
	private void OnDrawGizmosSelected()
	{
		for(int i = 0; i <= steps; i++)
		{
			if(i == 0)
			{
				continue;
			}
			Gizmos.color = Color.red;
			float ratioBefore = ((float)i-1f) / (float)steps;
			float ratio = (float)i / (float)steps;
			Gizmos.DrawLine(BezierCurve(GetStart, GetControl, GetEnd, ratioBefore), BezierCurve(GetStart, GetControl, GetEnd, ratio));
		}
	}
}
