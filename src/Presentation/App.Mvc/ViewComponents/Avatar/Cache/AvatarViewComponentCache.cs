using App.Infra.Integration.Redis.Attributes;
using App.Infra.Integration.Redis.Interfaces;

namespace App.Mvc.ViewComponents.Avatar.Cache
{
    [Redis(Name = "avatar_view_component", Database = 3, Expiry = 1)]
    public class AvatarViewComponentCache : ICache
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public bool Notify { get; set; }

        public string Avatar { get; set; }

        public static string Key(long customerId)
            => customerId.ToString();
    }
}
