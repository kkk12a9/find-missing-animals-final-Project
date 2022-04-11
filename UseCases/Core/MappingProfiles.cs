using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using AutoMapper;
using Domain;
using UseCases.AppUsers.Dtos;
// using UseCases.Categories;
using UseCases.Comments.Dtos;
using UseCases.Interfaces;
using UseCases.Locations;
using UseCases.Posts;
using UseCases.Posts.Dtos;
using UseCases.Tags;

namespace UseCases.Core
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			string currentUsername = "";

			CreateMap<Post, PostDto>();

			// CreateMap<PostTag, TagDto>()
			// 	.ForMember(dest => dest.Id, o => o.MapFrom(src => src.TagId))
			// 	.ForMember(dest => dest.TagName, o => o.MapFrom(src => src.Tag.TagName));
			// .ForMember(dest => dest.Posts, o => o.MapFrom(src => src.Post));

			// CreateMap<PostTag, PostDto>() // Note: deleted property
			// 	.ForMember(dest => dest.Id, o => o.MapFrom(src => src.PostId))
			// 	.ForMember(dest => dest.Title, o => o.MapFrom(src => src.Post.Title))
			// 	.ForMember(dest => dest.Content, o => o.MapFrom(src => src.Post.Content))
			//  .ForMember(dest => dest.)
			// 	.ForMember(dest => dest.PosterName,
			// 	o => o.MapFrom(src => src.Post.PostFollowers.FirstOrDefault(x => x.isPoster).ApplicationUser.UserName));

			CreateMap<Tag1, Tag1Dto>()
				.ForMember(dest => dest.Tag1Name, o => o.MapFrom(src => src.TagName));

			CreateMap<Tag2, Tag2Dto>()
				.ForMember(dest => dest.Tag2Name, o => o.MapFrom(src => src.TagName));

			CreateMap<Tag3, Tag3Dto>()
				.ForMember(dest => dest.Tag3Name, o => o.MapFrom(src => src.TagName));

			CreateMap<Tag4, Tag4Dto>()
				.ForMember(dest => dest.Tag4Name, o => o.MapFrom(src => src.TagName));

			CreateMap<Tag5, Tag5Dto>()
				.ForMember(dest => dest.Tag5Name, o => o.MapFrom(src => src.TagName));

			// mapping PostFollowing - PostParticipant
			CreateMap<PostFollowing, PostParticipantDto>()
				.ForMember(dest => dest.DisplayName, o => o.MapFrom(src => src.ApplicationUser.DisplayName))
				.ForMember(dest => dest.Username, o => o.MapFrom(src => src.ApplicationUser.UserName))
				.ForMember(dest => dest.Bio, o => o.MapFrom(src => src.ApplicationUser.Bio));
			// .ForMember(dest => dest.Image, o => o.MapFrom(src => src.ApplicationUser.Photos.FirstOrDefault(p => p.IsProfilePicture).Url))
			// .ForMember(dest => dest.IsParticipating, o =>
			// 	o.MapFrom(src => src.ApplicationUser.PostFollowings.Any(x => x.ApplicationUser.UserName == currentUsername)));

			CreateMap<PostLocation, PostLocationDto>()
				.ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
				.ForMember(dest => dest.PostCode, o => o.MapFrom(src => src.PostCode))
				.ForMember(dest => dest.RoadLocation, o => o.MapFrom(src => src.RoadLocation))
				.ForMember(dest => dest.Location, o => o.MapFrom(src => src.Location))
				.ForMember(dest => dest.ExtraLocation, o => o.MapFrom(src => src.ExtraLocation))
				.ForMember(dest => dest.Location, o => o.MapFrom(src => src.Location))
				.ForMember(dest => dest.Latitude, o => o.MapFrom(src => src.Latitude));

			// Note: cyclic mapping for EditPost (1)
			CreateMap<Post, PostDto>()
				.ForMember(dest => dest.PosterName,
				o => o.MapFrom(src => src.PostFollowers.FirstOrDefault(x => x.isPoster).ApplicationUser.UserName))
				.ForMember(dest => dest.PostLocation, o => o.MapFrom(src => src.PostLocation))
				.ForMember(dest => dest.Tag1Dto, o => o.MapFrom(src => src.Tag1Posts.Select(tag => tag.Tag1).FirstOrDefault()))
				.ForMember(dest => dest.Tag2Dto, o => o.MapFrom(src => src.Tag2Posts.Select(tag => tag.Tag2).FirstOrDefault()))
				.ForMember(dest => dest.Tag3Dto, o => o.MapFrom(src => src.Tag3Posts.Select(tag => tag.Tag3).FirstOrDefault()))
				.ForMember(dest => dest.Tag4Dto, o => o.MapFrom(src => src.Tag4Posts.Select(tag => tag.Tag4).FirstOrDefault()))
				.ForMember(dest => dest.Tag5Dto, o => o.MapFrom(src => src.Tag5Posts.Select(tag => tag.Tag5).FirstOrDefault()))
				.ForMember(dest => dest.Photos, o => o.MapFrom(src => src.Photos))
			// .ForMember(dest => dest.Photo1, o => o.MapFrom(src => src.Photos.Select(photo => photo).FirstOrDefault()))
			// .ForMember(dest => dest.Photo2, o => o.MapFrom(src => src.Photos.Select(photo => photo).FirstOrDefault()))
			.ForMember(dest => dest.PostParticipants, o => o.MapFrom(src => src.PostFollowers));

			// Note: cyclic mapping for EditPost (2)
			CreateMap<EditPostDto, Post>()
				.ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
				.ForMember(dest => dest.Title, o => o.MapFrom(src => src.Title))
				.ForMember(dest => dest.Content, o => o.MapFrom(src => src.Content))
				.ForMember(dest => dest.Date, o => o.MapFrom(src => src.Date))
				.ForMember(dest => dest.IsFound, o => o.MapFrom(src => src.IsFound))
				.ForMember(dest => dest.PostLocation, o => o.MapFrom(src => src.PostLocation));

			CreateMap<Post, EditPostDto>()
				// .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
				.ForMember(dest => dest.Title, o => o.MapFrom(src => src.Title))
				.ForMember(dest => dest.Content, o => o.MapFrom(src => src.Content))
				.ForMember(dest => dest.Date, o => o.MapFrom(src => src.Date))
				.ForMember(dest => dest.IsFound, o => o.MapFrom(src => src.IsFound))
				.ForMember(dest => dest.PostLocation, o => o.MapFrom(src => src.PostLocation));

			// CreateMap<ApplicationUser, PostParticipantDto>()
			// 	.ForMember(dest => dest.Image, o => o.MapFrom(x => x.Photos.FirstOrDefault(p => p.IsProfilePicture).Url));

			CreateMap<Comment, CommentDto>()
				.ForMember(dest => dest.DisplayName, o => o.MapFrom(src => src.ApplicationUser.DisplayName))
				.ForMember(dest => dest.Username, o => o.MapFrom(src => src.ApplicationUser.UserName));
			// .ForMember(dest => dest.ImageUrl, o => o.MapFrom(src => src.ApplicationUser.Photos
			// .FirstOrDefault(x => x.IsProfilePicture).Url));
		}
	}
}