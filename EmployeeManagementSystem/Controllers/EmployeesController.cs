using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Infrastructure;
using System.Security.Claims;
//using Microsoft.AspNetCore.Diagnostics;

namespace EmployeeManagementSystem.Controllers
{
    /*******************   CHANGED:  ******************************
Removed the Authorize attribute from here and moved it the  methods 
where the restrictions are required. 
******************************************************/
    public class EmployeesController : Controller
    {
        string userName;
        int userId;
        ICRUDRepository<Employee, int> _repository;
        public SendServiceBusMessage _SendServiceBusMessage;
        public EmployeesController(ICRUDRepository<Employee, int> repository, SendServiceBusMessage sendServiceBusMessage)
        {
            _repository = repository;
            _SendServiceBusMessage = sendServiceBusMessage;
        }
/************** CHANGES : ******************************************
* Add the Authorize attribute to the method 
* In the method, we are getting the Claim values like Name, NameIdentifier and Role 
* if the RoleName is not "admin" or any other role as required, 
*  then we are returning an Unauthorized() response back to the user.
* else valid response will be sent. 
********************************************************************/
        public ActionResult<IEnumerable<Employee>> Get()
        {
            try{
            var items = _repository.GetAll(); 
            return View(items);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        //URL: /api/employees/1   
        //try with id parameter values between 1 and 9 
        [HttpGet("{id}")]
        public ActionResult<Employee> GetDetails(int id) 
        {
            try{
            var  item = _repository.GetDetails(id);
            if( item==null )
                return NotFound();
            return item;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        // [HttpGet("employee")]
        // public Models.Employee GetEmployee()
        // {
        //     Models.Employee obj = new Models.Employee{
        //         EmployeeId= 12,
        //         FirstName = "RRR",
        //     };
        //     return obj;
        // }

        public IActionResult Create()
        {
            return View();
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]

        [HttpPost]
        public async Task<ActionResult<Employee>> Create(Employee emp)
        {
            if(!ModelState.IsValid)
            //throw new ArgumentException("Not found");
            userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            userId = Convert.ToInt32("0" + HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = Convert.ToString(HttpContext.User.FindFirst(ClaimTypes.Role).Value);
            if(role!="newemployee") 
            {
                return Unauthorized();
            } 
            if(userId==0) return BadRequest();
            try{
            if(emp==null) return BadRequest();
            _repository.Create(emp);
                await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                {
                    FirstName = emp.FirstName,
                    action = "Add",
                    actionMessage = "Employee Added Successfully"
                });
            return View();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public IActionResult update(int Id)
        {
            Employee emp = _repository.GetDetails(Id);
            if (emp == null)
            {
                return BadRequest();
            }
            else
            {
                return View(emp);
            }
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]
        [HttpPost]
        public async Task<ActionResult<Employee>> update(Employee emp)
        {
            userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            userId = Convert.ToInt32("0" + HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = Convert.ToString(HttpContext.User.FindFirst(ClaimTypes.Role).Value);
            if(role!="employee"&&role!="admin")
            { 
                return Unauthorized();
            }
            if(userId==0) return BadRequest();
            //if(emp==null)   return BadRequest();
            try{
            if(emp==null)
                return BadRequest();
            _repository.update(emp);
                await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                {
                    FirstName = emp.FirstName,
                    action = "Update",
                    actionMessage = "Employee Updated Successfully"
                });
                ViewBag.Message = string.Format("Updated Successfully");
            return View(emp);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public IActionResult Delete(int Id)
        {
            Employee emp = _repository.GetDetails(Id);
            if (emp == null)
            {
                return BadRequest();
            }
            else
            {
                return View(emp);
            }
        }


        //public IActionResult Delete()
        //{
        //    return View();
        //}


        [Microsoft.AspNetCore.Authorization.Authorize()]
        [HttpPost]
        public async Task <ActionResult<Employee>> Delete(int id,Employee emp)
        {
            userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            userId = Convert.ToInt32("0" + HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = Convert.ToString(HttpContext.User.FindFirst(ClaimTypes.Role).Value);
            if(role!="admin") 
            {
                return Unauthorized();
            }
            //end of the code inclusion. 
            if(userId==0) return BadRequest();
            try{
            _repository.Delete(id);
                await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                {
                    FirstName = emp.FirstName,
                    action = "Delete",
                    actionMessage = "Employee Deleted Successfully"
                });
                ViewBag.Message = string.Format("Deleted Successfully");
             return Redirect("/employees/Get");
        }
        catch(Exception ex)
            {
                throw;
            }
    
}
}
}