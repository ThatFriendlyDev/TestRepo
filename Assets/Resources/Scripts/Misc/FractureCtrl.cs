using DG.Tweening;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FractureCtrl : MonoBehaviour
{
	[MinMaxSlider(100, 300)]
	public Vector2 fractureForceMinMax;

	[SerializeField]
	private Transform _fracturesHolder;
	[SerializeField]
	private List<Transform> fracturePieces;
	[SerializeField]
	private MMFeedbacks onFractureFeedback;

	private void Start()
	{
		this.fracturePieces = new List<Transform>();
		
		foreach (Transform element in this._fracturesHolder)
		{
			element.localScale = new  Vector3(Random.Range(0.93f, 1f), Random.Range(0.93f, 1f), Random.Range(0.93f, 1f));
			this.fracturePieces.Add(element);
		}

		this.fracturePieces = this.fracturePieces.OrderBy(p => p.transform.position.y).Reverse().ToList();
	}

	public void Fracture()
	{
		StartCoroutine(FractureCo());
	}

	private IEnumerator FractureCo()
	{
		List<Transform> randomElementsSelection = new List<Transform>();

		int totalAmountOfPieces = this.fracturePieces.Count;
		int pieceToFractureCount = Random.Range(2, 5);

		if (pieceToFractureCount > totalAmountOfPieces)
			yield break;

		for (int i = 0; i < pieceToFractureCount; i++)
		{
			Transform randomPieceToFracture = this.fracturePieces[i];
			randomElementsSelection.Add(randomPieceToFracture);
			this.fracturePieces.Remove(randomPieceToFracture);
			totalAmountOfPieces--;
			yield return null;
		}

		foreach (Transform fracturedPiece in randomElementsSelection)
		{
			var fracturedPieceRb = fracturedPiece.gameObject.AddComponent<Rigidbody>();
			fracturedPieceRb.AddForce(-Vector3.forward * Random.Range(fractureForceMinMax.x, fractureForceMinMax.y));
			fracturedPieceRb.transform.DOScale(Vector3.zero, Random.Range(1.5f, 2.5f)).OnComplete(() => fracturedPieceRb.gameObject.SetActive(false));
			yield return null;
		}

		onFractureFeedback.PlayFeedbacks();
	}

	public void FractureExplode()
	{
		StartCoroutine(FractureExplodeCo());
	}

	private IEnumerator FractureExplodeCo()
	{

		List<Transform> randomElementsSelection = new List<Transform>();

		int totalAmountOfPieces = this.fracturePieces.Count;
		for (int i = 0; i < totalAmountOfPieces; i++)
		{
			var fracturedPieceRb = fracturePieces[i].gameObject.AddComponent<Rigidbody>();
			fracturedPieceRb.AddExplosionForce(10 * Random.Range(fractureForceMinMax.x, fractureForceMinMax.y), transform.position + Vector3.forward * 2 - Vector3.up * 5, 10);
			//fracturedPieceRb.gameObject.AddComponent<BoxCollider>();
			fracturedPieceRb.transform.DOScale(Vector3.zero, Random.Range(1.5f, 2.5f)).OnComplete(() => fracturedPieceRb.gameObject.SetActive(false));
		}
		yield return null;
	}
}
