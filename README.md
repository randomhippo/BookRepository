# Book repository

This project contains a book repository exposed via a REST api. The project will be written i .Net 6/C#
It is meant to be runnable while having the least amount configuration. 
That means you should be able to just clone repository, build project and run tests and application without needing to set up your own database.    
This project is used as a demo of my prgramming skills, and also being able to run a database without installing anything.

## Usage
To start the application, all you need to do is build and run. When the program is started, a new SQLLite database is created.
For the use GET use cases there is working end-to-end tests (this is not yet implemented for create/update).

## Improvements remaining 
Despite passing the acceptance criteria in specification there are some remaining improvements to be done.

### Check that PUT/POST conforms to REST standard
Check whether updated object should be returned completely, or a subset of it.

### Update comments and attributes for *BooksController*
It is currently hard to use unless you also read the specification. To make sure swagger works ok, the xml comments need to be written for each route.
See if it is possible for better suggested values. Default suggested datetime in Swagger UI is not accepted. The format in requirements is however.
### Add request validation and error processing
Add validation for the services. This is expected in production code. One way to add this without breaking separation of concerns is to add validation to the mediator pipeline. 
This way validation, handling of commands and error handling can be done in separate steps.
- **Data parsing**: Done in controller. Controller should just gather input and pass arguments to the mediator
- **Request Validation**: Check for invalid/badly formatted/out of bounds input data. If any validation errors occur, present an actionable error message.
- **Processing**: Done by handlers.
- **Error handling**: Any exceptions thrown in earlier phases will be processed to return correctly formed Http-code and a useful message.

## Structure
Below follows a basic overview of included projects.

The project contains the following 
- **Bookrepository.App:** The application itself. 
   -  Since the domain model contains few entity types, a single file is the options that will lead to the least clutter.
   -  It is meant to be able to run directly after build. This means that schema migration/adding basic data will be done as part of progeram startup as needed.
     - The data check will only check if the entities in the sample file is present during startup by comparing id:s. 
     - This means any delete will be undone after restart, but updates will be persisted as long as application runs.
   -  It should be self hosted. No installation of IIS should be required
- **Bookrepository.Tests**: Automated tests that should be always be run. These are meant to test the code in isolation, that is without starting the main application.  
- **Bookrepository.Tests.EndToEnd**: These are the use cases from the problem description, plus any other tests that need to be run on the final application.

### Dependencies
The following external dependencies should be used to implement project:
- SqlLite: To enble usage without having SqlServer/SqlServer Express installed, SqlLite will be used as the database
- MediaTR: Used to impement mediator pattern, and be able to separate api endpoints from implementation. Thin controllers.
- NHibernate: The main purpose of this is to be able to avoid needing to write SQL. It is not hard to build a sql code generator considering there is only one entity in the domain, but for any reasonable sized application an ORM is recommended to avoid having to write code to test code generation.
- Fluent NHibernate: (optional, depends on if default conventions are enough) used to be able to have clean domain model classes, without needing to add mapping attributes.
- Something to serilize JSON. Might not be nescessary, but perhaps required to translate Domain to json in spec. It is also possible to use a specific DTO class instead, but that would mean having a class that does not follow naming conventions.
- Log4Net if needed.

### Main app structure
This project will use a CQRS approach. When an endpoint on a controller is called, paramers from api is used to instantiate an request. 
A mediator makes sure the request is forwarded to the correct handler. Before the handler handles the request, it can be passed to a validator class according to need.
Handler class will in turn validate request parameters. If the request is valid it will call on injected repository to Access/persist data.

## Task list
Mainly used to kepp track of work progress without using an external program.

 - [X] Set up code repository and readme
 - [X] Create main app
 - [X] Set up self hosted server. Should contains a single endpoint to see that the server can process requests.
 - [X] Decide fine architecture details
 - [X] Set up test projects
 - [X] Implement domain model, ~~DTO as POCO~~(not needed, used built in attributes instead), and any related tests
 - [X] Add api endpoints according to spec, and create failing acceptance tests for them. **Update** Start with GET endpoints. Create/update will be done after read is fully complete
 - [X] Add services, and connect them to API endpoints. Inject dummy interface implementations at this point
 - [ ] Add validation pipeline and related middleware.
 - [X] Add data migration at main app startup
 - [X] Implement actual book repository for GET
 - [X] Make sure all acceptance tests pass for GET
 - [X] Repeat process for Create/Update
 - [ ] ~~(Optional) Attempt to use mapping instead of POCO not following conventions. If there is not ennough time this step can be skipped.~~
 - [ ] More detailed review & cleanup. Finalize documentation
 - [ ] Try to break app, and apply validation/additional error handling as needed
