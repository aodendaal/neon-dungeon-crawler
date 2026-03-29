using UnityEngine;
using Unity.Cinemachine; // This requires the Cinemachine/Unity Camera package
using QFSW.QC;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [Header("Virtual Cameras")]
    [SerializeField] private CinemachineCamera _menuCamera;
    [SerializeField] private CinemachineCamera _playerCamera;

    private void Awake()
    {
        Instance = this;
    }

    [Command("cam-game")]
    public void ShowGame()
    {
        _menuCamera.Priority = 10;
        _playerCamera.Priority = 20; // Higher priority takes control
    }

    [Command("cam-menu")]
    public void ShowMenu()
    {
        _menuCamera.Priority = 20;
        _playerCamera.Priority = 10;
    }

    public void LinkPlayer(Transform playerTransform)
    {
        if (_playerCamera != null)
        {
            _playerCamera.Follow = playerTransform;
            // Position the camera at "eye level" inside the player prefab
            _playerCamera.transform.SetParent(playerTransform);
            _playerCamera.transform.localPosition = new Vector3(0, 0.6f, 0);
            _playerCamera.transform.localRotation = Quaternion.identity;
        }
    }
}