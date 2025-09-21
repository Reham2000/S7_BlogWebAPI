using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Posts = await _unitOfWork.Posts.GetAllAsync();
                if (Posts is null || !Posts.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",
                        Data = new List<Post>()
                    });
                }
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Data Retrived Successfully ",
                    Data = Posts
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Post = await _unitOfWork.Posts.GetByIdAsync(id);
                if (Post is null)
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
                    Data = Post
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
        public async Task<IActionResult> Create(PostDTo PostDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Post Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                var Post = new Post
                {
                    Title = PostDTo.Title,
                    CreatedAt = DateTime.Now,
                    Content = PostDTo.Content,
                    CategoryId = PostDTo.CategoryId,
                    UserId = PostDTo.UserId,
                };
                await _unitOfWork.Posts.CreateAsync(Post);
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
        public async Task<IActionResult> Update(PostDTo PostDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Post Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                var OldPost = await _unitOfWork.Posts.GetByIdAsync(PostDTo.Id);
                if (OldPost is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",

                    });
                // Set new Data
                OldPost.Title = PostDTo.Title;
                OldPost.Content = PostDTo.Content;
                OldPost.CategoryId = PostDTo.CategoryId;
                // Update
                _unitOfWork.Posts.Update(OldPost);
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
                var oldPost = await _unitOfWork.Posts.GetByIdAsync(id);
                if (oldPost is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",

                    });
                _unitOfWork.Posts.Delete(oldPost);
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
