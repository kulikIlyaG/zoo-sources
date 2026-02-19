using System;

namespace Zoo.Gameplay.Session
{
    public interface IGameSessionData : IGameSessionDataReadOnly
    {
        void OnKillPredator();
        void OnKillPrey();
    }

    public interface IGameSessionDataReadOnly
    {
        int TotalKilledPreys { get; }
        int TotalKilledPredators { get; }
        
        event Action OnChangedTotalKilledPreys; 
        event Action OnChangedTotalKilledPredators;
    }
    
    internal sealed class GameSessionData : IGameSessionData
    {
        private int _killedPreys;
        private int _killedPredators;
        
        public int TotalKilledPreys => _killedPreys;
        public int TotalKilledPredators => _killedPredators;

        public event Action OnChangedTotalKilledPreys;
        public event Action OnChangedTotalKilledPredators;
        
        public void OnKillPrey()
        {
            _killedPreys++;
            OnChangedTotalKilledPreys?.Invoke();
        }

        public void OnKillPredator()
        {
            _killedPredators++;
            OnChangedTotalKilledPredators?.Invoke();
        }
    }
}