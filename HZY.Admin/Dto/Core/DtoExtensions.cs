using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Admin.Dto.Core
{
    using AutoMapper;

    public static class DtoExtensions
    {

        public static T2 MapTo<T1, T2>(this T1 Source)
            where T1 : class
            where T2 : class

        {
            if (Source == null) return default;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<T1, T2>());
            var mapper = config.CreateMapper();

            return mapper.Map<T2>(Source);
        }


    }
}
