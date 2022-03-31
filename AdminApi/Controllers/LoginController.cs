using AdminApi.IService;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AdminApi.Controllers
{

    [Route("api/Login")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        public LoginController(ILogger logger, IUserService userService, IRoleService roleService)
        {
            this.logger = logger;
            this.userService = userService;
            this.roleService = roleService;
        }


        [HttpGet]
        [Route("LoginAsync")]
        public async Task<MessageModel<TokenInfoViewModel>> LoginAsync(string Account, string Password)
        {
            try
            {
                if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Password))
                    return new MessageModel<TokenInfoViewModel> { Status = 201, Msg = "用户名或密码不能为空" };

                Password = SHA256Encryption.SHA256Hash(Password);

                var model = await userService.GetSingleAsync(Account, Password);

                if (model == null)
                    return new MessageModel<TokenInfoViewModel> { Status = 201, Msg = "账号或密码错误,请重试!" };

                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<TokenInfoViewModel>();
            }
        }
    }
}
