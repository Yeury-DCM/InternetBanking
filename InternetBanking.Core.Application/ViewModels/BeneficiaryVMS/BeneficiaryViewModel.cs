using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.BeneficiaryVMS
{
    public class BeneficiaryViewModel
    {
        public string Id { get; set; }  // Identificador único de la relación de beneficiario

        public string UserId { get; set; }  // ID del usuario que agrega al beneficiario

        public int ProductId { get; set; }  // ID del producto (cuenta) del beneficiario

        public string BeneficiaryFullName { get; set; }  // Nombre completo del beneficiario

        public string ProductType { get; set; }  // Tipo de producto (ejemplo: "Cuenta Ahorros", "Cuenta Corriente")

        public string AccountNumber { get; set; }  // Número de cuenta del beneficiario
    }
}
