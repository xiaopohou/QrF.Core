﻿using QrF.Core.Admin.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QrF.Core.Admin.Interfaces
{
    public interface IRoleBusiness
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<BasePageQueryOutput<QueryRoleDto>> GetPageList(QueryRolesInput input);
        /// <summary>
        /// 编辑信息
        /// </summary>
        Task EditModel(RoleDto input);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DelModel(Guid input);
    }
}
