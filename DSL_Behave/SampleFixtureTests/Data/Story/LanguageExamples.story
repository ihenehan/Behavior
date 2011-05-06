Story: Language Examples
	As a user 
	I want to write a test
	So that behavior can be validated

Before Story: Reset database
	Do Add item to story context

After Story: Post clean up
	Do Clean up the environment
	Do Add item to story context


Scenario Common: Set Table Data
	Given I have a common step
		| type		  |
		| data setup  |
		| system setup|

Before Scenario: Log in
	Do Add item to scenario context

@done
Scenario: Test the features of a Scenario
	Given I am a "test" user
	Given I have a step table
		| arg1	| arg2 |
		| foo   | bar  |
		| bar	| foo  |
	And the system is available
	When I write a "acceptance" test
	And I have "two" multiple arguments of "foo"
	Then Key "scenario" is in ScenarioContext
	And it should execute correctly

After Scenario: Log out
	Do Log out.

Before Scenario: Log in
	Do Add item to scenario context

@done
Scenario Outline: Test the features of a Scenario Outline
	Given I am a <role> user
	When I write a <type> test
	And I have argument at end <type>
	Then it should execute correctly

Test Data:
	| role | type |
	| user | foo  |
	| admin| bar  |
	| none | stuff|

@done
Scenario:Verify ScenarioContext clears
	Verify Key "scenario" is not in ScenarioContext