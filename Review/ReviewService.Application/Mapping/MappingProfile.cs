using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ReviewService.Application.Features.Comments.DTOs;
using ReviewService.Application.Features.Discussions.DTOs;
using ReviewService.Application.Features.Ratings.DTOs;
using ReviewService.Application.Features.Reviews.DTOs;
using ReviewService.Domain.Entities;
using ReviewService.Domain.ValueObjects;

namespace ReviewService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Discussion
            CreateMap<Discussion, DiscussionDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Comment
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.Text))
                .ForMember(dest => dest.WordCount, opt => opt.MapFrom(src => src.Content.WordCount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.LikeCount - src.DislikeCount))
                .ForMember(dest => dest.IsReply, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ParentCommentId)));

            // Review
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.Text))
                .ForMember(dest => dest.WordCount, opt => opt.MapFrom(src => src.Content.WordCount))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Helpfulness, opt => opt.MapFrom(src => src.HelpfulCount - src.UnhelpfulCount));

            // RatingEntity
            CreateMap<RatingEntity, RatingEntityDto>()
                .ForMember(dest => dest.RatingValue, opt => opt.MapFrom(src => src.RatingValue))
                .ForMember(dest => dest.IsForReview, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ReviewId)));

            // Value Objects
            CreateMap<UserInfo, UserInfoDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

            CreateMap<Rating, RatingDto>()
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.MaxScore, opt => opt.MapFrom(src => src.MaxScore))
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.GetPercentage()));

            CreateMap<DiscussionDto, Discussion>();
            CreateMap<CommentDto, Comment>();
            CreateMap<ReviewDto, Review>();
            CreateMap<RatingEntityDto, RatingEntity>();
            CreateMap<UserInfoDto, UserInfo>();
            CreateMap<RatingDto, Rating>();
        }
    }
}
