// Mapping/BookProfile.cs
using AutoMapper;
using LibraryAPI.Dtos;
using LibraryAPI.Models;

namespace LibraryAPI.Mapping;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>()
        .ForMember(d => d.AuthorFullName,
            o => o.MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
        .ForMember(d => d.AuthorAgeAtPublication,
            o => o.MapFrom(s => s.PublishedYear - s.Author.BirthYear));

        CreateMap<CreateAuthorRequest, Author>();
        CreateMap<CreateBookRequest, Book>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.PurchasePrice, o => o.Ignore())
            .ForMember(d => d.InternalNotes, o => o.Ignore())
            .ForMember(d => d.StorageLocation, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore());
    }
}