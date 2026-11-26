using CouponBook.Data.Entities;
namespace CouponBook.Business.Catalog;
public record CityDto(int Id,string NameAr,string NameEn);
public record CategoryDto(int Id,string NameAr,string NameEn,string Icon);
public record ProviderDto(int Id,string Name,string LogoUrl,CategoryDto Category);
public record VoucherDto(int Id,string Title,string Description,DiscountType DiscountType,decimal? DiscountValue,decimal FaceValue,int UsageLimit,string Terms,ProviderDto Provider);
public record BookListItemDto(int Id,string Title,string CoverUrl,CityDto City,decimal Price,string Currency,int ValidityDays,DateTime SaleEndAt,int VoucherCount,decimal TotalFaceValue,int CopiesLeft);
public record BookDetailDto(int Id,string Title,string Description,string CoverUrl,CityDto City,decimal Price,string Currency,int ValidityDays,DateTime SaleStartAt,DateTime SaleEndAt,decimal TotalFaceValue,int CopiesLeft,List<VoucherDto> Vouchers);
