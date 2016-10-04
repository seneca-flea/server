﻿using System;
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
            Mapper.CreateMap<ItemAdd, Item>();
            Mapper.CreateMap<ItemEdit, Item>();


            Mapper.CreateMap<Message, MessageBase>();
            Mapper.CreateMap<Message, MessageWithItem>();

#pragma warning restore CS0618
        }
    }
}