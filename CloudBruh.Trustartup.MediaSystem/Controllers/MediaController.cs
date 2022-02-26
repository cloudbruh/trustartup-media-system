using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CloudBruh.Trustartup.MediaSystem.Models;
using CloudBruh.Trustartup.MediaSystem.Services;

namespace CloudBruh.Trustartup.MediaSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MediaController : ControllerBase
{
    private readonly MediaContext _context;
    private readonly IConfiguration _configuration;
    private readonly MediaFileService _mediaFileService;

    private readonly string[] _allowedExtensions = {".png", ".jpg"};

    public MediaController(MediaContext context, IConfiguration configuration, MediaFileService mediaFileService)
    {
        _context = context;
        _configuration = configuration;
        _mediaFileService = mediaFileService;
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
        
        return File(await _mediaFileService.Download(filename), media.MimeType);
    }

    // GET: api/Media/file/d71efc75-9580-423b-b34e-b0db50c4572b.png
    [HttpGet("file/{filename}")]
    public async Task<ActionResult<Media>> GetMedia(string filename)
    {
        Media? media = await _context.Media.Where(media1 => media1.Link == filename).FirstOrDefaultAsync();

        if (media == null)
        {
            return NotFound();
        }

        return media;
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
        
        string filename;
        try
        {
            filename = await _mediaFileService.Save(file);
        }
        catch (ArgumentException e)
        {
            return ValidationProblem(e.Message);
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

        return CreatedAtAction("GetMedia", new { id = media.Id }, media);
    }

    // POST: api/Media
    [HttpPost]
    public async Task<ActionResult<Media>> PostMedia([Bind("UserId, IsPublic, Type, MimeType")] Media media)
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