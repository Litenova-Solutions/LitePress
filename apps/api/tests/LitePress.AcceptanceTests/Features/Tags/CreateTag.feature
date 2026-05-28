@acceptance @critical @usecase:tags/create-tag
Feature: Create Tag

  An authenticated Author creates tags for post assignment.

  @ac:AC-001 @critical
  Scenario: Author creates a unique tag
    Given an authenticated author exists
    And the author creates a tag named "Acceptance Unique Tag"
    Then the response is successful
    And the tag appears in the tag list

  @ac:AC-002 @critical
  Scenario: Duplicate tag name is rejected
    Given an authenticated author exists
    And the author creates a tag named "Acceptance Duplicate Tag"
    When the author creates another tag named "Acceptance Duplicate Tag"
    Then the response is a conflict problem
