using AutoMapper;
using RecipeManager.Data.Entities;
using RecipeManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.Data
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<Recipe, RecipeViewModel>().ReverseMap();
        }
    }
}
