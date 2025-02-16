using UnityEngine;

public class RTSManager : MonoBehaviour
{
	public static RTSManager instance;

	public RTSUnitManager units;

	[UnityEngine.Serialization.FormerlySerializedAs("audio")]
	public RTSAudio audioManager;

	public const int PlayerCount = 3;

	private void Awake()
	{
		if (instance != null) throw new System.Exception("Multiple RTSManager instances in the scene. You should only have one.");
		instance = this;

		units = new RTSUnitManager();
	}

	void OnDestroy()
	{
		units.OnDestroy();
		instance = null;
	}
}
