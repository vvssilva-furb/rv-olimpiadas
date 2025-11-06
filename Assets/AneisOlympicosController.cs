using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Rendering.CameraUI;

public class AneisOlimpicosController : MonoBehaviour
{
    public Image anelEuropa;
    public Image anelAsia;
    public Image anelAfrica;
    public Image anelOceania;
    public Image anelAmericas;

    private int coloredCount = 0;

    private readonly Color[] ringColors = new Color[]
    {
        new Color(0f, 0.45f, 0.73f),  // Blue
        new Color(1f, 0.83f, 0f),     // Yellow
        new Color(0f, 0f, 0f),        // Black
        new Color(0f, 0.6f, 0.3f),    // Green
        new Color(0.86f, 0.16f, 0.16f) // Red
    };

    void Start()
    {
        ResetRings();
    }

    public void ColorRing(int idx)
    {
        var ring = anelEuropa;

        switch (idx)
        {
            case 1:
                ring = anelAsia;
                break;

            case 2:
                ring = anelAfrica;
                break;

            case 3:
                ring = anelOceania;
                break;

            case 4:
                ring = anelAmericas;
                break;
        }

        if (ring.color != Color.black)
        {
            ring.color = ringColors[idx];
            coloredCount++;
        }
    }

    public void ResetRings()
    {
        coloredCount = 0;

        anelEuropa.color = Color.black;
        anelAsia.color = Color.black;
        anelAfrica.color = Color.black;
        anelOceania.color = Color.black;
        anelAmericas.color = Color.black;
    }
}
