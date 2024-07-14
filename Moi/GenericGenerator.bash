#!/bin/bash

# Function to create directories if they don't exist
createIfNotExists() {
    if [ ! -d "$1" ]; then
        mkdir -p "$1"
    fi
}

# Function to create a file with specified content
createJobFailedException() {
    createIfNotExists "$(dirname "$2")"
    echo "$3" >"$2"
    echo "File '$2' has been created with the specified content."
}

# Function to add content to files
addContentToFile() {
    echo "File  has been created with the specified content."
}

# Function to mimic Console.WriteLine()
consoleLog() {
    echo "$1"
}

AddScope() {
    local contentToAdd=$1
    local pathToAdd=$2
    local marker=$3

    existingContent=$(<"$pathToAdd")
    markerPosition=$(grep -b -o "$marker" "$pathToAdd" | cut -d: -f1)
    insertionIndex=$((markerPosition + ${#marker}))
    updatedContent=$(echo -e "${existingContent:0:insertionIndex}\n$contentToAdd${existingContent:insertionIndex}")
    echo -e "$updatedContent" >"$pathToAdd"
}

# Main function
main() {

    read -p "Enter Entity name: " name
    echo "Hello, $name!"

    while true; do
        read -p "Enter Is Auth 'y' or 'n': " isAuth

        isAuthLower=$(echo "$isAuth" | tr '[:upper:]' '[:lower:]')

        if [[ $isAuthLower == "y" ]]; then
            echo "You entered 'y'!"
            break
        elif [[ $isAuthLower == "n" ]]; then
            echo "You entered 'n'!"
            break
        else
            echo "Invalid input. Please enter 'y' or 'n'."
        fi
    done

    while true; do
        read -p "Enter Is Id Guid 'y' or 'n': " isInt

        isIntLower=$(echo "$isInt" | tr '[:upper:]' '[:lower:]')

        if [[ $isIntLower == "y" ]]; then
            echo "You entered 'y'!"
            break
        elif [[ $isIntLower == "n" ]]; then
            echo "You entered 'n'!"
            break
        else
            echo "Invalid input. Please enter 'y' or 'n'."
        fi
    done

    auth=""

    props_isInt=$isInt

    if [ "$props_isInt" = "y" ]; then
        id="Guid"
    else
        id="int"
    fi

    isAuth=$isAuth

    if [ "$isAuth" = "y" ]; then
        auth="[Authorize]"
    fi

    # Example string
    trimmedName=$name

    firstChar="${trimmedName:0:1}"
    firstCharUpper=$(echo "$firstChar" | tr '[:lower:]' '[:upper:]')
    entityName="${firstCharUpper}${trimmedName:1}"
    interfaceName="I${entityName}Repository.cs"
    repositoryName="${entityName}Repository.cs"
    dtoName="${entityName}Dto.cs"
    formName="${entityName}Form.cs"
    updateName="${entityName}Update.cs"
    filterName="${entityName}Filter.cs"
    serviceName="${entityName}Services.cs"
    controllerName="${entityName}sController.cs"

    checkedFile="$(pwd)/Entities/${entityName}.cs"
    entityPath="$(pwd)/Entities"
    dbContextPath="$(pwd)/DATA/DataContext.cs"
    iInterfacePath="$(pwd)/Interface"
    repositoryPath="$(pwd)/Respository"
    dataForm="$(pwd)/DATA/DTOs/${entityName}"
    servicesPath="$(pwd)/Services"
    controllersPath="$(pwd)/Controllers"

    if [ -f "$checkedFile" ]; then
        consoleLog "File Already Exists"
        return 1
    fi

    entityContent=$(
        cat <<EOF
namespace BackEndStructuer.Entities
{
    public class ${entityName} : BaseEntity<${id}>
    {

    }
}
EOF
    )



    interfaceContent=$(
        cat <<EOF
using BackEndStructuer.Entities;

namespace BackEndStructuer.Interface
{
    public interface I${entityName}Repository : IGenericRepository<${entityName} , ${id}>
    {
         
    }
}
EOF
    )

    dtoContent=$(
        cat <<EOF
namespace BackEndStructuer.DATA.DTOs
{

    public class ${entityName}Dto
    {

    }
}
EOF
    )

    updateContent=$(
        cat <<EOF
namespace BackEndStructuer.DATA.DTOs
{

    public class ${entityName}Update
    {

    }
}
EOF
    )

    formContent=$(
        cat <<EOF
namespace BackEndStructuer.DATA.DTOs
{

    public class ${entityName}Form 
    {

    }
}
EOF
    )

    filterContent=$(
        cat <<EOF
namespace BackEndStructuer.DATA.DTOs
{

    public class ${entityName}Filter : BaseFilter 
    {

    }
}
EOF
    )

    repositoryContent=$(
        cat <<EOF
using AutoMapper;
using BackEndStructuer.DATA;
using BackEndStructuer.Entities;
using BackEndStructuer.Interface;

namespace BackEndStructuer.Repository
{

    public class ${entityName}Repository : GenericRepository<${entityName} , ${id}> , I${entityName}Repository
    {
        public ${entityName}Repository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
EOF
    )

    serviceContent=$(
        cat <<EOF

using AutoMapper;
using BackEndStructuer.DATA.DTOs;
using BackEndStructuer.Entities;
using BackEndStructuer.Repository;
using BackEndStructuer.Services;

namespace BackEndStructuer.Services;


public interface I${entityName}Services
{
Task<(${entityName}? $(echo "$entityName" | tr '[:upper:]' '[:lower:]'), string? error)> Create(${entityName}Form $(echo "$entityName" | tr '[:upper:]' '[:lower:]')Form );
Task<(List<${entityName}Dto> $(echo "$entityName" | tr '[:upper:]' '[:lower:]')s, int? totalCount, string? error)> GetAll(${entityName}Filter filter);
Task<(${entityName}? $(echo "$entityName" | tr '[:upper:]' '[:lower:]'), string? error)> Update(${id} id , ${entityName}Update $(echo "$entityName" | tr '[:upper:]' '[:lower:]')Update);
Task<(${entityName}? $(echo "$entityName" | tr '[:upper:]' '[:lower:]'), string? error)> Delete(${id} id);
}

public class ${entityName}Services : I${entityName}Services
{
private readonly IMapper _mapper;
private readonly IRepositoryWrapper _repositoryWrapper;

public ${entityName}Services(
    IMapper mapper ,
    IRepositoryWrapper repositoryWrapper
    )
{
    _mapper = mapper;
    _repositoryWrapper = repositoryWrapper;
}
   
   
public async Task<(${entityName}? $(echo "$entityName" | tr '[:upper:]' '[:lower:]'), string? error)> Create(${entityName}Form $(echo "$entityName" | tr '[:upper:]' '[:lower:]')Form )
{
    throw new NotImplementedException();
      
}

public async Task<(List<${entityName}Dto> $(echo "$entityName" | tr '[:upper:]' '[:lower:]')s, int? totalCount, string? error)> GetAll(${entityName}Filter filter)
    {
        throw new NotImplementedException();
    }

public async Task<(${entityName}? $(echo "$entityName" | tr '[:upper:]' '[:lower:]'), string? error)> Update(${id} id ,${entityName}Update $(echo "$entityName" | tr '[:upper:]' '[:lower:]')Update)
    {
        throw new NotImplementedException();
      
    }

public async Task<(${entityName}? $(echo "$entityName" | tr '[:upper:]' '[:lower:]'), string? error)> Delete(${id} id)
    {
        throw new NotImplementedException();
   
    }

}
EOF
    )

    controllerContent=$(
        cat <<EOF

using BackEndStructuer.DATA.DTOs;
using BackEndStructuer.Helpers;
using BackEndStructuer.Properties;
using BackEndStructuer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackEndStructuer.DATA.DTOs;
using BackEndStructuer.Entities;
using System.Threading.Tasks;

namespace BackEndStructuer.Controllers
{
    public class ${entityName}sController : BaseController
    {
        private readonly I${entityName}Services _$(echo "$entityName" | tr '[:upper:]' '[:lower:]')Services;

        public ${entityName}sController(I${entityName}Services $(echo "$entityName" | tr '[:upper:]' '[:lower:]')Services)
        {
            _$(echo "$entityName" | tr '[:upper:]' '[:lower:]')Services = $(echo "$entityName" | tr '[:upper:]' '[:lower:]')Services;
        }

        ${auth}
        [HttpGet]
        public async Task<ActionResult<List<${entityName}Dto>>> GetAll([FromQuery] ${entityName}Filter filter) => Ok(await _$(echo "$entityName" | tr '[:upper:]' '[:lower:]')Services.GetAll(filter) , filter.PageNumber , filter.PageSize);

        ${auth}
        [HttpPost]
        public async Task<ActionResult<${entityName}>> Create([FromBody] ${entityName}Form $(echo "$entityName" | tr '[:upper:]' '[:lower:]')Form) => Ok(await _$(echo "$entityName" | tr '[:upper:]' '[:lower:]')Services.Create($(echo "$entityName" | tr '[:upper:]' '[:lower:]')Form));

        ${auth}
        [HttpPut("{id}")]
        public async Task<ActionResult<${entityName}>> Update([FromBody] ${entityName}Update $(echo "$entityName" | tr '[:upper:]' '[:lower:]')Update, ${id} id) => Ok(await _$(echo "$entityName" | tr '[:upper:]' '[:lower:]')Services.Update(id , $(echo "$entityName" | tr '[:upper:]' '[:lower:]')Update));

        ${auth}
        [HttpDelete("{id}")]
        public async Task<ActionResult<${entityName}>> Delete(${id} id) =>  Ok( await _$(echo "$entityName" | tr '[:upper:]' '[:lower:]')Services.Delete(id));
        
    }
}


EOF
    )

    createJobFailedException "$entityName.cs" "$entityPath/${entityName}.cs" "$entityContent"
    createJobFailedException "$interfaceName.cs" "$iInterfacePath/I${entityName}Repository.cs" "$interfaceContent"
    createJobFailedException "$repositoryName.cs" "$repositoryPath/${entityName}Repository.cs" "$repositoryContent"
    createJobFailedException "$dtoName.cs" "$dataForm/${entityName}Dto.cs" "$dtoContent"
    createJobFailedException "$formName.cs" "$dataForm/${entityName}Form.cs" "$formContent"
    createJobFailedException "$updateName.cs" "$dataForm/${entityName}Update.cs" "$updateContent"
    createJobFailedException "$filterName.cs" "$dataForm/${entityName}Filter.cs" "$filterContent"
    createJobFailedException "$serviceName.cs" "$servicesPath/${entityName}Services.cs" "$serviceContent"
    createJobFailedException "$controllerName.cs" "$controllersPath/${entityName}Controllers.cs" "$controllerContent"

    marker="// here to add"

    contentAddScope="services.AddScoped<I${entityName}Services, ${entityName}Services>();"
    addScopePath="$(pwd)/Extensions/ApplicationServicesExtension.cs"
    contentAddIWrapper="I${entityName%%.cs}Repository ${entityName%%.cs}{get;}"
    addIWrapperPath="$(pwd)/Interface/IRepositoryWrapper.cs"

    contentAddWrapper=$(
        cat <<EOF
private I${entityName%%.cs}Repository _${entityName%%.cs,};

public I${entityName%%.cs}Repository ${entityName%%.cs} {
    get {
        if(_${entityName%%.cs} == null) {
            _${entityName%%.cs} = new ${entityName%%.cs}Repository(_context, _mapper);
        }
        return _${entityName%%.cs};
    }
}
EOF
    )

    addWrapperPath="$(pwd)/Respository/RepositoryWrapper.cs"

    contentAddMapper=$(
        cat <<EOF
CreateMap<${entityName%%.cs}, ${entityName%%.cs}Dto>();
CreateMap<${entityName%%.cs}Form,${entityName%%.cs}>();
CreateMap<${entityName%%.cs}Update,${entityName%%.cs}>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
EOF
    )

    addMapperPath="$(pwd)/Helpers/UserMappingProfile.cs"


    dbContextPath="$(pwd)/DATA/DataContext.cs"


    dbContextContent=$(
        cat <<EOF
public DbSet<${entityName}> ${entityName}s { get; set; }
EOF
    )

    AddScope "$contentAddScope" "$addScopePath" "$marker"
    AddScope "$contentAddIWrapper" "$addIWrapperPath" "$marker"
    AddScope "$contentAddMapper" "$addMapperPath" "$marker"
    AddScope "$contentAddWrapper" "$addWrapperPath" "$marker"
    AddScope "$dbContextContent" "$dbContextPath" "$marker"


#     marker="// here to implement"

#     contentImplementToMapper=$(
#         cat <<EOF
# using BackEndStructuer.DATA.DTOs.${entityName%%.cs}Dto;
# using BackEndStructuer.DATA.DTOs.${entityName%%.cs}Form;
# using BackEndStructuer.DATA.DTOs.${entityName%%.cs}Update;
# EOF
#     )

#     AddScope "$contentImplementToMapper" "$addMapperPath" "$marker"

}

# Run the main function
main "$@"
