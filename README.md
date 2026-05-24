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
/api/home/posts/{page?}/?pageSize={pageSize}
/api/home/posts/private/{page?}/?pageSize={pageSize}
```

### ReferenceController:
```
/api/reference/additionalSignUpInfo
```

### UserController:
```
/api/user/signin
headers: {
Authentication: "Basic " + Base64Password
}

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

/api/user/profile/edit
body: {
  string UserId 
  string? Login 
  string? Nickname 
  string? Bio 
  string? Email 
  IFormFile? Avatar 
  string? OldBase64Password 
  string? Base64Password 
  string[]? Interests 
}

/api/user/users/find/{request} // request = user login or nickname
```

### PostController:
```
/api/post/add
body: {
  string UserId
  string Title
  IFormFile? PostImage
  string Bio
  string[] Interests
}

/api/post/getOwn/{userId}/{page?}/?pageSize={pageSize}
```

### CommentController:
```
/api/comment/add
body: {
  string UserId
  string PostId
  string Bio
}
```
