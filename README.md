# StackAPI
## .NET 8 Minimal API Recruitment Task

### How to run
using docker
```bash
$ docker-compose up -d
```

by building it yourself
```bash
$ cd StackAPI
$ dotnet publish -c Release -o out
$ cd out
$ dotnet StackAPI.dll
```

or using development version [Windows]
```powershell
cd StackAPI
$env:API_KEY="YOUR API KEY HERE"; $env:CONNECTION_STRING="YOUR CONNECTION STRING HERE"; dotnet run --launch-profile https
```

## Routes
#### POST
``
/tags
``
``
/tags/addMany
``
``
/tags/RemoveMany
``

#### GET
``
/tags
``
``
/tags/{tagId}
``
``
/tags/getTagByName/{tagName}
``
``
/tags/{sortBy}/{direction}/{offset}
``
``
/tags/share/{tagName}
``

``
/populateDb
``

#### DELETE
``
/tags/{tagId}
``


## Data Types
### CreateTagRequest
```yml
"hasSynonyms": boolean,
"isModeratorOnly": boolean,
"isRequired": boolean,
"count": number,
"name": string
```

### DeleteTagRequest
```yml
"id": string
```

### GetAllTagsResponse
```yml
"totalRecords": number | null,
"totalPages": number | null,
"pageSize": number | null,
"currentPage": number | null,
"tags": Array<TagResponse>
```

### GetTagShareResponse
```yml
"tagSharePercentage": number
```

### TagResponse
```yml
"id": string
"hasSynonyms": boolean,
"isModeratorOnly": boolean,
"isRequired": boolean,
"count": number,
"name": string
```

## Configuration
<b>CONNECTION_STRING</b> is an env variable to configurate db connection; if left empty, application will look for connection string in appsettings.json. You can change user creds from example ones, but remember to change them in
db configuration aswell

<b>API_KEY</b> is an env variable for you to set to link this app to your stack application. If you left this empty (<b>remove field from env variables</b>), application will run in restricted mode. For more information go here https://api.stackexchange.com/docs/throttle

<b> ASPNETCORE_URLS </b> is set by default to "http://+:5141", if you have https certs for image you can change it to i.e "http://+:5141;https://+:7031"
