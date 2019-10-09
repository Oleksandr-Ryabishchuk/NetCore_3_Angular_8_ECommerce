using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigProject_V_2.BusinessLayer.Interfaces;
using BigProject_V_2.DataAccessLayer.Data;
using BigProject_V_2.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigProject_V_2.Controllers
{
    [Route("api/product")]
    [ApiController]    
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;       
        public ProductController(IProductManager productManager)
        {            
            _productManager = productManager;
        }
        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productManager.GetAllProducts();
          
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest("Щось пішло не так. Якщо Вам потрібна докладна інформація або прайс-лист напишіть нам на email spicyco88@gmail.com");
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody]ProductModel model)
        {
            var result = await _productManager.AddProduct(model);

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Щось пішло не так. зверніться до оператора технічної підтримки");
        }

        
        [HttpPut]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("UpdateProduct/{id}")]       
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductModel model)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var result = await _productManager.UpdateProduct(id, model);

            if (result != null)
            {
                return Ok(new JsonResult("The Product with id " + id + " is updated"));
            }
            return BadRequest("Щось пішло не так. зверніться до оператора технічної підтримки");
        }

        [HttpDelete]
        [Authorize(Policy = "RequireAdministratorRole")]
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productManager.DeleteProduct(id);
            if (result != null)
            {
                return Ok(new JsonResult("The Product with id " + id + " is deleted"));
            }
            return BadRequest("Щось пішло не так. зверніться до оператора технічної підтримки");
        }
       
    }
}