﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QrF.Core.Config.Dto;
using QrF.Core.Config.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QrF.Core.Config.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("ConfigAPI/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsBusiness _business;
        private readonly IClientGroupBusiness _clientGroupbusiness;
        private readonly IMapper _mapper;
        public ClientsController(IClientsBusiness business, IClientGroupBusiness clientGroupbusiness,
            IMapper mapper)
        {
            _business = business;
            _clientGroupbusiness = clientGroupbusiness;
            _mapper = mapper;
        }
        /// <summary>
        /// 查询分页列表
        /// </summary>
        [HttpGet("GetPageList")]
        public async Task<IActionResult> GetPageListAsync([FromQuery] PageInput input)
        {
            var list = await _business.GetPageList(input);
            return Ok(list);
        }
        /// <summary>
        /// 查询授权组授权客户端
        /// </summary>
        [HttpGet("GetAccGroupList")]
        public async Task<IActionResult> GetAccGroupListAsync([FromQuery] ReRoutePageInput input)
        {
            var list = await _business.GetPageList(input);
            var pers = await _clientGroupbusiness.GetList(input.KeyId);
            foreach (var item in list.Rows)
            {
                item.IsAuth = pers.Count(o => o.Id == item.Id) > 0;
            }
            return Ok(list);
        }
        /// <summary>
        /// 分配客户端
        /// </summary>
        [HttpPost("ToAccClient")]
        public async Task<IActionResult> ToAccClientAsync([FromBody] ToAccGroupInput input)
        {
            var result = await _clientGroupbusiness.ToAccReRouteAsync(input);
            return Ok(new MsgResultDto { Success = result });
        }
        /// <summary>
        /// 编辑
        /// </summary>
        [HttpPost("Edit")]
        public async Task<IActionResult> EditAsync([FromBody] ClientsDto input)
        {
            await _business.EditModel(input);
            return Ok(new MsgResultDto { Success = true });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DelInput input)
        {
            if (input == null || !input.Id.HasValue) throw new Exception("编号不存在");
            await _business.DelModel(input.Id.Value);
            return Ok(new MsgResultDto { Success = true });
        }
    }
}
