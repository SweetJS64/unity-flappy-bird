namespace Game.Core
{
    public interface IBestScoreService
    {
        int GetBestScore();
        void SetBestScore(int value);
    }
}