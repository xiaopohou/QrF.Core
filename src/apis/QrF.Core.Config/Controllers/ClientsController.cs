﻿using Microsoft.AspNetCore.Mvc;
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
        public ClientsController(IClientsBusiness business)
        {
            _business = business;
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