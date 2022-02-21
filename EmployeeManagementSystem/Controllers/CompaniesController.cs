using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Infrastructure; 
using System.Security.Claims;



namespace EmployeeManagementSystem.Controllers
{
     public class CompaniesController : Controller

    {
       string userName;
        int userId;
        ICRUDRepository<Company, int> _repository;
        public SendServiceBusMessage _SendServiceBusMessage;
        public CompaniesController( ICRUDRepository<Company, int> repository, SendServiceBusMessage sendServiceBusMessage)
        { 
        _repository = repository;
         _SendServiceBusMessage = sendServiceBusMessage;
        }
        public ActionResult<IEnumerable<Company>> Get()
        {
            try
            {
            var items = _repository.GetAll(); 
            return View(items);
            }
        catch(Exception ex)
            {
                throw;
            }
    
        }
        //Add the EFCore.SQLServer package 
        //dotnet add package Microsoft.EntityFrameworkCore.SqlServer


         //URL: /api/employees/1
         //try with id parameter values between 1 and 9

         [HttpGet("{id}")]
         public ActionResult<Company> GetDetails(int id)
        {
            try{
            var item = _repository.GetDetails(id);
            if(item==null)
            return NotFound();
            return item;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]

        [HttpPost]
        public async Task <ActionResult<Company>> Create(Company cmp)
       {
            if (!ModelState.IsValid)
                //throw new ArgumentException("Not found");
                userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
             userId = Convert.ToInt32("0" + HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = Convert.ToString(HttpContext.User.FindFirst(ClaimTypes.Role).Value);
            if (role != "admin")
            {
                return Unauthorized();
            }
            if (userId == 0) return BadRequest();
            try
            {
                if (cmp == null) return BadRequest();
                _repository.Create(cmp);
                await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                {
                    CompanyName = cmp.CompanyName,
                    action = "Create",
                    actionMessage = "Company Created Successfully"
                });
                return Redirect("/companies/Get");
            }
            catch (Exception ex)
            {
                throw;
            }
    
  
            
       }
        public IActionResult update(int Id)
        {
            Company cmp = _repository.GetDetails(Id);
            if (cmp == null)
            {
                return BadRequest();
            }
            else
            {
                return View(cmp);
            }
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]

       [HttpPost]
       public async Task <ActionResult<Company>> update(Company cmp)
       {
            userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            userId = Convert.ToInt32("0" + HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = Convert.ToString(HttpContext.User.FindFirst(ClaimTypes.Role).Value);
            if(role!="admin") 
            {
                return Unauthorized();
            }
            if(userId==0) return BadRequest();
            // if(id==0)  return BadRequest();
            try 
            { 
                if (cmp == null)
                    return BadRequest();
                _repository.update(cmp);
                    await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                    {
                        CompanyName = cmp.CompanyName,
                        action = "update",
                        actionMessage = "Company Updated Successfully"
                    });
                    ViewBag.Message = string.Format("Updated Successfully");
                return View(cmp);
             }
            catch (Exception ex)
            {
                throw;
            }

       }
        public IActionResult Delete(int Id)
        {
            Company cmp = _repository.GetDetails(Id);
            if (cmp == null)
            {
                return BadRequest();
            }
            else
            {
                return View(cmp);
            }
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]
        [HttpPost]
        public async Task <ActionResult<Company>> Delete(int id,Company cmp)
        {

            userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            userId = Convert.ToInt32("0" + HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = Convert.ToString(HttpContext.User.FindFirst(ClaimTypes.Role).Value);
            if (role != "admin")
            {
                return Unauthorized();
            }
            //end of the code inclusion. 
            if (userId == 0) return BadRequest();
            try
            {
                _repository.Delete(id);
                await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                {
                    CompanyName = cmp.CompanyName,
                    action = "Delete",
                    actionMessage = "Company Deleted Successfully"
                });
                ViewBag.Message = string.Format("Company Deleted Successfully");
                return Redirect("/companies/Get");
            }
            catch (Exception ex)
            {
                throw;
            }
    
        }


     }

}