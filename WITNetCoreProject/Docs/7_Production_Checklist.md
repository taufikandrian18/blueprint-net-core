# Blueprint Backend .NET Core 3.1

## Production Service Checklist

### Checklist before deployed in production

- [ ] Not showing error or warning from `build-output`.
- [ ] Not Failed while building the application
- [ ] Not showing Error output, check [error handling](./3_Error_Handling.md) &
      [logging](./3_Error_Handling.md). for log messages to see the error details.
- [ ] Graceful shutdown. before kill the app.. give a little time for every request / on going process for sucessfully run in certain timeouts.
      Logic process is supposed to be like below:
  - API/service is going to process _N_-requests
  - API/service suddenly received kill signal (_Ctrl-C, sigkill, etc_)
  - API/service return healthcheck NOK, then waiting for certain _t_-second
    to make on going process to become sucessfull process.
  - API/service exit program
