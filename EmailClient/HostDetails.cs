namespace EmailClient
{
    public class HostDetails
    {
        public string Name { get; set; }
        public int Port { get; set; }

        public HostDetails(string hostName, int hostPort)
        {
            Name = hostName;
            Port = hostPort;
        }
    }
}
