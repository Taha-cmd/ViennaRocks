# Vienna Rocks

Love going to metal and rock concerts in Vienna? Well, this demo application will help you headbanging!

The app is a containerized rest api, that in turn calls the [TicketMaster api](https://developer.ticketmaster.com/products-and-docs/apis/getting-started/) to display to top 20 metal and rock concerts in Vienna in a simplified schema. The application was designed to follow the [12 factor app](https://12factor.net/) philosophy.

# Endpoints

### GET /concerts

Display the top 20 rock and metal concerts in Vienna offered by TicketMaster

# Getting Started

The application only requires a running docker engine and an internet connection to work.

### Running the Demo

Run the PowerShell script `.\StartDemo.ps1`, which basically does two things:

1. set the environment variable `env` to the value `demo`
2. run `docker compose up --build`

If you don't have PowerShell on your system, you can of course run the two simple steps manually in the CLI of your choice.

Navigate to [localhost:8080/concerts](http://localhost:8080/concerts) or [localhost:8080/swagger](http://localhost:8080/swagger)

### Architecture

Each instance of the application contains two containers:

- `web`: the rest api
- `redis`: an in memory database for caching

The `docker-compose.yml` file builds the `web` container from the source code and pulls the `redis` image from a public registry. The application is basically stateless, there is no persistence layer. However, since new events don't get announced every day, a caching layer does make a lot of sense. Therefore, all results are cached for 24 hours.

A running instance of the application requires an `environment`, which is a logical concept that includes all the required configuration parameters saved in environment variables. An `enviornment` maps physically to a `.env` file in the `Environments` folder. This file will be loaded into the container's environment and the application can access these variables during runtime. Unfortunately, we can't pass argument to the `docker compose up` command, but we still need to tell the compose file which environment file to load into the container, this is why we set the environment variable `env` before calling `docker compose`

### Create an Environment

To create an Environment called `dev`, create a file called `.env.dev` in the `Environments` folder. See `Environments/.env.template` to know what parameters an .env file should include. Before running the application, set the environment variable `env` to `dev`. When running the application `docker-compose.yml` will automatically look for `Environment/.env.dev` when building the container. A `demo` environment has already been created for you.

# Ideas for the future

- Create a CI/CD pipeline
- Add tests
- Include the hosting infrastructure in the project (Terraform or bicep)
- Kubernetes
  - environments as configmaps and secrets :white_check_mark:
  - deployments and services :white_check_mark:
  - ingress
  - helm chart :white_check_mark:
  - volume for redis

# Deployment Scenarios

it is a mess for now.

- Helm
- Docker Compose
- Kubernetes
