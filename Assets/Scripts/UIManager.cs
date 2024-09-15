using UnityEngine;
using TMPro;  // Assure-toi que cette directive est incluse pour utiliser TextMeshPro

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // Référence au texte pour le score
    public TextMeshProUGUI collectibleText;  // Référence au texte pour les collectibles

    private float score = 0;  // Variable pour stocker le score
    private int collectibles = 0;  // Variable pour stocker les collectibles

    public void UpdateScore(float value)
    {
        score = value;
        scoreText.text = "Score : " + score;
    }

    public void UpdateCollectibles()
    {
        collectibles++;
        collectibleText.text = "Collectibles : " + collectibles;
    }
}
