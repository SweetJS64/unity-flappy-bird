using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class BestScoreService : IBestScoreService
    {
        private const string BestScoreKey = "best_score";

        public int GetBestScore() => PlayerPrefs.GetInt(BestScoreKey, 0);

        public void SetBestScore(int value)
        {
            PlayerPrefs.SetInt(BestScoreKey, value);
            PlayerPrefs.Save();
        }
    }
}