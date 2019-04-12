using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Data.ValueTypeResolvers;
using Travelogue.Models;
using Travelogue.ViewModels;

namespace Travelogue.Data.Profiles
{
    public class PostViewModelProfile : Profile
    {
        //private readonly IConfigurationRoot _config;
        //private readonly ICommentRepository _comment;

        public PostViewModelProfile()
        {

        }

        //public PostViewModelProfile(IConfigurationRoot config, ICommentRepository comment) 
        //{
        //    _config = config;
        //    _comment = comment;
        //}

        protected override void Configure()
        {

            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.AllowsComments, opts => opts.MapFrom(src => src.AllowsComments))
                .ForMember(dest => dest.Comments, opts => opts.MapFrom(src => src.Comments)) // x => x.ResolveUsing<CommentsResolver>()
                                                                                                   // .ConstructedBy(() => new CommentsResolver(_comment)).FromMember(y => y.Id))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Image, opts => opts.MapFrom(src => src.Image))
                .ForMember(dest => dest.PostedOn, opts => opts.MapFrom(src => src.PostedOn))
                .ForMember(dest => dest.Published, opts => opts.MapFrom(src => src.Published))
                .ForMember(dest => dest.PostText, opts => opts.MapFrom(src => src.Text))
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.Stop, opts => opts.Ignore())
                .ForMember(dest => dest.Trip, opts => opts.Ignore())
                .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.TopTip, opts => opts.MapFrom(src => src.TopTip))
                .ForMember(dest => dest.DateUpdated, opts => opts.MapFrom(src => src.UpdatedOn));
        }
    }
}
