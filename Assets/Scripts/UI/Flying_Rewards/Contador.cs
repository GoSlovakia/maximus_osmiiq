using System;
using UnityEngine;
using TMPro;

public class Contador : MonoBehaviour
{
    [SerializeField]
    private int currentScore;
    [SerializeField]
    private int currentTargetScore;
    [SerializeField]
    private int scoreIncrement;
    [SerializeField]
    private TextMeshProUGUI text1;

    public bool contatings;

    private void Update()
    {
        if (contatings && currentScore<currentTargetScore)
        {
            currentScore += scoreIncrement;
            text1.text = currentScore.ToString();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        contatings = true;
        Invoke("StopContatings", 2f);
    }

    void StopContatings()
    {
        contatings = false;
    }
}
