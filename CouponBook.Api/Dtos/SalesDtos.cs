using System.ComponentModel.DataAnnotations;namespace CouponBook.Api.Dtos;
public class PurchaseRequest{public string? PromoCode{get;set;}public bool IsGift{get;set;}[MaxLength(30)]public string? RecipientMobile{get;set;}[MaxLength(300)]public string? GiftMessage{get;set;}[RegularExpression("^(card|apple_pay)$")]public string PaymentMethod{get;set;}="card";}
public class MockPayRequest{[Required]public int PaymentId{get;set;}[Required,RegularExpression("^(SUCCEEDED|FAILED)$")]public string Outcome{get;set;}="";}
