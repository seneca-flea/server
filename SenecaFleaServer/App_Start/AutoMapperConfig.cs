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
            Mapper.CreateMap<Item, ItemWithMedia>();
            Mapper.CreateMap<ItemAdd, Item>();
            Mapper.CreateMap<ItemEdit, Item>();
            Mapper.CreateMap<ItemWithMedia, ItemBase>();
            Mapper.CreateMap<ImageAdd, Image>();

            Mapper.CreateMap<MessageAdd, Message>();
            Mapper.CreateMap<Message, MessageBase>();
            Mapper.CreateMap<Message, MessageWithItem>();

            Mapper.CreateMap<ConversationAdd, Conversation>();
            Mapper.CreateMap<Conversation, ConversationBase>();
            Mapper.CreateMap<Conversation, ConversationWithMessage>();

            Mapper.CreateMap<UserAdd, User>();
            
            Mapper.CreateMap<LocationBase, Location>();
            // UserGetById - UserWithLocation
            Mapper.CreateMap<Location, LocationBase>();

            // User Get
            Mapper.CreateMap<User, UserBase>();
            Mapper.CreateMap<User, UserWithLocation>();
            Mapper.CreateMap<User, UserWithAllInfo>();
            

            Mapper.CreateMap<PurchaseHistory, PurchaseHistoryBase>();
            Mapper.CreateMap<PurchaseHistoryAdd, PurchaseHistory>();
#pragma warning restore CS0618
        }
    }
}