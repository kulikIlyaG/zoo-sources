namespace Zoo.Application.Keys
{
    public static class AssetsPaths
    {
        private const string ENTITIES_PATH_FORMAT = "Gameplay/Entities/Entity_{0}.prefab";
        
        public static string GetPathForEntity(string entityId)
        {
            return string.Format(ENTITIES_PATH_FORMAT, entityId);
        }
    }
}