using System.ComponentModel.DataAnnotations;namespace CouponBook.Api.Dtos;
public class RequestOtpRequest{[Required,MaxLength(30)]public string Mobile{get;set;}="";}
public class VerifyOtpRequest{[Required,MaxLength(30)]public string Mobile{get;set;}="";[Required,StringLength(6,MinimumLength=6)]public string Code{get;set;}="";[MaxLength(150)]public string? FullName{get;set;}}
public class UpdateProfileRequest{[Required,MaxLength(150)]public string FullName{get;set;}="";[EmailAddress]public string? Email{get;set;}[Required,RegularExpression("^(ar|en)$")]public string Language{get;set;}="ar";}
