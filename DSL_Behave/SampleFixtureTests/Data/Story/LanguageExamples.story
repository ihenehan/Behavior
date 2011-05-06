Story: Language Examples
	As a user 
	I want to write a test
	So that behavior can be validated

Before Story: Reset database
	Do Add item to story context

After Story: Post clean up
	Do Clean up the environment
	Do Add item to story context


Criterion Common: Set Table Data
	Given I have a common step
		| type		  |
		| data setup  |
		| system setup|

Before Criterion: Log in
	Do Add item to criterion context

@done
Criterion: Test the features of a Criterion
	Given I am a "test" user
	Given I have a step table
		| arg1	| arg2 |
		| foo   | bar  |
		| bar	| foo  |
	And the system is available
	When I write a "acceptance" test
	And I have "two" multiple arguments of "foo"
	Then Key "criterion" is in CriterionContext
	And it should execute correctly

After Criterion: Log out
	Do Log out.

Before Criterion: Log in
	Do Add item to criterion context

@done
Criterion Outline: Test the features of a Criterion Outline
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
Criterion:Verify CriterionContext clears
	Verify Key "criterion" is not in CriterionContext