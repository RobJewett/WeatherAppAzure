using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using weatherwebinarService.DataObjects;
using weatherwebinarService.Models;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Azure.Mobile.Server.Authentication;

namespace weatherwebinarService.Controllers
{
    [Authorize]
    public class UserController : TableController<User>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            weatherwebinarContext context = new weatherwebinarContext();
            DomainManager = new EntityDomainManager<User>(context, Request);
        }

        // GET tables/User
        public async Task<IQueryable<User>> GetAllUser()
        {
            var emailAddr = await GetUserInfo();
            return Query().Where(item => item.email == emailAddr.email);
            //return Query();
        }

        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<SingleResult<User>> GetUser(string id)
        {
            var emailAddr = await GetUserInfo();
            var result = Lookup(id).Queryable.Where(item => item.email == emailAddr.email);
            return new SingleResult<User>(result);
            //return Lookup(id);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<User> PatchUser(string id, Delta<User> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/User
        public async Task<IHttpActionResult> PostUser(User item)
        {
            var info = await GetUserInfo();
            item.email = info.email;
            item.firstName = info.firstname;
            item.lastName = info.lastname;
            User current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUser(string id)
        {
             return DeleteAsync(id);
        }

        // GET USER'S EMAIL ADDRESS FROM AZURE AD
        public class UserInfo
        {
            public string email;
            public string firstname;
            public string lastname;
        }
        private async Task<UserInfo> GetUserInfo()
        {
            string provider = ((ClaimsPrincipal)User).FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;
            UserInfo _info = new UserInfo();
            if (provider.Equals("aad"))
            {
                var credentials = await User.GetAppServiceIdentityAsync<AzureActiveDirectoryCredentials>(Request);
                var _email = credentials.UserClaims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value ?? string.Empty;
                _info.email = credentials.UserId;
                _info.firstname = credentials.UserClaims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? string.Empty;
                _info.lastname = credentials.UserClaims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? string.Empty;
            }
            return _info;
        }

    }
}
