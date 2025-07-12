using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Author;
using BookStore.API.Models.Book;

namespace BookStore.API.Configurration
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {

            /*Authors mappings profiles*/
            CreateMap<AuthorCreateDTO, Author>().ReverseMap();
            CreateMap<AuthorUpdateDTO, Author>().ReverseMap();
            CreateMap< Author, AuthorGetDTO>();

            /*Books Mappings Profiles*/
            CreateMap<BookCreateDTO, Book>();
            CreateMap<BookUpdateDTO, Book>();
            CreateMap<Book, BookGetListDTO>()
                .ForPath(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));

            CreateMap<Book, BooksGetDetailsDTO>()
               .ForPath(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));


        }
    }
}
