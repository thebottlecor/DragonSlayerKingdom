using UnityEngine;
using System.Collections;

public class RTSTimedDestruction : MonoBehaviour
{
	public float time = 1f;

	// Use this for initialization
	IEnumerator Start()
	{
		yield return CoroutineHelper.WaitForSeconds(time);
		GameObject.Destroy(gameObject);
	}
}
