﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.ProdutoAgregador;

namespace ProjetoLojaVirtual.Libraries.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produto, ProdutoItem>();
            CreateMap<Cliente, EnderecoEntrega>()
                .ForMember(dest=>dest.Id, opt=>opt.MapFrom(orig=>0))
                .ForMember(dest=>dest.Nome, opt=>opt.MapFrom(orig=>string.Format("Endereço do cliente ({0})", orig.Nome)));
        }
    }
}
