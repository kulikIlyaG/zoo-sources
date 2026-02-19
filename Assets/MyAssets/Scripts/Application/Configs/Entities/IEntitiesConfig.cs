using System.Collections.Generic;

namespace Zoo.Application.Configs
{
    public interface IEntitiesConfig : IConfig
    {
        IReadOnlyDictionary<string, EntityDescription> Collection { get; }
    }
}