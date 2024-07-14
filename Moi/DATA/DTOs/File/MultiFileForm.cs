using FluentValidation;

namespace GaragesStructure.DATA.DTOs.File;

public class MultiFileForm
{
    public List<IFormFile> Files { get; set; }
}

public class MultiFileFormValidator : AbstractValidator<MultiFileForm>
{
    public MultiFileFormValidator()
    {
        RuleFor(x => x.Files).NotNull().WithMessage("Files is required");
        RuleForEach(x => x.Files).NotNull().WithMessage("File is required");
    }
}