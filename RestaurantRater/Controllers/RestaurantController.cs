using RestaurantRater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRater.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();
        // POST (Create method)
        // api/Restaurant
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurant([FromBody]Restaurant model)
        {
            if (model is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }
            // If the model is valid...
            if (ModelState.IsValid)
            {
                // This is where we will store the model in the database
                _context.Restaurants.Add(model);
                int changeCount = await _context.SaveChangesAsync();

                return Ok("Your restaurant was created.");
            }

            // If the model is not valid, reject it.
            return BadRequest(ModelState);
        }

        // GET ALL
        // api/Restaurant
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }

        // GET BY ID
        // api/Restaurant/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri]int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if(restaurant != null)
            {
                return Ok(restaurant);
            }

            return NotFound();
        }

        //Put (update)
        // api/Restaurant
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRestaurant([FromUri] int id, [FromBody] Restaurant updatedRestaurant)
        {
            // Check the ids if they match
            if(id != updatedRestaurant?.Id)
            {
                return BadRequest("Id's do not match.");
            }

            // Check the ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            // If they match, find the restaurant in the database. 
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            // If it doesn't match, and the restaurant doesn't exist, then do something.
            if (restaurant is null)
                return NotFound();

            // Assuming restaurant does exist, update properties.
            restaurant.Name = updatedRestaurant.Name;
            restaurant.Address = updatedRestaurant.Address;
            

            // Save the changes.
            await _context.SaveChangesAsync();

            return Ok("Restaurant information was updated.");
        }

        // DELETE 
        // api/Restaurant/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRestaurant([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant is null)
                return NotFound();

            _context.Restaurants.Remove(restaurant);

            if(await _context.SaveChangesAsync() == 1)
            {
                return Ok("The restaurant was deleted.");
            }

            return InternalServerError();
        }
    }
}
