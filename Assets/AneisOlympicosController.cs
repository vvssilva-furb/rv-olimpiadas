using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class AneisOlimpicosController : NetworkBehaviour
{
    public static AneisOlimpicosController Instance;

    private void Awake() => Instance = this;
    public enum Ring
    {
        Europa = 0,
        Asia = 1,
        Africa = 2,
        Oceania = 3,
        Americas = 4
    }

    public Image anelEuropa;
    public Image anelAsia;
    public Image anelAfrica;
    public Image anelOceania;
    public Image anelAmericas;

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

    [ClientRpc]
    public void ColorRingClientRpc(Ring ring)
    {
        ColorRing(ring);  // your existing function
    }
    public void ColorRing(Ring idx)
    {
        if (!NetworkManager.Singleton.IsClient)
            return;

        Image ring = anelEuropa;

        switch (idx)
        {
            case Ring.Asia:
                ring = anelAsia;
                break;

            case Ring.Africa:
                ring = anelAfrica;
                break;

            case Ring.Oceania:
                ring = anelOceania;
                break;

            case Ring.Americas:
                ring = anelAmericas;
                break;
        }

        Debug.Log($"Coloring ring {ring}");
        ring.color = ringColors[(int)idx];
    }

    public void ResetRings()
    {
        if (!NetworkManager.Singleton.IsClient)
            return;

        anelEuropa.color = Color.white;
        anelAsia.color = Color.white;
        anelAfrica.color = Color.white;
        anelOceania.color = Color.white;
        anelAmericas.color = Color.white;
    }
}
