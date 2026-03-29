using UnityEngine;
using UnityEngine.UI;
using QFSW.QC;

public class MinimapDisplay : MonoBehaviour
{
    public static MinimapDisplay Instance { get; private set; }
    [SerializeField] private GameObject _playerIconPrefab;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private Transform _gridParent;

    private Image[,] _tileImages;
    private RectTransform _activePlayerIcon;
    public bool IsInitialized { get; private set; }

    private void Awake() => Instance = this;

    public void Initialize(int w, int h)
    {
        foreach (Transform child in _gridParent) Destroy(child.gameObject);
        _tileImages = new Image[w, h];
        Vector2 size = _tilePrefab.GetComponent<RectTransform>().sizeDelta;

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                var t = Instantiate(_tilePrefab, _gridParent);
                _tileImages[x, y] = t.GetComponent<Image>();
                _tileImages[x, y].GetComponent<RectTransform>().anchoredPosition = new Vector2(x * size.x, y * size.y);
            }

        _activePlayerIcon = Instantiate(_playerIconPrefab, _gridParent).GetComponent<RectTransform>();
        IsInitialized = true;
    }

    public void RefreshMap()
    {
        if (!IsInitialized) return;
        // Logic to color based on IsExplored and set Player Icon position
    }
}