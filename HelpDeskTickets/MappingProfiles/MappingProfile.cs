using AutoMapper;
using HelpDeskTickets.Core.DTOs.Responses;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.App.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTicketRequest, Ticket>()
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TicketStatus.Open));

            CreateMap<UpdateTicketRequest, Ticket>()
    .ForMember(dest => dest.Status,
               opt => opt.MapFrom(src => Enum.Parse<TicketStatus>(src.Status, true)));

            CreateMap<Ticket, TicketResponse>()
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
    .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src => src.AssignedToUser.Email));

            CreateMap<CreateDepartmentRequest, Department>();
            CreateMap<Department, DepartmentResponse>();
            CreateMap<CreateCommentRequest, Comment>()
    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.CreatedByUserName,
               opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.FirstName : null));
            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            CreateMap<History, HistoryResponse>()
    .ForMember(dest => dest.PerformedByUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
    .ForMember(dest => dest.LoggedAt, opt => opt.MapFrom(src => src.CreatedAt));

        }
    }
}