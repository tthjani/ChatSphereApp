using System.Collections.Concurrent;
using ChatSphere.Models;

namespace ChatSphere.DataService;

public class SharedDb
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new(); 
    
    public ConcurrentDictionary<string, UserConnection> Connections => _connections;
}