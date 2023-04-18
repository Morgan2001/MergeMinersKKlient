using Common.Utils.Repository;

namespace _Proxy.Data
{
    public class AliasRepository : Repository<AliasData>
    {
    }

    public class AliasData : IRepositoryItem
    {
        public string Id { get; }
        public string Alias { get; }

        public AliasData(string id, string alias)
        {
            Id = id;
            Alias = alias;
        }
    }
}