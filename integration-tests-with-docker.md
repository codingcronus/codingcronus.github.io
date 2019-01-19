---
layout: default
---

# Integration Testing using Docker Containers
Integration- and E2E-tests can often be brittle when testing against external services and infrastructure such as e.g. a database.
As a consultant giving advice to a multitude of different companies and organisations, I've seen the following scenario several times.

### Common Integration Test scenario
**Shared database**

In this common integration test scenario, multiple developers (or Continous Integration pipelines) are executing commands and queries against a shared database.
This is fine up until the point were a command alters data (aka State) that another test relies on. The database is no longer synchronized with requirements and assertions of the given integration test.

![Before](https://codingcronus.github.io/posts/integration-tests-with-containers/before.png)

In order to synchronzize the database, the data has to be truncated and re-inserted. This of course can be automated in the Test Fixture Tear Down, CI-process and whatnot. How ever since the database is shared, the script might be executed while others are querying the resource and hence the tests are very brittle.

### A better solution using Containers
**Dedicated database**

A  better solution is to have a dedicated database per developer. This requires quite some setup and is prone for subtle differences in each environment. Re-synchronizing the data in the Test Fixture Tear Down method would be trivial, but might also prove to be very time consuming to execute depending on the amount of data.

**Containerized database**

An even better solution would be to use Containers. We could go for a scenario like the one below.

![After](https://codingcronus.github.io/posts/integration-tests-with-containers/after.png)

Each container would use the same Docker Image with a dedicated database (MySQL, SQL Server, Oracle DB etc.) and the required integration test data baked into the image. This would allow us to respawn the database with its initial state in a matter of a few seconds.

### The Solution
The [FIRST](https://github.com/ghsukumar/SFDC_Best_Practices/wiki/F.I.R.S.T-Principles-of-Unit-Testing) principle in automated testing is an acronym for:
*   **F**ast
*   **I**ndependent
*   **R**epeatable
*   **S**elf-validating
*   **T**imely

You can follow this [link](https://github.com/ghsukumar/SFDC_Best_Practices/wiki/F.I.R.S.T-Principles-of-Unit-Testing) to read more about the details of FIRST principle. In essence integration tests often have a hard time being compliant with the **F**ast and the **R**epeatable rules of the FIRST principle. In other words: Integration tests are usually slow and brittle, which in turn makes them non-repeatable.

Hence our solution should strive for multiple success criteria.
#### Our success criteria:

*   Our tests should be fast to evaluate
*   Our database should always re-synchronize at startup
*   Our service implementation (the SUT) should not have any Docker dependency
*   Our Test Fixture should easily be able to integrate the Docker container usage
*   Our Test Fixture should be able to use multiple and different Docker containers

#### The Docker Image

While we could make a solution with any choice of database vendor, I will choose MySQL for this example. Switching with e.g. SQL Server is very easy and just a matter of choosing another Docker Image for the container. Microsoft has an official release for the SQL Server Docker Image [here](https://hub.docker.com/_/microsoft-mssql-server).

In order for our test to be fast, we should make sure that the Docker Container can start in the least amount of time possible.
The official MySQL Docker Image contains several features that we usually do not care about while performing integration testing and so we should look for a stripped down version. Luckily this can be found right [here](https://hub.docker.com/r/zanox/mysql/
# Header 1

This is a normal paragraph following a header. GitHub is a code hosting platform for version control and collaboration. It lets you and others work together on projects from anywhere.

## Header 2

> This is a blockquote following a header.
>
> When something is important enough, you do it even if the odds are not in your favor.

#### MySQL Docker Server service

```csharp
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace TddExample.Test.Infrastructure
{
    public class MySqlDockerServer : DockerServer
    {
        public string ConnectionString { get; }

        public MySqlDockerServer(string connectionString) : base (
            "AlineaNextIntegrationTestDb", 
            "alinea/next-integrationtest-mysql", 
            "latest"
            )
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected override async Task<bool> IsReady()
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
```

```ruby
# Ruby code with syntax highlighting
GitHubPages::Dependencies.gems.each do |gem, version|
  s.add_dependency(gem, "= #{version}")
end
```

#### Header 4

*   This is an unordered list following a header.
*   This is an unordered list following a header.
*   This is an unordered list following a header.

##### Header 5

1.  This is an ordered list following a header.
2.  This is an ordered list following a header.
3.  This is an ordered list following a header.

###### Header 6

| head1        | head two          | three |
|:-------------|:------------------|:------|
| ok           | good swedish fish | nice  |
| out of stock | good and plenty   | nice  |
| ok           | good `oreos`      | hmm   |
| ok           | good `zoute` drop | yumm  |

### There's a horizontal rule below this.

* * *

### Here is an unordered list:

*   Item foo
*   Item bar
*   Item baz
*   Item zip

### And an ordered list:

1.  Item one
1.  Item two
1.  Item three
1.  Item four

### And a nested list:

- level 1 item
  - level 2 item
  - level 2 item
    - level 3 item
    - level 3 item
- level 1 item
  - level 2 item
  - level 2 item
  - level 2 item
- level 1 item
  - level 2 item
  - level 2 item
- level 1 item

### Small image

![Octocat](https://assets-cdn.github.com/images/icons/emoji/octocat.png)

### Large image

![Branching](https://guides.github.com/activities/hello-world/branching.png)


### Definition lists can be used with HTML syntax.

<dl>
<dt>Name</dt>
<dd>Godzilla</dd>
<dt>Born</dt>
<dd>1952</dd>
<dt>Birthplace</dt>
<dd>Japan</dd>
<dt>Color</dt>
<dd>Green</dd>
</dl>

```
Long, single-line code blocks should not wrap. They should horizontally scroll if they are too long. This line should be long enough to demonstrate this.
```

```
The final element.
```
