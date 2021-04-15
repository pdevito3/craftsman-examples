namespace Application.Mappings
{
    using Application.Dtos.Recipe;
    using AutoMapper;
    using Domain.Entities;

    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            //createmap<to this, from this>
            CreateMap<Recipe, RecipeDto>()
                .ReverseMap();
            CreateMap<RecipeForCreationDto, Recipe>();
            CreateMap<RecipeForUpdateDto, Recipe>()
                .ReverseMap();
        }
    }
}