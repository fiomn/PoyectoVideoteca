using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplicationSearch.Data;
using WebApplicationSearch.Models;

namespace WebApplicationSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private TestUCRContext db = new TestUCRContext();

        [HttpGet("{inputSearch}")]
        public ActionResult<string> Get(string inputSearch)
        {
            try
            {
                var movies = new List<tb_MOVIE>();
                movies = db.tb_MOVIE.FromSqlRaw(@"exec getMovieByNG @inputSearch", new SqlParameter("@inputSearch", inputSearch)).ToList();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
