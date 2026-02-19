using System;
using VContainer;

namespace Zoo.Gameplay.Entities.Behaviours
{
    public interface ISimpleBehaviour
    {
        void Initialize(IObjectResolver resolver);
        void OnSleepRequested(){}
        void OnSleepFinished(){}
        void Tick();
        void DeInitialize();
    }

    [Serializable]
    public abstract class SimpleBehaviourType
    {
        public abstract ISimpleBehaviour CreateBehaviour(IEntity root);
    }

    public interface ISimpleBehaviourParameters {}
    
    public abstract class SimpleBehaviour<TParameters>: ISimpleBehaviour
        where TParameters : class, ISimpleBehaviourParameters
    {
        protected readonly IEntity _root;
        
        private IObjectResolver _resolver;

        protected SimpleBehaviour(IEntity root)
        {
            _root = root;
        }

        protected IObjectResolver Resolver => _resolver;

        public void Initialize(IObjectResolver resolver)
        {
            _resolver = resolver;
            ResolveReferences(_resolver);
            InitializeProcess();
        }

        protected virtual void InitializeProcess(){}
        protected virtual void ResolveReferences(IObjectResolver resolver){}
        public virtual void OnSleepRequested(){}
        public virtual void OnSleepFinished(){}
        public abstract void Tick();

        public virtual void DeInitialize(){}
    }
}