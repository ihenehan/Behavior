Story: Producer can Set Ticket Prices
As a Producer
I can set ticket prices
so Patrons can buy tickets online.

# The actual Acceptance Criteria written "on the card" is: "Test that I can supply separate prices for Adult, Child, and Senior."  In this case I'm separating these into three separate A/C.

	


# Here is the same A/C using the table-drive approach.
	
	Criterion Outline: Test that I can supply separate prices for Adult, Child, and Senior.
		Given a performance exists
		When I supply a price for <category> Tickets
		Then that price is stored and associated with <category> Tickets.

	Test Data:
		| category 	|
		| Adult 	|
		| Student	|
		| Senior	|