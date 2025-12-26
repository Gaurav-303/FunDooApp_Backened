using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LabelController : ControllerBase
{
    private readonly ILabelService _labelService;

    public LabelController(ILabelService labelService)
    {
        _labelService = labelService;
    }

    [HttpPost]
    public IActionResult CreateLabel(string labelName)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var label = _labelService.CreateLabel(userId, labelName);
        return Ok(label);
    }

    [HttpPost("assign")]
    public IActionResult AssignLabel(int noteId, int labelId)
    {
        _labelService.AssignLabelToNote(noteId, labelId);
        return Ok("Label assigned");
    }

    [HttpGet]
    public IActionResult GetMyLabels()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Ok(_labelService.GetMyLabels(userId));
    }
}
