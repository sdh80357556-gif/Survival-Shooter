// 파일명: UIManager.cs
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [field: SerializeField] public Text WaveInfoText { get; private set; }
    [field: SerializeField] public Text ScoreText { get; private set; }
    [field: SerializeField] public GameObject GameOverUI { get; private set; }

    private void OnEnable() => SetActiveGameOverUI(false);

    
    public void SetScore(int score)
    {
        if (ScoreText != null) ScoreText.text = $"Score: {score}";
    }

    public void SetActiveGameOverUI(bool active)
    {
        if (GameOverUI != null) GameOverUI.SetActive(active);
    }

  
    public void SetWaveText(int wave, int count)
    {
        if (WaveInfoText == null) return;
        StringBuilder sb = new StringBuilder();
        sb.Append("Wave: ").AppendLine(wave.ToString());
        sb.Append("Enemy Left: ").Append(count);
        WaveInfoText.text = sb.ToString();
    }
}