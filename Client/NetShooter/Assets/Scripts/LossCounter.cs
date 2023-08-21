using UnityEngine;
using UnityEngine.UI;

public class LossCounter : MonoBehaviour
{
    [SerializeField] private Text _text;

    private int _playerLoss = 0;
    private int _enemyLoss = 0;

    public void SetPlayerLoss(int value) {
        _playerLoss = value;
        UpdateText();
    }

    public void SetEnemyLoss(int value) {
        _enemyLoss = value;
        UpdateText();
    }

    private void UpdateText() => _text.text = $"{_playerLoss} : {_enemyLoss}";
}
