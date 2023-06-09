﻿using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using auth.Model.DTO;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _service;
        public BrandsController(IBrandService service)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpGet("GetBrands")]
        public IActionResult GetBrand()
        {
            var brands = _service.GetBrands();
            return Ok(brands);
        }
        [HttpPost]
        public IActionResult AddBrand(string name)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.AddBrand(name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateBrand(int id, BrandDTO brand)
        {
            try
            {
                if (brand == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.UpdateBrand(id, brand);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteBrand(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.DeleteBrand(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
