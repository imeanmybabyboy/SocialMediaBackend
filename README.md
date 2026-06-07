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

Edit profile:
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
```
