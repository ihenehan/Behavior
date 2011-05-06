@storyAttribute
Story: Sample Story
	As a user, 
	I want to write a test, 
	So that behavior can be validated

Before Story: Custom
	Given the database is in a known state.

After Story: Custom
	Given the environment is cleaned up.

Criterion Common:
Given I have a background step
	| type		  |
	| data setup  |
	| system setup|

Before Criterion: Custom
	Given I am logged in.

@test
Criterion:Do Stuff
Given I am a "test" user
And the system is available
When I write a "acceptance" test
Then it should execute correctly

After Criterion: Custom
	Given I am logged out.

Before Criterion: Custom
	Given I am logged in.

@test
Criterion Outline:Do More Stuff
Given I am a <role> user
When I write a <type> test
Then it should execute correctly

Test Data:
	| role | type |
	| user | foo  |
	| admin| bar  |
	| none | stuff|