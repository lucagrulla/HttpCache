using System;
using System.Web;
using System.Web.Mvc;

namespace HTTP.Attributes
{
   public sealed class CacheFilterAttribute : ActionFilterAttribute
   {
       private static readonly TimeSpan OneDay = new TimeSpan(1, 0, 0);
       private static readonly TimeSpan Zero = new TimeSpan(0, 0, 0);
       private TimeSpan duration = Zero;

       public CacheFilterAttribute(int seconds)
       {
           duration = TimeSpan.FromSeconds(seconds);
       }

       public override void OnActionExecuted(ActionExecutedContext filterContext)
       {
           var cache = filterContext.HttpContext.Response.Cache;

           if (duration.CompareTo(Zero) <= 0)
           {
               cache.SetCacheability(HttpCacheability.NoCache);
               cache.SetNoStore();
               cache.AppendCacheExtension("must-revalidate");
               cache.SetExpires(DateTime.Now.Subtract(OneDay));
           }
           else
           {
               cache.SetCacheability(HttpCacheability.Public);
               cache.SetExpires(DateTime.Now.Add(duration));
               cache.SetMaxAge(duration);
               cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
           }


       }
   }
}

