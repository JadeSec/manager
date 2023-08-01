using App.Infra.CrossCutting.Extensions;
using App.Infra.Integration.Redis;
using App.Mvc.ViewComponents.Avatar.Cache;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Mvc.ViewComponents.Avatar
{
    public class AvatarViewComponent : ViewComponent
    {
        //readonly Account _account;
        readonly RedisService _redis;

        public AvatarViewComponent(
            //Account account,
            RedisService redisService)
        {            
            //_account = account;            
            _redis = redisService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Delay(100);

            return View(); 
            //var customerId = Request.HttpContext.User.Sid<long>();

            //var cache = _redis.Get<AvatarViewComponentCache>(AvatarViewComponentCache.Key(customerId));
            //if (cache != null)
            //    return View(cache);

            //var data = new AvatarViewComponentCache();

            //data.Notify = await _account.CheckForNewNotificationsAsync(
            //   customerId: customerId
            //);

            //var customer = await _account.InfoAsync(customerId);

            //data.Avatar = customer.Selfie;
            //data.Name = customer.Name;
            //data.Surname = customer.Surname;

            //_redis.Set(
            //    key: AvatarViewComponentCache.Key(customerId),
            //    data: data
            //);

            //return View(data);
        }
    }   
}
