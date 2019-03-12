﻿using AutoMapper;
using QrF.Core.Admin.Domain;
using QrF.Core.Admin.Dto;
using QrF.Core.Admin.Infrastructure.DbContext;
using QrF.Core.Admin.Interfaces;
using QrF.Core.Utils.Extension;
using QrF.Core.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QrF.Core.Admin.Business
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly QrfSqlSugarClient _dbContext;
        private readonly IMapper _mapper;

        public RoleBusiness(QrfSqlSugarClient dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BasePageQueryOutput<QueryRoleDto>> GetPageList(QueryRolesInput input)
        {
            var list = new List<QueryRoleDto>();
            var totalNumber = 0;
            var query = await _dbContext.Queryable<Role>()
                .WhereIF(input.DeptId.HasValue, o => o.DeptId == input.DeptId.Value)
                .Select(o => new QueryRoleDto
                {
                    KeyId = o.KeyId,
                    CreateTime = o.CreateTime,
                    DeptId = o.DeptId,
                    Name = o.Name,
                    Codes = o.Codes,
                    CreateId = o.CreateId,
                })
                .ToPageListAsync(input.PageIndex, input.PageSize, totalNumber);
            list = query.Key;
            totalNumber = query.Value;
            return new BasePageQueryOutput<QueryRoleDto> { CurrentPage = input.PageIndex, Data = list, Total = totalNumber };
        }
        /// <summary>
        /// 编辑用户信息
        /// </summary>
        public async Task EditUser(UserDto input)
        {
            input.KeyId = input.KeyId ?? Guid.Empty;
            var model = _mapper.Map<UserDto, User>(input);
            if (model.KeyId != Guid.Empty)
            {
                model.UpdateTime = DateTime.Now;
                if (model.Password.IsNotNullAndWhiteSpace())
                {
                    model.Salt = Randoms.CreateRandomValue(8, false);
                    model.Password = $"{model.Password}{model.Salt}".ToMd5();
                    await _dbContext.Updateable(model)
                                    .IgnoreColumns(it => new { it.Account, it.CreateTime })
                                    .ExecuteCommandAsync();
                }
                else
                {
                    await _dbContext.Updateable(model)
                                    .IgnoreColumns(it => new { it.Account, it.Password, it.Salt, it.CreateTime })
                                    .ExecuteCommandAsync();
                }
            }
            else
            {
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                model.Salt = Randoms.CreateRandomValue(8, false);
                model.Password = $"{model.Password}{model.Salt}".ToMd5();
                await _dbContext.Insertable(model).ExecuteCommandAsync();
            }
        }
    }
}
