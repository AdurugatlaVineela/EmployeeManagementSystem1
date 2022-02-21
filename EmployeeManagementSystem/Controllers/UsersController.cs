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
    public class UsersController : Controller
    {
        string userName;
        int userId;
        ICRUDRepository<User, int> _repository;
        public SendServiceBusMessage _SendServiceBusMessage;
        public UsersController(ICRUDRepository<User, int> repository, SendServiceBusMessage sendServiceBusMessage)
        {
            _repository = repository;
            _SendServiceBusMessage = sendServiceBusMessage;
        }
        public ActionResult<IEnumerable<User>> Get()
        {
            try
            {
                var items = _repository.GetAll();
                //return items.ToList();
                return View(items);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // [HttpGet("Users")]
        // public Models.User GetUser()
        // {
        //     Models.User obj = new Models.User{
        //         Id=1,
        //         UserName="Jyo",
        //         UserAddress="Banglore",
        //         UserType="No",
        //         Password="AH6SKJ",
        //     };
        //    return obj;  
        // } 

             [HttpGet("{id}")]
           public ActionResult<User> GetDetails(int id)
           {
                try
                {
                    var item = _repository.GetDetails(id);
                    if (item == null)
                        return NotFound();

                    return item;
                }
                catch(Exception ex)
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
        
           public async Task <ActionResult<User>> Create(User usr)
           {
            userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            userId = Convert.ToInt32("0" + HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = Convert.ToString(HttpContext.User.FindFirst(ClaimTypes.Role).Value);
                if(role!="newemployee") 
                {
                    return Unauthorized();
                }
                try
               { 
                    if(userId==0) return BadRequest();
                       if(usr==null)
                       return BadRequest();

                   _repository.Create(usr);

                    await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                    {
                        UserName = usr.UserName,
                        action = "Create",
                        actionMessage = "User Created Successfully"
                    });
                    return Redirect("/Users/Get");
                }
                catch (Exception ex)
                {
                    throw;
                }
                   return usr;
    
               
           } 

           //[HttpPut("update/{id}")]
        public IActionResult update(int Id)
        {
            User usr = _repository.GetDetails(Id);
            if (usr == null)
            {
                return BadRequest();
            }
            else
            {
                return View(usr);
            }
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]

        [HttpPost]
          public async Task <ActionResult<User>> update(int id,User usr)
           {

                userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                userId = Convert.ToInt32("0" + HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = Convert.ToString(HttpContext.User.FindFirst(ClaimTypes.Role).Value);
                if (role != "admin")
                {
                    return Unauthorized();
                }
                if (userId == 0) return BadRequest();
            // if(id==0)  return BadRequest();
                try
                {
                    if (usr == null)
                        return BadRequest();
                    if (id == 0) return BadRequest();
                    _repository.update(usr);
                    await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                    {
                        UserName = usr.UserName,
                        action = "update",
                        actionMessage = "User Updated Successfully"
                    });
                    ViewBag.Message = string.Format("Updated Successfully");
                    return View(usr);
                }
                catch (Exception ex)
                   {
                        throw;
                 }
                    
                
           
          }
            //[HttpDelete("remove/{id}")]

        public IActionResult Delete(int Id)
        {
            User usr = _repository.GetDetails(Id);
            if (usr == null)
            {
                return BadRequest();
            }
            else
            {
                return View(usr);
            }
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]
        [HttpPost]
         public async Task <ActionResult<User>> Delete(int id, User usr)
           {
                userName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                userId = Convert.ToInt32("0" + HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var role = Convert.ToString(HttpContext.User.FindFirst(ClaimTypes.Role).Value);
                if(role!="admin") 
                {
                    return Unauthorized();
                }
                if(userId==0) return BadRequest();
                try
               { 
                       _repository.Delete(id);
                    await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                    {
                        UserName = usr.UserName,
                        action = "Delete",
                        actionMessage = "User Deleted Successfully"
                    });
                    ViewBag.Message = string.Format("User Deleted Successfully");
                    return Redirect("/Users/Get");
               }
                catch (Exception ex)
                {
                    throw;
                }
               // return Ok();
           
           }

        
    }
}