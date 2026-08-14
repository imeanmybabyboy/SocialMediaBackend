# SocialMediaBackend

## Description
Backend part of SocialMedia project made using ASP.NET Core Web Api. The project uses EF Core to work with database.

---
## Intallation

### 1. Repository clone
Paste the following command into terminal: 

```
git clone https://github.com/imeanmybabyboy/SocialMediaBackend.git
```
### 2. Open the project using Visual Studio
---

## Database connection
### 3. Open Package Manager Console:
- View -> Other Windows -> Package Manager Console

### 4. Run the following commands:
- Add-Migration Initial
- Update-Database

## Run the project (F5 / Ctrl + F5)

## API Endpoints

### HomeController:
```
Get all posts:
/api/home/posts/{page?}/?pageSize={pageSize}

Get private posts (only your race's posts)
/api/home/posts/private/{page?}/?pageSize={pageSize}
```

### ReferenceController:
```
Get additional sign up info (interests and races):
/api/reference/additionalSignUpInfo
```

### UserController:
```
Sign in:
/api/user/signin
headers: {
Authentication: "Basic " + Base64Password
}

Sign up:
/api/user/signup
body: {
  string Login 
  string Email 
  string Base64Password 
  string Nickname 
  IFormFile? Avatar
  string RaceId
  string[] Interests
}

Sign out:
/api/user/signout

Edit the profile:
/api/user/profile/edit
body: {
  string? Login 
  string? Nickname 
  string? Bio 
  string? Email 
  IFormFile? Avatar 
  string? OldBase64Password 
  string? Base64Password 
  string[]? Interests 
}

Get user (list of users) preview by request (login or nickname)
/api/user/users/find/{request} // request = user login or nickname

Get user profile by id:
/api/user/profileById/{userId}

Get user profile by login:
/api/user/profileByLogin/{userLogin}

Get own liked posts:
/api/user/likedPosts/{page?}/?pageSize={pageSize}

Get own saved posts:
/api/user/savedPosts/{page?}/?pageSize={pageSize}

Follow a user:
/api/user/toggleFollow/{userId}

Check user followers:
/api/user/{userId}/followers

Check user followings:
/api/user/{userId}/following

Delete the profile:
/api/user/deleteProfile
FormModel = {
  Base64Password = {base64password}
}
```

### PostController:
```
Add post:
/api/post/add
body: {
  string Title
  IFormFile? PostImage
  string Bio
  string[] Interests
}

Get your posts:
/api/post/getOwn//{page?}/?pageSize={pageSize}

Like a post:
/api/post/toggleLike/{postId}

Save a post:
/api/post/toggleSave/{postId}

Share a post:
/api/post/toggleShare/{postId}

Check post likes (users who liked post):
/api/post/{postId}/likes

Check post saves:
/api/post/{postId}/shares

Check post shares:
/api/post/{postId}/shares

Get someone else's posts:
/api/post/getUserPosts/{userId}/{page?}/?pageSize={pageSize}

Edit your post:
/api/post/edit
body (formData): {
  string PostId
  string? Title
  IFormFile? PostImage
  string? Bio
  string[]? Interests 
  bool? IsPrivate
}

Delete a post:
/api/post/{postId}/delete
```

### CommentController:
```
Add comment:
/api/comment/add
body: {
  string PostId
  string Bio
}

Like a comment:
/api/comment/toggleLike/{commentId}

Check comment likes:
/api/comment/{commentId}/likes

Edit a comment:
/api/comment/edit
body (formData): {
  string CommentId
  string? Bio
}
```

### ChatController:
```
Send a private message:
[POST]
/api/chat/send
body: {
  string TargetUserId
  string Text
}

Get message history with a user:
[GET]
/api/chat/{targetUserId}/messages/{page?}/?pageSize={pageSize}

Get your chats with users:
[GET]
/api/chat/list

Delete a chat:
[DELETE]
/api/chat/{chatId}/delete
```

### 1. Real-time chat (SignalR)
```
npm install @microsoft/signalr
```

### 2. Connect (after the user is signed in)
```
import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://<your-backend-host>/hubs/chat", { withCredentials: true })
  .withAutomaticReconnect()
  .build();

await connection.start();
```

### 3. Listen for incoming messages
```
connection.on("ReceiveMessage", (message) => {
  // fires on the recipient's connection when someone sends them a message
});

connection.on("MessageSent", (message) => {
  // fires on the sender's own connection, as delivery confirmation
});
```

### 4. Send a message
```
await connection.invoke("SendPrivateMessage", {
  targetUserId: "<guid of the recipient>",
  text: "hello!"
});
```
