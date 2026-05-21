# Exception Inventory

<!-- Last updated: 2026-05-21 -->

This file gives engineers and agents a reference for all custom exceptions so they
throw the correct type rather than inventing a new one or using a generic exception.
Before writing a validator, a repository method, or an aggregate method, check
this file.

Every new aggregate MUST add at least one `NotFoundException` entry. Every new
domain invariant that can be violated MUST add a corresponding exception entry.

Update this table in the same PR that introduces a new exception type.

---

## Exception Types

### Post Exceptions

| Exception Class | Category | Location | HTTP Status | When Thrown |
|:---|:---|:---|:---|:---|
| `PostNotFoundException` | NotFound | `Blog.Domain/Posts/Exceptions/PostNotFoundException.cs` | 404 | `IPostRepository.GetByIdAsync` when no Post exists with the given `PostId`. |
| `PostNotEditableException` | DomainInvariant | `Blog.Domain/Posts/Exceptions/PostNotEditableException.cs` | 409 | `Post.Update()` when the Post is not in `DraftPostState`. |
| `PostAlreadyPublishedException` | DomainInvariant | `Blog.Domain/Posts/Exceptions/PostAlreadyPublishedException.cs` | 409 | `Post.Publish()` when the Post is already in `PublishedPostState`. |
| `PostAlreadyArchivedException` | DomainInvariant | `Blog.Domain/Posts/Exceptions/PostAlreadyArchivedException.cs` | 409 | `Post.Archive()` when the Post is already in `ArchivedPostState`. |
| `PostCannotBeDeletedException` | DomainInvariant | `Blog.Domain/Posts/Exceptions/PostCannotBeDeletedException.cs` | 409 | `Post.Delete()` when the Post is in `PublishedPostState`. |
| `PostTagLimitExceededException` | DomainInvariant | `Blog.Domain/Posts/Exceptions/PostTagLimitExceededException.cs` | 422 | `Post.AddTag()` when the Post already has 10 Tags. |
| `PostTagAlreadyAssignedException` | DomainInvariant | `Blog.Domain/Posts/Exceptions/PostTagAlreadyAssignedException.cs` | 409 | `Post.AddTag()` when the given Tag is already on the Post. |
| `PostTagNotAssignedException` | DomainInvariant | `Blog.Domain/Posts/Exceptions/PostTagNotAssignedException.cs` | 409 | `Post.RemoveTag()` when the given Tag is not on the Post. |
| `PostSlugAlreadyExistsException` | Conflict | `Blog.Domain/Posts/Exceptions/PostSlugAlreadyExistsException.cs` | 409 | `IPostRepository` uniqueness check when a generated slug already exists. |

### Post Command Validation Exceptions

| Exception Class | Category | Location | HTTP Status | When Thrown |
|:---|:---|:---|:---|:---|
| `PostTitleRequiredException` | Validation | `Blog.Application.Write.Contracts/Posts/Exceptions/PostTitleRequiredException.cs` | 400 | `CreatePostCommandValidator` / `UpdatePostCommandValidator` when `Title` is null or empty. |
| `PostTitleTooLongException` | Validation | `Blog.Application.Write.Contracts/Posts/Exceptions/PostTitleTooLongException.cs` | 400 | `CreatePostCommandValidator` / `UpdatePostCommandValidator` when `Title` exceeds 200 characters. |
| `PostContentRequiredException` | Validation | `Blog.Application.Write.Contracts/Posts/Exceptions/PostContentRequiredException.cs` | 400 | `CreatePostCommandValidator` / `UpdatePostCommandValidator` when `Content` is null or empty. |
| `PostExcerptTooLongException` | Validation | `Blog.Application.Write.Contracts/Posts/Exceptions/PostExcerptTooLongException.cs` | 400 | `CreatePostCommandValidator` / `UpdatePostCommandValidator` when `Excerpt` exceeds 500 characters. |

### Tag Exceptions

| Exception Class | Category | Location | HTTP Status | When Thrown |
|:---|:---|:---|:---|:---|
| `TagNotFoundException` | NotFound | `Blog.Domain/Tags/Exceptions/TagNotFoundException.cs` | 404 | `ITagRepository.GetByIdAsync` when no Tag exists with the given `TagId`. |
| `TagNameAlreadyExistsException` | Conflict | `Blog.Domain/Tags/Exceptions/TagNameAlreadyExistsException.cs` | 409 | `ITagRepository` uniqueness check when a Tag with the same name (case-insensitive) already exists. |

### Tag Command Validation Exceptions

| Exception Class | Category | Location | HTTP Status | When Thrown |
|:---|:---|:---|:---|:---|
| `TagNameRequiredException` | Validation | `Blog.Application.Write.Contracts/Tags/Exceptions/TagNameRequiredException.cs` | 400 | `CreateTagCommandValidator` / `RenameTagCommandValidator` when `Name` is null or empty. |
| `TagNameTooLongException` | Validation | `Blog.Application.Write.Contracts/Tags/Exceptions/TagNameTooLongException.cs` | 400 | `CreateTagCommandValidator` / `RenameTagCommandValidator` when `Name` exceeds 50 characters. |

### Author Exceptions

| Exception Class | Category | Location | HTTP Status | When Thrown |
|:---|:---|:---|:---|:---|
| `AuthorNotFoundException` | NotFound | `Blog.Domain/Authors/Exceptions/AuthorNotFoundException.cs` | 404 | `IAuthorRepository.GetByIdAsync` when no Author exists with the given `AuthorId`. |

---

## HTTP Mapping Summary

| Category | HTTP Status | Description |
|:---|:---|:---|
| `NotFound` | 404 | The requested resource does not exist. |
| `DomainInvariant` | 409 | A business rule was violated. The request is valid but the operation is not allowed in the current state. |
| `Conflict` | 409 | A uniqueness constraint was violated. |
| `Validation` | 400 | The request input failed validation before the domain was touched. |
| `Unprocessable` | 422 | The request is structurally valid but exceeds a domain limit. |

> See `standards/docs/conventions/backend/06-exception-hierarchy.md` for the full exception
> base class hierarchy and `GlobalExceptionHandler` mapping rules.
