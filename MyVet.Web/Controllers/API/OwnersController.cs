using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVet.Common.Models;
using MyVet.Web.Data;

namespace MyVet.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OwnersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("GetOwnerByEmail")] //Se convirtio este metodo get a post por cuestiones de seguridad.
        //route se utiliza para darle otro nombre al metodo ya que los apis se buscan por GET POST etc.
        public async Task<IActionResult> GetOwner(EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var owner = await _dataContext.Owners
                .Include(o => o.Agendas)
                .Include(o => o.Pets)
                .Include(o => o.User)
                .FirstOrDefaultAsync(
                o => o.User.Email == emailRequest.Email);

            if (owner == null)
            {
                return NotFound();
            }

            return Ok(owner);
        }
    }
}