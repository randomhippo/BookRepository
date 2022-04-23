# Book repository

This project contains a book repository exposed via a REST api. The project will be written i .Net 6/C#
It is meant to be runnable while having the least amount configuration. 
That means you should be able to just clone repository, build project and run tests and application without needing to set up your own database.    
This project is used as a demo of my prgramming skills, and also being able to run a database without installing Sql Server express.

## Usage
TODO: Write instructions

##Structure
Below follows a basic overview of included projects.

The project contains the following 
- *Bookrepository:* The application itself. 
   -  Since the domain model contains few entity types, a single file is the options that will lead to the least clutter.
   -  It is meant to be able to run directly after build. This means that schema migration/adding basic data will be done as part of progeram startup as needed.
     - The data check will only check if the entities in the sample file is present during startup by comparing id:s. 
     - This means any delete will be undone after restart, but updates will be persisted.
   -  It should be self hosted. No installation of IIS should be required
- *Bookrepository.Tests*: Automated tests that should be always be run. These are meant to test the code in isolation, that is without starting the main application.  
- *Bookrepository.Tests.Acceptance*: These are the use cases from the problem description, plus any other tests that need to be run on the final application.

##Task list
Mainly used to kepp track of work progress without using an external program.

 - [X] Set up code repository and readme
 - [ ] Create main app
 - [ ] Set up self hosted server. Should contains a single endpoint to see that the server can process requests.
 - [ ] Decide fine architecture details
 - [ ] Set up test projects
 - [ ] Implement domain model, and any related tests
 - [ ] Add api endpoints according to spec, and create failing acceptance tests for them
 - [ ] Add services, and connect them to API endpoints. Inject dummy interface implementations at this point
 - [ ] Test request validation, make sure that validation pipeline is working correctly.
 - [ ] Add data migration at main app startup
 - [ ] Implement actual book repository
 - [ ] Make sure all acceptance tests pass
 - [ ] More detailed review & cleanup. Finalize documentation
 - [ ] Try to break app, and apply validation/additional error handling as needed
