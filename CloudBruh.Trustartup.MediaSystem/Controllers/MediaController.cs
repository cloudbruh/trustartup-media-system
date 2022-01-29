using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CloudBruh.Trustartup.MediaSystem.Models;

namespace CloudBruh.Trustartup.MediaSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MediaController : ControllerBase
{
    private readonly MediaContext _context;

    public MediaController(MediaContext context)
    {
        _context = context;
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

    // PUT: api/Media/5
    [HttpPut("{id:long}")]
    public async Task<IActionResult> PutMedia(long id, Media media)
    {
        if (id != media.Id)
        {
            return BadRequest();
        }

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
    public async Task<ActionResult<Media>> PostMedia(Media media)
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