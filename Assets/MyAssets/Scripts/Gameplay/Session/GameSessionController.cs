using System;
using Zoo.Gameplay.Entities;

namespace Zoo.Gameplay.Session
{
    public interface IGameSessionController : IGameSessionControllerReadOnly
    {
        void StartGame();
        void KillEntity(IEntity killer, IEntity victim);
    }

    public interface IGameSessionControllerReadOnly
    {
        event Action<IEntity, IEntity> OnEntityKilled;
    }

    internal sealed class GameSessionController : IGameSessionController
    {
        private readonly IEntitiesSpawnSystem _spawnSystem;
        private readonly IEntitiesController _entitiesController;
        
        private readonly IGameSessionData _gameSessionData;

        public GameSessionController(IEntitiesSpawnSystem spawnSystem, IEntitiesController entitiesController, IGameSessionData gameSessionData)
        {
            _spawnSystem = spawnSystem;
            _entitiesController = entitiesController;
            _gameSessionData = gameSessionData;
        }

        public void StartGame()
        {
            _spawnSystem.StartSpawning();
        }

        public void KillEntity(IEntity killer, IEntity victim)
        {
            switch (victim.Type)
            {
                case Predator:
                    _gameSessionData.OnKillPredator();
                    break;
                case Prey:
                    _gameSessionData.OnKillPrey();
                    break;
            }
            
            OnEntityKilled?.Invoke(killer, victim);
            
            _entitiesController.EntityEndLife(victim);
        }

        public event Action<IEntity, IEntity> OnEntityKilled;
    }
}