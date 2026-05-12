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
/api/home/posts
/api/home/post/add
```

### ReferenceController:
```
/api/reference/additionalSignUpInfo
```

### UserController:
```
/api/user/signin
/api/user/signup
/api/user/profile/edit
/api/user/users/find
```
