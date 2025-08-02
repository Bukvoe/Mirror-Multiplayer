using System.Collections.Generic;
using Game.CodeBase.Character;
using Game.CodeBase.Network;
using TMPro;
using UnityEngine;

namespace Game.CodeBase.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(RectTransform))]
    public class InGameUI : MonoBehaviour
    {
        private readonly Dictionary<Player, TextMeshProUGUI> _playerLabels = new();

        [SerializeField] private RectTransform _canvasRectTransform;
        [SerializeField] private TextMeshProUGUI _labelPrefab;
        [SerializeField] private float _labelVerticalOffset;

        private void Start()
        {
            CustomNetworkManager.Instance.PlayerAdded += OnPlayerAdded;
            CustomNetworkManager.Instance.PlayerRemoved += OnPlayerRemoved;
        }

        private void OnDestroy()
        {
            CustomNetworkManager.Instance.PlayerAdded -= OnPlayerAdded;
            CustomNetworkManager.Instance.PlayerRemoved -= OnPlayerRemoved;
        }

        private void Update()
        {
            foreach (var (player, label) in _playerLabels)
            {
                var worldPosition = player.transform.position + Vector3.up * _labelVerticalOffset;
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, screenPosition, null, out var localPos))
                {
                    label.rectTransform.anchoredPosition = localPos;
                    label.text = player.Nickname;
                }
            }
        }

        private void OnPlayerAdded(Player player)
        {
            var label = Instantiate(_labelPrefab, transform);
            label.rectTransform.SetAsLastSibling();
            label.text = player.Nickname;
            _playerLabels.Add(player, label);
        }

        private void OnPlayerRemoved(Player player)
        {
            if (_playerLabels.TryGetValue(player, out var label))
            {
                Destroy(label.gameObject);
                _playerLabels.Remove(player);
            }
        }
    }
}
