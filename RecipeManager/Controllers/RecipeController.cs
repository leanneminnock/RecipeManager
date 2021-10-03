using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeManager.Data;
using RecipeManager.Data.Entities;
using RecipeManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : Controller
    {
        private readonly IRecipeRepository _repo;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<RecipeViewModel> GetRecipeById(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Recipe recipe = _repo.GetRecipeById(id);
                    return Ok(_mapper.Map<RecipeViewModel>(recipe));
                }
                var errors = ModelState.Values.SelectMany(c => c.Errors).Select(e => e.ErrorMessage);
                return BadRequest($"Invalid request: {string.Join(", ", errors)}");
            }
            catch (NullReferenceException ex)
            {
                return NotFound($"No recipe matching your query {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<RecipeViewModel>> GetRecipeByQuery([FromQuery] string catagory, [FromQuery] string ingredient, [FromQuery] string meal)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    
                    if(catagory != null)
                    {
                        List<Recipe> recipes = new List<Recipe>(_repo.GetRecipesByCatagory(catagory));
                        return Ok(_mapper.Map<IEnumerable<RecipeViewModel>>(recipes));
                    }
                    else if (ingredient != null)
                    {
                        List<Recipe> recipes = new List<Recipe>(_repo.GetRecipesByIngredient(ingredient));
                        return Ok(_mapper.Map<IEnumerable<RecipeViewModel>>(recipes));
                    }
                    else if (meal != null)
                    {
                        List<Recipe> recipes = new List<Recipe>(_repo.GetRecipesByMeal(meal));
                        return Ok(_mapper.Map<IEnumerable<RecipeViewModel>>(recipes));
                    }
                    else
                    {
                        List<Recipe> recipes = new List<Recipe>(_repo.GetAllRecipes());
                        return Ok(_mapper.Map<IEnumerable<RecipeViewModel>>(recipes));
                    }

                }
                var errors = ModelState.Values.SelectMany(c => c.Errors).Select(e => e.ErrorMessage);
                return BadRequest($"Invalid request: {string.Join(", ", errors)}");
            }
            catch(NullReferenceException ex)
            {
                return NotFound($"No recipe matching your query {ex}");
            }
        }

        [HttpPost]
        public ActionResult<RecipeViewModel> CreateNewRecipe([FromBody] RecipeViewModel recipe)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Recipe r = _mapper.Map<Recipe>(recipe);
                    _repo.CreateNewRecipe(r);
                    if (!_repo.SaveAll()) { return BadRequest(); }
                    return Ok(_mapper.Map<RecipeViewModel>(r));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"No valid customer found {ex}");
            }
        }

        [HttpPut]
        public ActionResult<RecipeViewModel> UpdateRecipe(int id, [FromBody] RecipeViewModel recipe)
        {
            try
            {
                Recipe r = _mapper.Map<Recipe>(recipe);
                if (!_repo.UpdateRecipe(id, r)) { return BadRequest(); }

                if (!_repo.SaveAll()) { return BadRequest(); }
                return Ok(_mapper.Map<RecipeViewModel>(r));
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"No valid recipe could be found, {ex}");
            }
        }

        [HttpDelete]
        public ActionResult DeleteRecipeById(int id)
        {
            try
            {
                if (!_repo.DeleteRecipeById(id)) { return NotFound("Recipe Id Not Valid."); }
                if (!_repo.SaveAll()) { return BadRequest("Changes could not be saved."); }
                return Ok("Recipe  Deleted");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"No valid Recipe was found, {ex}");
            }
        }
    }
}
