@acceptance @critical @usecase:posts/delete-post
Feature: Delete Post

  An authenticated Author removes draft posts. Published posts cannot be deleted.

  @ac:AC-001 @critical
  Scenario: Author deletes a draft post
    Given an authenticated author exists
    And the author has a draft post titled "Acceptance delete draft"
    When the author deletes the post
    Then the response is no content
    When the author requests the post by id
    Then the response is not found

  @ac:AC-002 @critical
  Scenario: Deleting a published post is rejected
    Given an authenticated author exists
    And the author has a published post titled "Acceptance delete published"
    When the author deletes the post
    Then the response is a conflict problem
