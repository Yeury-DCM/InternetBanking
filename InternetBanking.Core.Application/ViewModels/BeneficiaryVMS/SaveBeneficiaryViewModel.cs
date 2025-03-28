using InternetBanking.Core.Application.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.BeneficiaryVMS
{
    public class SaveBeneficiaryViewModel 
    {
        public string Id { get; set; }  // Identificador único de la relación de beneficiario

        [Required]
        public string UserId { get; set; }  // ID del usuario que agrega al beneficiario

        [Required]
        public int ProductId { get; set; }  // ID del producto (cuenta) del beneficiario

        [Required]
        public string BeneficiaryFirstName { get; set; }  // Primer nombre del beneficiario

        [Required]
        public string BeneficiaryLastName { get; set; }  // Apellido del beneficiario

        [Required]
        public string ProductType { get; set; }  // Tipo de producto (ejemplo: "Cuenta Ahorros")

        [Required]
        public string AccountNumber { get; set; }  // Número de cuenta del beneficiario
    }

}
