using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Comments = await _unitOfWork.Comments.GetAllAsync();
                if (Comments is null || !Comments.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",
                        Data = new List<Comment>()
                    });
                }
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Data Retrived Successfully ",
                    Data = Comments
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
                var Comment = await _unitOfWork.Comments.GetByIdAsync(id);
                if (Comment is null)
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
                    Data = Comment
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
        public async Task<IActionResult> Create(CommentDTo CommentDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Comment Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                var Comment = new Comment
                {
                    CreatedAt = DateTime.Now,
                    Content = CommentDTo.Content,
                    PostId = CommentDTo.PostId,
                    UserId = CommentDTo.UserId,
                    
                };
                await _unitOfWork.Comments.CreateAsync(Comment);
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
        public async Task<IActionResult> Update(CommentDTo CommentDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Comment Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                var OldComment = await _unitOfWork.Comments.GetByIdAsync(CommentDTo.Id);
                if (OldComment is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",

                    });
                // Set new Data
                OldComment.Content = CommentDTo.Content;
                // Update
                _unitOfWork.Comments.Update(OldComment);
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
                var oldComment = await _unitOfWork.Comments.GetByIdAsync(id);
                if (oldComment is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",

                    });
                _unitOfWork.Comments.Delete(oldComment);
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
