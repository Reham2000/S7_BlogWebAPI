using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewCategoriesController : ControllerBase
    {
        // DI
        //private readonly IGenaricRepository<Category> _category;
        //public NewCategoriesController(IGenaricRepository<Category> category)
        //{
        //    _category = category;
        //}
        private readonly IUnitOfWork _unitOfWork;
        public NewCategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Categories = await _unitOfWork.Categories.GetAllAsync(
                      //predicate:  c => c.Id == 6,
                      includes: new Expression<Func<Category, object>>[]
                      {
                          c => c.Posts
                      }
                    );
                if(Categories is null || ! Categories.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",
                        Data = new List<Category>()
                    });
                }
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Data Retrived Successfully ",
                    Data = Categories.Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Posts = c.Posts.Select(p => new
                        {
                            PostId = p.Id,
                            Title = p.Title,
                            Content = p.Content,
                            CreatedAt = p.CreatedAt,
                            CategoryId = p.CategoryId,
                            UserId = p.UserId
                        })
                    })
                });
            }catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An Error Ouccered While Handling Data",
                    Error = ex.Message
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (Category is null )
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",
                        
                    });
                }
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Data Retrived Successfully ",
                    Data = Category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An Error Ouccered While Handling Data",
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTo categoryDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                var Category = new Category
                {
                    Name = categoryDTo.Name,
                };
                await _unitOfWork.Categories.CreateAsync(Category);
                var Result = await _unitOfWork.SaveAsync();
                if (Result > 0)
                    return StatusCode(201, new
                    {
                        StatusCode = 201,
                        Message = "Data Added Successfully",

                    });
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Data Not Added",

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An Error Ouccered While Handling Data",
                    Error = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDTo categoryDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                var OldCategory = await _unitOfWork.Categories.GetByIdAsync(categoryDTo.Id);
                if (OldCategory is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",
                        
                    });
                // Set new Data
                OldCategory.Name = categoryDTo.Name;
                // Update
                _unitOfWork.Categories.Update(OldCategory);
                var Result = await _unitOfWork.SaveAsync();
                if (Result > 0)
                    return StatusCode(200, new
                    {
                        StatusCode = 200,
                        Message = "Data Updated Successfully",

                    });
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Data Not Updated",

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An Error Ouccered While Handling Data",
                    Error = ex.Message
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var oldCategory = await _unitOfWork.Categories.GetByIdAsync(id);
                if (oldCategory is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",

                    });
                _unitOfWork.Categories.Delete(oldCategory);
                var Result = await _unitOfWork.SaveAsync();
                if (Result > 0)
                    return StatusCode(200, new
                    {
                        StatusCode = 200,
                        Message = "Data Deleted Successfully",

                    });
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Data Not Deleted",

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An Error Ouccered While Handling Data",
                    Error = ex.Message
                });
            }
        }
    }
}
