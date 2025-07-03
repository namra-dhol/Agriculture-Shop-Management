using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;

namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactFormController : ControllerBase
    {
        private readonly AgriShopContext context;

        public ContactFormController(AgriShopContext context)
        {
            this.context = context;
        }

        #region GetAllContactForms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactForm>>> GetContactForms()
        {
            var forms = await context.ContactForms.ToListAsync();
            return Ok(forms);
        }
        #endregion

        #region GetContactFormById
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactForm>> GetContactFormById(int id)
        {
            var form = await context.ContactForms.FindAsync(id);
            if (form == null)
                return NotFound();

            return Ok(form);
        }
        #endregion

        #region InsertContactForm
        [HttpPost]
        public IActionResult InsertContactForm(ContactForm contactForm)
        {
            contactForm.SubmittedAt = DateTime.Now;

            context.ContactForms.Add(contactForm);
            context.SaveChanges();

            return CreatedAtAction(nameof(GetContactFormById), new { id = contactForm.ContactId }, contactForm);
        }
        #endregion

        #region UpdateContactFormById
        [HttpPut("{id}")]
        public IActionResult UpdateContactForm(int id, ContactForm contactForm)
        {
            if (id != contactForm.ContactId)
                return BadRequest();

            var existingForm = context.ContactForms.Find(id);
            if (existingForm == null)
                return NotFound();

            // Update fields
            existingForm.UserId = contactForm.UserId;
            existingForm.Name = contactForm.Name;
            existingForm.Email = contactForm.Email;
            existingForm.Message = contactForm.Message;
            existingForm.SubmittedAt = DateTime.Now;

            context.ContactForms.Update(existingForm);
            context.SaveChanges();
            return NoContent();
        }
        #endregion

        #region DeleteContactFormById
        [HttpDelete("{id}")]
        public IActionResult DeleteContactForm(int id)
        {
            var form = context.ContactForms.Find(id);
            if (form == null)
                return NotFound();

            context.ContactForms.Remove(form);
            context.SaveChanges();
            return NoContent();
        }
        #endregion

        #region FilterContactForms
        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<ContactForm>>> FilterContactForms(
            [FromQuery] string? name,
            [FromQuery] string? email)
        {
            var query = context.ContactForms.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(f => f.Name!.Contains(name));

            if (!string.IsNullOrEmpty(email))
                query = query.Where(f => f.Email!.Contains(email));

            return await query.ToListAsync();
        }
        #endregion

        #region GetTopNContactForms
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<ContactForm>>> GetTopNContactForms([FromQuery] int n = 2)
        {
            var forms = await context.ContactForms.Take(n).ToListAsync();
            return Ok(forms);
        }
        #endregion
    }
}
