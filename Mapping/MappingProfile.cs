using AutoMapper;

namespace RecipesApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RecipesApi.Models.Recipe, RecipesApi.DTOs.RecipeDto>();
            CreateMap<RecipesApi.DTOs.CreateRecipeDto, RecipesApi.Models.Recipe>();
            CreateMap<RecipesApi.DTOs.UpdateRecipeDto, RecipesApi.Models.Recipe>();
        }
    }
}
