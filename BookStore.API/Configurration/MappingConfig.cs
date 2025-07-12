using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Author;

namespace BookStore.API.Configurration
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {

            CreateMap<AuthorCreateDTO, Author>().ReverseMap();
            CreateMap<AuthorUpdateDTO, Author>().ReverseMap();
            CreateMap< Author, AuthorGetDTO>();
        }
    }
}
