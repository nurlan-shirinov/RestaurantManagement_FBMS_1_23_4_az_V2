using Application.CQRS.Categories.Commands.Requests;
using Application.CQRS.Categories.Commands.Responses;
using Application.CQRS.Users.DTOs;
using AutoMapper;
using Domain.Entites;
using static Application.CQRS.Users.Handlers.Register;
using static Application.CQRS.Users.Handlers.Update;

namespace Application.AutoMapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<User, GetByIdDto>().ReverseMap();
        CreateMap<RegisterCommand, User>().ReverseMap();
        CreateMap<UpdateCommand, User>().ReverseMap();
        CreateMap<User, RegisterDto>().ReverseMap();
        CreateMap<User, UpdateDto>();

        CreateMap<Category , CreateCategoryRequest>().ReverseMap();
        CreateMap<CreateCategoryResponse, Category>().ReverseMap();
    }
}