using AutoMapper;
using ExpressWorld.Application.DTOs;
using ExpressWorld.Core.Entities;
using ExpressWorld.Shared.DTOs;

namespace ExpressWorld.Shared.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SomeOtherGuyProductDTO, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProductDescription))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price * (1 - (decimal)(src.DiscountPercentage / 100))))
                .ForMember(dest => dest.NumberOfGuests, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(_ => "SomeOtherGuy"))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ImageUrls))
                 .ForMember(dest => dest.DestinationName, opt => opt.MapFrom(src => "Unknown Destination"));

            CreateMap<BigGuyProductDTO, Product>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductDetailData.Id))
                  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductDetailData.Name))
                  .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProductDetailData.ProductDescription))
                  .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount * (1 - (decimal)src.Price.AppliedDiscount)))
                  .ForMember(dest => dest.NumberOfGuests, opt => opt.MapFrom(src => src.ProductDetailData.Capacity))
                  .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(_ => "TheBigGuy"))
                  .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Photos))
                   .ForMember(dest => dest.DestinationName, opt => opt.MapFrom(src => "Unknown Destination"));

            CreateMap<TourGuyProductDTO, Product>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.DiscountPrice))  // Assuming discount price is the final price
              .ForMember(dest => dest.NumberOfGuests, opt => opt.MapFrom(src => src.MaximumGuests))
              .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(_ => "TheTourGuy"))
              .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(img => img.Url).ToList()))
              .ForMember(dest => dest.DestinationName, opt => opt.MapFrom(src => "Unknown Destination")); //opt => opt.Ignore());

            CreateMap<Product, ProductDTO>()
            //.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))  // Optional, direct mapping
            //.ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.DestinationName, opt => opt.NullSubstitute("Unknown Destination"))  // Default value if null
            .ForSourceMember(src => src.ImageUrls, opt => opt.DoNotValidate());  // Ignore ImageUrls if in Product
        }
    }
}
