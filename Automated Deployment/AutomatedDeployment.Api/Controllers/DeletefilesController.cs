using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutomatedDeployment.Api.Services;



namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]/{hubid}/{appid}")]
    [ApiController]
    public class DeletefilesController : ControllerBase
    {
        private readonly IDeletefilesrepository deletefilesrepository;
        public DeletefilesController(IDeletefilesrepository _deletefilesrepository)
        {
            deletefilesrepository = _deletefilesrepository;
        }
        [HttpGet]
        public IActionResult GetALL(int hubid, int appid)
        {
            try
            {
                var result = deletefilesrepository.GetAllfiles(hubid, appid);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        [HttpDelete]
        public IActionResult Delete(int hubid, int appid, List<IFormFile> files)
        {
            try
            {
                deletefilesrepository.Deletefiles(hubid, appid, files);
                return Ok();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
    }
}
