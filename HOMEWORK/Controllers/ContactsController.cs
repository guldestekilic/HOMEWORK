using FullContact.Contacts.API.Models.Fields;
using HOMEWORK.Data;
using HOMEWORK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HOMEWORK.Controllers
{
    [ApiController]
        [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;
        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async  Task<IActionResult> GetContacts()
        {
           return Ok( await dbContext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contacts = await dbContext.Contacts.FindAsync(id);

            if (contacts==null)
            {
                return NotFound();
            }
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contacts = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address, 
                Email = addContactRequest.Email,
                Fullname = addContactRequest.Fullname,
                Phone = addContactRequest.Phone

            };

            await dbContext.Contacts.AddAsync(contacts);
            await dbContext.SaveChangesAsync();

            return Ok(contacts);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contacts = await  dbContext.Contacts.FindAsync(id);
            if (contacts != null)
            {
                contacts.Fullname = updateContactRequest.Fullname;
                contacts.Address = updateContactRequest.Address;
                contacts.Phone = updateContactRequest.Phone;
                contacts.Email = updateContactRequest.Email;

                await dbContext.SaveChangesAsync();

                return Ok(contacts);

            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public  async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact !=null)
            {
                dbContext.Contacts.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }

            return NotFound();
        }

    }
}
