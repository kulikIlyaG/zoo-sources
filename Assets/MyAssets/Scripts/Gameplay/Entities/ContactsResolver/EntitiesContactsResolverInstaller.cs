using VContainer;

namespace Zoo.Gameplay.Entities
{
    public static class EntitiesContactsResolverInstaller
    {
        private static void BuildActions(IContainerBuilder builder)
        {
            builder.Register<ContactActionPushBothEntities>(Lifetime.Singleton).As<ContactAction>();
            builder.Register<ContactActionKillEntity>(Lifetime.Singleton).As<ContactAction>();
        }
        
        public static void Install(IContainerBuilder builder)
        {
            BuildActions(builder);

            builder.Register<EntitiesContactsResolver>(Lifetime.Singleton).As<IEntitiesContactsResolver>();
        }
    }
}
