using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private ISupplierService _service;

        public SuppliersController(ISupplierService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [HttpPost]
        public IActionResult Add(Supplier request)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.Create(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(Supplier request, int id)
        {
            try
            {
                if (!ModelState.IsValid || request == null)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.Update(request, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}