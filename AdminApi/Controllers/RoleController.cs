using AdminApi.Context.DomainModel;
using AdminApi.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "admin")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IRoleService roleService;
        private readonly IMapper mapper;

        public RoleController(ILogger logger, IRoleService roleService, IMapper mapper)
        {
            this.logger = logger;
            this.roleService = roleService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<MessageModel<List<KeyValue_Model>>> GetAllRoles()
        {
            try
            {
                List<Role> roleList = await roleService.GetAllRolesAsync();
                List<KeyValue_Model> roles = new List<KeyValue_Model>();
                foreach (var item in roleList)
                {
                    roles.Add(new KeyValue_Model { Label = item.Name, Value = item.Name });
                }
                return new MessageModel<List<KeyValue_Model>> { Msg = "获取成功", Status = 200, Response = roles };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<List<KeyValue_Model>>();
            }
        }


        [HttpGet]
        [Route("GetRoles")]
        public async Task<MessageModel<List<RoleInfo_Model>>> GetRoles()
        {
            try
            {
                List<Role> roleList = await roleService.GetAllRolesAsync();
                return new MessageModel<List<RoleInfo_Model>> { Msg = "获取成功", Status = 200, Response = mapper.Map<List<RoleInfo_Model>>(roleList) };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<List<RoleInfo_Model>>();
            }
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<MessageModel<string>> AddRole([FromBody] AddRole_Model addRole_Model)
        {
            try
            {
                bool IsOK = await roleService.AddAsync(new Role { Name = addRole_Model.Name, Description = addRole_Model.Description, CreateDate = DateTime.Now, UpdateDate = DateTime.Now });
                if (IsOK == true)
                {
                    return new MessageModel<string> { Msg = "添加成功", Status = 200, Response = "添加成功" };
                }
                return new MessageModel<string> { Msg = "添加失败", Status = 200, Response = "添加失败" };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<string>();
            }
        }


        [HttpPost]
        [Route("UpdateRole")]
        public async Task<MessageModel<string>> UpdateRole([FromBody] EidtRole_Model editRole_Model)
        {
            try
            {
                bool IsOK = await roleService.UpdateAsync(new Role { Id = editRole_Model.Id, UpdateDate = DateTime.Now, Name = editRole_Model.Name, Description = editRole_Model.Description });
                if (IsOK == true)
                {
                    return new MessageModel<string> { Msg = "修改成功", Status = 200, Response = "添加成功" };
                }
                return new MessageModel<string> { Msg = "修改失败", Status = 200, Response = "添加失败" };
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message.ToString());
                return new MessageModel<string>();
            }
        }


        [Route("DeleteRole")]
        [HttpGet]
        public async Task<MessageModel<string>> DeleteRole(int id)
        {
            try
            {
                bool IsOk = await roleService.DeleteAsync(id);
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


    }
}
