using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        FileUtils fileUtils;
        public BusinessController(IConfiguration configuration, ILogger<AuthController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            UnitofWork = new UnitofWork(context);
            PasswordUtils = new PasswordUtils();
            fileUtils = new FileUtils(context);
        }

        [HttpGet]
        [Route("Users/GetAll")]
        public IActionResult UsersGetAll()
        {
            return Ok(UnitofWork.UserRepositroy.GetRecords<UserModel>(u => u.IsActive).ToList());
        }

        #region Product

        [HttpGet]
        [Route("Products/GetAll")]
        public IActionResult ProductGetAll()
        {
            var data = UnitofWork.ProductRepositroy.GetRecords<ProductModel>(u => u.IsActive).ToList();
            foreach (var item in data)
            {
                item.Productgroup = UnitofWork.ProductgroupRepository.GetSingleRecord<ProductgroupModel>(u => u.Uuid == item.Groupuui);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("Products/GetSelected")]
        public IActionResult ProductsGetselected(string guid)
        {
            ProductModel Data = UnitofWork.ProductRepositroy.GetSingleRecord<ProductModel>(u => u.Uuid == guid);
            Data.Productgroup = UnitofWork.ProductgroupRepository.GetSingleRecord<ProductgroupModel>(u => u.Uuid == Data.Groupuui);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("Products/Add")]
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

        [Route("Products/Update")]
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

        [Route("Products/Delete")]
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

        #endregion

        #region Productgroup
        [HttpGet]
        [Route("Productgroups/GetAll")]
        public IActionResult ProductgroupGetAll()
        {
            var data = UnitofWork.ProductgroupRepository.GetRecords<ProductgroupModel>(u => u.IsActive).ToList();
            foreach (var item in data)
            {
                item.Products = UnitofWork.ProductRepositroy.GetRecords<ProductModel>(u => u.Groupuui == item.Uuid && u.IsActive);
                item.Category = UnitofWork.CategoriesRepository.GetSingleRecord<CategoryModel>(u => u.Uuid == item.Categoryuuid);
                item.Subcategory = UnitofWork.SubcategoriesRepositroy.GetSingleRecord<SubcategoryModel>(u => u.Uuid == item.Subcategoryuuid);
                item.Company = UnitofWork.CompanyRepository.GetSingleRecord<CompanyModel>(u => u.Uuid == item.Companyuuid);

            }
            return Ok(data);
        }

        [HttpGet]
        [Route("Productgroups/GetSelected")]
        public IActionResult ProductgroupsGetselected(string guid)
        {
            ProductgroupModel Data = UnitofWork.ProductgroupRepository.GetSingleRecord<ProductgroupModel>(u => u.Uuid == guid);
            Data.Category = UnitofWork.CategoriesRepository.GetSingleRecord<CategoryModel>(u => u.Uuid == Data.Categoryuuid);
            Data.Products = UnitofWork.ProductRepositroy.GetRecords<ProductModel>(u => u.Groupuui == Data.Uuid && u.IsActive);
            Data.Subcategory = UnitofWork.SubcategoriesRepositroy.GetSingleRecord<SubcategoryModel>(u => u.Uuid == Data.Subcategoryuuid);
            Data.Company = UnitofWork.CompanyRepository.GetSingleRecord<CompanyModel>(u => u.Uuid == Data.Companyuuid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [HttpGet]
        [Route("Productgroups/GetFiles")]
        public IActionResult ProductgroupsGetFiles(string guid)
        {
            List<FileModel> files = new List<FileModel>();
            var products = UnitofWork.ProductRepositroy.GetRecords<ProductModel>(u => u.Groupuui == guid);
            foreach (var item in products)
            {
                files.AddRange(UnitofWork.FileRepository.GetRecords<FileModel>(u => u.Productuui == item.Uuid));
                
            }
            return Ok(files);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("Products/GetImage")]
        public IActionResult ProducsGetImage(string guid)
        {
            FileModel Data = UnitofWork.FileRepository.GetSingleRecord<FileModel>(u=>u.Productuui==guid);
            if (Data != null)
                return File(fileUtils.GetFile(Data), Data.Filetype);
            else
                return NotFound();
        }

        [Route("Productgroups/Add")]
        [HttpPost]
        public IActionResult ProductgroupAdd(ProductgroupModel model)
        {
            List<ProductresponseModel> response = new List<ProductresponseModel>();
            string groupuuid = Guid.NewGuid().ToString();
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Createduser = username;
            model.IsActive = true;
            model.Createdtime = DateTime.Now;
            model.Uuid = groupuuid;
            UnitofWork.ProductgroupRepository.Add(model);
            foreach (var item in model.Products)
            {
                item.Id = 0;
                string productuuid = Guid.NewGuid().ToString();
                item.Groupuui = groupuuid;
                item.Createduser = username;
                item.IsActive = true;
                item.Createdtime = DateTime.Now;
                item.Uuid = productuuid;
                UnitofWork.ProductRepositroy.Add(item);
                response.Add(new ProductresponseModel { Productname = item.Name, Productuuid = productuuid });
            }
            UnitofWork.Complate();
            return Ok(response);
        }

        [Route("Productgroups/Update")]
        [HttpPut]
        public IActionResult Productgroupupdate(ProductgroupModel model)
        {
            List<ProductresponseModel> response = new List<ProductresponseModel>();
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Updateduser = username;
            model.Updatetime = DateTime.Now;
            UnitofWork.ProductgroupRepository.update(UnitofWork.ProductgroupRepository.GetSingleRecord<ProductgroupModel>(u => u.Uuid == model.Uuid), model);
            foreach (var item in model.Products)
            {
                if (string.IsNullOrEmpty(item.Uuid))
                {
                    item.Id = 0;
                    string productuuid = Guid.NewGuid().ToString();
                    item.Createduser = username;
                    item.IsActive = true;
                    item.Createdtime = DateTime.Now;
                    item.Uuid = productuuid;
                    item.Groupuui = model.Uuid;
                    UnitofWork.ProductRepositroy.Add(item);
                    response.Add(new ProductresponseModel { Productname = item.Name, Productuuid = productuuid });
                }
                else
                {
                    if (item.IsDataChanged)
                    {
                        item.Updateduser = username;
                        item.Updatetime = DateTime.Now;
                        if (!item.IsActive)
                        {
                            FileModel filemodel = UnitofWork.FileRepository.GetSingleRecord<FileModel>(u => u.Productuui == item.Uuid);
                            if(filemodel != null)
                            {
                                filemodel.Deleteuser = username;
                                filemodel.IsActive = false;
                                filemodel.Deletetime = DateTime.Now;
                                if (fileUtils.DeleteFile(filemodel))
                                {
                                    UnitofWork.FileRepository.update(UnitofWork.FileRepository.GetSingleRecord<FileModel>(u => u.Uuid == filemodel.Uuid), filemodel);
                                }
                            }
                        }
                        UnitofWork.ProductRepositroy.update(UnitofWork.ProductRepositroy.GetSingleRecord<ProductModel>(u => u.Uuid == item.Uuid), item);
                    }
                    if (item.IsFileChanged && item.IsActive)
                    {
                        response.Add(new ProductresponseModel { Productname = item.Name, Productuuid = item.Uuid,IsFileUpdate=true });
                    }
                }
            }
            UnitofWork.Complate();
            return Ok(response);
        }

        [Route("Productgroups/Delete")]
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

        #endregion

        #region Company
        [HttpGet]
        [Route("Company/GetAll")]
        public IActionResult CompanyGetAll()
        {
            return Ok(UnitofWork.CompanyRepository.GetRecords<CompanyModel>(u => u.IsActive).ToList());
        }
        [HttpGet]
        [Route("Company/GetSelected")]
        public IActionResult CompanyGetselected(string guid)
        {
            CompanyModel Data = UnitofWork.CompanyRepository.GetSingleRecord<CompanyModel>(u => u.Uuid == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }
        [Route("Company/Add")]
        [HttpPost]
        public IActionResult CompanyAdd(CompanyModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Createduser = username;
            model.IsActive = true;
            model.Createdtime = DateTime.Now;
            model.Uuid = Guid.NewGuid().ToString();
            UnitofWork.CompanyRepository.Add(model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Company/Update")]
        [HttpPut]
        public IActionResult Companyupdate(CompanyModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Updateduser = username;
            model.Updatetime = DateTime.Now;
            UnitofWork.CompanyRepository.update(UnitofWork.CompanyRepository.GetSingleRecord<CompanyModel>(u => u.Uuid == model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }

        [Route("Company/Delete")]
        [HttpDelete]
        public IActionResult Companydelete(CompanyModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Deleteuser = username;
            model.Deletetime = DateTime.Now;
            model.IsActive = false;
            UnitofWork.CompanyRepository.update(UnitofWork.CompanyRepository.GetSingleRecord<CompanyModel>(u => u.Uuid == model.Uuid), model);
            UnitofWork.Complate();
            return Ok();
        }
        #endregion

        #region Categories

        [HttpGet]
        [Route("Categories/GetAll")]
        public IActionResult CategoriesGetAll()
        {
            return Ok(UnitofWork.CategoriesRepository.GetRecords<CategoryModel>(u => u.IsActive).ToList());
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

        [Route("Categories/Update")]
        [HttpPut]
        public IActionResult Categoriesupdate(CategoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.Updateduser = username;
            model.Updatetime = DateTime.Now;
            UnitofWork.CategoriesRepository.update(UnitofWork.CategoriesRepository.GetSingleRecord<CategoryModel>(u => u.Uuid == model.Uuid), model);
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

        #endregion

        #region subcategories 
        [HttpGet]
        [Route("Subcategories/GetAll")]
        public IActionResult SubcategoriesGetAll()
        {
            var data = UnitofWork.SubcategoriesRepositroy.GetRecords<SubcategoryModel>(u => u.IsActive).ToList();
            foreach (var item in data)
            {
                item.Category = UnitofWork.CategoriesRepository.GetSingleRecord<CategoryModel>(u => u.Uuid == item.Categoryuui);
            }
            return Ok(data);
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
            Data.Category = UnitofWork.CategoriesRepository.GetSingleRecord<CategoryModel>(u => u.Uuid == Data.Categoryuui);
            return Ok(Data);
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
        #endregion


      
    }
}
