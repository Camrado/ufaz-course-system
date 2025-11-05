using CourseSystem.Application.Languages.CreateLanguage;
using CourseSystem.Application.Languages.DeleteLanguage;
using CourseSystem.Application.Languages.GetAllLanguages;
using CourseSystem.Application.Languages.GetLanguage;
using CourseSystem.Application.Languages.UpdateLanguage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.API.Controllers;

[ApiController]
[Route("api/languages")]
public sealed class LanguageController : ControllerBase
{
    private readonly IMediator _sender;

    public LanguageController(IMediator sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetAllLanguages(CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new GetAllLanguagesQuery(), cancellationToken));
    }

    [HttpGet("{languageId:int}")]
    public async Task<IActionResult> GetLanguageById(int languageId, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new GetLanguageQuery(languageId), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateLanguage([FromBody] CreateLanguageCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetLanguageById), new { languageId = result.Id }, result);
    }

    [HttpPut("{languageId:int}")]
    public async Task<IActionResult> UpdateLanguage(int languageId, [FromBody] UpdateLanguageCommand request,
        CancellationToken cancellationToken)
    {
        request.LanguageId = languageId;
        return Ok(await _sender.Send(request, cancellationToken));
    }

    [HttpDelete("{languageId:int}")]
    public async Task<IActionResult> DeleteLanguage(int languageId, CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(new DeleteLanguageCommand(languageId), cancellationToken));
    }
}
