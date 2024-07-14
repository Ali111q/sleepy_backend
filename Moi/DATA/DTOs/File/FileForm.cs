using FluentValidation;

namespace GaragesStructure.DATA.DTOs.File;

public class FileForm
{
    public IFormFile File { get; set; }
}

public class FileFormValidator : AbstractValidator<FileForm>
{
    public FileFormValidator()
    {
        RuleFor(x => x.File).NotNull().WithMessage("File is required");
    }
}