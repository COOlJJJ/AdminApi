using AdminApi.Context.DomainModel;
using AdminApi.IService;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Serilog;
using System;
using System.Collections.Generic;
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
        private readonly IUserRoleService userRoleService;

        public LoginController(ILogger logger, IUserService userService, IUserRoleService userRoleService)
        {
            this.logger = logger;
            this.userService = userService;
            this.userRoleService = userRoleService;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<MessageModel<TokenInfoViewModel>> Login([FromBody] LoginForm loginForm)
        {
            try
            {
                if (string.IsNullOrEmpty(loginForm.username) || string.IsNullOrEmpty(loginForm.password))
                    return new MessageModel<TokenInfoViewModel> { Status = 201, Msg = "用户名或密码不能为空" };

                string Password = SHA256Encryption.SHA256Hash(loginForm.password);

                var user = await userService.GetSingleAsync(loginForm.username, Password);

                if (user == null)
                    return new MessageModel<TokenInfoViewModel> { Status = 201, Msg = "账号或密码错误,请重试!" };

                string roles = await userRoleService.GetUserAllRolesName(user.Id);

                string token = JwtHelper.IssueJwt(new TokenModelJwt
                {
                    Role = roles,
                    ID = user.Id.ToString()
                });
                return new MessageModel<TokenInfoViewModel>
                {
                    Msg = "登录成功",
                    Status = 200,
                    Response = new TokenInfoViewModel
                    {
                        access_token = token,
                        token_type = "Bearer",
                        expires_in = 3600
                    }
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<TokenInfoViewModel>();
            }
        }


        [HttpGet]
        [Route("GetUserInfo")]
        public async Task<MessageModel<UserInfo_Model>> GetUserInfo(string token)
        {
            try
            {
                TokenModelJwt tokenModelJwt = JwtHelper.SerializeJwt(token);
                int userid = Convert.ToInt32(tokenModelJwt.ID);
                User user = await userService.GetSingleAsync(userid);
                List<string> roles = await userRoleService.GetUserAllRolesNameList(userid);
                return new MessageModel<UserInfo_Model> { Msg = "获取成功", Status = 200, Response = new UserInfo_Model { roles = roles, name = user.UserName, userid = user.Id, email = user.Email } };
            }
            catch (Exception ex)
            {

                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<UserInfo_Model>();
            }
        }
    }

    public class LoginForm
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
