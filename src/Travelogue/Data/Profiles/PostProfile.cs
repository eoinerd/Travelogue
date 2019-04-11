using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;
using Travelogue.ViewModels;

namespace Travelogue.Data.Profiles
{
    public class PostProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<PostViewModel, Post>()
                .ForMember(dest => dest.AllowsComments, opts => opts.MapFrom(src => src.AllowsComments))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Image, opts => opts.MapFrom(src => src.Image))
                .ForMember(dest => dest.PostedOn, opts => opts.MapFrom(src => src.PostedOn))
                .ForMember(dest => dest.Published, opts => opts.MapFrom(src => src.Published))
                .ForMember(dest => dest.Text, opts => opts.MapFrom(src => src.Text))
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.UpdatedOn, opts => opts.MapFrom(src => src.DateUpdated))
                .ForMember(dest => dest.UserName, opt => opt.Ignore());
        }
    }
}
