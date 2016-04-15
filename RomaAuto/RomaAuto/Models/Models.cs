using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RomaAuto.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "შევსება აუცილებელია")]
        [DisplayName("სახელი")]
        [StringLength(100, ErrorMessage = "{0} მინუმუმ უნდა იყოს {2} სიმბოლო.", MinimumLength = 6)]
        public string Name { get; set; }

        [Required(ErrorMessage = "შევსება აუცილებელია")]
        [DisplayName("გვარი")]
        [StringLength(100, ErrorMessage = "{0} მინუმუმ უნდა იყოს {2} სიმბოლო.", MinimumLength = 6)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "შევსება აუცილებელია")]
        [StringLength(100, ErrorMessage = "{0} მინუმუმ უნდა იყოს {2} სიმბოლო.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        public int AdminCategoryId { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "სახელი")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "პაროლი")]
        public string Password { get; set; }
    }

    public class MainUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Category { get; set; }
    }

    public class OrderModel
    {
        public int OrderID { get; set; }
        public int Manufacturer { get; set; }
        public int CarModelID { get; set; }
        public DateTime OpenDate { get; set; }
        public string Category { get; set; }
        public DateTime BirthDate { get; set; }
        public string Transmision { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Part { get; set; }
        public string Note { get; set; }
        public DateTime CloseDate { get; set; }
        public int OpenOperatorID { get; set; }
        public int CloseOperatorID { get; set; }



    }
    public class SellersModel
    {
        public int SellerID { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CityID { get; set; }
        public int ManufacturerID { get; set; }

        public int ModelID { get; set; }
        public int CarCategoryID { get; set; }
        public int TransmisionID { get; set; }
        public string Note { get; set; }
        public int Helped { get; set; }
        public int DontHelped { get; set; }



    }
    public class AllOrder
    {
        public List<Order>orders { get; set; }
        public List<Operator>operators { get; set; }
        public List<Saler>sellers { get; set; }
        public List<Seller_Order>salerOrders { get; set; }
    }

    public class OrdersList
    {
        public int SalerID { get; set; }
        public int SellerPartID { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public int? Helped { get; set; }
        public int? NotHelped { get; set; }
        public string Note { get; set; }
        public string Manufacturer { get; set; }
    }
   
}