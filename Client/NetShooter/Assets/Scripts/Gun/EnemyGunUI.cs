using UnityEngine;
using UnityEngine.UI;

public class EnemyGunUI : MonoBehaviour
{
    [SerializeField] private Image _image;

    Color[] colors = { Color.black, Color.green, Color.yellow, Color.red };

    public void IndicateGun(int index) => _image.color = colors[index];
}
