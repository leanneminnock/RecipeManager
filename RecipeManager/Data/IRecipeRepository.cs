using RecipeManager.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.Data
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetAllRecipes();
        Recipe GetRecipeById(int id);
        IEnumerable<Recipe> GetRecipesByCatagory(string catagory);
        IEnumerable<Recipe> GetRecipesByIngredient(string ingredient);
        IEnumerable<Recipe> GetRecipesByMeal(string meal);
        void CreateNewRecipe(Recipe recipe);
        bool UpdateRecipe(int id, Recipe recipe);
        bool SaveAll();
        bool DeleteRecipeById(int id);

    }
}
