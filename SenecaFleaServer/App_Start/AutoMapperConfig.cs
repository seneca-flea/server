using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SenecaFleaServer.Models;

namespace SenecaFleaServer
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
#pragma warning disable CS0618
            Mapper.CreateMap<Item, ItemBase>();
        }
    }
}