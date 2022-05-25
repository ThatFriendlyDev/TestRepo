using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBallScaler : MonoBehaviour
{
    public float _scaleDecreaseStep;
	public float _minScale;
	[SerializeField]
	private MeshRenderer _ballMeshRenderer;
	[SerializeField]
	private ParticleSystem _burstEffect;
    public void DecreaseScale()
	{
		if (this._ballMeshRenderer.enabled)
		{
			this.transform.localScale -= Vector3.one * this._scaleDecreaseStep;
			this._burstEffect.transform.localScale = this.transform.localScale;
		}
	}

	public void Update()
	{
		if (this.transform.localScale.x < this._minScale)
		{
			this.transform.localScale -= Time.deltaTime * Vector3.one * 0.2f;
			this._burstEffect.transform.localScale = this.transform.localScale;
		}

		if (this.transform.localScale.x < 0.1f)
		{
			_ballMeshRenderer.enabled = false;
		}
	}
}
