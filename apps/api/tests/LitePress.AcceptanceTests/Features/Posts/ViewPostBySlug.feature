@acceptance @critical @usecase:posts/view-post-by-slug
Feature: View Post By Slug

  Anonymous readers fetch published posts by URL slug. Draft slugs are not exposed.

  @ac:AC-001 @critical
  Scenario: Published post is readable by slug
    Given an authenticated author exists
    And the author has a published post titled "Acceptance public slug read"
    When an anonymous caller requests the post by slug
    Then the response is successful
    And the post is readable by slug on the public API

  @ac:AC-002 @critical
  Scenario: Draft post slug is not publicly readable
    Given an authenticated author exists
    And the author has a draft post titled "Acceptance draft slug hidden"
    When an anonymous caller requests the post by slug
    Then the response is not found
