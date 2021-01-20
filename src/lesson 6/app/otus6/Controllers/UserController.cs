using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using otus6.Database;
using otus6.Models;
namespace otus6.Controllers
{
    public class UserController : Controller
    {
        private readonly Repository<User> _repository;
        public UserController(AppDb context)
        {
            _repository = new Repository<User>(context);
        }
        [Route("/user/{id}")]
        [Route("/otusapp/user/{id}")]
        [HttpGet]
        public async Task<object> Get(long id)
        {           
            var result = await _repository.Get(id);
            if (result == null)
                return new Error { Code = 1, Message = $"User {id} not found." };

            return result;
        }

        [Route("/user")]
        [Route("/otusapp/user")]
        [HttpPost]
        public async Task<object> Create([FromBody]User user)
        {
            var result = await _repository.Create(user);
            return Ok($"User {result.Id} created");
        }

        [Route("/user/{id}")]
        [Route("/otusapp/user/{id}")]
        [HttpDelete]
        public async Task<object> Delete(long id)
        {
            var result = await _repository.Delete(id);
            if (result == null)
                return new Error { Code = 1, Message = $"User {id} not found." };
            return NoContent();
        }

        [Route("/user/{id}")]
        [Route("/otusapp/user/{id}")]
        [HttpPut]
        public async Task<object> Update(long id, [FromBody]User user)
        {
            user.Id = id;
            var result = await _repository.Update(id, user);
            if (result == null)
                return NotFound($"User {id} not found");
            return Ok($"User {id} updated");
        }
    }
}
