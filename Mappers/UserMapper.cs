using AutoMapper;
using System.Collections.Generic;

namespace Onesoftdev.AspCoreJwtAuth.Mappers
{
    public class UserMapper
    {
        public static Models.User GetUserModelFromEntity(Entities.User userEntity)
        {
            return new MapperConfiguration(configure =>
                configure.CreateMap<Entities.User, Models.User>()
                .ForMember(dest => dest.Verified, opt => opt.MapFrom(src =>
                src.IsVerified))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src =>
                GetUserDetailsModelFromEntity(src.UserDetails))))
                .CreateMapper().Map<Models.User>(userEntity);
        }

        public static Models.UserDetails GetUserDetailsModelFromEntity(Entities.UserDetails userDetails)
        {
            var detailsMapper = new MapperConfiguration(configure =>
            configure.CreateMap<Entities.UserDetails, Models.UserDetails>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
            $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
            src.EmailAddress))
            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src =>
            src.MobileNumber)))
            .CreateMapper().Map<Models.UserDetails>(userDetails);
            return detailsMapper;
        }

        public static IEnumerable<Models.User> GetUserModelsFromEntities(IEnumerable<Entities.User> users)
        {
            return new MapperConfiguration(configure =>
                configure.CreateMap<Entities.User, Models.User>()
                .ForMember(dest => dest.Verified, opt => opt.MapFrom(src =>
                src.IsVerified))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src =>
                GetUserDetailsModelFromEntity(src.UserDetails))))
                .CreateMapper().Map<IEnumerable<Models.User>>(users);
        }

        public static Models.UserUpdate GetUserUpdateModelFromEntity(Entities.User user)
        {
            return new MapperConfiguration(configure =>
                configure.CreateMap<Entities.User, Models.UserUpdate>()
                .ForMember(dest => dest.DetailsUpdate, opt => opt.MapFrom(src =>
                src.UserDetails)))
                .CreateMapper().Map<Models.UserUpdate>(user);
        }

        public static Entities.User GetUserEntityFromCreateModel(Models.UserCreate userCreateModel)
        {
            return new MapperConfiguration(configure =>
                configure.CreateMap<Models.UserCreate, Entities.User>()
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => 
                src.Verified))
                .ForMember(dest => dest.UserDetails, opt => opt.MapFrom(src =>          // ======> This may cause a bug as the create model userdetails object has not been explicityly mapped to the entity userdetails object. I am not sure why this works.
                src.DetailsCreate))
                .AfterMap((userCreate, user) => { userCreate.DetailsCreate.UserId = user.Id; })      // ======> This may cause a bug as the user.Id should be null or all zero's because this is a new user and guid has not been generated yet.
                .AfterMap((userCreate, user) => { user.UserDetails = user.UserDetails; }))           // ======> This may cause a bug as the the same user details object is being mapped to itself.
                .CreateMapper().Map<Entities.User>(userCreateModel);
        }
    }
}
