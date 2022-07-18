using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	[Space]
	[Header("Camera:")]
	public float shakeMagnetude;
	public float shakeTime;
	Camera mainCamera;

	public static CameraShake Instance;

    private void Awake()
    {
		Instance = this;
    }

    private void Start()
    {
		mainCamera = gameObject.GetComponent<Camera>();
    }

    public void ShakeCamera(float shakeMagnetude, float shakeTime)
	{
		InvokeRepeating("StartCameraShaking", 0f, 0.005f);
		Invoke("StopCameraShaking", shakeTime);
	}

	void StartCameraShaking()
	{
		float cameraShakingOffsetX = Random.value * shakeMagnetude * 2 - shakeMagnetude;
		float cameraShakingOffsetY = Random.value * shakeMagnetude * 2 - shakeMagnetude;
		Vector3 cameraIntermadiatePosition = mainCamera.transform.position;
		cameraIntermadiatePosition.x += cameraShakingOffsetX;
		cameraIntermadiatePosition.y += cameraShakingOffsetY;
		mainCamera.transform.position = cameraIntermadiatePosition;
	}

	void StopCameraShaking()
	{
		CancelInvoke("StartCameraShaking");
	}

}
