using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.Validacao
{
    public class EmailUnicoColaboradorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Email = value as string;

            IColaboradorRepository _colaboradorRepository = (IColaboradorRepository) validationContext.GetService(typeof(IColaboradorRepository));
            List<Colaborador> colaboradores = _colaboradorRepository.ObterColaboradorPorEmail(Email);

            return base.IsValid(value, validationContext);
        }

    }
}
