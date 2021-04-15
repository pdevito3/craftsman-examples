namespace Application.Dtos.Recipe
{
    using Application.Dtos.Shared;

    public class RecipeParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}