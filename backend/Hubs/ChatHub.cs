using System.Threading.Tasks.Dataflow;
using ChatSphere.DataService;
using ChatSphere.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatSphere.Hubs;

public class ChatHub : Hub
{
   private readonly SharedDb _shared;
   
   public ChatHub(SharedDb shared) => _shared = shared;
   
   public async Task JoinChat(UserConnection connection)
   {
      await Clients.All
         .SendAsync("RecieveMessage", $"{connection.Username} has joined.");
   }

   public async Task JoinSpecificChatRoom(UserConnection connection)
   {
      await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
      
      _shared.Connections[Context.ConnectionId] = connection;
      
      await Clients.Group(connection.ChatRoom)
         .SendAsync("JoinSpecificChatRoom", $"{connection.Username} has joined to this room.");
   }

   public async Task SendMessage(string message)
   {
      if (_shared.Connections.TryGetValue(Context.ConnectionId, out UserConnection connection))
      {
         await Clients.Group(connection.ChatRoom)
            .SendAsync("RecieveSpecificMessage", connection.Username, message);
      }
   }
}