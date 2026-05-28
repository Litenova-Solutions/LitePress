@acceptance @critical @usecase:posts/publish-post
Feature: Publish Post

  An authenticated Author publishes a Draft post, making it visible on the public web.

  @ac:AC-001 @critical
  Scenario: Author publishes a draft post
    Given an authenticated author exists
    And the author has a draft post titled "Acceptance publish draft"
    When the author publishes the post
    Then the response is successful
    And the post is visible in the published posts feed

  @ac:AC-003 @critical
  Scenario: Publishing an already published post is rejected
    Given an authenticated author exists
    And the author has a published post titled "Acceptance already published"
    When the author publishes the post
    Then the response is a conflict problem

  @ac:AC-004 @authz @critical
  Scenario: Unauthenticated publish request is rejected
    Given the author has a draft post titled "Acceptance unauthenticated publish"
    When an unauthenticated caller publishes the post
    Then the response is unauthorized
