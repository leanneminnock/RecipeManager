using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using NUnit.Framework;
using RecipeManager.Data;
using RecipeManager.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManagerTests
{
    public class Tests
    {
        //Create In Memory Database
        private DbContextOptions<RecipeContext> options = new DbContextOptionsBuilder<RecipeContext>()
                              .UseInMemoryDatabase(databaseName: "RecipeDataBase")
                              .Options;
        [OneTimeSetUp]
        public void Setup()
        {
            using (var context = new RecipeContext(options))
            {
                Ingredient flour = new Ingredient()
                {
                    Id = 1,
                    Name = "Flour"
                };
                Ingredient eggs = new Ingredient()
                {
                    Id = 2,
                    Name = "Eggs"
                };
                Ingredient sugar = new Ingredient()
                {
                    Id = 3,
                    Name = "Sugar"
                };
                Ingredient chocolate = new Ingredient()
                {
                    Id = 4,
                    Name = "Chocolate"
                };
                Ingredient bakingPowder = new Ingredient()
                {
                    Id = 5,
                    Name = "Baking Powder"
                };
                Ingredient butter = new Ingredient()
                {
                    Id = 6,
                    Name = "Butter"
                };
                
                context.Recipes.Add(new Recipe
                {
                    Id = 1,
                    Name = "Madera",
                    Catagory = "Cake",
                    Meal = "Desert",
                    Ingredients = new List<Ingredient> { flour, butter, sugar, bakingPowder, eggs }
                    
                });

                context.Recipes.Add(new Recipe
                {
                    Id = 2,
                    Name = "Chocolate buns",
                    Catagory = "Buns",
                    Meal = "Snack",
                    Ingredients = new List<Ingredient> { flour, butter, sugar, bakingPowder, chocolate }

                });

                context.SaveChanges();
            }
        }

        [Test]
        public void RecipeRepository_GetAllRecipes_Valid()
        {
            using (var context = new RecipeContext(options))
            {
                RecipeRepository repo = new RecipeRepository(context);
                List<Recipe> recipes = new List<Recipe>(repo.GetAllRecipes());

                Assert.IsNotNull(recipes);
                Assert.AreEqual(2, recipes.Count);
            };
        }

        [Test]
        public void RecipeRepository_GetRecipeById_Valid_Id()
        {
            using (var context = new RecipeContext(options))
            {
                RecipeRepository repo = new RecipeRepository(context);
                var recipe = repo.GetRecipeById(1);

                Assert.IsNotNull(recipe);
                Assert.AreEqual(1, recipe.Id);
                Assert.AreEqual("Madera", recipe.Name);
                Assert.AreEqual("Cake", recipe.Catagory);
                Assert.AreEqual("Desert", recipe.Meal);
            };
        }

         [Test]
        public void RecipeRepository_GetRecipeById_InValid_Id()
        {
            using (var context = new RecipeContext(options))
            {
                RecipeRepository repo = new RecipeRepository(context);
                var recipe = repo.GetRecipeById(1);

                Assert.IsNotNull(recipe);
                Assert.AreNotEqual(2, recipe.Id);
                Assert.AreNotEqual("Birthday", recipe.Name);
                Assert.AreNotEqual("Bun", recipe.Catagory);
                Assert.AreNotEqual("Party", recipe.Meal);
            };
        }

        
         [Test]
        public void RecipeRepository_GetRecipesByCatagory_Valid_Catagory()
        {
            using (var context = new RecipeContext(options))
            {
                RecipeRepository repo = new RecipeRepository(context);
                List<Recipe> recipes = new List<Recipe>(repo.GetRecipesByCatagory("Cake"));
                Recipe r = recipes.FirstOrDefault(r => r.Catagory == "Cake");

                Assert.IsNotNull(r);
                Assert.AreEqual(1, r.Id);
            };
        }
         [Test]
        public void RecipeRepository_GetRecipesByIngredient_Valid_Ingredient()
        {
            using (var context = new RecipeContext(options))
            {
                RecipeRepository repo = new RecipeRepository(context);
                List<Recipe> recipes = repo.GetRecipesByIngredient("Chocolate").ToList();
                bool result = recipes.All(r => r.Ingredients.Any(i => i.Name == "Chocolate"));

                Assert.IsTrue(result);
            };
        }

         [Test]
        public void RecipeRepository_GetRecipesByMeal_Valid_Meal()
        {
            using (var context = new RecipeContext(options))
            {
                RecipeRepository repo = new RecipeRepository(context);
                List<Recipe> recipes = repo.GetRecipesByMeal("Desert").ToList();
                Recipe r = recipes.FirstOrDefault(r => r.Meal == "Desert");

                Assert.IsNotNull(r);
                Assert.AreEqual(1, r.Id);
            };
        }


    }
}