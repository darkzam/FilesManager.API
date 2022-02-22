using FilesManager.Application.Models;

namespace FilesManager.API.Models
{
    public class ParseOperationDto
    {
        public string Input { get; set; }
        public string Ouput { get; set; }

        public ParseOperationDto()
        { }

        public ParseOperationDto(ParseOperation parseOperation)
        {
            Input = parseOperation.Input;
            Ouput = parseOperation.Ouput;
        }
    }
}