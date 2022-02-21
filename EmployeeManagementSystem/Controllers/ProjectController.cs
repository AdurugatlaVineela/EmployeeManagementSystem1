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
    public class ProjectsController : Controller
    {
        string userName;
        int userId;
        ICRUDRepository<Project, int> _repository;
        public SendServiceBusMessage _SendServiceBusMessage;
        public ProjectsController(ICRUDRepository<Project, int> repository, SendServiceBusMessage sendServiceBusMessage)
        {
            _repository = repository;
            _SendServiceBusMessage = sendServiceBusMessage;
        }
                public ActionResult<IEnumerable<Project>> Get()
                {
                   var items = _repository.GetAll();
                   return View(items);
                }

        //  [HttpGet("project")]
        //         public Models.Projects GetProject()
        //         {
        //             Models.Projects obj = new Models.Projects{
        //                 ProjectId=1,
        //                 ProjectName="R",
        //                 ProjectDescription="SSSS",
        //             };
        //            return obj;  

        [HttpGet("{id}")]
        public ActionResult<Project> GetDetails(int id)
        {
            try
            {
                var item = _repository.GetDetails(id);
                if (item == null)
                    return NotFound();

                return item;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

          //[HttpPost("addnew")]
        public IActionResult Create()
        {
            return View();
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]

        [HttpPost]

        public async Task <ActionResult<Project>> Create(Project prj)
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

                if (prj == null)
                    return BadRequest();
                _repository.Create(prj);
                await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                {
                    ProjectName = prj.ProjectName,
                    action = "Create",
                    actionMessage = "Project Created Successfully"
                });
                return Redirect("/Projects/Get");
                return View(prj);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // [HttpPut("update/{id}")]
        public IActionResult update(int Id)
        {
            Project prj = _repository.GetDetails(Id);
            if (prj == null)
            {
                return BadRequest();
            }
            else
            {
                return View(prj);
            }
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]

        [HttpPost]

        public async Task <ActionResult<Project>> update(int id, Project prj)
          {
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

                if (prj == null)
                    return BadRequest();
                if (id == 0) return BadRequest();
                _repository.update(prj);
                await _SendServiceBusMessage.sendServiceBusMessage(new ServiceBusMessageData
                {
                    ProjectName = prj.ProjectName,
                    action = "update",
                    actionMessage = "project Updated Successfully"
                });
                ViewBag.Message = string.Format("Updated Successfully");
                return View(prj);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IActionResult Delete(int Id)
        {
            Project prj = _repository.GetDetails(Id);
            if (prj == null)
            {
                return BadRequest();
            }
            else
            {
                return View(prj);
            }
        }
        [Microsoft.AspNetCore.Authorization.Authorize()]
        [HttpPost]
        public async Task<ActionResult<Project>> Delete(int id, Project prj)
        {
            ;
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
                    ProjectName = prj.ProjectName,
                    action = "Delete",
                    actionMessage = "Project Deleted Successfully"
                });
                ViewBag.Message = string.Format("Project Deleted Successfully");
                return Redirect("/projects/Get");
            }
            catch (Exception ex)
            {
                throw;
            }

        }


    }
}