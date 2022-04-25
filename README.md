# Book repository

This project contains a book repository exposed via a REST api. The project will be written i .Net 6/C#
It is meant to be runnable while having the least amount configuration. 
That means you should be able to just clone repository, build project and run tests and application without needing to set up your own database.    
This project is used as a demo of my prgramming skills, and also being able to run a database without installing anything.

## Usage
To start the application, all you need to do is build and run. 
When the program is started, a new SQLLite database is created, with the following path: **%localappdata%\books.db**.
For the use GET use cases there is working end-to-end tests (this is not yet implemented for create/update).

## Possible improvements
- Fix hardcoded SQLLite database, so different test fixtures can have their own databases
- Tests. Currently the the tests are few for services, but there is a demo of end to end tests as well as tests done in isolation.

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
- **Bookrepository.Tests.EndToEnd**: These are the use cases from the problem description (except PUT/POST due to issue of all contexts using the same hardcoded database), plus any other tests that need to be run on the final application.

### Dependencies
The following external dependencies should be used to implement project:
- **SqlLite**: To enble usage without having SqlServer/SqlServer Express installed, SqlLite will be used as the database
- **MediaTR**: Used to impement mediator pattern, and be able to separate api endpoints from implementation. Thin controllers.
- **Entitiy frame work core**: The main purpose of this is to be able to avoid needing to write SQL. It is not hard to build a sql code generator considering there is only one entity in the domain, but for any reasonable sized application an ORM is recommended to avoid having to write code to test code generation.
- **Fluent Validation**: Used to validate the requests sent to the app services.
- **xunit + fluent assertions**: Used to create easy to read tests

### Main app structure
This project will use a CQRS approach. When an endpoint on a controller is called, paramers from api is used to instantiate an request. 
A mediator makes sure the request is forwarded to the correct handler. Before the handler handles the request, it can be passed to a validator class according to need.
Handler class will in turn validate request parameters. If the request is valid it will call on injected repository to Access/persist data.

This approach was selected to enable different parts of the application to be tested in isolation. Each of the services can be tested, and their dependencies can be injected in the constructor, 
either explicitly or via 


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
 - [X] Add validation pipeline and related middleware.
 - [X] Add data migration at main app startup
 - [X] Implement actual book repository for GET
 - [X] Make sure all acceptance tests pass for GET
 - [X] Repeat process for Create/Update
 - [X] ~~(Optional) Attempt to use mapping instead of POCO not following conventions. If there is not ennough time this step can be skipped.~~
 - [X] More detailed review & cleanup. Finalize documentation
 - [X] Try to break app, and apply validation/additional error handling as needed
