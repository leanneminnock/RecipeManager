using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.Data.Entities
{
    //public enum Meal
    //{
    //    Breakfast,
    //    Brunch,
    //    Lunch,
    //    Dinner,
    //    Snack
    //}
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public string Catagory { get; set; }
        public string Meal { get; set; }
    }
}
