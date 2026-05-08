using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CloudDevProj2.Models;
using Azure.Storage.Blobs;
using Microsoft.IdentityModel.Tokens;

namespace CloudDevProj2.Controllers
{
    public class PeopleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;

        public PeopleController(AppDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            return View(await _context.People.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,FirstName,Lastname,MiddleName,Title,Gender,Email,PhoneNumber,ImageFile")] PersonViewModel person)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(person);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(person);

            var file = person.ImageFile;
            var imageURL = string.Empty;

            //validation
            if (file.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("", "Max 5MB allowed");
            }

            var allowed = new[] {".jpg", ".png", ".jpeg"};
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowed.Contains(ext))
            {
                ModelState.AddModelError("", "Invalid file type");
            }

            if (ModelState.IsValid)
            {
                if (file.Length != 0)
                {
                    imageURL = SaveImageToBlobStorage(file);
                }

                var newPerson = new Person
                {
                    FirstName = person.FirstName,
                    Lastname = person.Lastname,
                    MiddleName = person.MiddleName,
                    Title = person.Title,
                    Gender = person.Gender,
                    Email = person.Email,
                    PhoneNumber = person.PhoneNumber,
                    ImageURL = imageURL
                };

                _context.Add(newPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(person);
        }
        private string? SaveImageToBlobStorage(IFormFile imageFile)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient("person-images");
                containerClient.CreateIfNotExists();
                var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString()
                    + System.IO.Path.GetExtension(imageFile.FileName));
                using (var stream = imageFile.OpenReadStream())
                {
                    blobClient.Upload(stream);
                }
                return blobClient.Uri.ToString();
            }

            catch (Exception ex)
            {
                throw;
            }
        }


        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            var viewModel = new PersonViewModel
            {
                PersonId = person.PersonId,
                FirstName = person.FirstName,
                Lastname = person.Lastname,
                MiddleName = person.MiddleName,
                Title = person.Title,
                Gender = person.Gender,
                Email = person.Email,
                PhoneNumber = person.PhoneNumber
            };

            return View(viewModel);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonId,FirstName,Lastname,MiddleName,Title,Gender,Email,PhoneNumber,ImageFile")] PersonViewModel person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            var existingPerson = await _context.People.FindAsync(id);

            if (existingPerson == null)
            {
                return NotFound();
            }

            var file = person.ImageFile;
            var imageURL = existingPerson.ImageURL;

            if (file != null)
            {
                if (file.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "Max 5MB allowed");
                }

                var allowed = new[] { ".jpg", ".png", ".jpeg" };
                var ext = Path.GetExtension(file.FileName).ToLower();

                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("", "Invalid file type");
                }
            }

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    imageURL = SaveImageToBlobStorage(file);
                }

                existingPerson.FirstName = person.FirstName;
                existingPerson.Lastname = person.Lastname;
                existingPerson.MiddleName = person.MiddleName;
                existingPerson.Title = person.Title;
                existingPerson.Gender = person.Gender;
                existingPerson.Email = person.Email;
                existingPerson.PhoneNumber = person.PhoneNumber;
                existingPerson.ImageURL = imageURL;

                _context.Update(existingPerson);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(person);
        }



        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.PersonId == id);
        }
    }
}
