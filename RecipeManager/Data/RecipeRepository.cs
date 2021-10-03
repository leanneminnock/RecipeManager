using Microsoft.EntityFrameworkCore;
using RecipeManager.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.Data
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly RecipeContext _ctx;

        public RecipeRepository(RecipeContext ctx)
        {
            _ctx = ctx;
        }
        public IEnumerable<Recipe> GetAllRecipes()
        {
            return _ctx.Recipes;
        }

        public Recipe GetRecipeById(int id)
        {
            return _ctx.Recipes.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Recipe> GetRecipesByCatagory(string catagory)
        {
            return _ctx.Recipes.Where(r => r.Catagory == catagory);
        }

        public IEnumerable<Recipe> GetRecipesByIngredient(string ingredient)
        {

            return _ctx.Recipes.Include(r => r.Ingredients).Where(r => r.Ingredients.Any(i => i.Name == ingredient));
        }

        public IEnumerable<Recipe> GetRecipesByMeal(string meal)
        {
            return _ctx.Recipes.Where(r => r.Meal == meal);
        }

        public void CreateNewRecipe(Recipe recipe)
        {
            _ctx.Recipes.Add(recipe);
        }

        public bool UpdateRecipe(int id, Recipe recipe)
        {
            Recipe currentRecipe = GetRecipeById(id);
            if (currentRecipe == null)
            {
                throw new ArgumentException();
            }
            else if (id != recipe.Id)
            {
                throw new ArgumentException("Original recipe Id and updated recipe Id do not match.");
            }
            _ctx.Recipes.Remove(currentRecipe);
            _ctx.Recipes.Add(recipe);
            return true;
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        public bool DeleteRecipeById(int id)
        {
            Recipe recipe = GetRecipeById(id);
            if (recipe == null)
            {
                throw new ArgumentException();
            }
            _ctx.Recipes.Remove(recipe);
            return true;
        }
    }
}
