#A sample story definition with examples of available features.
Story: Sample Story
	As a user, 
	I want to write a test, 
	So that behavior can be validated

#Runs once at beginning of story execution
Before Story: Reset database
	Given the database is in a known state.

#Runs once at end of story execution
After Story: Post clean up
	Given the environment is cleaned up.
	And the database is in a known state.


#Is inserted into the beginning of every scenario sequence in this story
Scenario Common: Set Table Data
Given I have a common step
	| type		  |
	| data setup  |
	| system setup|

#Runs before the next scenario defined.
Before Scenario: Log in
	Given I am logged in.

#Scenario to be executed
@test
Scenario:Do Stuff
Given I am a "test" user
Given I have a step table
	| arg1	| arg2 |
	| foo   | bar  |
	| bar	| foo  |
And the system is available
When I write a "acceptance" test
And I have "two" multiple arguments of "foo"
Then it should execute correctly

#Runs after last scenario defined.
After Scenario: Log out
	Given I am logged out.

#Runs before the next scenario defined.
Before Scenario: Log in
	Given I am logged in.

#Scenario to be executed
@test
Scenario Outline:Do More Stuff
Given I am a <role> user
When I write a <type> test
And I have argument at end <type>
Then it should execute correctly

Test Data:
	| role | type |
	| user | foo  |
	| admin| bar  |
	| none | stuff|