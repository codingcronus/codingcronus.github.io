using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TddExample.Test.Infrastructure
{
    public abstract class DockerServer
    {
        private DockerClient client;

        public string ContainerName { get; }
        public string ImageName { get; }
        public string ImageTag { get; }
        public bool KeepAlive { get; }

        public DockerServer(string containerName, string imageName, string imageTag, bool keepAlive=false)
        {
            ContainerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
            ImageName = imageName ?? throw new ArgumentNullException(nameof(imageName));
            ImageTag = imageTag ?? throw new ArgumentNullException(nameof(imageTag));
            KeepAlive = keepAlive;
            client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();//, defaultTimeout: new TimeSpan(0, 2, 0)).CreateClient();
        }

        protected abstract Task<bool> IsReady();

        public async Task Connect()
        {
            var container = await DownloadImageToContainer();
            if (container == null) throw new NullReferenceException("Could not download Docker image to container");

            await StartContainer(container);

            var i = 0;
            while (!await IsReady())
            {
                i++;

                if (i > 20)
                    throw new TimeoutException($"Container {ContainerName} does not seem to be responding in a timely manner");

                await Task.Delay(1000);
            }
        }

        private async Task<ContainerListResponse> DownloadImageToContainer()
        {
            var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });

            var container = containers.FirstOrDefault(c => c.Names.Contains("/" + ContainerName));
            if (container == null)
            {
                // Download image (locally)
                //var image = 
                //await client.Images.CreateImageAsync(new ImagesCreateParameters() { FromImage = ImageName, Tag = ImageTag }, new AuthConfig(), new Progress<JSONMessage>());

                // Create the container
                var config = new Config()
                {
                    Hostname = "localhost"
                };

                // Configure the ports to expose
                var hostConfig = new HostConfig()
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { "80/tcp", new List<PortBinding> { new PortBinding { HostIP = "127.0.0.1", HostPort = "8080" } } },
                        { "3306/tcp", new List<PortBinding> { new PortBinding { HostIP = "0.0.0.0", HostPort = "3306" } } },
                    }
                };

                // Create the container
                var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters(config)
                {
                    Image = ImageName + ":" + ImageTag,
                    Name = ContainerName,
                    Tty = false,
                    HostConfig = hostConfig,
                });

                // Get the container object
                containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                container = containers.First(c => c.ID == response.ID);
            }

            return container;
        }

        private async Task StartContainer(ContainerListResponse container)
        {
            // Stop and remove existing container. Get a new container.
            if (container.State == "running" && !KeepAlive)
            {
                await client.Containers.StopContainerAsync(container.ID, new ContainerStopParameters());
                await client.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters());
                container = await DownloadImageToContainer();
            }

            // Start the container is needed
            if (container.State != "running")
            {
                var started = await client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
                if (!started) throw new Exception("Cannot start Docker container");
            }
        }
    }
}
