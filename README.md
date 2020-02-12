# BackendService

## Build Instructions

You will need Docker Desktop or Docker with Docker Compose and Postman. You will also need a command/terminal or an IDE of choice that can integrate with Docker like Visual Studio Code, Visual Studio Community, etc.

### CLI

- Open a command shell and navigate to the root directory of the project.
- Run `docker-compose up -d` without attaching to the orchestration or `docker-compose up` if you would like to stay attached to the orchestration

### IDE of Choice

- Open the solution in the IDE
- Choose the docker-compose project as the startup project if not already selected and launch the debugger or container from the tool of choice.

## Running the App

Located in `./BackendService/Postman` is a json file, `BackendService.postman_collection.json`, that contains testing to interact with the endpoints of the project. The database also has a manager available for use at `localhost:8080`.

Endpoints
* Application, https://localhost:5001
* DB Manager (Adminer), localhost:8080
* DB (Postgres), localhost:5432

Login for the DB can be found in the `./BackendService/appsettings.json` file.

## Background

I decided to go with containers to avoid any "works on my machine" issues that may come out of this. Unfortunately this caused a few issues connecting the app to a SQL Server container. It looked like SQL Server was not liking things routed inside Docker for some reason. This may have been a config issue that I was not able to resolve.
So, I swapped SQL Server for Postgres and redid EF Core with pgsql. Due to the delay I focused on getting things complete sacrificed testing but created some Postman requests to serve as testing with debugging on the endpoints. The connection to the third party service is commented out since it is supposed to be stubbed but that's the 
gist of how it would go barring some auth additions. The project uses a code first approach and therefore migration is used. The first response to the api does take a minute to run the migration if it hasn't ran.
This is not a good way to go for production and is only there to make initial launch easier for this project. In production I would run migrations manually and persist data on disk instead of the Docker volume used. 
I attempted to cover a lot of the error handling from the third-party and the app itself by hitting the logger per request. Normally this would be stored in a DB or a output file (ran nightly) instead of just output to console.