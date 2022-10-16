# 1. Codebase :white_check_mark:

ViennaRocks is maintained in a single repository that contains everything needed for the application to be deployed into any environment. It contains all the source files, build scripts and references to the vendored dependencies that can be retrieved during the build. Therefore, the repository can be deployed from any CI/CD system.

# 2. Dependencies :white_check_mark:

Application's dependencies are retrieved within the docker build from nuget and runtime dependencies (redis database) are retrieved from a public container registry. The application only requires docker on the target machine and does not assume the presence of anything else.

# 3. Config :white_check_mark:/:x:

Deployment specific configuration values, like api keys or connection string, are stored in the environment. They are loaded into the container during the build and persisted during runtime. However, the 12-factor-app philosophy mentions the "grouping of variables into environments like dev or prod" being a bad thing. Variables should be managed separately for each deploy. In our use case, grouping the variables into environment is solution I chose, because managing variables separately requires some sort of variable substitution or a similar mechanism of a CI/CD system. Running the application locally, grouping into environment is much easier. But this is not necessarily a contradiction. Managing individual variables is still possible.

# 4. Backing Services :white_check_mark:

ViennaRocks uses a redis database for caching. The connection string is read from the environment. Other than that, the application knows absolutely nothing about the backing service. The backing service is deployed with the application with `docker compose`, the compose file sets the connection string environment variable for consumption by the container.

# 5. Build, release, run :white_check_mark:/:x:

The current setup supports this constraint, but does not implements it fully. Using the `Dockerfile`, a `build` can be generated. Each `.env` file represents a `config`, using `docker compose`, a `build` can be combined with a `config` to create an immutable release. However, since we don't have a production environment for now, the `run` step is missing and therefore, release are not uniquely tagged.

# 6. Processes :white_check_mark:

The application runs a single process with a backing service for caching. However, the application is basically stateless, the caching service is only used to increase performance. The application can be restarted, relocated to a different process/disk and will still be functional. It does not rely on any data in the disk or cache. The cache can also be cleared without affecting the functionality.

# 7. Port binding :white_check_mark:

Using `docker-compose.yml` we can create port bindings for the services to communicate with each other within the execution environment. The docker engine serves as the routing layer and can inject the application's url into a another container's environment for consumption.

# 8. Concurrency :x:

Each application instance is deployed as pair of `web` and `redis` containers. To scale the `web` instances separately and on demand to use the same `redis` instance, we would need a container orchestration tool like kubernetes or docker swarm, which was not implemented.

# 9. Disposability :x:

No explicit implementation of signal handling.

# 10. Dev/prod parity :white_check_mark:

The application is run with `docker compose`, which always contains the same (backing) services. The only thing that changes between the environments is the configuration. Docker makes it easy running "heavy weight" backing services in a lightweight manner. Developer's don't have to worry about setting up a local database instance.

# 11. Logs :white_check_mark:

The application logs to `stdout` and does not concern itself with log files or persisting the logs into a database. In this simple implementation, the container was not started in the detached mode and all the logs were visible on the terminal. For production, the `stdout` output can be captured and persisted somewhere by the execution environment without changing the application.

# 12. Admin processes :x:

.NET does not provide a REPL shell for C#. I guess we can exec into the running container to run commands, but we probably won't even have PowerShell in the container, since we are using a minimal image optimized to serve as an ASP.NET Core runtime.
