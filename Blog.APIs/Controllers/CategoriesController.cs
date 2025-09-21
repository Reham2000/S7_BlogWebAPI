using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.APIs.Controllers
{
    [Route("V1.3/[controller]")]
    // Root URL => https://localhost:7053
    // Base URL => Root + Route => https://localhost:7053/V1.3/Categories
    // Spatial URL  => https://localhost:7053/V1.3/Categories/GetByUserId/{id}
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //private readonly ICategoryService _category;
        //public CategoriesController(ICategoryService category)
        //{
        //    _category = category;
        //}
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("Old")] // https://localhost:7053/V1.3/Categories/Old
        public async Task<IEnumerable<Category>> GetAll2()
        {
            var Categories = await _unitOfWork.CategoryService.GetAllAsync();
            if (Categories == null || !Categories.Any())
                return new List<Category>();
            return Categories;
        }


        [HttpGet] // https://localhost:7053/V1.3/Categories
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Categories = await _unitOfWork.CategoryService.GetAllAsync();
                if(Categories == null || !Categories.Any())
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "No Categories Found",
                        Data = new List<Category>()
                    }); // Status 404
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Categories Retrived Successfully",
                    Data = Categories
                });

            }catch(Exception ex){
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occured while Retriving Data",
                    Error = ex.Message
                });
            } 
        }

        [HttpGet("{id}")]// // https://localhost:7053/V1.3/Categories/{id}
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Category = await _unitOfWork.CategoryService.GetByIdAsync(id);
                if (Category is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category With Id {id} Not Found",
                    });
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category Retrived Successfully",
                    Data = Category
                });

            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occured while Retriving Data",
                    Error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTo categoryDto)
        {
            try
            {
                // validate with dataanotation
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode= StatusCodes.Status400BadRequest,
                        Message = "Invalid Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                var Category = new Category
                {
                    Name = categoryDto.Name,
                };
                await _unitOfWork.CategoryService.CreateAsync(Category);
                return StatusCode(201, new
                {
                    StatusCode = 201,
                    Message = "Category Created Successfully"
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occured while Retriving Data",
                    Error = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDTo categoryDTo)
        {
            try
            {
                var OldCategory = await _unitOfWork.CategoryService.GetByIdAsync(categoryDTo.Id);
                if (OldCategory == null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "No Category Found"
                       
                    });
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                OldCategory.Name = categoryDTo.Name;
                await _unitOfWork.CategoryService.UpdateAsync(OldCategory);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category Updated Successfully",
                    Data = OldCategory
                    //Data = await _unitOfWork.CategoryService.GetByIdAsync(categoryDTo.Id)
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occured while Updateing Data",
                    Error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id ,CategoryDTo categoryDTo)
        {
            try
            {
                var OldCategory = await _unitOfWork.CategoryService.GetByIdAsync(id);
                if (OldCategory == null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "No Category Found"

                    });
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                OldCategory.Name = categoryDTo.Name;
                await _unitOfWork.CategoryService.UpdateAsync(id,OldCategory);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category Updated Successfully",
                    Data = OldCategory
                    //Data = await _unitOfWork.CategoryService.GetByIdAsync(categoryDTo.Id)
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occured while Updateing Data",
                    Error = ex.Message
                });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var Category = await _unitOfWork.CategoryService.GetByIdAsync(id);
                if (Category == null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category With Id {id} Not Found"
                    });
                await _unitOfWork.CategoryService.DeleteAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category Deleted Successfully",
                    DeletedData = Category
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An Error Occured while Deleteing Data",
                    Error = ex.Message
                });
            }
        }
    }
}
