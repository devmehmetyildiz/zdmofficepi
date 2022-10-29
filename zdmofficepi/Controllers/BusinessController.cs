using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zdmofficepi.DataAccess;
using zdmofficepi.Models;
using zdmofficepi.Utils;

namespace zdmofficepi.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDBContext _context;
        UnitofWork UnitofWork;
        PasswordUtils PasswordUtils;

        public BusinessController(IConfiguration configuration, ILogger<AuthController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            UnitofWork = new UnitofWork(context);
            PasswordUtils = new PasswordUtils();
        }

        [HttpGet]
        [Route("Categories/GetAll")]
        public IActionResult CategoriesGetAll()
        {
            return Ok(UnitofWork.CategoriesRepository.GetRecords<CategoryModel>(u => u.IsActive).ToList());
        }

        [HttpGet]
        [Route("Productgroup/GetAll")]
        public IActionResult ProductgroupGetAll()
        {
            return Ok(UnitofWork.ProductgroupRepository.GetRecords<ProductgroupModel>(u => u.IsActive).ToList());
        }

        [HttpGet]
        [Route("Product/GetAll")]
        public IActionResult ProductGetAll()
        {
            return Ok(UnitofWork.ProductRepositroy.GetRecords<ProductModel>(u => u.IsActive).ToList());
        }

        [HttpGet]
        [Route("Subcategories/GetAll")]
        public IActionResult SubcategoriesGetAll()
        {
            return Ok(UnitofWork.SubcategoriesRepositroy.GetRecords<SubcategoryModel>(u => u.IsActive).ToList());
        }

        [HttpGet]
        [Route("Users/GetAll")]
        public IActionResult UsersGetAll()
        {
            return Ok(UnitofWork.UserRepositroy.GetRecords<UserModel>(u => u.IsActive).ToList());
        }

        [HttpGet]
        [Route("Categories/GetSelected")]
        public IActionResult CategoriesGetselected(string guid)
        {
            CategoryModel Data = UnitofWork.CategoriesRepository.GetSingleRecord<CategoryModel>(u => u.Uuid == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [HttpGet]
        [Route("Subcategories/GetSelected")]
        public IActionResult SubcategoriesGetselected(string guid)
        {
            SubcategoryModel Data = UnitofWork.SubcategoriesRepositroy.GetSingleRecord<SubcategoryModel>(u => u.Uuid == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [HttpGet]
        [Route("Productgroups/GetSelected")]
        public IActionResult ProductgroupsGetselected(string guid)
        {
            ProductgroupModel Data = UnitofWork.ProductgroupRepository.GetSingleRecord<ProductgroupModel>(u => u.Uuid == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [HttpGet]
        [Route("Products/GetSelected")]
        public IActionResult ProductsGetselected(string guid)
        {
            ProductModel Data = UnitofWork.ProductRepositroy.GetSingleRecord<ProductModel>(u => u.Uuid == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("Categories/Add")]
        [HttpPost]
        public IActionResult CategoriesAdd(CategoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Createduser = username;
            model.IsActive = true;
            model.Createdtime = DateTime.Now;
            model.Uuid = Guid.NewGuid().ToString();
            UnitofWork.CategoriesRepository.Add(model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Productgroup/Add")]
        [HttpPost]
        public IActionResult ProductgroupAdd(ProductgroupModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Createduser = username;
            model.IsActive = true;
            model.Createdtime = DateTime.Now;
            model.Uuid = Guid.NewGuid().ToString();
            UnitofWork.ProductgroupRepository.Add(model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Product/Add")]
        [HttpPost]
        public IActionResult ProductAdd(ProductModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Createduser = username;
            model.IsActive = true;
            model.Createdtime = DateTime.Now;
            model.Uuid = Guid.NewGuid().ToString();
            UnitofWork.ProductRepositroy.Add(model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Subcategories/Add")]
        [HttpPost]
        public IActionResult SubcategoriesAdd(SubcategoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Createduser = username;
            model.IsActive = true;
            model.Createdtime = DateTime.Now;
            model.Uuid = Guid.NewGuid().ToString();
            UnitofWork.SubcategoriesRepositroy.Add(model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Categories/Update")]
        [HttpPut]
        public IActionResult Categoriesupdate(CategoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Updateduser = username;
            model.Updatetime = DateTime.Now;
            UnitofWork.CategoriesRepository.update(UnitofWork.CategoriesRepository.GetSingleRecord<CategoryModel>(u=>u.Uuid==model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Productgroup/Update")]
        [HttpPut]
        public IActionResult Productgroupupdate(ProductgroupModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Updateduser = username;
            model.Updatetime = DateTime.Now;
            UnitofWork.ProductgroupRepository.update(UnitofWork.ProductgroupRepository.GetSingleRecord<ProductgroupModel>(u => u.Uuid == model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Product/Update")]
        [HttpPut]
        public IActionResult Productupdate(ProductModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Updateduser = username;
            model.Updatetime = DateTime.Now;
            UnitofWork.ProductRepositroy.update(UnitofWork.ProductRepositroy.GetSingleRecord<ProductModel>(u => u.Uuid == model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Subcategories/Update")]
        [HttpPut]
        public IActionResult Subcategoriesupdate(SubcategoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Updateduser = username;
            model.Updatetime = DateTime.Now;
            UnitofWork.SubcategoriesRepositroy.update(UnitofWork.SubcategoriesRepositroy.GetSingleRecord<SubcategoryModel>(u => u.Uuid == model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Categories/Delete")]
        [HttpDelete]
        public IActionResult Categoriesdelete(CategoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Deleteuser = username;
            model.Deletetime = DateTime.Now;
            model.IsActive = false;
            List<SubcategoryModel> Subcategories = UnitofWork.SubcategoriesRepositroy.GetRecords<SubcategoryModel>(u => u.Categoryuui == model.Uuid);
            foreach (var item in Subcategories)
            {
                item.Deleteuser = username;
                item.Deletetime = DateTime.Now;
                item.IsActive = false;
                UnitofWork.SubcategoriesRepositroy.update(UnitofWork.SubcategoriesRepositroy.GetSingleRecord<SubcategoryModel>(u => u.Uuid == item.Uuid), item);
            }
            UnitofWork.CategoriesRepository.update(UnitofWork.CategoriesRepository.GetSingleRecord<CategoryModel>(u => u.Uuid == model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Productgroup/Delete")]
        [HttpDelete]
        public IActionResult Productgroupdelete(ProductgroupModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Deleteuser = username;
            model.Deletetime = DateTime.Now;
            model.IsActive = false;
            List<ProductModel> Products = UnitofWork.ProductRepositroy.GetRecords<ProductModel>(u => u.Groupuui == model.Uuid);
            foreach (var item in Products)
            {
                item.Deleteuser = username;
                item.Deletetime = DateTime.Now;
                item.IsActive = false;
                UnitofWork.ProductRepositroy.update(UnitofWork.ProductRepositroy.GetSingleRecord<ProductModel>(u => u.Uuid == item.Uuid), item);
            }
            UnitofWork.ProductgroupRepository.update(UnitofWork.ProductgroupRepository.GetSingleRecord<ProductgroupModel>(u => u.Uuid == model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Product/Delete")]
        [HttpDelete]
        public IActionResult Productdelete(ProductModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Deleteuser = username;
            model.Deletetime = DateTime.Now;
            model.IsActive = false;
            UnitofWork.ProductRepositroy.update(UnitofWork.ProductRepositroy.GetSingleRecord<ProductModel>(u => u.Uuid == model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Subcategories/Delete")]
        [HttpDelete]
        public IActionResult Subcategoriesdelete(SubcategoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Deleteuser = username;
            model.Deletetime = DateTime.Now;
            model.IsActive = false;
            UnitofWork.SubcategoriesRepositroy.update(UnitofWork.SubcategoriesRepositroy.GetSingleRecord<SubcategoryModel>(u => u.Uuid == model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }
    }
}
