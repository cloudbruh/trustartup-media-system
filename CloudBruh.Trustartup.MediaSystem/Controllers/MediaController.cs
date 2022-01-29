using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CloudBruh.Trustartup.MediaSystem.Models;

namespace CloudBruh.Trustartup.MediaSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MediaController : ControllerBase
{
    private readonly MediaContext _context;

    private readonly IConfiguration _configuration;

    private readonly string[] _allowedExtensions = {".png", ".jpg"};

    public MediaController(MediaContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // GET: api/Media
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Media>>> GetMedia()
    {
        return await _context.Media.ToListAsync();
    }

    // GET: api/Media/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Media>> GetMedia(long id)
    {
        Media? media = await _context.Media.FindAsync(id);

        if (media == null)
        {
            return NotFound();
        }

        return media;
    }
    
    // GET: api/Media/5/download
    [HttpGet("{id:long}/download")]
    public async Task<IActionResult> DownloadMedia(long id)
    {
        Media? media = await _context.Media.FindAsync(id);

        if (media == null)
        {
            return NotFound();
        }

        string? filename = media.Link;
        if (filename == null)
        {
            return NotFound();
        }
        
        string path = Path.Combine(_configuration.GetValue<string>("Storage:Path"), filename);

        return File(await System.IO.File.ReadAllBytesAsync(path), "image/png");
    }

    // POST: api/Media/5/upload
    [HttpPost("{id:long}/upload")]
    public async Task<IActionResult> UploadMedia(IFormFile file, long id)
    {
        Media? media = await _context.Media.FindAsync(id);
        if (media == null)
        {
            return NotFound();
        }
        
        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
        {
            return ValidationProblem("Invalid file extension.");
        }
        if (file.Length > 1024*1024)
        {
            return ValidationProblem("File is too large.");
        }

        string filename = Path.ChangeExtension(Guid.NewGuid().ToString(), extension);
        string path = Path.Combine(_configuration.GetValue<string>("Storage:Path"), filename);

        await using (FileStream stream = System.IO.File.Create(path))
        {
            await file.CopyToAsync(stream);
        }
        
        media.Link = filename;
        
        _context.Entry(media).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MediaExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Media
    [HttpPost]
    public async Task<ActionResult<Media>> PostMedia([Bind("UserId, IsPublic, Type")] Media media)
    {
        _context.Media.Add(media);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMedia", new { id = media.Id }, media);
    }

    // DELETE: api/Media/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteMedia(long id)
    {
        Media? media = await _context.Media.FindAsync(id);
        if (media == null)
        {
            return NotFound();
        }

        _context.Media.Remove(media);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MediaExists(long id)
    {
        return _context.Media.Any(e => e.Id == id);
    }
}