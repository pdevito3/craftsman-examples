namespace Application.Validation.Recipe
{
    using Application.Dtos.Recipe;
    using FluentValidation;

    public class RecipeForCreationDtoValidator: RecipeForManipulationDtoValidator<RecipeForCreationDto>
    {
        public RecipeForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}