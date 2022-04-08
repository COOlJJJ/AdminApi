using AdminApi.Context.DomainModel;
using AdminApi.IService;
using AutoMapper;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminApi.Controllers
{
    [Route("api/User")]
    [ApiController]
    [Authorize(Policy = "admin")]
    public class UserController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IUserRoleService userRoleService;

        public UserController(ILogger logger, IUserService userService, IMapper mapper, IUserRoleService userRoleService)
        {
            this.logger = logger;
            this.userService = userService;
            this.mapper = mapper;
            this.userRoleService = userRoleService;
        }

        [Route("GetUserList")]
        [HttpGet]
        public async Task<MessageModel<UserList_Model>> GetUserList(string query, int pagenum, int pagesize)
        {
            try
            {
                var pagedList = await userService.GetAllAsync(new QueryParameter { PageIndex = pagenum - 1, Search = query, PageSize = pagesize });
                UserList_Model userList_Model = new UserList_Model
                {
                    UserList = mapper.Map<List<User_Model>>(pagedList.Items),
                    Totalpage = pagedList.TotalCount
                };
                foreach (var item in userList_Model.UserList)
                {
                    item.Roles = await userRoleService.GetUserAllRolesName(item.Id);
                }

                return new MessageModel<UserList_Model> { Msg = "获取成功", Status = 200, Response = userList_Model };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<UserList_Model>();
            }
        }


        [Route("AddUser")]
        [HttpPost]
        public async Task<MessageModel<string>> AddUser([FromBody] AddUser_Model user_Model)
        {
            try
            {
                string password = SHA256Encryption.SHA256Hash(user_Model.password);
                bool IsOk = await userService.AddAsync(
                    new User
                    {
                        Account = user_Model.account,
                        Email = user_Model.email,
                        UserName = user_Model.username,
                        UpdateDate = DateTime.Now,
                        PassWord = password,
                        CreateDate = DateTime.Now,
                        ReMark = user_Model.remark,
                        Status = "1"
                    });
                if (IsOk == true)
                    return new MessageModel<string> { Msg = "添加成功", Status = 200, Response = string.Empty };
                return new MessageModel<string> { Msg = "添加失败", Status = 201, Response = string.Empty };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<string>();
            }


        }


        [Route("EditUser")]
        [HttpPost]
        public async Task<MessageModel<string>> EditUser([FromBody] EditUser_Model model)
        {
            try
            {
                bool IsOk = await userService.UpdateAsync(new User { ReMark = model.remark, Id = model.userid });
                if (IsOk == true)
                    return new MessageModel<string> { Msg = "修改成功", Status = 200, Response = string.Empty };
                return new MessageModel<string> { Msg = "修改失败", Status = 201, Response = string.Empty };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<string>();
            }
        }

        [Route("ChangeStatus")]
        [HttpGet]
        public async Task<MessageModel<string>> ChangeStatus(int userid, string status)
        {
            try
            {
                bool IsOk = await userService.UpdateStatusAsync(userid, status);
                if (IsOk == true)
                    return new MessageModel<string> { Msg = "修改成功", Status = 200, Response = string.Empty };
                return new MessageModel<string> { Msg = "修改失败", Status = 201, Response = string.Empty };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<string>();
            }
        }



        [Route("UpdatePassword")]
        [HttpPost]
        public async Task<MessageModel<string>> UpdatePassword(dynamic obj)
        {
            try
            {
                dynamic objdyn = JsonConvert.DeserializeObject(Convert.ToString(obj));
                var userid = Convert.ToInt32(objdyn.userid);
                var password = SHA256Encryption.SHA256Hash(Convert.ToString(objdyn.password));
                bool IsOk = await userService.UpdatePasswordAsync(userid, password);
                if (IsOk == true)
                    return new MessageModel<string> { Msg = "修改成功", Status = 200, Response = string.Empty };
                return new MessageModel<string> { Msg = "修改失败", Status = 201, Response = string.Empty };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<string>();
            }
        }


        [Route("DeleteUser")]
        [HttpGet]
        public async Task<MessageModel<string>> DeleteUser(int userid)
        {
            try
            {
                bool IsOk = await userService.DeleteAsync(userid);
                if (IsOk == true)
                    return new MessageModel<string> { Msg = "删除成功", Status = 200, Response = string.Empty };
                return new MessageModel<string> { Msg = "删除失败", Status = 201, Response = string.Empty };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<string>();
            }
        }


        [Route("GiveRoles")]
        [HttpPost]
        public async Task<MessageModel<string>> GiveRoles(GiveRole_Model giveForm)
        {
            try
            {

                int userid = Convert.ToInt32(giveForm.userid);
                List<string> roles = giveForm.newrole;
                bool IsOk = await userRoleService.GiveRoles(userid, roles);
                if (IsOk == true)
                    return new MessageModel<string> { Msg = "分配角色成功", Status = 200, Response = string.Empty };
                return new MessageModel<string> { Msg = "分配角色失败", Status = 201, Response = string.Empty };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<string>();
            }
        }

    }
}

