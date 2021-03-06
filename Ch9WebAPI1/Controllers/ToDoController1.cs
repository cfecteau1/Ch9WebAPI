﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ch9WebAPI1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ch9WebAPI1.Controllers
{
    [Route("api/todo")]
    public class ToDoController1 : Controller
    {
        private readonly ToDoContext _context;

        public ToDoController1(ToDoContext context)
        {
            _context = context;

            if (_context.ToDoItems.Count() == 0)
            {
                _context.ToDoItems.Add(new ToDoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<ToDoItem> GetAll()
        {
            return _context.ToDoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetToDo")]
        public IActionResult GetById(long id)
        {
            var item = _context.ToDoItems.FirstOrDefault(t => t.Id == id);
            if(item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        [HttpPost]
        public IActionResult Create([FromBody] ToDoItem item)
            {
                if (item == null)
                {
                    return BadRequest();
                }
                _context.ToDoItems.Add(item);
                _context.SaveChanges();

                return CreatedAtRoute("GetToDo", new { id = item.Id }, item);
            }
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ToDoItem item)
        {
            if(item == null || item.Id !=id)
            {
                return BadRequest();
            }

            var todo = _context.ToDoItems.FirstOrDefault(t => t.Id == id);
            if(todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.ToDoItems.Update(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.ToDoItems.FirstOrDefault(t => t.Id == id);
            if(todo == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
        /*
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPut("{id}")]

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
