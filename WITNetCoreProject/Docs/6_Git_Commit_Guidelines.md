# Blueprint Backend .NET Core 3.1

## Git Commit Guidelines

Copied from https://karma-runner.github.io/6.3/dev/git-commit-msg.html

### Format **commit message**

using this format below:

```javascript
<req_commit_type>(<req_commit_scope>): <req_commit_title>

<optional_commit_description>

<optional_commit_footer>
```

condition:

- `req_commit_type` (mandatory) is one of this below:

  - `feat` = development new feature for user
  - `fix` = tag in certain bug fix source code in production
  - `style` = format source code, space, tab, indent that not directly impact to production deployment.
  - `refactor` = refactor production code. sample : rename variable, move logic to new function
  - `test` = added unit test that not directly impact to production deployment.
  - `chore` = fix script, dockerfile, Makefile etc. that not directly impact to production deployment.
  - `docs` = documentation changes, comment & README file

- `req_commit_scope` (mandatory) for module scope in the source code. Example:

  - config
  - log
  - service
  - repository
  - postgres
  - echo
  - grpc

- `req_commit_title` (mandatory) for brief description changes.

- `optional_commit_description` _(optional)_ full description about the changes in source code. you could be able to add some information, like (why & how), (what).

- `optional_commit_footer` _(optional)_ ticket number link to JIRA, Gitlab &
  info `breaking changes API`.

### Example

- changes on issue ABC-123, bugfix at login endpoint, because other could be able to login without send the device ID.

```javascript
fix(login): add device ID validation to login endpoint

wit.id/browse/ABC-123
```

- reformat source code from IDE (right click -> reformat code)

```javascript
style(format): apply reformat code from IDE
```

- Issue PAY-123, add new feature, payment using AsingPay. create API client / service object.

```javascript
feat(asingpay): implement json http api client

wit.id/browse/PAY-123
```

- change Makefile target, auto reformat code before run in local.

```javascript
chore(Makefile): reformat code before run at local machine
```
