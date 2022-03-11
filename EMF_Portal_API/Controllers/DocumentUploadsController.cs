using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMF_Portal_API.IdentityAuth;
using EMF_Portal_API.Model;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace EMF_Portal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentUploadsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public DocumentUploadsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/DocumentUploads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentUpload>>> GetDocumentUploads()
        {
            return await _context.DocumentUploads.ToListAsync();
        }

        // GET: api/DocumentUploads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentUpload>> GetDocumentUpload(int id)
        {
            var documentUpload = await _context.DocumentUploads.FindAsync(id);

            if (documentUpload == null)
            {
                return NotFound();
            }

            return documentUpload;
        }

        // PUT: api/DocumentUploads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumentUpload(int id, DocumentUpload documentUpload)
        {
            if (id != documentUpload.DocumentID)
            {
                return BadRequest();
            }

            _context.Entry(documentUpload).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentUploadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DocumentUploads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DocumentUpload>> PostDocumentUpload(DocumentUpload documentUpload)
        {
            _context.DocumentUploads.Add(documentUpload);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocumentUpload", new { id = documentUpload.DocumentID }, documentUpload);
        }
        //document upload added post API
        [HttpPost]
        [Route("UploadDocument")]
        public async Task<IActionResult> UploadDocument(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var rootPath = Path.Combine(_environment.ContentRootPath, "Resources", "DocumentUpload");

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            foreach (var file in files)
            {
                var filePath = Path.Combine(rootPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    var documentupload = new DocumentUpload
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Filesize = file.Length
                    };
                    await file.CopyToAsync(stream);

                    _context.DocumentUploads.Add(documentupload);

                    await _context.SaveChangesAsync();
                }
            }
            return Ok(new { count = files.Count, size });
        }

        [HttpPost]
        [Route("Document/{id}")]
        public async Task<IActionResult> Document(int id)
        {
            var provider = new FileExtensionContentTypeProvider();

            var document = await _context.DocumentUploads.FindAsync(id);

            if(document == null)
            {
                return NotFound();
            }

            var file = Path.Combine(_environment.ContentRootPath, "Resources", "DocumentUpload", document.FileName);

            //string contentType;
            if (!provider.TryGetContentType(file, out string contentType))
            {
                contentType = "application/octet-stream";
            }
          
            byte[] fileBytes;
            if (System.IO.File.Exists(file))
            {
                fileBytes = System.IO.File.ReadAllBytes(file);
            }
            else
            {
                return NotFound();
            }

            return File(fileBytes, contentType, document.FileName);
        }
        // DELETE: api/DocumentUploads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentUpload(int id)
        {
            var documentUpload = await _context.DocumentUploads.FindAsync(id);
            if (documentUpload == null)
            {
                return NotFound();
            }

            _context.DocumentUploads.Remove(documentUpload);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentUploadExists(int id)
        {
            return _context.DocumentUploads.Any(e => e.DocumentID == id);
        }
    }
}
