using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] private Text _text;
    public void UpdateText(string gunInfo) => _text.text = gunInfo;
}
